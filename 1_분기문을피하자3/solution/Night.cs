
namespace _1_분기문을피하자3.solution
{
    public class Night : ITimeState
    {
        public ITimeState SetTimeMorning() => new Morning();
        public ITimeState SetTimeEvening() => new Evening();
        public ITimeState SetTimeNight() => this;
        public ITimeState TimeProceed() => new Morning();
        public void eating()
        {
            //Do NightEating();
        }

    }
}