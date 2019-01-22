using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.second
{
    public class HeadHunter : IObserver
    {
        public string Name { get; }
        public HeadHunter(string name)
        {
            Name = name;
        }

        public void update()
        {
            Console.WriteLine($"I'm {Name} HeadHunter. I receive your company notification.");
        }
    }
}
