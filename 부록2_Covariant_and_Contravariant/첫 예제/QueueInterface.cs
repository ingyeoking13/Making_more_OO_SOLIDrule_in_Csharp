using System.Collections;

namespace 부록2_Covariant_and_Contravariant
{
    public interface QueueInterface<out T> : IEnumerable
    {
    }
}