using System.Collections.Generic;

namespace _1_분기문을_피하자
{
    public interface Iselector
    {
       IEnumerable<int> Pick(int[] p);
    }

    public class oddRule : Iselector
    {
        public IEnumerable<int> Pick(int[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i] % 2 == 0) continue;
                yield return p[i];
            }
        }
    }

    public class evenRule : Iselector
    {
        public IEnumerable<int> Pick(int[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i] % 2 == 1) continue;
                yield return p[i];
            }
        }
    }
}