using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_분기문을피하자3.solution
{
    public interface ITimeState
    {
        void eating();
        ITimeState SetTimeMorning();
        ITimeState SetTimeEvening();
        ITimeState SetTimeNight();
        ITimeState TimeProceed();
    }
}
