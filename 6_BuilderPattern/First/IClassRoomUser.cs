using System;

namespace _5_AbstractFactoryAdvanced
{
    public interface IClassRoomUser
    {
        void Join();
        void Exit();
        void SetIdentity(IUserIdentity userIdentity);
    }

    public abstract class Student : IClassRoomUser
    {
        public void Join() { }
        public void Exit() { }

        public abstract void SetIdentity(IUserIdentity userIdentity);
    }

    public class OrdinaryStudent : Student
    {
        public override void SetIdentity(IUserIdentity userIdentity)
        {
            OrdinaryIdCard idCard = userIdentity as OrdinaryIdCard;
            if (idCard == null) throw new System.Exception();
            Console.WriteLine(  "Id Card set To " + idCard.DomesticID);
        }
    }
    public class InternationalStudent : Student
    {
        public override void SetIdentity(IUserIdentity userIdentity)
        {
            InternationalIdCard idCard = userIdentity as InternationalIdCard;
            if (idCard == null) throw new System.Exception();
            Console.WriteLine( "Id Card set To " +  idCard.InternationalID);
        }
    }
}
