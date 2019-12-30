using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PageBuilder.Infrastructure.Event
{

    /// <summary>
    /// 이벤트를 publish, subscribe하기 위한 기본 클래스
    /// </summary>
    public abstract class EventBase
    {
        private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();

        /// <summary>
        /// UI Thread Dispatching 
        /// </summary>
        public SynchronizationContext SynchronizationContext { get; set; }

        /// <summary>
        /// 현재 모든 구독(subscription) 리스트를 들고옴
        /// </summary>
        protected ICollection<IEventSubscription> Subscriptions {  get { return _subscriptions; } }

        /// <summary>
        /// 특정 <see cref="IEventSubscription"/>을 구독 리스트에 추가합니다.
        /// </summary>
        /// <param name="eventSubscription">구독자</param>
        /// <returns>모든 구독자가 고유하게 가지고 있는 <see cref="SubscriptionToken"/>을 반환합니다.</returns>
        /// <remarks>구독을 내부 리스트에 저장하고, 새로운 <see cref="SubscriptionToken"/>을 할당 합니다.</remarks>
        protected virtual SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
        {
            if (eventSubscription == null) throw new ArgumentException(nameof(eventSubscription));
            lock (Subscriptions)
            {
                Subscriptions.Add(eventSubscription);
            }
            return eventSubscription.subscriptionToken;
        }

        /// <summary>
        /// <see cref="IEventSubscription"/> 리스트에서 제공되는 모든 실행 전략을 호출합니다. 
        /// </summary>
        /// <param name="arguments">인자는 리스너에 제공됩니다.</param>
        /// <remarks>실행 전략을 수행하기 전에, 이 클래스는 리스트에서 모든 구독자들을 가지치기 합니다. 가지치기를 당하는 대상은
        /// <see cref="IEventSubscription.GetExecutionStrategy"/>메서드를 수행하였을 때 
        /// <see langword="null"/> <see cref="Action{T}"/>를 반환하는 것들입니다. </remarks>
        protected virtual void InternalPublish(params object[] arguments)
        {
            List<Action<object[]>> executionStrategies = PruneAndReturnStrategies();
            foreach (var ex in executionStrategies)
            {
                ex(arguments);
            }
        }

        /// <summary>
        /// <see cref="SubscriptionToken"/>이 매칭하는 구독자를 제거합니다. 
        /// </summary>
        /// <param name="token">이벤트를 구독할 때, <see cref="EventBase"/>이 반환하는 <see cref="SubscriptionToken"/> 입니다.</param>
        public virtual void Unsubscribe(SubscriptionToken token)
        {
            lock(Subscriptions)
            {
                IEventSubscription subscription = Subscriptions.FirstOrDefault(evt => evt.subscriptionToken == token);
                if (subscription != null)
                    Subscriptions.Remove(subscription);
            }
        }

        /// <summary>
        /// <see cref="SubscriptionToken"/>이 매칭하는 구독자가 존재하는지 여부를 판단합니다. 
        /// </summary>
        /// <param name="token">이벤트를 구독할 때, <see cref="EventBase"/>이 반환하는 <see cref="SubscriptionToken"/> 입니다.</param>
        /// <returns></returns>
        public virtual bool Contains(SubscriptionToken token)
        {
            lock (Subscriptions)
            {
                IEventSubscription subscription = Subscriptions.FirstOrDefault(evt => evt.subscriptionToken == token);
                return subscription != null;
            }
        }

        private List<Action<object[]>> PruneAndReturnStrategies()
        {
            List<Action<object[]>> res = new List<Action<object[]>>();

            lock(Subscriptions)
            {
                for (int i=0; i<Subscriptions.Count; )
                {
                    Action<object[]> listItem = _subscriptions[i].GetExecutionStrategy();

                    if (listItem == null) _subscriptions.RemoveAt(i);
                    else
                    {
                        res.Add(listItem);
                        i++;
                    }
                }
            }
            return res;
        }

        public void Prune()
        {
            lock (Subscriptions)
            {
                for (int i=0; i<Subscriptions.Count;)
                {
                    if (_subscriptions[i].GetExecutionStrategy() == null)
                    {
                        _subscriptions.RemoveAt(i);
                    }
                    else i++;
                }
            }
        }
    }
}
