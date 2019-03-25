namespace _4_객체를_만드는_이유2.Fourth
{
    class Program
    {
        static void Main(string[] args)
        {

            Algorithm algorithm = new Algorithm(new OrdinaryClassRoomUserFactory());
            algorithm.doJoinAndExit("teacher");
            algorithm.doJoinAndExit("student");
        }
    }
}
