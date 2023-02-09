using System;
using System.Runtime.CompilerServices;

namespace ThisIsCSharp.Chapter16_ReflectionAttribute
{
    public static class Trace
    {
        public static void WriteLine(string message,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            Console.WriteLine($"{file}\n(Line:{line})\n {member}: {message}");
        }
    }

    class CallerInfo
    {
        static void Main(string[] args)
        {
            Trace.WriteLine("즐거운 프로그래밍");
        }
    }
}
