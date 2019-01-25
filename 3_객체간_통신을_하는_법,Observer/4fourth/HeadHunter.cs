using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.fourth
{
    public class HeadHunter : IObserver, ISubject
    {
        List<IObserver> observers = new List<IObserver>();
        public string Name { get; }
        public HeadHunter(string name)
        {
            Name = name;
        }

        public void register(IObserver o)
        {
            observers.Add(o);
        }

        public void headhunterGetNotifiedFromCompany(object sender, string Department, string Level, int experiencedWage)
        {
            Console.WriteLine($"I'm {Name} HeadHunter. I received {(sender as Company).Name} notification.");
            headhunterNotifyPerson((sender as Company).Name, Department, Level, experiencedWage);
        }

        public void headhunterNotifyPerson(string companyName, string department, string level, int experiencedWage)
        {
            Console.WriteLine($"*** {Name} HeadHunter notify that he has something to you");
            foreach (var o in observers) o.personGetNotifiedFromHeadHunter(this, companyName, department, level, experiencedWage);
            Console.WriteLine();
        }

        public void personGetNotifiedFromHeadHunter(object sender, string companyName, string Department, string Level, int experiecedWage)
        {
            Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {companyName} company");
            Console.WriteLine($"=={companyName}  : {Department}, {Level} {experiecedWage} KRW job==");
        }

        public void companyNotifyObserver() { } //empty
        public void personGetNotifiedFromCompany(object sender, int newPersonWage) { } //empty
    }
}
