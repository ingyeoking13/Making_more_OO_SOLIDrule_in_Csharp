using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_객체를_만드는_이유는1
{
    public class Student: Person
    {
        public Person A { get; }
        public Student(Person a) : base(a.Name)
        {
            A = a;
        }
        public override string description => "학생인 " + A.description;
    }
}
