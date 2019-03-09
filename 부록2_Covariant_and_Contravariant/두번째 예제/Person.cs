using System;

namespace 부록2_Covariant_and_Contravariant.Second
{
    public class Person { public int age; }
    public class Student : Person { }

    public class Time<T> where T : Person
    {
        public EventHandler<Student> delegateHandler; // kind of delegate type
        public Action<Student> actionHandler; // kind of delegate type
        public Func<Student, Person> funcHandler; // kind of delegate type 
    }
}
