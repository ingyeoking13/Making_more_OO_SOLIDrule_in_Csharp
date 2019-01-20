namespace _1_분기문을피하자3.solution
{
    public class Evening : ITimeState
    {
        public ITimeState SetTimeMorning() => new Morning();
        public ITimeState SetTimeEvening() => this;
        public ITimeState SetTimeNight() => new Night();
        public ITimeState TimeProceed() => new Night();
        void ITimeState.eating()
        {
            //Do EveningEating();
        }
    }
}