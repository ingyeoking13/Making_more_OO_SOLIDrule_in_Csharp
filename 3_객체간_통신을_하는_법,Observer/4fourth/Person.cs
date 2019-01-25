using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.fourth
{
    public class Person : IObserver
    {
        public string Name { get; }
        public Person(string name)
        {
            Name = name;
        }

        public void personGetNotifiedFromCompany(object sender, int newPersonWage)
        {
            Console.WriteLine($"I'm {Name}. I reveived message from {(sender as Company).Name}. {newPersonWage} KRW Job");
        }

        public void personGetNotifiedFromHeadHunter(object sender, string companyName, string Department, string Level, int experiecedWage)
        {
            Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {companyName} company");
            Console.WriteLine($"=={companyName}  : {Department}, {Level} {experiecedWage} KRW job==");
        }

        public void headhunterGetNotifiedFromCompany(object sender, string Department, string Level, int experiencedWage) { }//empty
    }
}
