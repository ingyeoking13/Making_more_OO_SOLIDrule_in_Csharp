using System;

namespace _4_decoratorPattern
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
