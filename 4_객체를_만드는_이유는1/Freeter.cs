using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_객체를_만드는_이유는1
{
    public class Freeter : Person
    {
        public Person A { get; }
        public Freeter(Person a) : base(a.Name)
        {
            A = a;
        }
        public override string description => "프리터이고 " + A.description;
    }
}
