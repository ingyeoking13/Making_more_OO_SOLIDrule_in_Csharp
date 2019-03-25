namespace _5_AbstractFactoryAdvanced
{
    public interface IUserIdentity
    {
    }
    public class OrdinaryIdCard : IUserIdentity
    {
        public string DomesticID { get; set; }
        public OrdinaryIdCard(string str)
        {
            DomesticID = str;
        }
    }
    public class InternationalIdCard : IUserIdentity
    {
        public string InternationalID { get; set; }
        public InternationalIdCard(string str)
        {
            InternationalID = str;
        }
    }
}
