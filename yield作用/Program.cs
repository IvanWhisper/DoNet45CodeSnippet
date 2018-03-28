using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace yield作用
{
    class Program
    { 
        static void Main(string[] args)
        {
            Console.WriteLine("start");
            Task.Run(async delegate
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine(string.Format("线程[{0}]", Thread.CurrentThread.ManagedThreadId.ToString()));
                    Console.WriteLine(i);
                    //await Task.Delay(1);
                    await Task.Yield(); // fork the continuation into a separate work item
                    Console.WriteLine(string.Format("线程[{0}]",Thread.CurrentThread.ManagedThreadId.ToString()));
                }
            });
            Console.WriteLine("end");
            Console.ReadKey();
        }
    }
}
