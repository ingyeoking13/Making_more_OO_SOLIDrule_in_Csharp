## Csharp에서 Event와 Delegate

### Introduction   

버튼을 클릭하고, 키보드를 누르는 행위를 사건이라한다.   
나는 `Application`을 만들 때 이 `Event`라는 것에 매우 고통받았고, 또 그럴수록 `Event`에 대해 정이 갔다.  
`Button.Click`, `TextBox.KeyDown` 등의 녀석들에 매우 고통받았고, 오류없이 잘 수행되면 기분이 좋았었다.  

그러나 정작 `event`가 어떤것인지 잘 몰랐다. 

이 문서에서는 `delegate type`, `event keyword`, `EventHander`, `Action`을 다룬다.  

### Event보다 먼저 Delegate를 살펴보자

`event`의 문법은 `delegate`를 **동반**하기 때문에 `delegate`를 먼저 살펴보는 것이 현명하다.  
혼동해서는 안되는 점은 `delegate`가 `event`를 포함하는 관계가 아니며, `event`가 `delegate`를 포함하는 관계가 아니다. `(delegate != event)`  
표현 그대로 `event keyword`는 `delegate`를 **동반** 한다.    
문서에서 기술하겠지만, `delegate type`은 단독으로 쓰일 수 있지만, `event`는 단독으로 쓰일 수 없고 `delegate`를 **동반** 해야한다.        

