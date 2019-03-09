namespace _4_객체를_만드는_이유2.Third
{
    public class Algorithm
    {

        public void doJoinAndExit(string arg0, string arg1)
        {
            AbstractClassRoomUserFactory factory = new OrdinaryClassRoomUserFactory();
            if (arg0 == "International") factory = new InternationalClassRoomUserFactory();

            IClassRoomUser person = factory.createClassRoomUser(arg1);
            person.Join();
            person.Exit();
        }

        public void doJoinAndStay(string arg0, string arg1)
        {
            AbstractClassRoomUserFactory factory = new OrdinaryClassRoomUserFactory();
            if (arg0 == "International") factory = new InternationalClassRoomUserFactory();

            IClassRoomUser person = factory.createClassRoomUser(arg1);
            person.Join();
        }

        public void doExit(string arg0, string arg1)
        {
            AbstractClassRoomUserFactory factory = new OrdinaryClassRoomUserFactory();
            if (arg0 == "International") factory = new InternationalClassRoomUserFactory();

            IClassRoomUser person = factory.createClassRoomUser(arg1);

            person.Exit();
        }
    }
}
