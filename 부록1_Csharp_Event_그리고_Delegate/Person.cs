using System;

namespace 부록1_Csharp_Event_그리고_Delegate
{
    //delegate void NameChangedDelegate(string currentName, string newName);
    public delegate void NameChangedDelegate(object sender, NameChangedEventArgs e);

    public class NameChangedEventArgs : EventArgs
    {
        public string CurrentName { get; }
        public string NewName { get; }

        public NameChangedEventArgs(string currentName, string newName)
        {
            CurrentName = currentName;
            NewName = newName;
        }
    }

    public class Person
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    OnNameChanged?.Invoke(this, new NameChangedEventArgs(_name, value));
                }
                _name = value;
            }
        }

        public NameChangedDelegate OnNameChanged; //<-- delegate 
        //public event NameChangedDelegate OnNameChanged; //<-- fine. event delegate Type
        //public event EventHandler<NameChangedEventArgs> OnNameChanged; // <-- fine. event EventHandler<T>
        //public Action<Person,NameChangedEventArgs> OnNameChanged; // <-- fine. Action<in T,in T>  

        public void Speak(string syntax)
        {
            Console.WriteLine(syntax);
        }
    }
    public class Student : Person { }
}
