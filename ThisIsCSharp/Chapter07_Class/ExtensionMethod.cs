using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyExtension;

namespace MyExtension
{
    public static class IntegerExtension
    {
        public static double Square(this double myInt)
        {
            return myInt * myInt;
        }

        public static int Power(this int myInt, int exponent)
        {
            int result = myInt;
            for (int i = 1; i < exponent; i++)
                result = result * myInt;
            return result;
        }
    }
}

namespace CSharpTester
{
    class ExtensionMethod
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"3.141592^2 : {3.141592.Square()}");
        }
    }
}
