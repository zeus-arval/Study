using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContinuationTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ImitateReturningResultFromTask();
            //PrintChainOfTasks();
        }
        #region Void tasks
        static void ContinuationVoidTasks()
        {
            double powedRandomNum = 0.0;
            int randomNum = 0;

            Task randomNumCreationTask = new Task(() =>
            {
                Random random = new Random();
                randomNum = random.Next(1, 200);

                Console.WriteLine($"Generated radnom number\t[{randomNum}]");
                Console.WriteLine($"Task's ID: \t[{Task.CurrentId}]");
            });

            Task powingRandomNum = randomNumCreationTask.ContinueWith(PrintTask);

            randomNumCreationTask.Start();
            powingRandomNum.Wait();
            Console.WriteLine("The end of the Main method");
            Task.WaitAll();
            Console.WriteLine($"{powedRandomNum}");

            void PrintTask(Task t)
            {
                powedRandomNum = Math.Pow((double)randomNum, 2);

                Console.WriteLine($"Powed a random number\t[{powedRandomNum}]");
                Console.WriteLine($"Task's ID: [{Task.CurrentId}]");
                Console.WriteLine($"Last task's ID: [{t.Id}]");
                Thread.Sleep(1000);
            }
        }

        static void PrintChainOfTasks()
        {
            Task task1 = new Task(() => Console.WriteLine($"Current Task: {Task.CurrentId}"));

            // задача продолжения
            Task task2 = task1.ContinueWith(t =>
                Console.WriteLine($"Current Task: {Task.CurrentId}  Previous Task: {t.Id}"));

            Task task3 = task2.ContinueWith(t =>
                Console.WriteLine($"Current Task: {Task.CurrentId}  Previous Task: {t.Id}"));


            Task task4 = task3.ContinueWith(t =>
                Console.WriteLine($"Current Task: {Task.CurrentId}  Previous Task: {t.Id}"));

            task1.Start();

            task4.Wait();   //  ждем завершения последней задачи
            Console.WriteLine("Конец метода Main");
        }
        #endregion
        #region Returning results tasks
        static void ImitateReturningResultFromTask()
        {
            int n1 = 4, n2 = 10;
            Task<int> sumTask = new Task<int>(() => Sum(n1, n2));

            Task printTask = sumTask.ContinueWith(task => PrintTask(task.Result));

            sumTask.Start();
            printTask.Wait();
            Console.WriteLine("The end of the main function");

            int Sum(int a, int b) => a + b;
            void PrintTask(int result) => Console.WriteLine($"Sum is [{result}]");
        }
        #endregion
    }
}
