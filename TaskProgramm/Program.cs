using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskProgramm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            object locker = new object();
            Task[] tasks = new Task[3];
            for(int i = 0; i < 3; i++)
            {
                tasks[i] = new Task(() =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Task [{i}] finishes its job");
                });
                tasks[i].Start();
             
            }

            Console.WriteLine("Done!");
            Task.WaitAll(tasks);
        }
    }
}
