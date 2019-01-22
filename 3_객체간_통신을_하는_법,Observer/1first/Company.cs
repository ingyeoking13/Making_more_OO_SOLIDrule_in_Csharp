using System;
using System.Collections.Generic;

namespace _3_객체간_통신을_하는_법_Observer.first
{
    public class Company
    {
        public List<Person> subscriber = new List<Person>();

        public string Name { get; }

        public Company(string name)
        {
            Name = name;
        }

        public void register(Person p)
        {
            subscriber.Add(p);
        }

        public void NotifyNewCrewWanted()
        {

            Console.WriteLine($"{Name} company Notify that they want New Crew.");
            foreach (var p in subscriber)
            {
                p.apply();
            }
        }
    }
}
