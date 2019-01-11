using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_분기문을피하자2.solution
{
    class mGwaCalc : ICalc
    {
        private readonly List<IRule> rules;
        public mGwaCalc()
        {
            rules = new List<IRule>();

            rules.Add(new koreanRule());
            rules.Add(new englishRule());
            rules.Add(new koreanRule());
        }
        public int getScore(Student student)
        {
            return rules.First(rules => rules.isMatch(student)).myScore(student);
        }
    }
}
