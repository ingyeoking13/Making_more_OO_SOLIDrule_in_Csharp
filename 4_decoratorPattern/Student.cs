namespace _4_decoratorPattern
{
    public class Student: Person
    {
        public Person A { get; }
        public Student(Person a) : base(a.Name)
        {
            A = a;
        }
        public override string description => "학생인 " + A.description;
    }
}
