namespace _4_객체를_만드는_이유2.Fourth
{
    public abstract class AbstractClassRoomUserFactory
    {
        public abstract IClassRoomUser createClassRoomUser(string args);
    }

    public class OrdinaryClassRoomUserFactory : AbstractClassRoomUserFactory
    {
        public override IClassRoomUser createClassRoomUser(string args)
        {
            IClassRoomUser ret = null;
            if (args == "student") ret= new OrdinaryStudent();
            else if (args == "teacher") ret= new OrdinaryTeacher();
            else if (args == "parent") ret= new OrdinaryParent();
            return ret;
        }
    }

    public class InternationalClassRoomUserFactory : AbstractClassRoomUserFactory
    {
        public override IClassRoomUser createClassRoomUser(string args)
        {
            IClassRoomUser ret = null;
            if (args == "student") ret= new InternationalStudent();
            else if (args == "teacher") ret= new InternationalTeacher();
            else if (args == "parent") ret= new InternationalParent();
            return ret;
        }
    }

}
