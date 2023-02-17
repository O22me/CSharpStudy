using System;

namespace FUP
{
    public class Header : ISerializable
    {
        public uint MSGID { get; set; }             // 메시지 식별번호
        public uint MSGTYPE { get; set; }          // 메시지 종류
        public uint BODYLEN { get; set; }          // 본문 길이
        public byte FRAGMENTED { get; set; }   // 분할 여부
        public byte LASTMSG { get; set; }         // 마지막 메시지 여부
        public ushort SEQ { get; set; }              // 메시지의 파편 번호

        public Header() { }
        public Header(byte[] bytes) //헤더간 복사 EX : Header header2 = header1.GetBytes();
        {
            MSGID = BitConverter.ToUInt32(bytes, 0);    //순서 중요!
            MSGTYPE = BitConverter.ToUInt32(bytes, 4);
            BODYLEN = BitConverter.ToUInt32(bytes, 8);
            FRAGMENTED = bytes[12];
            LASTMSG = bytes[13];
            SEQ = BitConverter.ToUInt16(bytes, 14);
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[16];

            byte[] temp = BitConverter.GetBytes(MSGID);
            //temp(0번 부터)에서 bytes(0번 부터)로 temp.Length(4)만큼 복사
            Array.Copy(temp, 0, bytes, 0, temp.Length);

            temp = BitConverter.GetBytes(MSGTYPE);
            Array.Copy(temp, 0, bytes, 4, temp.Length); //temp.Length = 4

            temp = BitConverter.GetBytes(BODYLEN);
            Array.Copy(temp, 0, bytes, 8, temp.Length); //temp.Length = 4

            bytes[12] = FRAGMENTED;  //길이 1
            bytes[13] = LASTMSG;        //길이 1

            temp = BitConverter.GetBytes(SEQ);
            Array.Copy(temp, 0, bytes, 14, temp.Length); //temp.Length = 2

            return bytes;
        }

        public int GetSize()    //헤더 크기는 고정
        {
            return 16;
        }
    }
}
