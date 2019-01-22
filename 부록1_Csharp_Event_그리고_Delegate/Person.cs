using System;
using static 부록1_Csharp_Event_그리고_Delegate.Program;

namespace 부록1_Csharp_Event_그리고_Delegate
{
    //public delegate void NameChangedDelegate(string currentName, string newName);
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
            set {
                if (_name != value)
                {
                    OnNameChanged?.Invoke(this, new NameChangedEventArgs(_name, value));
                }
                _name = value;
            }
        }

        //public NameChangedDelegate OnNameChanged;
        public event NameChangedDelegate OnNameChanged;

        public void Speak(string syntax)
        {
            Console.WriteLine(syntax);
        }
    }

    public class Name
    {
    }

}
