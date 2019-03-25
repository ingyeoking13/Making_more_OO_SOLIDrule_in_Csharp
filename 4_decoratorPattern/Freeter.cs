namespace _4_decoratorPattern
{
    public class Freeter : Person
    {
        public Person A { get; }
        public Freeter(Person a) : base(a.Name)
        {
            A = a;
        }
        public override string description => "프리터이고 " + A.description;
    }
}
