using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter15_LINQ
{
    class Profile
    {
        public string Name { get; set; }
        public int Height { get; set; }
    }

    class SimpleLinq
    {
        static void Main(string[] args)
        {
            Profile[] arrProfile =
            {
                new Profile() {Name = "정우성", Height = 186},
                new Profile() {Name = "김태희", Height = 158},
                new Profile() {Name = "고현정", Height = 172},
                new Profile() {Name = "이문세", Height = 178},
                new Profile() {Name = "하하", Height = 171},
            };

            var profiles = from profile in arrProfile
                           where profile.Height < 175
                           orderby profile.Height ascending
                           select new   //select문이 var형식을 결정함.
                           {
                               Name = profile.Name,
                               InchHeight = profile.Height * 0.393
                           };

            foreach (var profile in profiles)
                Console.WriteLine($"{profile.Name}, {profile.InchHeight}");
        }
    }
}
