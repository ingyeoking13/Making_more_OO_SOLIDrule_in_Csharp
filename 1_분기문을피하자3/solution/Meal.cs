namespace _1_분기문을피하자3.solution
{
    public class Meal
    {
        ITimeState nowState { get; set; }

        public Meal(ITimeState nowState)
        {
            this.nowState = nowState;
        }
        public void eating()
        {
            nowState.eating();
        }

        public void TimeProceed()
        {
            nowState = nowState.TimeProceed();
        }

    }
}
