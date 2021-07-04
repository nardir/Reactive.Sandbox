namespace Rx.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    //https://www.c-sharpcorner.com/UploadFile/746765/getting-started-with-reactive-extensions/

    static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Interactive vs Reactive");

            //Pull();

            //await PullAsync();

            Push();

            Console.ReadLine();
        }

        static void Pull() 
        {
            //Interactive, the consumer pulls from the producer
            Console.WriteLine("Synchronous Interactive (Pull)");

            //Producer
            IEnumerable<int> evenNumbers = Enumerable.Range(1, 20).Where(n => n % 2 == 0); //explicit: IEnumerable<int> vs var

            //Consumer
            foreach (var number in evenNumbers)
            {
                Console.WriteLine(number);
            }
        }

        static async Task PullAsync()
        {
            //Interactive, the consumer pulls from the producer
            Console.WriteLine("Asynchronous Interactive (Pull)");

            await foreach (int number in GetData(1, 20, 10))
            {
                Console.WriteLine(number);
            }
        }

        static async IAsyncEnumerable<int> GetData(int start, int count, int delay)
        {
            for (int n = start; n <= count; n++)
            {
                if (n % 2 == 0)
                {
                    yield return n;

                    await Task.Delay(delay);
                }
            }
        }

        static void Push()
        {
            //Reactive, the consumer pushes to the producer
            Console.WriteLine("Asynchronous Interactive (Pull)");

            //var consumer = Observable.Range(1, 20)
            //    .Where(n => n % 2 ==0)
            //    .Delay(TimeSpan.FromSeconds(2)) //Producer
            //    .Subscribe(n => Console.WriteLine(n)); //Consumer

            IObservable<int> p = Observable.Generate(1
                , n => (n <= 20)
                , n => n + 1
                , n => n
                , _ => TimeSpan.FromSeconds(1)).Where(n => n % 2 == 0);

            using (IDisposable s = p.Subscribe(n => Console.WriteLine(n)))
            {
                Console.WriteLine("Press enter to dispose subscription");

                Console.ReadLine();
            }

        }

    }
}
