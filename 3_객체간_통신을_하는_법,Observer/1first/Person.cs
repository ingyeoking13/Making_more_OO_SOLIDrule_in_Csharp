using System;

namespace _3_객체간_통신을_하는_법_Observer.first
{
    public class Person
    {
        public string Name { get; set; }
        public Person(string name)
        {
            Name = name;
        }
        public void apply()
        {
            Console.WriteLine($"I'm {Name}. I apply your company");
        }
    }
}
