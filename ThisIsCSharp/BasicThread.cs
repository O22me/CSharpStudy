using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThisIsCSharp
{
    class BasicThread
    {
        static void DoSomething()
        {
            for (int i = 0;  i < 5; i++)
            {
                Console.WriteLine($"Do something : {i}");
                Thread.Sleep(1000);
            }
        }
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(DoSomething));

            Console.WriteLine("Starting thread...");
            t1.Start(); //fork : 스레드는 DoSomething() 실행

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Main : {i}");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Waiting until thread stops...");
            t1.Join();  //join

            Console.WriteLine("Finished");
        }
    }
}
