using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace 부록3_Delegate_and_weakReference
{
    public class Person
    {
        public string name;
        public int age;
        public Person(string _name, int _age) { name = _name; age = _age; }
        public void sayHello() => Console.WriteLine($"Hi, I'm {name}. I'm {age} old");
    }
    public class ViewCycle
    {
        public delegate void OnChangedDelegate();

        private bool _switch;
        public bool Switch { get { return _switch; } set { _switch = value; if(Switch) onSwitchChange?.Invoke(); } }
        public event OnChangedDelegate onSwitchChange;
    }

    public class DelegateReference
    {
        public Delegate _delegate;
        public WeakReference _weakReference;
        public MethodInfo _method;
        public Type _delegateType;

        public DelegateReference(Delegate _delegate, WeakReference _wr, MethodInfo _method, Type _delegateType)
        {
            this._delegate = _delegate;
            this._weakReference = _wr;
            this._method = _method;
            this._delegateType = _delegateType;
        }
    }

    public class ViewCycle2
    {
        private bool _switch;
        public bool Switch { 
            get { return _switch; } 
            set {
                _switch = value;
                if (_switch)
                {
                    foreach (var i in onSwitchChangeSubscribeList)
                    {
                        if (i._weakReference.Target != null) 
                            i._method.CreateDelegate(i._delegateType, i._weakReference.Target).DynamicInvoke();
                    }
                }
            } 
        }
        public List<DelegateReference> onSwitchChangeSubscribeList = new List<DelegateReference>();
    }

    class App
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ViewCycle vc = new ViewCycle();

            Person a = new Person("yohan", 29);
            Person b = new Person("junsang", 29);

            vc.Switch = true;

            vc.onSwitchChange += a.sayHello;
            vc.onSwitchChange += b.sayHello;

            vc.Switch = true;
            Console.WriteLine("1********************1 - Added a, b class say Hello");
            vc.Switch = false;

            Person c = new Person("seulgi", 29);
            Person d = new Person("minhwan", 29);

            vc.onSwitchChange += c.sayHello;
            vc.onSwitchChange += d.sayHello;

            vc.Switch = true;
            Console.WriteLine("2********************2 - Added c, d class say Hello");

            vc.onSwitchChange -= a.sayHello;
            b = null;
            c = null;
            d = null;

            vc.Switch = false;
            vc.Switch = true;
            Console.WriteLine("3********************3 - subtract a say Hello, and allocate b, c, d object to null");

            vc = new ViewCycle();
            vc.Switch = false;
            vc.Switch = true;

            Console.WriteLine("4********************4 - reallocate to view cycle");


            Console.WriteLine("***Beauty of Weak Reference***");

            // =====  beauty of Weak Reference  ==== 
            ViewCycle2 vc2 = new ViewCycle2();

            ((Action)(()=>
            {
                Person a = new Person("yohan", 29);
                Person b = new Person("junsang", 29);
                Person c = new Person("seulgi", 29);
                Person d = new Person("minhwan", 29);

                Delegate da = (Action)a.sayHello;
                Delegate db = (Action)b.sayHello;
                Delegate dc = (Action)c.sayHello;
                Delegate dd = (Action)d.sayHello;

                var dr = 
                new List<DelegateReference> { 
                    new DelegateReference(null, new WeakReference(da.Target), da.GetMethodInfo(), da.GetType())
                    ,new DelegateReference(null, new WeakReference(db.Target), db.GetMethodInfo(), db.GetType())
                    ,new DelegateReference(null, new WeakReference(dc.Target), dc.GetMethodInfo(), dc.GetType())
                    ,new DelegateReference(null, new WeakReference(dd.Target), dd.GetMethodInfo(), dd.GetType())
                };

                foreach(var item in dr)
                    vc2.onSwitchChangeSubscribeList.Add(item);
            })).Invoke();


            vc2.Switch = true;
            Console.WriteLine("1********************1 - added a, b, c, d method ");
            vc2.Switch = false;

            GC.Collect();

            vc2.Switch = true;
            Console.WriteLine("2********************2 - a, b, c, d  garbage collected ");
        }
    }
}
