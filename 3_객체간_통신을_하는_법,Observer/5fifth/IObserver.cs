namespace _3_객체간_통신을_하는_법_Observer.fifth
{
    public interface IObserver<T>
    {
        void getNotified(object sender, T data);
    }
}
