using System;
using System.Collections.Generic;

namespace _4_객체를_만드는_이유2.Second
{
    class Program
    {
        static void Main(string[] args)
        {

            doJoinAndExit("teacher");
            doJoinAndExit("student");
        }

        static void doJoinAndExit(string args)
        {
            IClassRoomUser person = 
                ClassRoomUserFactory.createClassRoomUser(args);

            person.Join();
            person.Exit();
        }

        static void doJoinAndStay(string args)
        {

            IClassRoomUser person = 
                ClassRoomUserFactory.createClassRoomUser(args);

            person.Join();
        }

        static void doExit(string args)
        {
            IClassRoomUser person = 
                ClassRoomUserFactory.createClassRoomUser(args);

            person.Exit();
        }
    }
}
