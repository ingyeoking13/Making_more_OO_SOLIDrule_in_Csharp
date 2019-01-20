using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_abstract_vs_interface
{
    class inheritedClassRoom  : Igwacalculatorabstract
    {
        private readonly List<Student> students;

        public inheritedClassRoom()
        {
            students = new List<Student>();
        }

        public override void getscore()
        {
            int score = 0;
            foreach (var student in students)
            {
                score += 
                rules.First(rules => rules.isMatch(student)).myScore(student);
            }

        }

    }
}
