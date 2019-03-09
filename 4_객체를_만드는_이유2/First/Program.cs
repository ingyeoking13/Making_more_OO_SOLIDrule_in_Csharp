/*
using System;

namespace _4_객체를_만드는_이유2.First
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
            IClassRoomUser person;
            if (args == "teacher") person = new Teacher();
            else if (args == "student") person = new Student();
            else throw new Exception();

            person.Join();

            person.Exit();
        }

        static void doJoinAndStay(string args)
        {
            IClassRoomUser person;
            if (args == "teacher") person = new Teacher();
            else if (args == "student") person = new Student();
            else throw new Exception();

            person.Join();
        }

        static void doExit(string args)
        {
            IClassRoomUser person;
            if (args == "teacher") person = new Teacher();
            else if (args == "student") person = new Student();
            else throw new Exception();

            person.Exit();
        }
    }
}
*/
