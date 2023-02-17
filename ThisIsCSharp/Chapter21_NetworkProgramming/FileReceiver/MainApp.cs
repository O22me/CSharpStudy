using FUP;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileReceiver
{
    //Client의 파일을 받는 'Server'
    class MainApp
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("사용법 : {0} <Directory>", Process.GetCurrentProcess().ProcessName);
                return;
            }
            uint msgID = 0;

            string dir = args[0];
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);

            const int bindPort = 5425;
            TcpListener server = null;
            try
            {
                IPEndPoint localAddress = new IPEndPoint(0, bindPort);

                server = new TcpListener(localAddress);
                server.Start();

                Console.WriteLine("파일 업로드 서버 시작... ");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("클라이언트 접속: {0}", ((IPEndPoint)client.Client.RemoteEndPoint).ToString());

                    NetworkStream stream = client.GetStream();

                    //대기 상태로 전환
                    Message reqMsg = MessageUtil.Receive(stream);

                    //파일 전송 요청(0x01)이 아니라면 닫기.
                    if (reqMsg.Header.MSGTYPE != CONSTANTS.REQ_FILE_SEND)
                    {
                        stream.Close();
                        client.Close();
                        continue;
                    }

                    BodyRequest reqBody = (BodyRequest)reqMsg.Body;

                    Console.WriteLine("파일 업로드 요청이 왔습니다. 수락하시겠습니까? yes/no");
                    string answer = Console.ReadLine(); //사용자 입력 yes/no

                    Message rspMsg = new Message();
                    
                    #region rspMsg에 '파일 전송 수락' 메시지 작성
                    rspMsg.Body = new BodyResponse()
                    {
                        MSGID = reqMsg.Header.MSGID,
                        RESPONSE = CONSTANTS.ACCEPTED
                    };
                    rspMsg.Header = new Header()
                    {
                        MSGID = msgID++,
                        MSGTYPE = CONSTANTS.REP_FILE_SEND,
                        BODYLEN = (uint)rspMsg.Body.GetSize(),
                        FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                        LASTMSG = CONSTANTS.LASTMSG,
                        SEQ = 0
                    };
                    #endregion

                    if (answer != "yes")
                    {
                        #region rspMsg에 '파일 전송 거부' 메시지 작성
                        rspMsg.Body = new BodyResponse()
                        {
                            MSGID = reqMsg.Header.MSGID,
                            RESPONSE = CONSTANTS.DENIED
                        };
                        #endregion
                        MessageUtil.Send(stream, rspMsg);
                        stream.Close();
                        client.Close();

                        continue;
                    }
                    else
                        MessageUtil.Send(stream, rspMsg);

                    Console.WriteLine("파일 수신을 시작합니다...");

                    long fileSize = reqBody.FILESIZE;
                    string fileName = Encoding.Default.GetString(reqBody.FILENAME);
                    FileStream file = new FileStream(dir + @"\" + fileName, FileMode.Create);

                    uint? dataMsgId = null;
                    ushort prevSeq = 0;

                    //여기서부터 reqMsg는 Sender의 fileMsg
                    while ((reqMsg = MessageUtil.Receive(stream)) != null)
                    {
                        Console.Write("#");
                        //파일 전송 데이터(0x03)이 아니라면 종료.
                        if (reqMsg.Header.MSGTYPE != CONSTANTS.FILE_SEND_DATA) break;

                        //첫번째 전송 데이터의 ID
                        if (dataMsgId == null) dataMsgId = reqMsg.Header.MSGID;
                        //두번째 그 이상
                        else
                        {
                            //전송 데이터의 ID는 동일, 안 맞다면 종료.
                            if (dataMsgId != reqMsg.Header.MSGID) break;
                        }

                        //서버에서 계산하는 SEQ와 전송 받은 데이터의 SEQ가 안 맞으면 종료. 즉 순서대로 안오면 종료.
                        if (prevSeq++ != reqMsg.Header.SEQ)
                        {
                            Console.WriteLine("{0}, {1}", prevSeq, reqMsg.Header.SEQ);
                            break;
                        }

                        //파일 쓰기.
                        file.Write(reqMsg.Body.GetBytes(), 0, reqMsg.Body.GetSize());

                        //분할 메시지가 아니라면 종료(1개일꺼니까)
                        if (reqMsg.Header.FRAGMENTED == CONSTANTS.NOT_FRAGMENTED) break;
                        //마지막 메시지라면 종료.
                        if (reqMsg.Header.LASTMSG == CONSTANTS.LASTMSG) break;
                    }

                    //파일크기 얻은후 stream 종료.
                    long recvFileSize = file.Length;
                    file.Close();

                    Console.WriteLine();
                    Console.WriteLine("수신 파일 크기 : {0} bytes", recvFileSize);

                    Message rstMsg = new Message();
                    #region 파일 수신 결과 성공 메시지 작성
                    rstMsg.Body = new BodyResult()
                    {
                        MSGID = reqMsg.Header.MSGID,
                        RESULT = CONSTANTS.SUCCESS
                    };
                    rstMsg.Header = new Header()
                    {
                        MSGID = msgID++,
                        MSGTYPE = CONSTANTS.REQ_FILE_SEND,
                        BODYLEN = (uint)rstMsg.Body.GetSize(),
                        FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                        LASTMSG = CONSTANTS.LASTMSG,
                        SEQ = 0
                    };
                    #endregion

                    //파일 크기랑 stream에서 적은 크기랑 같으면 전송 성공 메시지 전송
                    if (fileSize == recvFileSize)
                        MessageUtil.Send(stream, rstMsg);
                    else
                    {
                        #region 파일 수신 결과 실패 메시지 작성
                        rstMsg.Body = new BodyResult
                        {
                            MSGID = reqMsg.Header.MSGID,
                            RESULT = CONSTANTS.FAIL
                        };
                        #endregion
                        MessageUtil.Send(stream, rstMsg);
                    }
                    Console.WriteLine("파일 전송을 마쳤습니다.");

                    stream.Close();
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("서버를 종료합니다.");
        }
    }
}
