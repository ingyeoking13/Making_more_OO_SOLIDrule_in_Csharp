namespace _5_AbstractFactoryAdvanced
{
    public partial class Algorithm
    {
        public void problem1()
        {
            AbstractClassRoomUserFactory factory = new OrdinaryClassRoomUserFactory();
            IClassRoomUser student = factory.createStudent();

            IUserIdentity idCard = new OrdinaryIdCard("987231");
            student.SetIdentity(idCard);

            idCard = new InternationalIdCard("1234567");
            student.SetIdentity(idCard); // <- without Compile-time Error
        }
    }
}
