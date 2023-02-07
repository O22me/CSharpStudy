using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter14_Lambda
{
    class ExpressionTreeViaLambda
    {
        static void Main(string[] args)
        {
            Expression<Func<int, int, int>> expression = (a, b) => 1 * 2 + (a - b);
            Func<int, int, int> func = expression.Compile();

            //x = 7, y = 8
            Console.WriteLine($"1*2+({7}-{8}) = {func(7, 8)}");
        }
    }
}
