using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter18_FileIO
{
    class TextFile
    {
        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter(new FileStream("a.dat", FileMode.Create));

            sw.Write(int.MaxValue);
            sw.Write("Good Morning!");
            sw.Write(uint.MaxValue);
            sw.Write("안녕하세요!");
            sw.Write(double.MaxValue);

            sw.Close();

            StreamReader sr = new StreamReader(new FileStream("a.dat", FileMode.Open));

            Console.WriteLine($"File size : {sr.BaseStream.Length} bytes");

            while (!sr.EndOfStream)
            {
                Console.WriteLine(sr.ReadLine());
            }

            sr.Close();
        }
    }
}
