namespace _4_decoratorPattern
{
    public class Person
    {
        public string Name { get; }

        public Person(string name) {
            Name = name;
        }

        public virtual string description { get => "사람"; }
        public string Introduce()=> "이름이 " + Name + "이며, "  + description + "입니다";
    }
}
