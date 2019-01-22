using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.second
{
    public class Company
    {
        public List<IObserver> subscriber = new List<IObserver>();

        public string Name { get; }

        public Company(string name)
        {
            Name = name;
        }

        public void register(IObserver p)
        {
            subscriber.Add(p);
        }

        public void NotifyNewCrewWanted()
        {

            Console.WriteLine($"{Name} company Notify that they want New Crew.");
            foreach (var p in subscriber)
            {
                p.update();
            }
        }
    }
}
