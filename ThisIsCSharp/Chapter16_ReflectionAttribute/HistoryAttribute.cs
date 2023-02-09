using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter16_ReflectionAttribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =true)]
    class History : Attribute
    {
        private string programmer;
        public double version;
        public string changes;

        public History(string programmer)
        {
            this.programmer = programmer;
            changes = "First release";          //변수명은 오름차순으로.
            version = 1.0;
        }

        public string GetProgrammer()
        {
            return programmer;
        }
    }

    [History("Sean", changes = "2017-11-01 Created class stub", version = 0.1)]
    [History("Bob", changes = "2017-12-03 Added Func() Method", version = 0.2)]
    class MyClass
    {
        public void Func()
        {
            Console.WriteLine("Func()");
        }
    }

    class HistoryAttribute
    {
        static void Main(string[] args)
        {
            Type type = typeof(MyClass);
            Attribute[] attributes = Attribute.GetCustomAttributes(type);

            Console.WriteLine("MyClass change history...");

            foreach (Attribute a in attributes)
            {
                History h = a as History;
                if (h != null)
                {
                    Console.WriteLine("Ver:{0}, Programmer:{1}, Changes:{2}", h.version, h.GetProgrammer(), h.changes);
                }
            }
        }
    }
}
