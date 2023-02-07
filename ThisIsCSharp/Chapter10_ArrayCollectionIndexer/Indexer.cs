using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTester
{
    class MyList<T>
    {
        private T[] array;

        public MyList()
        {
            array = new T[3];
        }

        public T this[int index]  //인덱서
        {
            get
            {
                return array[index];
            }

            set
            {
                if (index >= array.Length)
                {
                    Array.Resize<T>(ref array, index + 1);
                    Console.WriteLine($"Array resized : {array.Length}");
                }

                array[index] = value;
            }
        }

        public int Length
        {
            get { return array.Length; }
        }
    }

    class Indexer
    {
        static void Main(string[] args)
        {
            MyList<string> list = new MyList<string>();

            list[0] = "abc";
            list[1] = "def";
            list[2] = "ghi";
            list[3] = "jkl";
            list[4] = "mno";

            for (int i = 0; i < list.Length; i++)
                Console.WriteLine(list[i]);
        }
    }
}
