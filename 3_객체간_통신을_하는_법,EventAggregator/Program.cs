using System;
using System.Collections.Generic;

namespace _3_객체간_통신을_하는_법_EventAggregator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            App myApp = new App();
            myApp.OnLoad();
        }
    }

    public class App
    {
        List<Student> students = new List<Student>();
        public void OnLoad()
        {

        }
    }

    public class Student
    {
        public string SayHi() => "My Name is Yohan";
    }

    public class ClassRoom
    {
    }

}
