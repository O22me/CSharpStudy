using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter19_ThreadTask
{
    class ParallelLoop
    {
        static bool IsPrime(long number)
        {
            if (number < 2) return false;
            if (number % 2 == 0 && number != 2) return false;

            for (long i = 2; i < number; i++)
                if (number % i == 0) return false;

            return true;
        }

        static void Main(string[] args)
        {
            //from 에서 to 까지의 숫자 사이의 소수구하기
            long from = Convert.ToInt64(args[0]);
            long to = Convert.ToInt64(args[1]);

            Console.WriteLine("Please press enter to start...");
            Console.ReadLine();
            Console.WriteLine("Started...");

            DateTime startTime = DateTime.Now;
            List<long> total = new List<long>();

            Parallel.For(from, to, (long i) =>
            {
                if (IsPrime(i)) total.Add(i);
            });

            DateTime endTime = DateTime.Now;

            TimeSpan ellapsed = endTime - startTime;

            Console.WriteLine("Prime number count between {0} and {1} : {2}", from, to, total.Count);

            Console.WriteLine("Ellapsed time : {0}", ellapsed);
        }
    }
}
