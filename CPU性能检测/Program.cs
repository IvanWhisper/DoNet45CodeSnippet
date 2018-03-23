
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections;

namespace CPU性能检测
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            参数说明：
            public PerformanceCounter (
             string categoryName,
             string counterName,
             string instanceName
            )
            categoryName
            性能计数器关联的性能计数器类别（性能对象）的名称。 
            counterName
            性能计数器的名称。 
            instanceName
            性能计数器类别实例的名称，或者为空字符串 ("")（如果该类别包含单个实例）。
            */


            //GetCategoryNameList();
            GetInstanceNameListANDCounterNameList("Process");
            //PerformanceCounterFun("LogicalDisk", "% Free Space", "C:");
            Console.ReadKey();


        }
        public static void GetCategoryNameList()
        {
            PerformanceCounterCategory[] myCat2;
            myCat2 = PerformanceCounterCategory.GetCategories();
            for (int i = 0; i < myCat2.Length; i++)
            {
                Console.WriteLine(myCat2[i].CategoryName);
            }
        }
        public static void GetInstanceNameListANDCounterNameList(string CategoryName)
        {
            string[] instanceNames;
            ArrayList counters = new ArrayList();
            PerformanceCounterCategory mycat = new PerformanceCounterCategory(CategoryName);
            try
            {
                instanceNames = mycat.GetInstanceNames();
                if (instanceNames.Length == 0)
                {
                    counters.AddRange(mycat.GetCounters());
                }
                else
                {
                    for (int i = 0; i < instanceNames.Length; i++)
                    {
                        counters.AddRange(mycat.GetCounters(instanceNames[i]));
                    }
                }
                for (int i = 0; i < instanceNames.Length; i++)
                {
                    Console.WriteLine(instanceNames[i]);
                }
                Console.WriteLine("******************************");
                foreach (PerformanceCounter counter in counters)
                {
                    Console.WriteLine(counter.CounterName);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to list the counters for this category");
            }
        }
        private static void PerformanceCounterFun(string CategoryName, string CounterName,string InstanceName)
        {
            PerformanceCounter pc = new PerformanceCounter(CategoryName, CounterName, InstanceName);
            for(int i=0;i<10000;i++)
            {
                Thread.Sleep(1000); // wait for 1 second   
                float cpuLoad = pc.NextValue();
                Console.WriteLine("CPU load = " + cpuLoad + " %.");
            }
        }
    }
}
