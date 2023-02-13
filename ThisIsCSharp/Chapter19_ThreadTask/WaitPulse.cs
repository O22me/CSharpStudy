using System;
using System.Threading;

namespace ThisIsCSharp
{
    class Counter
    {
        const int LOOP_COUNT = 20;

        readonly object thisLock;
        bool lockedCount = false;

        private int count;
        public int Count => count;

        public Counter()
        {
            thisLock = new object();
            count = 0;
        }

        public void Increase()
        {
            int loopCount = LOOP_COUNT;
            while (loopCount-- > 0)
            {
                lock (thisLock)
                {
                    Console.WriteLine("Increase LOCK");
                    while (count > 0 || lockedCount == true)    //Increasse() 함수는 count > 0, count 초기값 0
                        Monitor.Wait(thisLock);

                    lockedCount = true;
                    Console.WriteLine("Increase : " + ++count);
                    lockedCount = false;

                    Monitor.Pulse(thisLock);
                }
            }
        }

        public void Decrease()
        {
            int loopCount = LOOP_COUNT;
            while (loopCount-- > 0)
            {
                lock (thisLock)
                {
                    Console.WriteLine("Decrease LOCK");
                    while (count < 0 || lockedCount == true)    //Decrease() 함수는 count < 0
                        Monitor.Wait(thisLock);

                    lockedCount = true;
                    Console.WriteLine("Decrease : " + --count);
                    lockedCount = false;

                    Monitor.Pulse(thisLock);
                }
            }
        }
    }
    //역시 여러번 보면 이해가 된다니까.

    class WaitPulse
    {
        static void Main(string[] args)
        {
            Counter counter = new Counter();

            Thread incThread = new Thread(new ThreadStart(counter.Increase));
            Thread decThread = new Thread(new ThreadStart(counter.Decrease));

            incThread.Start();
            decThread.Start();

            incThread.Join();
            decThread.Join();

            Console.WriteLine(counter.Count);
        }
    }
}
