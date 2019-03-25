namespace _4_decoratorPattern
{
    public class Programming : Person
    {
        public Person A { get; }
        public Programming(Person a) : base(a.Name)
        {
            A = a;
        }

        public override string description => "프로그래밍 좋아하는 " + A.description;
    }
}
