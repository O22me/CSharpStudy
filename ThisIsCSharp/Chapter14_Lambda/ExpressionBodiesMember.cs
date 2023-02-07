using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter14_Lambda
{
    class FriendList
    {
        private List<string> list = new List<string>();

        public void Add(string name) => list.Add(name);
        public void Remove(string name) => list.Remove(name);
        public void PrintAll()
        {
            foreach (var s in list)
                Console.WriteLine(s);
        }

        public FriendList() => Console.WriteLine("FriendList()");   //생성자
        ~FriendList() => Console.WriteLine("~FriendList()");        //종료자

        //public int Capacity => list.Capacity; //읽기전용

        public int Capacity
        {
            get => list.Capacity;
            set => list.Capacity = value;
        }

        //public string this[int index] => list[index]; //읽기전용
        public string this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }
    }

    class ExpressionBodiesMember
    {
        static void Main(string[] args)
        {
            FriendList obj = new FriendList();
            obj.Add("Eeny");
            obj.Add("Meeny");
            obj.Add("Miny");
            obj.Remove("Eeny");
            obj.PrintAll();

            Console.WriteLine($"{obj.Capacity}");
            obj.Capacity = 10;
            Console.WriteLine($"{obj.Capacity}");

            Console.WriteLine($"{obj[0]}");
            obj[0] = "Moe";
            obj.PrintAll();
        }
    }
}
