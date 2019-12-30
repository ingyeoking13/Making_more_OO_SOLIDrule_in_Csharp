# Infrastructure

* EventCenter   
* EventMessage Type  
* EventMessage Factory  


## 0. zero step 
**How To Use**

```c#
//event Define, 만약 인자가 필요하면 
public class myEventWithIntegerList : PubSubEvent<List<int>> {};
EventAggregator.GetEvent<myEvent>().Publish(new List<int>{1,2,3});

public class myEventWithString : PubSubEvent<string> {};
EventAggregator.GetEvent<myEvent>().Publish("HI...?");

//event Define, 만약 인자가 필요없다면
public class yohan : PubSubEvent {} ;
EventAggregator.GetEvent<yohan>().Publish();


EventAggregator.GetEvent<yohan>().Subscribe( (Action)(()=>{ ///....  
}));

```

## 1. first step

**DelegateReference.cs**  

**목적**

기존 이벤트 센터를 이용할 땐, delegate를 사용했다. (`Event`가 내부에 `delegate`를 가지고 있음 ).   
이러한 구조의 문제점은 `EventHandler a += b.listen` 라고 이벤트를 청취하는 식으로 사용하게 되면,  
`b` 개체의 라이프 타임보다 `EventHandler a` 변수의 라이프 타임이 더 길때, b 개체가 가비지 콜렉터에 의해 수집이 되지 않는 다는 점이었다.   

메모리 누수를 방지하는 코드를 작성하기 위해서 항상 `a -= b.listen` 라는 명시적인 코드를 작성해주어야 했다.  

**전략**  

앞으로는 해당 개체가 이벤트 청취를 수행할 때 `delegate`를 직접적으로 가지고 있지 않고, 필요할 때마다 리스너 개체의 해당 메서드의 `delegate`개체를 만들어 사용한다.   
구현법은 `delegate.Target`이 리스너 개체를 반환하는 것에 착안하는데, `delegate`를 베이스로 둔 `_weakReference.Target`을 이용해 접근한다. (`TryGetDelegate` 메서드 참조)   

```c#
public class DelegateReference : IDelegateReference
{
    //...
    private readonly WeakReference _weakReference;
    private readonly MethodInfo _method;
    private readonly Type _delegateType;

    public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
    {
        //....
        {
            _weakReference = new WeakReference(@delegate.Target);
            _method = @delegate.GetMethodInfo();
            _delegateType = @delegate.GetType();
        }
    }

    public Delegate Target
    { get { //...
            return TryGetDelegate(); }

    }

    private Delegate TryGetDelegate()
    {
        if (_method.IsStatic)
        {
            return _method.CreateDelegate(_delegateType, null);
        }
        object target = _weakReference.Target;
        if (target != null)
        {
            return _method.CreateDelegate(_delegateType, target);
        }
        return null;
    }
}
```

## 2. second step

**EventSubscription.cs**  

**목적**   
구독 이벤트를 표현한 클래스이다. **delegateReference**를 직접적으로 가지고 있으며, 수행한다. 

```c#
public class EventSubscription : IEventSubscription
{
    private readonly IDelegateReference _actionReference;
    public SubscriptionToken subscriptionToken { get; set; }

    //...

    public Action Action { get { return (Action)_actionReference.Target; } }
    public Action<object[]> GetExecutionStrategy()
    {
        Action action = this.Action;
        if ( action != null)
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
```

**SubscriptionToken**은 구독정보를 저장하고 있는 토큰 클래스이다. 이 구독 토큰은 `PubSubEvent`에서 생성됨. 그다지 중요하지 않다.  

## 3. second step  

**EventBase.cs**

**목적**  

추상클래스    
구독 정보들(`List<IEventSubscription>`)을 가지고 있다. 그리고 구독 정보들을 더하고, 제거하는 역할을 한다.   
그리고 구독 정보들을 순회하며 리스너들에게 알릴 때 `weakReference.Target==null`이라면, 자신의 구독정보 리스트에서 제거한다.   

```c#
protected virtual void InternalPublish(params object[] arguments)
{
    List<Action<object[]>> executionStrategies = PruneAndReturnStrategies();
    foreach (var ex in executionStrategies)
    {
        ex(arguments);
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
    return res;Fift
}
```

**PubSubEvent.cs**

**목적**

추상클래스 **EventBase**를 상속받는 클래스  

## 4. fourth step

`EventSubscription<T>`, `PubSubEvent<T>` 이벤트에 인자가 필요한 경우.  

## 5. fifth step

어플리케이션이 동작하는 동안, 모든 이벤트를 관리하는 곳은 `EventAggregator` 이다. 

```c#
public static class EventAggregator 
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
```

## 6. sixth step

**ThreadOption**




