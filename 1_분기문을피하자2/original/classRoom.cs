using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_분기문을피하자2
{
    class classRoom
    {
        private readonly List<Student> students;

        public classRoom()
        {
            students = new List<Student>();
        }

        public int getTotalScore()
        {
            int score=0;

            foreach (var s in students)
            {

                if (s.studentDesired == StudentDesired.english) // 만약의 자신의 목표와 같다면
                {
                    score += s.english * 2 + s.math + s.korean; // 해당 점수 가중치
                }
                else if (s.studentDesired == StudentDesired.korean)
                {
                    score += s.korean * 2 + s.math + s.english;
                }
                else score += s.math * 2 + s.english + s.korean;

            }

            return score;
        }

        public int getTotalScore_IGwa()
        {
            int score = 0;

            foreach (var s in students)
            {

                if (s.studentDesired == StudentDesired.english) // 만약의 자신의 목표와 같다면
                {
                    score += s.english * 2 + s.math + s.korean; // 해당 점수 가중치
                }
                else if (s.studentDesired == StudentDesired.korean)
                {
                    score += s.korean * 2 + s.math + s.english;
                }
                //else  if .... added 4개 더 등록
            }

            return score;
        }
    }
}
