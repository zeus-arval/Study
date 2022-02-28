using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLearning
{
    internal class Program
    {
        public string BankAccount { get; private set; }
        public delegate void AccountHandler(string message);
        AccountHandler notify;

        private event EventHandler Notify
        {
            add => {
                if (value is null) return;
                notify += value;
                global::System.Console.WriteLine($"Added a {value}");
            }
            remove => {
                if (value is null) return;
                notify -= value;
                global::System.Console.WriteLine($"Removed a {value}");
            }
        }
        static void Main(string[] args)
        {

        }

    }
}
