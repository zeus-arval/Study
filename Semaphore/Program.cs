using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for(int i = 1; i < 6; i++)
            {
                Reader reader = new Reader(in i);
            }
        }
    }
    public class Reader
    {
        static Semaphore semaphore = new Semaphore(3, 3);
        Thread thread;
        private int count;

        public Reader(in int i)
        {
            thread = new Thread(Read) { Name = $"Reader {i}"};
            thread.Start();
            count = 7;
        }

        private void Read()
        {
            while(count > 0)
            {
                semaphore.WaitOne();
                Console.WriteLine($"{thread.Name} enters a library");

                Console.WriteLine($"{thread.Name} starts reading a book");

                Thread.Sleep(100);

                Console.WriteLine($"{thread.Name} leaves a library");

                semaphore.Release();
                count--;
                Thread.Sleep(1000);
            }
        }
    }
}
