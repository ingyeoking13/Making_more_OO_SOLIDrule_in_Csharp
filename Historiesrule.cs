using System;
using System.Collections.Generic;

namespace DoUnDoPractice_ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Dojob();
        }

        private void Dojob()
        {

            WorkOrchestra orchestra = new WorkOrchestra();
            while (true)
            {
                Console.WriteLine("====================");
                Console.WriteLine("1. Job1 { argument List<string> }");
                Console.WriteLine("2. Job2 { argument None }");
                Console.WriteLine("3. Job3 { argument MyClass }");
                Console.WriteLine("4. UndoJob ");

                if (orchestra.will_redo_histories.Count > 0)
                {
                    Console.WriteLine("5. RedoJob ");
                }


                Console.WriteLine("====================");
                int num;
                bool fine = int.TryParse( Console.ReadLine(), out num);
                if (fine == false) continue;

                var s = orchestra.passed_histories;
                var p = orchestra.will_redo_histories;

                switch (num)
                {
                    case 1:
                        s.Push(new Work { work_type = 1, Argument = new List<string> { "HI", "Bye" } });
                        p.Clear();
                        break;
                    case 2:
                        s.Push(new Work { work_type = 2, Argument = null });
                        p.Clear();
                        break;
                    case 3:
                        s.Push(new Work
                        {
                            work_type = 3,
                            Argument = new MyClass { age = 13, support = new MyClass.MyChildClass { Todo = "Nothing" } }
                        });
                        p.Clear();
                        break;
                    case 4:
                        Work passed = s.Peek();
                        s.Pop();
                        p.Push(passed);

                        Console.Write(" Remove \t");
                        passed.ShowJob();
                        break;
                    case 5:
                        if (p.Count == 0) break;

                        Work redoWork = p.Peek();
                        p.Pop();
                        s.Push(redoWork);

                        Console.Write(" Recover \t");
                        redoWork.ShowJob();

                        break;

                    default:
                        Console.WriteLine(" Remained in passed-Histories ");
                        foreach( var w in s)
                            Console.Write(w.ShowJob());

                        Console.WriteLine("\n==============================\n");

                        Console.WriteLine(" Remained in next-Histories ");
                        foreach( var w in p)
                            Console.Write(w.ShowJob());

                        Console.WriteLine("\n==============================\n");

                        break;
                }
                Console.WriteLine("\n==============================\n");
            }
        }
    }

    class Work
    {
        public int work_type { get; set; }
        public object Argument { get; set; }
        public string ShowJob ()
        {
            string res = string.Empty;
            res += $"---> {work_type}   ";

            if (Argument == null) ;
            else if (Argument.GetType() == typeof(List<string>))
            {
                List<string> l = (List<string>)Argument;
                foreach (var i in l)
                {
                    res += $"{i} ";
                }
            }
            else if (Argument.GetType() == typeof(MyClass))
            {
                res += $"{((MyClass)Argument).age}";
            }
            return res + "\n";
        }
    }

    class MyClass
    {
        public int age { get; set; }
        public class MyChildClass
        {
            public string Todo { get; set; }
        }
        public MyChildClass support { get; set; }
    }

    class WorkOrchestra
    {
        public Stack<Work> passed_histories { get; set; }
        public Stack<Work> will_redo_histories { get; set; }
        public WorkOrchestra()
        {
            passed_histories = new Stack<Work>();
            will_redo_histories = new Stack<Work>();
        }
    }
}
