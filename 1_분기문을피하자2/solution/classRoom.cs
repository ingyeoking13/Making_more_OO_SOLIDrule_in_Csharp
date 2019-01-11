using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_분기문을피하자2.solution
{
    class classRoom
    {
        private readonly List<Student> students;
        private readonly ICalc calc;

        public classRoom(ICalc calcc)
        {
            students = new List<Student>();
            calc = calcc;
        }

        public int getTotalScore()
        {
            int score = 0;
            foreach (var student in students)
            {
                score += calc.getScore(student);
            }
            return score;
        }
    }
}
