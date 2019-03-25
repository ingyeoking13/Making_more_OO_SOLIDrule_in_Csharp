using System.Collections.Generic;

namespace _4_객체를_만드는_이유2.Second
{
    public interface IClassRoomUser
    {
        void Join();
        void Exit();
    }

    public class Student : IClassRoomUser
    {
        public void Join() {}
        public void Exit() {}
    }

    public class Teacher : IClassRoomUser
    {
        public void Join() {}
        public void Exit() {}
    }

    public class Parent : IClassRoomUser
    {
        public void Join() {}
        public void Exit() {}
    }
}
