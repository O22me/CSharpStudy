using System;
using System.IO;

namespace FUP
{
    //앞선 클래스를 활용한 실질적 기능들.
    public class MessageUtil
    {
        public static void Send(Stream writer, Message msg) //Message는 Header와 Body로 이루어져 있다.
        {
            writer.Write(msg.GetBytes(), 0, msg.GetSize());
        }

        public static Message Receive(Stream reader)
        {
            int totalRecv = 0;
            int sizeToRead = 16;
            //hBuffer = Header에 대한 버퍼를 나타냅니다.
            byte[] hBuffer = new byte[sizeToRead];

            while (sizeToRead > 0)
            {
                byte[] buffer = new byte[sizeToRead];
                int recv = reader.Read(buffer, 0, sizeToRead);
                if (recv == 0)
                    return null;

                buffer.CopyTo(hBuffer, totalRecv);
                totalRecv += recv;
                sizeToRead -= recv;
            }
            //헤더 수신 완료

            Header header = new Header(hBuffer);

            totalRecv = 0;  //index역할
            byte[] bBuffer = new byte[header.BODYLEN]; //bBuffer Body크기만큼 할당
            sizeToRead = (int)header.BODYLEN;             //바디크기 만큼 읽기

            // [!] 여기서 디버깅 오류
            while(sizeToRead > 0)
            {
                byte[] buffer = new byte[sizeToRead];
                int recv = reader.Read(buffer, 0, sizeToRead);
                if (recv == 0)
                    return null;
                buffer.CopyTo(bBuffer, totalRecv);
                totalRecv += recv;
                sizeToRead -= recv;
                //totalRecv -= recv;              //totalRecv -= recv; 여기를 잘못적었구나
            }
            //바디 수신 완료.

            ISerializable body = null;
            switch (header.MSGTYPE)
            {
                case CONSTANTS.REQ_FILE_SEND:
                    body = new BodyRequest(bBuffer);
                    break;
                case CONSTANTS.REP_FILE_SEND:
                    body = new BodyResponse(bBuffer);
                    break;
                case CONSTANTS.FILE_SEND_DATA:
                    body = new BodyData(bBuffer);
                    break;
                case CONSTANTS.FILE_SEND_RES:
                    body = new BodyResult(bBuffer);
                    break;
                default:
                    throw new Exception(string.Format("Unknown MSGTYPE : {0}", header.MSGTYPE));
            }

            return new Message() { Header = header, Body = body };
        }
    }
}
