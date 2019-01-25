using System;

namespace _3_객체간_통신을_하는_법_Observer.fifth
{
    public class Company 
    {
        public GenericNotifier<int> person;
        public GenericNotifier<Tuple<string, string, int>> HeadHunter; 

        public string Name { get; }

        public Company(string name)
        {
            Name = name;
        }

        public void NotifyNewCrewWanted()
        {
            Console.WriteLine($"*** {Name} company Notify that they want New Crew.");
            companyNotifyObserver();
            Console.WriteLine();
        }

        public void companyNotifyObserver()
        {
            person?.notify(this, (int)1e6);
            HeadHunter?.notify(this, Tuple.Create("IT", "CTO", (int)2e9));
        }
    }
}
