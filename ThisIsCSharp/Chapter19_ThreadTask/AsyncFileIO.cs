using System;
using System.IO;
using System.Threading.Tasks;

namespace ThisIsCSharp.Chapter19_ThreadTask
{
    class AsyncFileIO
    {
        static async Task<long> CopyAsync(string FromPath, string ToPath)
        {
            using (var fromStream = new FileStream(FromPath, FileMode.Open))
            {
                long totalCopied = 0;

                using (var toStream = new FileStream(ToPath, FileMode.Create))
                {
                    byte[] buffer = new byte[1024];
                    int nRead = 0;
                    //더 이상 읽어오는게 없을때까지 buffer에 쓰기 반복(nRead == 0 이면 읽어온게 없는 것.)
                    while ((nRead = await fromStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        //buffer에 쓰여있는 만큼 쓰기.
                        await toStream.WriteAsync(buffer, 0, nRead);
                        totalCopied += nRead;
                    }
                }

                return totalCopied;
            }
        }

        static async void DoCopy(string FromPath, string ToPath)
        {
            long totalCopied = await CopyAsync(FromPath, ToPath);
            Console.WriteLine($"Copied Total {totalCopied} Bytes.");
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage : AsyncFileIO <Source> <Destination>");
                return;
            }

            DoCopy(args[0], args[1]);

            Console.ReadLine(); //비동기를 기다리기 위한 ReadLine
        }
    }
}
