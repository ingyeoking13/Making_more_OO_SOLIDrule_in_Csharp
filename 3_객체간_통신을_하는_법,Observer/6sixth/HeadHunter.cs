using _3_객체간_통신을_하는_법_Observer._5fifth;
using System;

namespace _3_객체간_통신을_하는_법_Observer.sixth
{
    public class HeadHunter 
    {
        public event EventHandler<Tuple<string, string, string, int>> person;
        public event EventHandler<Tuple<string, string, string, int>> headHunter;

        public string Name { get; }
        public HeadHunter(string name)
        {
            Name = name;
        }

        public void headhunterGetNotifiedFromCompanyHandler(object sender, Tuple<string, string, int> data)
        {
            Console.WriteLine($"I'm {Name} HeadHunter. I received {(sender as Company).Name} notification.");
            headhunterNotifyPerson((sender as Company).Name, data.Item1, data.Item2,data.Item3);
        }

        public void headhunterNotifyPerson(string companyName, string department, string level, int experiencedWage)
        {
            Console.WriteLine($"*** {Name} HeadHunter notify that he has something to you");
            person?.Invoke(this, Tuple.Create(companyName, department, level, experiencedWage));
            Console.WriteLine();
        }

        public void personGetNotifiedFromHeadHunterHandler(object sender, Tuple<string , string , string , int> data )
        {
            Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {data.Item1} company");
            Console.WriteLine($"=={data.Item1}  : {data.Item2}, {data.Item3} {data.Item4} KRW job==");
        }

    }
}
