using System;
using System.Collections.Generic;
using System.Threading;
/// <summary>
/// 클래스의 목적 
/// 1 프로세스가 살아 있는 동안 모든 이벤트를 관리하는 이벤트 센터 생성   
/// 2 기존 EventCenter의 "String", "Dictionary <string, dynamic>" 보다 나은 유지 경험을 제공해주는 엄격 Type의 Event Center  
/// 3 Publisher(post)가 Subscriber(register)보다 오래 살경우, 반드시 Unsubscribe을 해줘야하는 Event Pattern 보다 약한 이벤트 패턴인 Weak Event Pattern 구현
/// 4 Subscriber의 동작에 쓰레드 옵션 ( 동작 우선순위 ) 를 제공 
/// reference  
/// Prism -Event Aggregator 
/// </summary>

namespace PageBuilder.Data.WeakEventCenter
{
    /// <summary>
    /// 어느 쓰레드가 해당 Subscriber를 호출할 지 결정
    /// </summary>
    public enum ThreadOption
    { 
        /// <summary>
        /// Publisher가 존재하는 쓰레드에서 호출
        /// </summary>
        PublisherThread,
        /// <summary>
        /// UI thread에서 호출
        /// </summary>
        UIThread,
        /// <summary>
        /// BackGround에서 호출
        /// </summary>
        BackgroundThread
    }
}

namespace PageBuilder.Infrastructure.Event
{
    //public interface IEventAggregator
    //{
    //    TEventType GetEvent<TEventType>() where TEventType : EventBase, new();
    //}
    public static class EventAggregator //: IEventAggregator
    {
        private static readonly Dictionary<Type, EventBase> events = new Dictionary<Type, EventBase>();
        private static readonly SynchronizationContext syncContext = SynchronizationContext.Current;

        /// <summary>
        /// EventAggregator에 의해 관리되는 이벤트를 가져옵니다. 
        /// </summary>
        /// <typeparam name="TEventType"><see cref="EventBase"/>를 상속받는 이벤트 타입을 전달합니다.</typeparam>
        /// <returns><typeparamref name="TEventType"/>의 형식의 이벤트 개체의 싱글턴 개체를 반환합니다.</returns>
        public static TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            lock (events)
            {
                EventBase existingEvent = null;
                if (!events.TryGetValue(typeof(TEventType), out existingEvent))
                {
                    TEventType newEvent = new TEventType();
                    newEvent.SynchronizationContext = syncContext;
                    events[typeof(TEventType)] = newEvent;
                    return newEvent;
                }
                else return (TEventType)existingEvent;
            }
        }
    }
}
