namespace _3_객체간_통신을_하는_법_Observer._5fifth
{
    public class ObserverDelegate<T> : fifth.IObserver<T>
    {
        public delegate void observerDelegate(object sender ,T data);
        observerDelegate @delegate;

        public ObserverDelegate(observerDelegate method)
        {
             this.@delegate= method;
        }

        public void getNotified(object sender, T data)
        {
            @delegate?.Invoke(sender, data);
        }
    }
}
