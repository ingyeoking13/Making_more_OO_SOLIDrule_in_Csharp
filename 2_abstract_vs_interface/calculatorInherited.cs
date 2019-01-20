using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_abstract_vs_interface
{
    public abstract class Igwacalculatorabstract
    {
        public abstract void getscore();
        protected readonly List<IRule> rules;
        public Igwacalculatorabstract()
        {
            rules = new List<IRule>();
            rules.Add(new koreanRule());
            rules.Add(new englishRule());
            rules.Add(new koreanRule());
            rules.Add(new korean_mathRule());
        }

    }
}
