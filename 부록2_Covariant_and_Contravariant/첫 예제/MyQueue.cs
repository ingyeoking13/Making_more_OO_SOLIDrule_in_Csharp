using System.Collections;

namespace 부록2_Covariant_and_Contravariant
{
    public class MyQueue<T> : QueueInterface<T>
    {
        static private int Size = 20;
        private int head = 0;
        private int tail = 0;
        private T[] q = new T[Size];
        public T Pop()
        {
            return q[head++];
        }
        public void Add(T data)
        {
            q[tail++] = data;
        }

        public IEnumerator GetEnumerator()
        {
            return q.GetEnumerator();
        }
    }
}
