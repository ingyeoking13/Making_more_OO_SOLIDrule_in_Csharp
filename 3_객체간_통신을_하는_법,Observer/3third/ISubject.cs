using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_객체간_통신을_하는_법_Observer.third
{
    public interface ISubject
    {
        void register(IObserver o);
        void notify();
    }
}
