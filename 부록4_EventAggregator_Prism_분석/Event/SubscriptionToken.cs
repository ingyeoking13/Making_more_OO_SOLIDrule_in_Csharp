using System;

namespace PageBuilder.Infrastructure.Event
{
    /// <summary>
    /// <see cref="EventBase"/>에 구독을 하면 주어지는 구독 토큰입니다. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<-Safe->")]
    public class SubscriptionToken : IEquatable<SubscriptionToken>, IDisposable
    {
        private readonly Guid _token;
        private Action<SubscriptionToken> _unsubscribeAction;

        /// <summary>
        /// 새 <see cref="SubscriptionToken"/> 개체를 초기화합니다. 
        /// </summary>
        /// <param name="unsubscribeAction"></param>
        public SubscriptionToken(Action<SubscriptionToken> unsubscribeAction)
        {
            _unsubscribeAction = unsubscribeAction;
            _token = Guid.NewGuid();
        }

        /// <summary>
        /// <see cref="EventBase"/>에 해당 구독 정보를 지우면서, 구독 토큰을 해제합니다. 
        /// </summary>
        public virtual void Dispose()
        {
            if (_unsubscribeAction != null)
            {
                _unsubscribeAction(this);
                _unsubscribeAction = null;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 현재 개체가 다른 개체와 같은 타입인지 검사합니다. 
        /// </summary>
        /// <param name="other">이 개체와 비교할 개체입니다.</param>
        /// <returns>만약 <paramref name="other"/>와 같다면 <see langword="true"/>를 반환합니다.</returns>
        public bool Equals(SubscriptionToken other)
        {
            if (other == null) return false;
            return Equals(_token, other._token);
        }

        /// <summary>
        /// 해당 <see cref="Object"/>가 현재 <see cref="Object"/>와 같은지 체크합니다. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as SubscriptionToken);
        }
        public override int GetHashCode() => _token.GetHashCode();
    }

}
