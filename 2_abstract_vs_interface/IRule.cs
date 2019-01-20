using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_abstract_vs_interface
{
    public interface IRule
    {
        bool isMatch(Student student);
        int myScore(Student student);
    }

    public class mathRule : IRule
    {
        public bool isMatch(Student student)
        {
            return (student.studentDesired == StudentDesired.math);
        }

        public int myScore(Student student)
        {
            return student.math * 2 + student.korean + student.english;
        }
    }

    public class englishRule : IRule
    {
        public bool isMatch(Student student)
        {
            return (student.studentDesired == StudentDesired.english);
        }

        public int myScore(Student student)
        {
            return student.english * 2 + student.korean + student.math;
        }
    }

    public class koreanRule : IRule
    {
        public bool isMatch(Student student)
        {
            return (student.studentDesired == StudentDesired.korean);
        }

        public int myScore(Student student)
        {
            return student.korean * 2 + student.math + student.english;
        }
    }

    public class korean_mathRule : IRule
    {
        public bool isMatch(Student student)
        {
            return (student.studentDesired == StudentDesired.korean_math);
        }

        public int myScore(Student student)
        {
            return (int)(student.korean*1.5 + student.math*1.5 + student.english);
        }
    }
}
