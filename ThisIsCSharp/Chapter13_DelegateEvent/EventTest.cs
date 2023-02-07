using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp
{
    delegate void EventHandler(string message);

    class MyNotifier
    {
        public event EventHandler SomethingHappened;
        public void DoSomething(int number)
        {
            int temp = number % 10;

            if (temp != 0 && temp % 3 == 0)
            {
                SomethingHappened(String.Format("{0} : 짝", number));
            }
        }
    }
    class EventTest
    {
        static public void MyHandler(string message) => Console.WriteLine(message);

        static void Main(string[] args)
        {
            MyNotifier notifier = new MyNotifier();

            //EventHandler가 어떻게 동작할지 전달. MyHandler처럼 작동하길 바람.
            notifier.SomethingHappened += new EventHandler(MyHandler);  

            for (int i = 0; i < 30; i++)
            {
                notifier.DoSomething(i);
            }
        }
    }
}
