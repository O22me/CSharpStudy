using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ThisIsCSharp.Chapter18_FileIO
{
    [Serializable]
    class NameCard
    {
        public string Name;
        public string Phone;
        public int Age;
    }

    class Serialization
    {
        static void Main(string[] args)
        {
            /*File stream, BinaryFormatter 생성*/
            Stream ws = new FileStream("a.dat", FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();

            NameCard nc = new NameCard();
            nc.Name = "김세훈";
            nc.Phone = "010-123-4567";
            nc.Age = 27;

            //ws.Write(buffer); 가 아닌 Serializer를 이용한 파일쓰기
            serializer.Serialize(ws, nc);
            ws.Close();

            Stream rs = new FileStream("a.dat", FileMode.Open);
            BinaryFormatter deserializer = new BinaryFormatter();

            NameCard nc2;
            nc2 = (NameCard)deserializer.Deserialize(rs);
            rs.Close();

            Console.WriteLine($"Name:\t{nc.Name}");
            Console.WriteLine($"Phone:\t{nc.Phone}");
            Console.WriteLine($"Age:\t{nc.Age}");
        }
    }
}
