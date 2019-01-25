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
            Console.WriteLine($"*** {Name} company Notify that they want New Crew.");
            companyNotifyObserver();
            Console.WriteLine();
        }

        public void companyNotifyObserver()
        {
            foreach (var p in subscriber)
            {
                if (p is Person) p.personGetNotifiedFromCompany(this, (int)1e6);
                else p.headhunterGetNotifiedFromCompany(this, "IT", "CTO", (int)2e9);
            }
        }

        public void headhunterNotifyPerson(string companyName, string department, string level, int experiencedWage) { } // empty
    }
}
