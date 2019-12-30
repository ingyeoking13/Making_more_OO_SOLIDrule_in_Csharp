using PageBuilder.Data.WeakEventCenter;
using System;
using System.Linq;

namespace PageBuilder.Infrastructure.Event
{
    /// <summary>
    /// 이벤트 구독, 발송을 관리하는 클래스 
    /// </summary>
    public class PubSubEvent : EventBase
    {
        /// <summary>
        /// <see langword="delegate"/>를 특정 이벤트에 구독시킨다. delegate는 발송자 쓰레드위에서 수행되며, 
        /// <see cref="PubSubEvent"/>는 해당 <paramref name="action"/>의 <see cref="WeakReference"/>를 가진다. 
        /// </summary>
        /// <param name="action">이벤트가 발송될 때 수행할 delegate이다.</param>
        /// <returns>추가된 구독에 주어지는 고유 <see cref="SubscriptionToken"/>을 반환한다.</returns>
        /// <remarks>PubSubEvent 콜렉션은 thread-safe이다.</remarks>
        public SubscriptionToken Subscribe(Action action)
        {
            return Subscribe(action, ThreadOption.PublisherThread);
        }
        public SubscriptionToken Subscribe(Action action, ThreadOption threadOption)
        {
            return Subscribe(action, threadOption, false);
        }
        public virtual SubscriptionToken Subscribe(Action action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
        {
            IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);

            EventSubscription subscription;
            switch (threadOption)
            {
                case ThreadOption.PublisherThread:
                    subscription = new EventSubscription(actionReference);
                    break;
                //case ThreadOption.BackgroundThread:
                //    subscription = new BackgroundEventSubscription(actionReference);
                //    break;
                //case ThreadOption.UIThread:
                //    if (SynchronizationContext == null) throw new InvalidOperationException(Resources.EventAggregatorNotConstructedOnUIThread);
                //    subscription = new DispatcherEventSubscription(actionReference, SynchronizationContext);
                //    break;
                default:
                    subscription = new EventSubscription(actionReference);
                    break;
            }

            return InternalSubscribe(subscription);
        }

        /// <summary>
        /// <see cref="PubSubEvent"/>를 발송합니다. 
        /// </summary>
        public virtual void Publish() => InternalPublish(); 
        /// <summary>
        /// 구독자 리스트에서 가장 먼저 매칭하는 <see cref="Action"/>을 제거합니다. 
        /// </summary>
        /// <param name="subscriber">이벤트를 구독하는데 사용된 <see cref="Action"/>입니다.</param>
        public virtual void Unsubscribe(Action subscriber)
        {
            lock(Subscriptions)
            {
                IEventSubscription eventSubscription = Subscriptions.Cast<EventSubscription>().FirstOrDefault(evt => evt.Action == subscriber);
                if (eventSubscription != null) Subscriptions.Remove(eventSubscription);
            }
        }
        /// <summary>
        /// 구독자 리스트에서 매칭하는 <see cref="Action"/>이 있는지 검사합니다.
        /// </summary>
        /// <param name="subscriber">이벤트를 구독하는데 사용된 <see cref="Action"/>입니다.</param>
        /// <returns>매칭하는 <see cref="Action"/>이 있으면 <see langword="true"/>를 반환합니다.</returns>
        public virtual bool Contains(Action subscriber)
        {
            IEventSubscription eventSubscription;
            lock (Subscriptions)
            {
                eventSubscription = Subscriptions.Cast<EventSubscription>().FirstOrDefault(evt => evt.Action == subscriber);
            }
            return eventSubscription != null;
        }
    }

    /// <summary>
    /// 이벤트 구독, 발송을 관리하는 클래스 
    /// </summary>
    /// <typeparam name="T">구독자가 반드시 전달해야할 데이터 타입입니다.</typeparam>
    public class PubSubEvent<T> : EventBase
    { 
        public SubscriptionToken Subscribe(Action<T> action) => Subscribe(action, ThreadOption.PublisherThread, false, null);
        public SubscriptionToken Subscribe(Action<T> action, ThreadOption publisherThread) => Subscribe(action, publisherThread, false, null);
        public SubscriptionToken Subscribe(Action<T> action, Predicate<T> filter) => Subscribe(action,ThreadOption.PublisherThread, false, filter);
        public SubscriptionToken Subscribe(Action<T> action, ThreadOption threadOption, Predicate<T> filter) => Subscribe(action, threadOption, false, filter);
        public SubscriptionToken Subscribe(Action<T> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive) => Subscribe(action, threadOption, false, null);
        public SubscriptionToken Subscribe(Action<T> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<T> filter)
        {
            IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
            IDelegateReference filterReference;
            if (filter != null)
            {
                filterReference = new DelegateReference(filter, keepSubscriberReferenceAlive);
            }
            else filterReference = new DelegateReference(new Predicate<T>(delegate { return true; }), true);

            EventSubscription<T> subscription;

            switch(threadOption)
            {
                case ThreadOption.PublisherThread:
                    subscription = new EventSubscription<T>(actionReference, filterReference);
                    break;
                //case ThreadOption.BackgroundThread:
                //    subscription = new BackgroundEventSubscription(actionReference);
                //    break;
                //case ThreadOption.UIThread:
                //    if (SynchronizationContext == null) throw new InvalidOperationException(Resources.EventAggregatorNotConstructedOnUIThread);
                //    subscription = new DispatcherEventSubscription(actionReference, SynchronizationContext);
                //    break;
                default:
                    subscription = new EventSubscription<T>(actionReference, filterReference);
                    break;
            }
            return InternalSubscribe(subscription);
        }

        public virtual void  Publish(T args)
        {
            InternalPublish(args);
        }
        public virtual void Unsubscribe(Action<T> subscriber)
        {
            lock (Subscriptions)
            {
                IEventSubscription eventSubscription = Subscriptions.Cast<EventSubscription<T>>().FirstOrDefault(evt => evt.Action == subscriber);
                if (eventSubscription != null)
                {
                    Subscriptions.Remove(eventSubscription);
                }
            }
        }
        public virtual bool Contains(Action<T> subscriber)
        {
            IEventSubscription eventSubscription;
            lock (Subscriptions)
            {
                eventSubscription = Subscriptions.Cast<EventSubscription<T>>().FirstOrDefault(evt => evt.Action == subscriber);
            }
            return eventSubscription != null;
        }
    }
}
