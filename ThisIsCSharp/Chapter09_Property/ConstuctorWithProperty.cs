using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp
{
    class BirthdayInfo
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int Age
        {
            get
            {
                return new DateTime(DateTime.Now.Subtract(Birthday).Ticks).Year;
            }
        }
    }

    class ConstuctorWithProperty
    {
        static void Main(string[] args)
        {
            BirthdayInfo birth = new BirthdayInfo() { Name = "sehun", Birthday = new DateTime(1997, 9, 26) };
            Console.WriteLine($"Name : {birth.Name}");
            Console.WriteLine($"Birthday : {birth.Birthday.ToShortDateString()}");
            Console.WriteLine($"Age : {birth.Age}");
        }
    }
}
