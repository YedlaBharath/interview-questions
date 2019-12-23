using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threading_asynchronus
{
    class Program
    {
        public static void threads()
        {
            Thread t1 = new Thread(new ThreadStart(tasknoasync));
            t1.Start();
        }
       
        public static async Task<int> taskasync1()
        {
            int count = 0;
            await Task.Run(()=>
                {
                for(int i=1;i<51;i++)
                {
                    Console.WriteLine(i);
                    count++;
                }
            });
            return count;
        }
        public static void tasknoasync()
        {
            for(int i=100;i<200;i++)
            {
                Console.WriteLine(i);
            }
        }
        static async Task Main(string[] args)
        {
            Func<int, int> y = x => 1 + 1 ;
            Console.WriteLine(y(1));
            threads();
            int count = await taskasync1();
            Console.WriteLine("Total count is {0}",count);
            Console.ReadLine();
        }
    }
}
