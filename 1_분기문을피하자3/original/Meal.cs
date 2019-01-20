namespace _1_분기문을피하자3.original
{
    public class Meal
    {
        bool morning = true;
        bool evening = false;
        bool night = false;

        public void eating()
        {

            if (morning)
            {
                DoMorning();
            }
            else if (evening) DoEvening();
            else DoNight();
        }
        public void DoMorning() { }
        public void DoEvening() { }
        public void DoNight() { }
    }
}
