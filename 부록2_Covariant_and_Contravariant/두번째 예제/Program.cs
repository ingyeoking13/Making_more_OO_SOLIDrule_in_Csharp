using System;

namespace 부록2_Covariant_and_Contravariant.Second
{
    class Program
    {
        /* delegate/EventHandler Contravariant : Params Type */
        private static void Timer_delegateHandler(object sender, Student e)=> e.age++; // naive
        private static void Timer_delegateHandler2(object sender, Person e)=> e.age++; // Contravariant

        /* action Contravariant : Params Type */
        private static void Timer_actionHandler(Student obj) => obj.age++; // naive
        private static void Timer_actionHandler2(Person obj) => obj.age++; // Contravariant

        /* Func Covariant : Return Type */
        private static Person Timer_funcHandler(Student arg) // naive
        {
            arg.age++;
            Person ret = new Person();
            ret.age = arg.age;
            return ret;
        }

        private static Student Timer_funcHandler2(Person arg) {  // return : Covariant ,  params : contravariant  
            // return Type은 Person, params Type은 Student 여야하지만 역전되어도 Func에 붙일 수 있다.  
            arg.age++;
            Student ret = new Student();
            ret.age = arg.age;
            return ret;
        }
        static void Main(string[] args)
        {
            Time<Student> timer = new Time<Student>();
            Student kimchi = new Student();

            /* delegate/EventHandler Contravariant : Params Type */
            kimchi.age = 13;
            timer.delegateHandler += Timer_delegateHandler; 
            timer.delegateHandler += Timer_delegateHandler2; // Contravariant : Params Type  
            timer.delegateHandler.Invoke(null, kimchi);
            Console.WriteLine(kimchi.age);

            /* action Contravariant : Params Type */
            kimchi.age = 13;
            timer.actionHandler += Timer_actionHandler;
            timer.actionHandler += Timer_actionHandler2; // Contravariant : Params Type 
            timer.actionHandler.Invoke(kimchi);

            Console.WriteLine(kimchi.age);

            /* Func Covariant : Return Type */
            kimchi.age = 13;
            timer.funcHandler += Timer_funcHandler; 
            timer.funcHandler += Timer_funcHandler2; // Covariant : Return Type and Covariant : Params Type
            timer.funcHandler.Invoke(kimchi); // Func은 return value를 받을 수 있지만.. 생략하겠다.  

            Console.WriteLine(kimchi.age);  
        }
    } 
}
