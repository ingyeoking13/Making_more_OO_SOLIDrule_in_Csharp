namespace _4_객체를_만드는_이유2.Fourth
{
    public class Algorithm
    {
        private readonly AbstractClassRoomUserFactory factory;
        public Algorithm(AbstractClassRoomUserFactory factory)
        {
            this.factory = factory;
        }

        public void doJoinAndExit(string arg)
        {
            IClassRoomUser person = factory.createClassRoomUser(arg);
            person.Join();
            person.Exit();
        }

        public void doJoinAndStay(string arg)
        {
            IClassRoomUser person = factory.createClassRoomUser(arg);
            person.Join();
        }

        public void doExit(string arg)
        {
            IClassRoomUser person = factory.createClassRoomUser(arg);
            person.Exit();
        }
    }
}
