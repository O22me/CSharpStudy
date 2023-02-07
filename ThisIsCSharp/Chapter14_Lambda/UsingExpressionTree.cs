using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter14_Lambda
{
    class UsingExpressionTree
    {
        static void Main(string[] args)
        {
            //1*2+(x-y)
            Expression const1 = Expression.Constant(1);
            Expression const2 = Expression.Constant(2);

            Expression leftExp = Expression.Multiply(const1, const2); //1 * 2

            Expression param1 = Expression.Parameter(typeof(int));  //x
            Expression param2 = Expression.Parameter(typeof(int));  //y

            Expression rightExp = Expression.Subtract(param1, param2); //x - y

            Expression exp = Expression.Add(leftExp, rightExp);         //left는 상수, right는 매개변수

            Expression<Func<int, int, int>> expression = 
                Expression<Func<int, int, int>>.Lambda<Func<int, int, int>>
                (exp, new ParameterExpression[]{ (ParameterExpression)param1, (ParameterExpression)param2 });

            Func<int, int, int> func = expression.Compile();

            //x = 7, y = 8
            Console.WriteLine($"1*2+({7}-{8}) = {func(7, 8)}");
        }
    }
}
