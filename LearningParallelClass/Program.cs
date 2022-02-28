using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningParallelClass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RegisterCancellation();
        }

        #region DifferentParallelMethods
        public void MakeParallelWork()
        {
            Parallel.Invoke(
                Print,
                () =>
                {
                    Console.WriteLine($"Making task {Task.CurrentId}");
                    Thread.Sleep(1000);
                },
                () => Square(5)
                );

            void Print()
            {
                Console.WriteLine($"Making task {Task.CurrentId}");
                Thread.Sleep(1000);
                Console.WriteLine($"Finishing task {Task.CurrentId}");
            }
            void Square(int n)
            {
                Console.WriteLine($"Making task {Task.CurrentId}");
                Thread.Sleep(3000);
                Console.WriteLine($"Result is {n * n}");
            }
        }
        #endregion

        #region Make calculations with For

        static void MakeCalculationsWithFor()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ParallelLoopResult result = Parallel.For(1, 10, Square);

            if(!result.IsCompleted)
                Console.WriteLine($"Loop execution is finished on {result.LowestBreakIteration}'th Iteration");

            void Square(int n, ParallelLoopState pls)
            {
                if (n == 5) pls.Break();

                Console.WriteLine($"Making task {Task.CurrentId}");
                Console.WriteLine($"Square of {n} is {n * n}\t\t{watch.ElapsedMilliseconds}");
                Thread.Sleep(2000);
            }
        }
        #endregion

        #region Make calculations with For

        static void MakeCalculationsWithForeach()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Parallel.ForEach(new List<int>() { 1, 2, 3, 4, 5}, Square);

            void Square(int n)
            {
                Console.WriteLine($"Making task {Task.CurrentId}");
                Console.WriteLine($"Square of {n} is {n * n}\t\t{watch.ElapsedMilliseconds}");
                Thread.Sleep(2000);
            }
        }
        #endregion

        #region CancellationToken

        public static void ExitMethodSoft()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            // задача вычисляет квадраты чисел
            Task task = new Task(() =>
            {
                for (int i = 1; i < 10; i++)
                {
                    if (token.IsCancellationRequested)  // проверяем наличие сигнала отмены задачи
                    {
                        Console.WriteLine("Task is cancelled");
                        return;     //  выходим из метода и тем самым завершаем задачу
                    }
                    Console.WriteLine($"Number's {i} square is {i * i}");
                    Thread.Sleep(200);
                }
            }, token);
            task.Start();

            Thread.Sleep(1000);
            // после задержки по времени отменяем выполнение задачи
            cancelTokenSource.Cancel();
            // ожидаем завершения задачи
            Thread.Sleep(1000);
            //  проверяем статус задачи
            Console.WriteLine($"Task Status: {task.Status}");
            cancelTokenSource.Dispose(); // освобождаем ресурсы
        }
        public static void ExitMethodHard()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            Task task = new Task(() =>
            {
                for(int i = 0; i < 10; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    Console.WriteLine($"Square of number {i} is {i * i}");
                    Thread.Sleep(200);
                }
            }, token);

            try
            {
                task.Start();
                Thread.Sleep(1000);
                cancellationTokenSource.Cancel();

                //task.Wait();
            }
            catch(AggregateException ae)
            {
                foreach(Exception e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        Console.WriteLine("Operation is cancelled");
                    else
                        Console.WriteLine(e.Message);
                }
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }
            Console.WriteLine($"Task Status is [{task.Status}]");
        }
        public static void RegisterCancellation()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            // задача вычисляет квадраты чисел
            Task task = new Task(() =>
            {
                int i = 1;
                token.Register(() =>
                {
                    Console.WriteLine("Operation is cancelled");
                    i = 10;
                });
                for (; i < 10; i++)
                {
                    Console.WriteLine($"Square of number {i} is {i * i}");
                    Thread.Sleep(400);
                }
            }, token);
            task.Start();

            Thread.Sleep(1000);
            // после задержки по времени отменяем выполнение задачи
            cancelTokenSource.Cancel();
            // ожидаем завершения задачи
            Thread.Sleep(1000);
            //  проверяем статус задачи
            Console.WriteLine($"Task Status: {task.Status}");
            cancelTokenSource.Dispose(); // освобождаем ресурсы
        }

        #endregion
    }
}
