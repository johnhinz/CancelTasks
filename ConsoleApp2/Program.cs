using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> list = Enumerable.Range(0, 100);

            Parallel.ForEach(list, async (i) =>
            {
                CancellationTokenSource source = new CancellationTokenSource();

                source.CancelAfter(10000);
                CancellationToken token = source.Token;
                token.ThrowIfCancellationRequested();

                try
                {
                    await ProcessAsync(i, token);
                    //if (await Task.WhenAny(task, Task.Delay(10000)) == task)
                    //{
                    //    await task;
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"Task {i} Cancelled");
                    //}
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine($"Cancelled: {i}");
                }


            });
            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static async Task<int> ProcessAsync(int i, CancellationToken token)
        {
            //CancellationTokenSource _pendingRequestsCts = new CancellationTokenSource();

            //CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(token, _pendingRequestsCts.Token);
            //if (hasTimeout)
            //{
            //    cts.CancelAfter(_timeout);
            //}


            Console.WriteLine($"In: {i}");
            Task.Delay(i * 1000).Wait();

            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            Console.WriteLine($"Out: {i}");
            return i;



        }
    }
}
