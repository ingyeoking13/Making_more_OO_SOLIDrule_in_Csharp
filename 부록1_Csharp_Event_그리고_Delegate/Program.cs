using System;

namespace 부록1_Csharp_Event_그리고_Delegate
{
    public class Program
    {
        public delegate void HelloWorldDelegate(string helloWorld);
        public delegate void GoodByeWorldDelegate(string GoodByeWorld);

        static void Main(string[] args)
        {
            Person yohan = new Person();
            //HelloWorldDelegate helloWorld = new HelloWorldDelegate(yohan.Speak);
            //helloWorld("helloWorld");

            //GoodByeWorldDelegate goodByeWorld = new GoodByeWorldDelegate(yohan.Speak);
            //goodByeWorld = new GoodByeWorldDelegate(yohan.Speak);
            //goodByeWorld("GoodByeWorld");

            yohan.Name = "Yohan"; // set

            yohan.OnNameChanged += (s,e) => { Console.WriteLine($"Name change from {e.CurrentName} to {e.NewName}"); };

            yohan.Name = "Kimchi"; // first change

            yohan.OnNameChanged += (s, e) => { Console.WriteLine($"hmmm, Do Not change more"); };
            yohan.Name = "gaelim"; // second change


        }
    }
}
