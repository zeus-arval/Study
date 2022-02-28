using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threads
{
    internal class Program
    {
        static Mutex mutex = new Mutex();
        static AutoResetEvent waitHandler = new AutoResetEvent(true);
        private static object locker = new object();
        private static int x = 0;
        public static void PrintWithMutex()
        {
            mutex.WaitOne();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} -> Number [{i}]");
                Thread.Sleep(100);
            }
            mutex.ReleaseMutex();
        }
        public static void PrintInLock()
        {
            lock (locker)
            {
                for ( int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} -> Number [{i}]");
                    Thread.Sleep(100);
                }
            }
        }
        public static void PrintWithMonitor()
        {
            bool acquiredLock = false;
            try
            {
                Monitor.Enter(locker, ref acquiredLock);
                x = 1;
                for(int i = x; i < 6; i++)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name}: {x++}");
                    Thread.Sleep(100);
                }
            }
            finally
            {
                if (acquiredLock) Monitor.Exit(locker);
            }
        }
        public static void PrintWithAutoResetEvent()
        {
            waitHandler.WaitOne();
            for(int i = 0; i < 6; i++)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} -> {i}");
                Thread.Sleep(100);
            }
            waitHandler.Set();
        }
        static void Main(string[] args)
        {
            //Artur = new Person("Artur");
            //ThreadStart threadStart = new ThreadStart(PrintOnAnotherThread);
            //Thread thread = new Thread(threadStart);
            //thread.Start();

            //Console.WriteLine(thread.ThreadState.ToString()); 
            //while(true)
            //{
            //    Artur.Name = "Artjom";
            //    Console.BackgroundColor = ConsoleColor.Red;
            //    Artur.PrintOutName();
            //    Thread.Sleep(300);
            //}

            for(int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(PrintWithAutoResetEvent) { Name = $"Thread{i}" };
                thread.Start();
            }
        }
    }
}
