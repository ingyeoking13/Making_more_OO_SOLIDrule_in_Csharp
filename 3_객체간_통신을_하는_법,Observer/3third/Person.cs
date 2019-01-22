using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.third
{
    public class Person : IObserver
    {
        public string Name { get; }
        public Person(string name)
        {
            Name = name;
        }

        public void update()
        {
            Console.WriteLine($"I'm {Name}. I reveived message");
        }
    }
}
