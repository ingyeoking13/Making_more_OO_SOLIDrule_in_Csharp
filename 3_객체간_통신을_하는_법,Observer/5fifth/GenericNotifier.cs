using System.Collections.Generic;

namespace _3_객체간_통신을_하는_법_Observer.fifth
{
    public class GenericNotifier<T>
    {
        private List<IObserver<T>> notifyList;

        public GenericNotifier(IObserver<T>[] observer)
        {
            notifyList = new List<IObserver<T>>(observer);
        }

        public void notify(object sender, T data)
        {
            foreach (var o in notifyList) o.getNotified(sender, data);
        }
    }
}
