using System;

namespace ThisIsCSharp.Chapter14_Lambda
{
    class StatementLambda
    {
        delegate string Concatenate(string[] args);

        static void Main(string[] args) //명령인수를 이용해보자.
        {
            Concatenate concat = (arr) =>
            {
                string result = "";
                foreach (string s in arr)
                    result += s;

                return result;
            };

            Console.WriteLine(concat(args));
        }
    }
}
