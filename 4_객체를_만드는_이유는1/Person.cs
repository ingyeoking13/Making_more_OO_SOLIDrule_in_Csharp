using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_객체를_만드는_이유는1
{
    public class Person
    {
        public string Name { get; }

        public Person(string name) {
            Name = name;
        }

        public virtual string description { get => "사람"; }
        public string Introduce()=> "이름이 " + Name + "이며, "  + description + "입니다";
    }
}
