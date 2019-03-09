namespace 부록2_Covariant_and_Contravariant
{
    public class Person
    {
        public string name;
        public Person(string name)
        {

        }
    }
    public class Student : Person
    {
        public Student(string name) : base(name)
        {
        }
    }
}
