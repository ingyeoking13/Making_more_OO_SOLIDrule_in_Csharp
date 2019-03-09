namespace _1_분기문을피하자3.solution
{
    public class Morning : ITimeState
    {
        public ITimeState SetTimeMorning() => this;
        public ITimeState SetTimeEvening() => new Evening();
        public ITimeState SetTimeNight() => new Night();
        public ITimeState TimeProceed() => new Evening();
        public void eating()
        {
            //Do MorningEating();
        }
    }
}