`delegate type` 변수는 간단하게 말하면 메서드를 가르킨다. 좀 더 엄밀히 말하자면 메서드를 `delegate`로 Wrapping 한 객체를 가르킨다. 이 객체를 `delegate instance`라 한다.  [C#2.0 이전에서는 delegate로 항상 Wrapping 해서 사용해야했다.](https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/delegates/how-to-declare-instantiate-and-use-a-delegate)   

### Delegate 는 그럼 메서드인가요? 변수인가요?   

이미 *C/C++* 를 알고 있는 사람은 *C/C++* 에서 사용되는 함수 포인터는 변수와 같다고 생각한다. ~~void (*function)() 같은 포인터 덩어리 아닌가요?~~   
`Delegate` 또한 메서드가 아닌 레퍼런스 타입의 변수다. C# 에서는 `delegate type`이라고 한다.   
내부적으로는 `C#`의 `delegate`와 `C/C++`의 `function Pointer`가 다르더라도, 개발자 단에서의 사용법은 유사하다.  
`javascript`와 같은 함수형 언어에서는 변수가 함수를 가르키는 것은 새로운 일이 아니다.   

아래는 `Delegate` 사용 예제이다.

````C#
class Program
{
    public delegate void HelloWorldDelegate(string helloWorld);
    public delegate void GoodByeWorldDelegate(string GoodByeWorld);

    static void Main(string[] args)
    {
        Person yohan = new Person();
        HelloWorldDelegate helloWorld = new HelloWorldDelegate(yohan.Speak);
        helloWorld("helloWorld");

        GoodByeWorldDelegate goodByeWorld = new GoodByeWorldDelegate(yohan.Speak);
        goodByeWorld = new GoodByeWorldDelegate(yohan.Speak);
        goodByeWorld("GoodByeWorld");
    }
}

public class Person
{
    public void Speak(string syntax)
    {
        Console.WriteLine(syntax);
    }
}
````

**결과물**

````
helloWorld
GoodByeWorld
````

### `Delegate`는 사실 더 멋집니다.  

다음 예제는 `C#` 으로 어플리케이션을 만들 때 자주 보았던 상황일 것이다.  
위 예제의 `Person Class`를 조금 더 가꾸어 보았다.  

````C#
public delegate void NameChangedDelegate(string currentName, string newName);
public class Person
{
    private string _name;
    public string Name
    {
        get { return _name; }
        set {
            if (_name != value)
            {
                OnNameChanged?.Invoke(_name, value);
            }
            _name = value;
        }
    }

    public NameChangedDelegate OnNameChanged;
}
````

같은 `namespace`안에 `NameChangedDelgate` 이름의 `delegate`를 선언했다.  
반환형은 `void` 이고 `parameter`는  2개의 `string`을 받는다.  
`Person` 클래스에는 해당 `delegate type`의 필드를 생성하였다.   
`_name` 필드를 수정할 때마다 `delegate` 필드에 연결된 메서드를 수행해준다.  

아래는 `Main`에서 `Person` 클래스 객체를 생성하고 `delegate` 필드에 (람다표현식)메서드를 참조하게 했다. (실제론 `delegate instance`들을 참조하는 것이지만 표현상 메서드를 참조한다고 하겠음.)    
`delegate`는 **+= 연산자**로 여러개의 메서드를 참조할 수 있다. 또는 **-= 연산자** 로 등록된 메서드를 뺄수 있다.  
또는 **= 연산자** 로 메서드를 할당할 수 있다. **=null**을 통해 등록된 모든 메서드(엄밀하게는 `delegate instance`)들의 참조를 잃는다.     

````C#
static void Main(string[] args)
{
    Person yohan = new Person();
    yohan.Name = "Yohan"; // set

    yohan.OnNameChanged = new NameChangedDelegate(
        (s, s2) => { Console.WriteLine($"Name change from {s} to {s2}"); }
        );

    yohan.Name = "Kimchi"; // first change

    yohan.OnNameChanged += new NameChangedDelegate(
        (s, s2) => { Console.WriteLine($"hmmm, Do Not change more"); }
        );

    yohan.Name = "gaelim"; // second change

    yohan.OnNameChanged = null; 
    yohan.Name = "yohan"; // third change, Invoke Nothing.
}
````

결과물 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

**더 멋진 점**은, C#2.0 이후 컴파일러에서 함수를 자동으로 해당 `delegate instance`로 변환시켜준다. 따라서 `new NameChangedDelegate` 생성자는 생략할 수 있다.   
다음은 최종적인 소스다.

````C#
yohan.Name = "Yohan"; // set

yohan.OnNameChanged = 
    (s, s2) => { Console.WriteLine($"Name change from {s} to {s2}"); };

yohan.Name = "Kimchi"; // first change

yohan.OnNameChanged += 
    (s, s2) => { Console.WriteLine($"hmmm, Do Not change more"); };
yohan.Name = "gaelim"; // second change

yohan.OnNameChanged = null; 
yohan.Name = "yohan"; // third change, Invoke nothing.
````

결과물 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

이로써, 코드 단에서는 `delegate`는 그저 메서드를 가르키는 변수다 라고 생각하면 될 정도로 간소해졌다.  

### 이제 `event`를 사용해보자. 

`event`의 작성법은 상당히 간단하다. 작성한 `delegate` 앞에 `event` 키워드를 붙이면 된다.     
`Person`의 `delegate type` 필드를 다음과 같이 수정하자.   
````C#
public event NameChangedDelegate OnNameChanged; // Person field
````
그리고 `event`는 항상 **-=, += 연산자**를 동반하므로 Main을 수정한다.  

나머지 소스는 다음과 같다.  
````C#
static void Main(string[] args)
{
    Person yohan = new Person();
    yohan.Name = "Yohan"; // set
    yohan.OnNameChanged += 
        (s, s2) => { Console.WriteLine($"Name change from {s} to {s2}"); };

    yohan.Name = "Kimchi"; // first change
    yohan.OnNameChanged += 
        (s, s2) => { Console.WriteLine($"hmmm, Do Not change more"); };

    yohan.Name = "gaelim"; // second change
}
````

결과물 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

사용에 있어 `delegate` 와 `event delegate` 의 다른점은 크게 다음과 같다.  
* `event`키워드를 동반한 경우 **= 할당연산자**를 사용할 수 없다. 따라서 `= null`과 같은 동작을 할 수 없다.   
* `event`키워드가 붙은 경우 다른 클래스에서 해당 `event delegate` 필드를 직접적으로 조작할 수 없다. **+=** **-=** 동작만 가능하다.  

두 번째에 대한 예는 다음과 같다.  

````C#
// with pure delegate 
class OtherClass_Method()
{
    Person yohan = new Person();
    yohan.OnNameChanged.Invoke("PrevName", "NewName"); //<-- success. can be invoked from Other Class.
}

// with event delegate
class OtherClass_Method()
{
    Person yohan = new Person();
   // yohan.OnNameChanged.Invoke("PrevName", "NewName"); //<-- failed. can not be invoked from Other class.
}
````

`event` 키워드가 동반하는 동작은 시스템 내부적(컴파일러 등)으로 더 복잡한 단계가 있을 수 있다.   
하지만 개발에 있어서의 `event keyword`를 사용함으로써 발생하는 제한은 다음이라 생각한다.   
개발자는 `event keyword`를 통해 클래스 책임에 대한 `design`을 명확할 수있는 척도라고만 기술할 수 있다.  

프레임워크에서의 예로, `Button.Click` 는 해당 `Class`에서만 `Invoke` 할 수 있게 설계되었다.   
즉, `event keyword`를 포함하며 `Button` 이 `Click`되었을 때만 내부적으로 `Invoke`할 수 있게끔 했다. ~~(그러나)~~[^1]     

### (event) delegate 실제 사용은 ...

위 예제에서 우리는 다음과 같은 `delegate type`을 사용하였다.  
````C#
public delegate void NameChangedDelegate(string currentName, string newName);
````
그러나 실제 사용에 있어서는 위와 같이, 원시 타입(`string`, `int` 따위의)을 이용하는 `design`은 별로 선택되지 않는다.   

관례상 `.Net Framework`에서 `event delegate` 또는 `delegate`는 항상 다음과 같은 양식을 쓴다.    
````C#
public delegate void NameChangedDelegate(object sender, NameChangedEventArgs e);
````
`sender`는 해당 `event`를 일으킨 객체를 `this` 키워드를 통해 넘긴다. `Person` 등의 특정 클래스로 제한할 수 있지만, 관례상 *(정말 .NET 관례상이다.)* `object` 타입으로 한다. [^2][^3]   

그리고 위에서 사용되었던 `currentName`, `newName` 정보는 `...EventArgs`의 클래스로 `Wrapping`한 뒤 `EventArgs`를 상속한다.

````C#
public delegate void NameChangedDelegate(object sender, NameChangedEventArgs e);

public class NameChangedEventArgs : EventArgs
{
    public readonly string currentName;
    public readonly string newName;

    public NameChangedEventArgs(string currentName, string newName)
    {
        this.currentName = currentName;
        this.newName = newName;
    }
}

//Main 
yohan.OnNameChanged += (s,e) => { Console.WriteLine($"Name change from {e.CurrentName} to {e.NewName}"); }; 
````

이제까지 `event` 에 대해서 살펴보았다. 이 `event` 키워드라는 것은 사실 `delegate`와 중대한 다른점이 없음을 알게되었다.   
둘 중에 무엇을 사용하든간에 우리는 `Person`클래스의 `Name` 프로퍼티를 통해 `_name` 프라이빗 필드가 바뀐다는 것을 목격할 때마다, `delegate`가 가르키는 메서드를 수행한다.     

### Depth in (Event) Delegate

`event delegate` 필드를 가지고 있는 클래스 `Person`을 자신의 변경을 알리는 `Publisher게시자`라고 할 수 있다.   
`Program`클래스는 `Person`의  `_name` 변화가 있을 때마다 자신이 수행할 메서드 집합을 가지고 있다.  
바뀔때마다 출력하는 `Console class`는 `Subscriber구독자`라 할 수 있다.   

재밌는 점은 이 `delegate`라는 것은 객체 간의 통신을 적극적으로 돕는다. 클래스 간의 `구독자` `게시자` 를 간소하게 설정해줄 수 있다.   
자세한 내용은 3챕터 `객체간 통신을 하는법, Observer` 챕터 후반부 에서 확인하자.   

### delegate, event는 알겠습니다. EventHandler와 Action은 무엇인가요?   

둘 다 `degelgate void`이며 기본적으로 사용 방향은 유사하다.   
`EventHandler`와 `Action` 은 `Generic` 으로 인자를 결정한다. 아래 예를 보자.  

````C# 
public event EventHandler<NameChangedEventArgs> OnNameChanged; // <-- EventHandler
public Action<Person,NameChangedEventArgs> OnNameChanged; // <-- Action
````

우리가 작성한 소스의 `delegate` 필드를 둘 중 어느것으로 바꾸어 작성하여도 잘 동작한다.  

`EventHandler<T>`에 마우스를 가져다대면 다음과 같은 설명을 볼 수 있다.    
`delegate void EventHandler<T> (object sender, T args)`  
반면 `Action<T>` 에 마우스를 가져다대면 다음과 같은 설명을 볼 수 있다.   
`delegate void Action<in T1, in T2> (T1 args1,T2 args2)`   

**무엇이 다른가?**  

* `EventHandler<T>`는 기본적으로 `object sender` 라는 객체를 던져줘야한다. 따라서 사용시 `OnNameChanged(this, T)` 으로 사용한다.   
* 그러나 `Action`은 명시적으로 `<classType(또는 object), T>` 등으로 정의해줘야, `OnNameChanged(this, T)` 로 사용할 수 있다.  
* `EventHandler`는 이름의 의도와 맞게 `event keyword`를 동반한 `event EventHandler<T>`로 사용하는 것이 좋다. 반대로 `Action`은 `event`키워드를 붙이지 않는다.  

**`<in T>`와 `<T>` 가 다릅니다. EventArgs는 `<in>` 키워드가 없으니 `contravariant`를 지원 안하는 것 아닌가요?**  

그렇지 않다. 둘 다 `delegate type` 이므로 기본적으로 `Paramter`에 대한 `Contravariant`를 지원한다.  
`Action` 에서는 명시적으로 `<in T>` 라고 적었을 뿐이다.  

***그런데 `Contravariant`가 무슨 용어인가요? 다음 부록 챕터`Covariant and Contravariant`를 보세요!***  

\[^1] : 그러나, `wpf/UWP` 경우 `Button`은 `UIElement`를 상속하고 있으므로 `UIElement.RaiseEvent()`를 쓸 수 있다. 따라서, 다른 클래스에서도 (주로 페이지 자신, 부모페이지, 자식페이지)`Button.RaiseEvent`를 통해 `Click Event`를 발생하게끔 할 수 있다. 주관적인 판단으론 이런 경우, 특정 프레임워크에서만 가능한 방법론은 지양하는 것이 좋고, 좀 더 일반적인 방향으로 메서드(`delegate instance`) 들을 따로 저장해두어 공유하는 방향이 좀 더 건설적이라 맺음 짓고 싶다.   
\[^2] : 다시 한번 강조하자면, 정말 관례상이다. 이것은 전통적인 `publisher/Notifier게시자`, `subscriber/observer관측자` 역할을 강조하기 위해, 주로 사용되는 객체 자체를 넘기는 `Pull` 방식의 일종이다. 더 노골적으로 `.NET`에서 제공하는 것이 `EventHandler<TeventArgs>(object sender, TEventArgs TArgs)` 이며, 사용법은 `eventHandlerName.Invoke(this, myEventArgs)`로 주로 사용된다. 더 엄밀한 사용법은 `event` 키워드를 앞에 붙여 `event EventHandler<TeventArgs>` 로 사용하며, 해당 필드를 가지고 있는 클래스에서만 `Invoke`한다.    
\[^3] : 두번째 이유론, `delegate`의 `parameter`를 다음과 같이 특정 클래스로 제한하더라도, (예: `delegate void MYDeleagetType(Person p)`) 다음과 같은 메서드를 붙일 수 있다. (예: `method(object sender)`). 이것은 기본적으로 `delegate`가 `Parameter`에 대해 `Contravariant`를 지원하기 때문이다.  

