using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.third
{
    public class HeadHunter : IObserver, ISubject
    {
        List<IObserver> observers = new List<IObserver>();
        public string Name { get; }
        public HeadHunter(string name)
        {
            Name = name;
        }

        public void update()
        {
            Console.WriteLine($"I'm {Name} HeadHunter. I received your notification.");
        }

        public void notify()
        {
            Console.WriteLine($"*** {Name} HeadHunter notify that he has something to you");
            foreach (var o in observers) o.update();
            Console.WriteLine();
        }

        public void register(IObserver o)
        {
            observers.Add(o);
        }
    }
}
