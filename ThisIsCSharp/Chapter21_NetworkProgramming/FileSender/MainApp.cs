using FUP;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileSender
{
    //Server에 파일 업로드하는 'Client'
    class MainApp
    {
        const int CHUNK_SIZE = 4096;

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("사용법 : {0} <Server IP> <File Path>", Process.GetCurrentProcess().ProcessName);
                return;
            }

            string serverIP = args[0];
            const int serverPort = 5425;
            string filePath = args[1];

            try
            {
                IPEndPoint clientAddress = new IPEndPoint(0, 0);
                IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

                Console.WriteLine("클라이언트: {0}, 서버: {1}", clientAddress.ToString(), serverAddress.ToString());

                uint msgID = 0;

                Message reqMsg = new Message();
                #region 파일 전송 요청 메시지 작성
                reqMsg.Body = new BodyRequest()
                {
                    FILESIZE = new FileInfo(filePath).Length,
                    FILENAME = Encoding.Default.GetBytes(filePath)
                };

                reqMsg.Header = new Header()
                {
                    MSGID = msgID++,
                    MSGTYPE = CONSTANTS.REQ_FILE_SEND,
                    BODYLEN = (uint)reqMsg.Body.GetSize(),
                    FRAGMENTED = CONSTANTS.NOT_FRAGMENTED,
                    LASTMSG = CONSTANTS.LASTMSG,
                    SEQ = 0
                };
                #endregion

                /*디버깅용 콘솔 출력창
                Console.WriteLine("FILESIZE: " + new FileInfo(filePath).Length);    //26
                Console.WriteLine("FILENAME: " + Encoding.Default.GetString(reqMsg.Body.GetBytes())); //abcdef.txt
                Console.WriteLine((uint)reqMsg.Body.GetSize());     //18 = sizeof(long){8} + bytes[].Length{abcdef.txt =10}
                 */

                TcpClient client = new TcpClient(clientAddress);
                client.Connect(serverAddress);

                NetworkStream stream = client.GetStream();

                MessageUtil.Send(stream, reqMsg);

                Message rspMsg = MessageUtil.Receive(stream);

                #region return 서버 비정상 응답 OR 파일 전송 거부
                if (rspMsg.Header.MSGTYPE != CONSTANTS.REP_FILE_SEND)
                {
                    Console.WriteLine("정상적인 서버 응답이 아닙니다. {0}", rspMsg.Header.MSGTYPE);
                    return;
                }

                if (((BodyResponse)rspMsg.Body).RESPONSE == CONSTANTS.DENIED)
                {
                    Console.WriteLine("서버에서 파일 전송을 거부했습니다.");
                    return;
                }
                #endregion

                using (Stream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    byte[] rbytes = new byte[CHUNK_SIZE];   //CHUNK_SIZE 4096

                    long readValue = BitConverter.ToInt64(rbytes, 0);

                    int totalRead = 0;
                    ushort msgSeq = 0;

                    //보내려는 파일의 바이트 크기에 따라 쪼개보낼지 결정.
                    byte fragmented = (fileStream.Length < CHUNK_SIZE) ? CONSTANTS.NOT_FRAGMENTED : CONSTANTS.FRAGMENTED;

                    //totalRead 읽은 크기 누적. 파일 크기보다 작다면 아직 보낼 파일이 남은것.
                    while (totalRead < fileStream.Length)
                    {
                        int read = fileStream.Read(rbytes, 0, CHUNK_SIZE);
                        totalRead += read;  //읽은 만큼 추가.

                        Message fileMsg = new Message();

                        byte[] sendBytes = new byte[read];          //읽은 크기만큼 바이트 배열 생성
                        Array.Copy(rbytes, 0, sendBytes, 0, read);   //파일 읽은 만큼(rbytes) 보낼 배열(sendBytes)에 저장

                        #region 파일 데이터 메시지 작성
                        fileMsg.Body = new BodyData(sendBytes);
                        fileMsg.Header = new Header()
                        {
                            MSGID = msgID,
                            MSGTYPE = CONSTANTS.FILE_SEND_DATA,
                            BODYLEN = (uint)fileMsg.Body.GetSize(),
                            FRAGMENTED = fragmented,
                            LASTMSG = (totalRead < fileStream.Length) ? CONSTANTS.NOT_LASTMSG : CONSTANTS.LASTMSG,
                            SEQ = msgSeq++
                        };
                        #endregion

                        Console.WriteLine("#");

                        MessageUtil.Send(stream, fileMsg);
                    }

                    Console.WriteLine();

                    Message rstMsg = MessageUtil.Receive(stream);

                    BodyResult result = ((BodyResult)rstMsg.Body);
                    Console.WriteLine("파일 전송 성공: {0}", result.RESULT == CONSTANTS.SUCCESS);
                }

                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("클라이언트를 종료합니다.");
        }
    }
}
