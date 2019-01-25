using _3_객체간_통신을_하는_법_Observer._5fifth;
using System;

namespace _3_객체간_통신을_하는_법_Observer.fifth
{
    public class Person 
    {
        public ObserverDelegate<int> personGetNotifiedFromCompany;
        public ObserverDelegate<Tuple< string, string, string, int>> personGetNotifiedFromHeadHunter;
        public string Name { get; }

        public Person(string name)
        {
            Name = name;
            personGetNotifiedFromCompany = new ObserverDelegate<int>
                ( (s, d) => this.personGetNotifiedFromCompanyHandler(s, d) );

            personGetNotifiedFromHeadHunter = new ObserverDelegate<Tuple<string, string, string, int>>
                ( (s,d) => this.personGetNotifiedFromHeadHunterHandler(s, d) );
        }

        public void personGetNotifiedFromCompanyHandler(object sender, int newPersonWage)
        {
            Console.WriteLine($"I'm {Name}. I reveived message from {(sender as Company).Name}. {newPersonWage} KRW Job");
        }

        public void personGetNotifiedFromHeadHunterHandler(object sender, Tuple<string , string , string , int> data )
        {
            Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {data.Item1} company");
            Console.WriteLine($"=={data.Item1}  : {data.Item2}, {data.Item3} {data.Item4} KRW job==");
        }
    }
}
