using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_abstract_vs_interface
{
    public enum StudentDesired
    {
        korean, math, english,
        korean_math, math_english, english_korean
    };


    public class Student
    {
        public int korean { get; set; }
        public int math { get; set; }
        public int english { get; set; }
        public StudentDesired studentDesired { get; set; }
    }
}
