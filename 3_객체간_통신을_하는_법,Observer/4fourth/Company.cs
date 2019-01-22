using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.fourth
{
    public class Company : ISubject
    {
        public List<IObserver> subscriber = new List<IObserver>();

        public string Name { get; }
        public int Salary { get; }

        public Company(string name, int salary)
        {
            Name = name;
            Salary = salary;
        }

        public void register(IObserver p)
        {
            subscriber.Add(p);
        }

        public void NotifyNewCrewWanted()
        {
            Console.WriteLine($"*** {Name} company Notify that they want New Crew.");
            notify(Salary);
            Console.WriteLine();
        }

        public void notify(int salary)
        {
            foreach (var p in subscriber) p.update(salary);
        }
    }
}
