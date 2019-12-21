using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.IO;

namespace Threading_full
{
    // 1)
    class Program
    {
        static ManualResetEvent manual = new ManualResetEvent(false);
        public void thread()
        {
            Thread t1 = Thread.CurrentThread;
            Console.WriteLine(t1.Name+"has been invoked");
            for (int i=0; i<100;i++)
            {
                Console.WriteLine(i);
                if (i.Equals(50))
                {
                    Thread.Sleep(10000);
                    manual.WaitOne();
                    
                }
            }
        }
        public static void thread2()
        {
            Thread t2 = Thread.CurrentThread;
            Console.WriteLine(t2.Name+"has been invoked");
            
            for (int i=100;i>0;i--)
            {
                Console.WriteLine(i);
            }
            Console.ReadLine();
            manual.Set();
            
        }
    }
    // 2)
    class threadlocksync
    {
        public void threadsync1()
        {
            lock(this)
            {
                Thread ts1 = Thread.CurrentThread;
                Console.WriteLine(ts1.Name + " has been Invoked");
                for (int i=500;i<600;i++)
                {
                    Console.WriteLine(i);
                }
            }
        }
        public void threadsync2()
        {
            lock(this)
            {
                Thread ts2 = Thread.CurrentThread;
                Console.WriteLine(ts2.Name + " has been Invoked");
                for (int i = 600; i > 500; i--)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
    // 3)
    class threadmutexsycn
    {
        static Mutex mutex = new Mutex();
        public void mutexsync1()
        {
            for(int i=0;i<1;i++)
            {
                mutexsync2();
            }
        }
        public void mutexsync2()
        {
            mutex.WaitOne();// this method will lock the Thread.
            Thread tms1 = Thread.CurrentThread;
            Console.WriteLine("{0} Has Entered mutexsync method",tms1.Name);
            Thread.Sleep(1000);
            Console.WriteLine("{0} Has leaved mutexsync method", tms1.Name);
            Console.ReadLine();
            mutex.ReleaseMutex();// This Method will Release the Thread.
        }
    }
    class mutexclassinit
    {
        public static Mutex mutex = new Mutex();
    }
    class mutextmethod1class
    {
        public mutextmethod1class()
        {
            Thread tmm = new Thread(new ThreadStart(Go1));
            tmm.Name = " Thread 1 of mutex method 1 class";
            tmm.Start();
        }
        public void Go1()
        {
            mutexclassinit.mutex.WaitOne();
            Console.WriteLine("mutexmethod1class is waiting for the mutex");
            Thread tmmN = Thread.CurrentThread;
            Console.WriteLine("mutex has started for mutexmethod1class"+tmmN.Name);
            int i = 0;
            while(i<10)
            {
                Console.WriteLine("In mutexmethod1 class count is " + i);
                i++;
            }
            Thread.Sleep(500);
            Console.WriteLine("mutexmethod1class has been Released");
            mutexclassinit.mutex.ReleaseMutex();

        }
    }
    class mutexmethod2class
    {
        public mutexmethod2class()
        {
            Thread tmm = new Thread(Go2);
            tmm.Name = " Thread 2 of mutex method 2 class";
            tmm.Start();
        }
        public void Go2()
        {     
            mutexclassinit.mutex.WaitOne();
            Console.WriteLine("mutexmethod2class is waiting for the mutex");
            Thread tmmN = Thread.CurrentThread;
            Console.WriteLine("mutex has started for mutexmethod2class"+tmmN.Name);
            int i = 10;
            while(i<20)
            {
                Console.WriteLine("In mutexmethod2 class count is " + i);
                i++;
            }
            Thread.Sleep(500);
            Console.WriteLine("mutexmethod2class has been Released");
            mutexclassinit.mutex.ReleaseMutex();
        }
    }
    // 4)
    class timerclass
    {
        private System.Timers.Timer timer;
        public void timermethod() // Now according to the interval i set the timer execute the block code will executed.
        {
            timer = new System.Timers.Timer();
            timer.Enabled = true; // Timer is on;
            timer.Interval = 5000; // i set timer to 5sec
            timer.Elapsed += Timer_Elapsed; // This is the Method or Event that will be Elapsed or Executed for every interval.
            timer.AutoReset = true; // This AutoReset will get and set that event should Elaspsed or not if it is false Event will be raised only once if it is true repeatedly it is raised.
            Console.WriteLine("press any key to exit");
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Raised :{0} " + e.SignalTime);
        }
    }
    // 5)
    class monitorsync
    {
        static object send = new object();
        public monitorsync()
        {
            for(int i=0;i<5;i++)
            {
                Thread tM1 = new Thread(new ThreadStart(write));
                tM1.Name = "Monitor Thread " + i.ToString();
                tM1.Start();
            }
            
        }
        public void write()
        {
            Monitor.Enter(send);
            string tMN = Thread.CurrentThread.Name;
            Console.WriteLine("{0} waiting to wite into the string Buffer",tMN);
            Thread.Sleep(2000);
            try
            {
                string text = "Hi This is Bharath and thread is " + tMN;
                FileStream fs = new FileStream("E:\\Monitor.txt", FileMode.OpenOrCreate);
                StreamWriter stw = new StreamWriter(fs);
                stw.Write(text);
                stw.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("{0} The Thread has written into the string Buffer", tMN);
                Monitor.Exit(send);
            }
            
        }
        /*public void readmonitor()
        {
            FileStream fs = new FileStream("E:\\Monitor.txt", FileMode.OpenOrCreate);
            Console.WriteLine("Reading String Bulider");
            StreamReader str = new StreamReader(fs);
            str.ReadToEnd();
        }*/
    }

    class Mainclass
    {
        public static void Main()
        {
            // 5) Now another way for Syncronization of threads.
            // Monitor:- Moniter is a mechanism for ensuring that only one thread at a time running a certain piece of code(critical section).
            // A Monitor has a lock, and only one thread at a time may acquire it.
            // To run certain block of code a Thread must acquire Monitor.
            // Monitor is unbound which means it can be accessed from any context.
            // Monitor class is a collection of static methods, so we only static methods and fields while using Monitor.
            /*Monitor.Enter() : Acquires an exclusive lock on the specified object. This action also marks the beginning of a critical section.
              Monitor.Exit() : Releases an exclusive lock on the specified object.This action also marks the end of a critical section protected by the locked object.
              Monitor.Pules() : Notifies a thread in the waiting queue of a change in the locked object's state.
              Monitor.Wait() : Releases the lock on an object and blocks the current thread until it reacquires the lock.
              Monitor.PulesAll() : Notifies all waiting threads of a change in the object's state.
              Monitor.TryEnter() : Attempts to acquire an exclusive lock on the specified object.*/

            monitorsync ms = new monitorsync();



            /*// 4) Now we user Timers For any Event.
            // Timer Executes a block of code at a given interval of time.
            // Suppose when you want to fire an Event after a particular time like if you want backup your filer every day than you can set timer to Elapse my folder to back up every day and set timer as 24 hrs.
            timerclass tc = new timerclass();
            tc.timermethod();*/




            /*// 3) Now Another way of making a Thread Synchrnous using Mutex.
            // Mutex is like a lock but it can work accross multiple process.
            // We call the "WaitHandle.waitOne()" method to lock and "ReleaseMutex" to unlock.
            for(int i=0;i<10;i++)
            {
                threadmutexsycn tms = new threadmutexsycn();
                Thread tms1 = new Thread(new ThreadStart(tms.mutexsync1));
                //tms1.Name = string.Format("Thread " + i);
                tms1.Name = "Thread " + i.ToString();
                tms1.Start();
            }
            // Another example using mutex.
            mutextmethod1class mm1c = new mutextmethod1class();
            mutexmethod2class mm2c = new mutexmethod2class();*/
            
            


            /*// 2) Thread synchronization using lock keyword but synchronization can be possible in many ways.
            // Thread Synchronization has been achieved using lock Keyword.
            threadlocksync ts = new threadlocksync();
            Thread ts1 = new Thread(new ThreadStart(ts.threadsync1));
            ts1.Start();
            ts1.Name = "New Synchronization Thread 1";

            Thread ts2 = new Thread(ts.threadsync2);
            ts2.Name = "New synchronization Thread 2";
            ts2.Start();*/




            /*// 1) Bellow You have Two Threads and You have invoked both threads but they will run asynchronusly and make the results unexpected.
            Program obj = new Program();
            Thread t1 = new Thread(obj.thread);// we create thread by Thread class.
            t1.Name = "Thread 1";
            t1.Start();
            t1.Priority = ThreadPriority.Highest;
            
            Thread t2 = new Thread(new ThreadStart(Program.thread2));// we can create a thread by Thread class and the constructor of ThreadStart class.
            t2.Name = "Thread 2"; // The Thead Name can be Changed.s
            t2.Start(); // Thread has been started.
            t2.Priority = ThreadPriority.Lowest;// This gives us the priority of which thread to run first but it is not guarented because thread is highly system independent..
            t2.Join();// until this thread is terminated or completes its task the other calling threads will be in wait.*/

            Console.ReadLine();
        }
    }
}
