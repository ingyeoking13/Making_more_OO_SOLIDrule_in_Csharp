namespace _4_객체를_만드는_이유2.Third
{
    public interface IClassRoomUser
    {
        void Join();
        void Exit();
    }

    public abstract class Student : IClassRoomUser
    {
        public void Join() { }
        public void Exit() { }
    }

    public class OrdinaryStudent : Student { }
    public class InternationalStudent : Student { }

    public abstract class Teacher : IClassRoomUser
    {
        public void Join() {}
        public void Exit() {}
    }

    public class OrdinaryTeacher : Teacher { }
    public class InternationalTeacher : Teacher { }

    public abstract class Parent : IClassRoomUser
    {
        public void Join() {}
        public void Exit() {}
    }

    public class OrdinaryParent : Parent { }
    public class InternationalParent : Parent { }
}
