namespace _5_AbstractFactoryAdvanced
{
    public abstract class AbstractClassRoomUserFactory
    {
        public abstract IClassRoomUser createStudent();
    }

    public class OrdinaryClassRoomUserFactory : AbstractClassRoomUserFactory
    {
        public override IClassRoomUser createStudent() => new OrdinaryStudent();
    }

    public class InternationalClassRoomUserFactory : AbstractClassRoomUserFactory
    {
        public override IClassRoomUser createStudent()=> new InternationalStudent();
    }
}
