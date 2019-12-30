using System;
using System.Globalization;

namespace PageBuilder.Infrastructure.Event
{
    /// <summary>
    /// <see cref="EventBase"/>에 의해 사용되는 이벤트 구독 계약을 정의합니다. 
    /// </summary>
    public interface IEventSubscription
    {
        /// <summary>
        /// 이 <see cref="IEventSubscription"/>의 아이디인 <see cref="SubscriptionToken"/>을 get, set 합니다.  
        /// </summary>
        SubscriptionToken subscriptionToken { get; set; }
        /// <summary>
        /// 이 이벤트의 실행 전략을 가져옵니다. 
        /// </summary>
        /// <returns>실행 전략과 함께 <see cref="Action{T}"/>을 반환하거나, <see cref="IEventSubscription"/>이 더이상 유효하지 않다면 <see langword="null"/>을 반환합니다.</returns>
        Action<object[]> GetExecutionStrategy();
    }

    /// <summary>
    /// 액션을 수행하기 위한 <see cref="Delegate"/>을 구하는 방법을 제시합니다.
    /// </summary>
    public class EventSubscription : IEventSubscription
    {
        private readonly IDelegateReference _actionReference;
        public SubscriptionToken subscriptionToken { get; set; }

        /// <summary>
        /// <see cref="EventSubscription"/> 개체를 생성합니다.
        /// </summary>
        /// <returns></returns>
        public EventSubscription(IDelegateReference actionReference)
        {
            if (actionReference == null)
                throw new ArgumentNullException(nameof(actionReference));
            if (!(actionReference.Target is Action))
                throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, "InvalidDelegateRerefenceTypeException",
                    typeof(Action).FullName, nameof(actionReference)));

            this._actionReference = actionReference;
        }

        public Action Action { get { return (Action)_actionReference.Target; } }

        /// <summary>
        /// 이 이벤트를 발송하기 위한 실행 전략을 얻습니다.
        /// </summary>
        /// <returns>실행 전략과 함께 <see cref="System.Action"/>을 반환합니다. 만약 <see cref="IEventSubscription"/>이 유효하지 않다면 <see langword="null"/>을 반환합니다.</returns>
        /// <remarks>
        /// <see cref="Action"/>이 유효하지 않다는 것은, <see cref="_actionReference"/>가 참조하는 개체가 가비지 컬렉터에 의해 수집되었다는 것이며
        /// <see langword="null"/>을 반환합니다. 
        /// </remarks>
        public Action<object[]> GetExecutionStrategy()
        {
            Action action = this.Action;
            if (action != null)
            {
                return arguments =>
                {
                    if (action == null) throw new ArgumentNullException(nameof(action));
                    action();
                };
            }
            return null;
        }
    }

    /// <summary>
    /// 액션을 수행하기 위한 <see cref="Delegate"/>을 구하는 방법을 제시합니다. 
    /// 필터가 존재하는 경우, 필터의 반환 값에 결과가 다릅니다.   
    /// </summary>
    public class EventSubscription<T> : IEventSubscription
    {
        private readonly IDelegateReference _actionReference;
        private readonly IDelegateReference _filterReference;
        public SubscriptionToken subscriptionToken { get; set; }

        /// <summary>
        /// <see cref="IDelegateReference"/>가 참조하는 <see cref="Action{T}"/>를 반환합니다. 
        /// </summary>
        public Action<T> Action { get { return (Action<T>)_actionReference.Target; } }

        /// <summary>
        /// <see cref="IDelegateReference"/>가 참조하는 <see cref="Predicate{T}"/>를 반환합니다. 
        /// </summary>
        public Predicate<T> Filter { get { return (Predicate<T>)_filterReference.Target; } }

        /// <summary>
        /// 새 <see cref="EventSubscription"/>를 생성합니다.  
        /// </summary>
        /// <param name="actionReference"><see cref="Action{T}"/>의 참조형식입니다.</param>
        /// <param name="filterReference"><see cref="Predicate{T}"/>의 참조형식입니다.</param>
        public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
        {
            if (actionReference == null) throw new ArgumentNullException(nameof(actionReference));
            if (!(actionReference.Target is Action<T>)) throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "InvalidDelegateRerefenceTypeException", typeof(Action<T>).FullName, nameof(actionReference)));

            if (actionReference == null) throw new ArgumentNullException(nameof(filterReference));
            if (!(filterReference.Target is Action<T>)) throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "InvalidDelegateRerefenceTypeException", typeof(Action<T>).FullName, nameof(actionReference)));

            _actionReference = actionReference;
            _filterReference = filterReference;
        }

        /// <summary>
        /// 이 이벤트를 발송하기 위한 실행 전략을 호출합니다.
        /// </summary>
        /// <returns>실행 전략과 함께 <see cref="System.Action"/>을 반환합니다. 만약 <see cref="IEventSubscription"/>이 유효하지 않다면 <see langword="null"/>을 반환합니다.</returns>
        /// <remarks>
        /// <see cref="Action"/>이 유효하지 않다는 것은, <see cref="_actionReference"/>가 참조하는 개체가 가비지 컬렉터에 의해 수집되었다는 것이며
        /// <see langword="null"/>을 반환합니다. 그렇지 않다면, <see cref="Filter"/> delegate가 계산될 것이고 만약 <see langword="true"/>를 반환한 경우 
        /// <see cref="Action"/>을 수행합니다. 
        /// </remarks>
        public Action<object[]> GetExecutionStrategy()
        {
            Action<T> action = this.Action;
            Predicate<T> filter = this.Filter;

            if ( action != null && filter != null)
            {
                return arguments =>
                {
                    T argument = default(T);
                    if (arguments != null && arguments.Length > 0 && arguments[0] != null)
                    {
                        argument = (T)arguments[0];
                    }
                    if(filter(argument))
                    {
                        if (action == null) throw new ArgumentNullException(nameof(action));
                        action(argument);
                    }
                };
            }
            return null;
        }
    }
}
