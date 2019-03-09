using System;

namespace _4_객체를_만드는_이유2.Second
{
    public static class ClassRoomUserFactory
    {
        public static IClassRoomUser createClassRoomUser(string args)
        {
            IClassRoomUser person;
            if (args == "teacher") person = new Teacher();
            else if (args == "student") person = new Parent();
            else if (args == "parent") person = new Student();
            else throw new Exception();
            return person;
        }
    }
}
