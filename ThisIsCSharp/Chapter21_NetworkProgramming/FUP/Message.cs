namespace FUP
{
    // 각 속성 필드를 나타내기 위한 상수값 지정
    // 코드 작성시 CONSTANTS.REQ_FILE_SEND 형식으로 접근
    public class CONSTANTS
    {
        /// [메시지의 종류]
        /// REQ_FILE_SEND 파일 전송 요청
        /// REP_FILE_SEND 파일 전송 요청에 대한 응답
        /// FILE_SEND_DATA 파일 전송 데이터
        /// FILE_SEND_RES 파일 수신 결과
        public const uint REQ_FILE_SEND = 0x01;
        public const uint REP_FILE_SEND = 0x02;
        public const uint FILE_SEND_DATA = 0x03;   
        public const uint FILE_SEND_RES = 0x04;

        /// 메시지 분할 여부
        public const byte NOT_FRAGMENTED = 0x00;
        public const byte FRAGMENTED = 0x01;

        /// 마지막 메시지 여부
        public const byte NOT_LASTMSG = 0x00;
        public const byte LASTMSG = 0x01;

        /// 파일 전송 승인 여부
        public const byte ACCEPTED = 0x01;
        public const byte DENIED = 0x00;

        /// 파일 전송 성공 여부
        public const byte FAIL = 0x00;
        public const byte SUCCESS = 0x01;
    }

    //Message, Header, Body는 자신의 데이터를 byte 배열로 변환, 그리고 크기를 반환.
    public interface ISerializable
    {
        byte[] GetBytes();
        int GetSize();
    }

    public class Message : ISerializable
    {
        public Header Header { get; set; }      // 프로퍼티를 이용한 Header설정
        // ISerializable를 상속하는 객체. EX : BodyRequest, BodyResponse, BodyData, BodyResult를 받을 예정. 따라서 객체 명 Body.
        public ISerializable Body { get; set; }


        public byte[] GetBytes()
        {
            //bytes 에 Header, Body의 사이즈의 합만큼 할당.
            byte[] bytes = new byte[GetSize()];

            Header.GetBytes().CopyTo(bytes, 0); //0~할당
            Body.GetBytes().CopyTo(bytes, Header.GetSize());    //Header담겨 있으니 다음부터 할당.

            return bytes;
        }

        //Header와 Body 사이즈의 합 반환.
        public int GetSize()
        {
            return Header.GetSize() + Body.GetSize();
        }
    }
}
