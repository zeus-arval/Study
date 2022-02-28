using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnerTask
{
    internal class Program
    {
        static Stopwatch watch = new Stopwatch();
        static void Main(string[] args)
        {
            watch.Start();
            var outer = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Starting Outer Task");
                var inner = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Starting Inner Task");
                    //code
                    Console.WriteLine("Finishing Inner task {0}", watch.Elapsed);
                }, TaskCreationOptions.AttachedToParent);
                Console.WriteLine("Finishing Outer Task {0}", watch.Elapsed);
            });
            outer.Wait();
            Console.WriteLine("End of the main method");
            watch.Start();
        }
        public void PrintWithAnotherColor(ConsoleColor color, string text)
        {
            ChangeConsoleColor(color);
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public void ChangeConsoleColor(ConsoleColor color) => Console.BackgroundColor = color;
    }
}
