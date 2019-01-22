## Csharp에서 Event와 Delegate란1 

### Introduction   

C# 에서 `Event`는 다음과 같은 역할을 수행한다.  

**Event를 이용하면 한 클래스가 다른 클래스나 객체에게 연락할 수 있다**  
1 `Publisher`는 이벤트를 발생시키고   
2 하나 이상의 `Subscribers`가 이벤트를 수행한다.   

### 잠깐, Delegate를 먼저 봐야겠어 

잠깐 `event`에 대한 자세한 설명을 집어넣자. (방금 막 `event`이야기를 꺼내셨잖아요?)   
`event`를 설명하기 전에 `delegate`를 먼저 살펴보는게 우선되어야 할 것 같다.   

왜냐하면 `event`의 문법은 `delegate`를 동반하기 때문이다!.   

`delegate`는 메서드를 가르킨다. 함수 포인터와 매우 유사하다.     

### Delegate 는 그럼 메서드인가요? 변수인가요?   

어 음... 좋은 질문!  
이미 *C/C++* 를 알고 있는 사람은 C/C++에서 사용되는 함수 포인터는 변수와 같다고 생각한다.  
`Delegate` 또한 메서드가 아닌 변수이다. C# 에서는 `delegate type`이라고 한다. (`delegate`는 사용 전 `new` keyword로 할당되어야한다.)    
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


### `Delegate`는 사실 여러분이 처음 생각한 것 보다 더 멋집니다.  

다음 예제는 C# 으로 어플리케이션을 만들 때 자주 보았던 상황일 수 있다. 배경지식이 없어도 된다!   
위 예제의 Person Class를 조금 더 가꾸어 보았다.  

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

    public void Speak(string syntax)
    {
        Console.WriteLine(syntax);
    }
}
````

같은 `namespace`안에 `NameChangedDelgate` 이름의 `delegate`를 선언했다.  
반환형은 `void` 이고 `parameter`는  2개의 `string`을 받는다.  
`Person` 클래스에는 해당 `delegate type`의 필드를 생성하였다.   
`_name` 필드를 수정할 때마다 `delegate` 필드에 연결된 메서드를 수행해준다.  

아래는 `Main`에서 `Person` 클래스 객체를 생성하고 `delegate` 필드에 (람다)함수를 참조하게 했다.   
`delegate`는 **+= 연산자**로 여러개의 함수를 참조할 수 있다. 


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
}
````

결과물 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

C# 컴파일러에서 함수를 자동으로 해당 delegate type으로 변환시켜준다. 따라서 new UserDelegate 생성자는 생략할 수 있다.   
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
yohan.Name = "yohan"; // second change
````

결과물 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

### 지금 `event`를 사용해보자. 

`event`의 작성법은 꽤나 간단하다.    
`Person`의 delegate type 필드를 다음과 같이 수정하자.   
그리고 `event`는 항상 **-=, += 연산자**를 동반하므로 Main을 수정한다.  
이제 `event`이기에 `= null` 은 되지 않아요!.

````C#
public event NameChangedDelegate OnNameChanged; // Person field

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

그런데 관례상 .Net Framework에서 이벤트는 항상 다음과 같은 양식의 `delegate`를 쓴다.  
`sender`는 해당 `event`를 일으킨 객체를 `this` 키워드를 통해 넘긴다. `Person` 이라 제한할 수 있지만, 관례상 *(정말 .NET 관례상이다.)* `object` 타입으로 한다.  
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

`event` 에 대해서 살펴보았다. 이 `event`라는 것은 사실 `delegate`와 큰 다름이 없음을 알게되었다.   
우리는 `Person`클래스의 `Name` 프로퍼티를 통해 `_name` 프라이빗 필드가 바뀐다는 것을 목격할 때마다, `delegate`가 가르키는 메서드를 수행한다.     

### 그러면 이렇게 생각해보아도 될까? 

`event` 필드를 가지고 있는 클래스 `Person`을 자신의 변경을 알리는 `Publisher게시자`라고 할 수 있다.   
`Program`클래스는 `Person`의  `_name` 변화가 있을 때마다 자신이 수행할 메서드를 가지고 있다.  
그럼 `Program`은 `Subscribers구독자` 이다.  

### 힌트! 

재밌는 점은 이 `event`라는 것은 객체 간의 통신을 적극적으로 돕는다.   
자세한 내용은 3챕터 `객체간 통신을 하는법, Observer` 에서 확인하자.   

