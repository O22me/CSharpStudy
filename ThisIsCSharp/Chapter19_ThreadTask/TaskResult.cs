using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter19_ThreadTask
{
    class TaskResult
    {
        //정적 메소드(소수 판별기)
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
            int taskCount = Convert.ToInt32(args[2]);

            //복습 : Func<in 매개변수 형식, out 반환 형식>
            Func<object, List<long>> FindPrimeFunc = (objRange) =>
            {
                //형변환 objRange : long[2] = {currentFrom, currentTo}
                long[] range = (long[])objRange;
                List<long> found = new List<long>();

                //currentFrom 부터 currentTo 까지
                for (long i = range[0]; i < range[1]; i++)
                {
                    if (IsPrime(i)) found.Add(i);   //소수라면 found 리스트에 추가.
                }

                return found;
            };

            //Task<TResult> : TResult를 반환하는 Task이고, Func를 인수로 받는다.
            //List<long>을 반환하는 Task 배열을 선언.
            Task<List<long>>[] tasks = new Task<List<long>>[taskCount];
            long currentFrom = from;                //첫번째 명령 인수
            long currentTo = to / tasks.Length;    //두번째 명령 인수 / 스레드 개수

            //스레드의 개수만큼 반복
            for (int i = 0; i < tasks.Length; i++)
            {
                Console.WriteLine("Task[{0}] : {1} ~ {2}", i, currentFrom, currentTo);  //i번째 스레드가 계산할 범위
                //i번째 task에 할당 = FindPrimeFunc 대리자, 해당 대리자에서 사용할 in 매개변수(배열)
                tasks[i] = new Task<List<long>>(FindPrimeFunc, new long[] { currentFrom, currentTo });

                //다음 할당을 위한 증가.
                currentFrom = currentTo + 1;

                //왜 -2?
                if (i == tasks.Length - 1) currentTo = to;
                else currentTo = currentTo + (to / tasks.Length);   //to / tasks.Length = 스레드당 맡은 범위.
            }

            Console.WriteLine("Please press enter to start...");
            Console.ReadLine();
            Console.WriteLine("Started...");

            DateTime startTime = DateTime.Now;

            foreach (Task<List<long>> task in tasks)
                task.Start();

            List<long> total = new List<long>();

            foreach (Task<List<long>> task in tasks)
            {
                task.Wait();
                total.AddRange(task.Result.ToArray());
            }

            DateTime endTime = DateTime.Now;

            TimeSpan ellapsed = endTime - startTime;

            Console.WriteLine("Prime number count between {0} and {1} : {2}", from, to, total.Count);

            Console.WriteLine("Ellapsed time : {0}", ellapsed);
        }
    }
}
