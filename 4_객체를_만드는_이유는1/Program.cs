using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_객체를_만드는_이유는1
{
    class Program
    {
        static void Main(string[] args)
        { 
            Person kimchi = new Person("김치");
            Console.WriteLine(kimchi.Introduce());

            kimchi = new Student(kimchi);
            Console.WriteLine(kimchi.Introduce());

            kimchi = new Programming(kimchi);
            Console.WriteLine(kimchi.Introduce());

            kimchi = new Freeter(kimchi);
            Console.WriteLine(kimchi.Introduce());

            Person yohan = new Freeter(new Student(new Programming(new Person("요한"))));
            Console.WriteLine(yohan.Introduce());

        }
    }
}
