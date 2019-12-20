using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace delegates_full
{
    class A { }
    class B : A { }
    class C { }

    class Program
    {
        public delegate A str();
        static A MethodA()
        {
            Console.WriteLine("Method A");
            return new A();
        }
        static B MethodB()
        {
            Console.WriteLine("method B");
            return new B();
        }
        // delegate normal example and the above code is about delegate covariance means it can call a method that return parent or child class object.
        public delegate void cal(int n);
        static int num = 100;
        public static void add(int m)
        {
            num = num + m;
            Console.WriteLine(num);
        }
        static void mul(int m)
        {
            num = num * m;
            Console.WriteLine(num);

        }

        static void Main(string[] args) // this is the first thread that will start in the program.
        {
            // IEnumerable and IEnumerator 
            // Both are used to loop through the collections.
            Iemunc objenum = new Iemunc();
            objenum.Enum();

            // AutoResetEvent and ManualResetEvent
            // Both are same only one Set is Enough to Release the WaitOne Thread in ManualResetEvent but in AutoResetEvent as many WaitOne are there you need Set as many.
            AutoRest autoobj = new AutoRest();
            autoobj.mainthread();

            // delegates uses events example.
            mydelclass mydelobj = new mydelclass();

            // delegate covarince which return the parent or child class object.
            mydelobj.mymethod();
            str s = new str(MethodA);
            s();
            str s1 = MethodB;
            s1();

            // delegate initialization.
            cal c = new cal(mul);
            cal c2 = add;// delegate inference
            c(10);
            c2(10);

            // How to make a field Immutable example.
            Immutable imm = new Immutable("Bharath", "Yedla");
            Console.WriteLine(imm.Name);
            Console.WriteLine(imm.Surname);
            // imm.Surname = "jnosvm"; // You cannot set the values externally because the fields are immutable or readonly and if you comment out this line you get error over here.

            // delegate is representative between two things which helps us to callback and do the data communication between them.
            myprogram my = new myprogram();
            my.longrun(Mydelegate);
            Console.ReadKey();
        }
        public static void Mydelegate(int i)
        {
            Console.WriteLine(i);
        }

    }
    class myprogram
    {
        public delegate void mydelegate(int i);
        public void longrun(mydelegate my)
        {
            for (int i = 0; i < 10000; i++)
            {
                my(i);
            }
        }
    }
    class Immutable
    {
        // immutables are once loaded they cannot be changed internally or externally.
        private readonly string name;// step 2: make the varible readonly mean we can get the value at the runtime but we cannot set the value to the field.
        private readonly string surname;
        public Immutable(string names, string surnames)// step 1: use constructors to initilize varible.
        {
            name = names;
            surname = surnames;
        }
        public string Name
        {
            get { return name; }// step 3: only getters and remove setters, so we cannot set values to the variables.
        }

        public string Surname
        {
            get { return surname; }

        }
    }

    public delegate void mydel(int x);
    class mydelclass
    {
        public void mymethod()
        {
            Console.WriteLine("Enter Any Value");
            int i = int.Parse(Console.ReadLine());
            mydel t = new mydel(square);
            mydel t1 = new mydel(cube);
            t(i);
            t1(i);
            muticastdeligate obj = new muticastdeligate();
            obj.multicastevent += user1.Xhandler;
            obj.multicastevent += user2.Yhandler;
            obj.Notify(i);
            Console.ReadLine();

        }
        static void square(int i)
        {
            Console.WriteLine(i * i);
        }
        static void cube(int i)
        {
            Console.WriteLine(i * i * i);
        }
    }
    class muticastdeligate
    {
        // muticastdelegate which is nothing but a delegate having multiple handlers or methods assigned to it is known as muticast delegates.

        // delegates enables a publisher subscriber pattern where delegate object is a publisher( which is where event is raised) and 
        // targeted methods are subscriber( which is that Notifiaction by the users action.)

        // An example of Youtube channel - 
        // there is channel called pub and it has 1000 subscribers when ever a video is posted by pub channel it is notified to the 1000 subscriber,
        // and one subscriber cannot interfier and make change or override the other subscriber so we need to declare the delegate as an event.
        // Events formalize this publisher subscriber pattern.
        // so, an event is an encapsulation over delegates which means it prevents other sources to change them.
        // just like properties(getter & setter) enacapsulate private fields events encapsulate delegates.
        // we need our delegates to fire automatically when something changes so we use events.

        public event mydel multicastevent;
        public void Notify(int x)
        {
            if (multicastevent != null)
            {
                multicastevent(x);
            }
        }
    }
    class user1// user1 and user2 are handlers of multicast delegate. 
    {
        public static void Xhandler(int x)
        {
            Console.WriteLine("Event Recieved by user1 object");

        }
    }
    class user2
    {
        public static void Yhandler(int x)
        {
            Console.WriteLine("Event Recieved by user2 object");

        }
    }
    // AutoResetEvent class is actually helps you to achieve synchronization of threads by using the singnaling methodology.
    class AutoRest
    {
        static AutoResetEvent objauto = new AutoResetEvent(false);
        public void mainthread()
        {
            new Thread(auto).Start(); // it will invoke auto method in a different thread.
            Console.ReadLine();
            objauto.Set();// signaled to start again Release waitone.
            // For Every WaitOne there has to be a Set to Release it other wise the thread will be in wait section.
        }
        public void auto()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i == 5)
                {
                    objauto.WaitOne(); // wait the thread will wait when i =5 and when Set method started again then they will be automatically reset.
                }
                Console.WriteLine(i);
            }
        }
        // When we use autoResetEvent for every waitOne you have to call a Set But
        // When we use a ManualResetEvent how waitone you have if you have one Set it will Release all the WaitOne Threads to run.
        // Example AutoResetEvent is like a Turn Style Gate Which only one person can enter at a time.
        // But ManualResetEvent is like a noramel gate once it is opened every one can rush into it.

    }
    class Iemunc
    {
        public void Enum()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            IEnumerable<int> ienum = (IEnumerable<int>)list; // IEnumerable gives you iteration but it does'nt remember the state or row of the collection it internally uses foreach loop.
            Enum1(ienum);
            //IEnumerator<int> ienum = list.GetEnumerator(); // IEnumerator gives you iteration and it knows at which row or state it is currently looping.
            //Enum1(ienum);
            Console.ReadLine();
            
        }
        public void Enum1(IEnumerable<int> o)
        {
            foreach (int i in o)
            {
                if (i >= 3)
                {
                    Enum2(o);
                }
                Console.WriteLine(i);
            }

        }
        public void Enum2(IEnumerable<int> o)
        { 
            foreach (int j in o)
            {
                Console.WriteLine(j);
            }
        }
        /*public void Enum1(IEnumerator<int> o)
        {
            while(o.MoveNext())
            {
                Console.WriteLine(o.Current.ToString());
                if (Convert.ToInt32(o.Current) > 3)
                {
                    Enum2(o);
                }
            }
        }
        public void Enum2(IEnumerator<int> o)
        {
            while(o.MoveNext())
            {
                Console.WriteLine(o.Current.ToString());
            }           
        }*/

    }
}
