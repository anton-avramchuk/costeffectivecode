using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            //var filenames = new List<string>();
            //for (var i = 0; i < 6; i++)
            //{
            //    filenames.Add($"{i}.txt");
            //}

            //ReadAllFile(filenames[0]);
            //await Task.Run(() => ReadAllFile(filenames[0]));

            //// Do the timing
            //var syncTimer = Stopwatch.StartNew();
            //foreach (var f in filenames)
            //{
            //    ReadAllFile(f);
            //}
            //syncTimer.Stop();

            //var asyncTimer = Stopwatch.StartNew();
            //await Task.WhenAll(filenames.Select(f => Task.Run(() => ReadAllFile(f))));
            //asyncTimer.Stop();

            //Console.WriteLine("Sync time: {0}ms", syncTimer.ElapsedMilliseconds);
            //Console.WriteLine("Async time: {0}ms", asyncTimer.ElapsedMilliseconds);
        }

        static void ReadAllFile(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false))
            {
                byte[] buff = new byte[file.Length];
                file.Read(buff, 0, (int)file.Length);
            }
        }

        static async Task ReadAllFileAsync(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                byte[] buff = new byte[file.Length];
                await file.ReadAsync(buff, 0, (int)file.Length);
            }
        }
    }
}
