## Generic, Covariance Contravariance   

#### introduction

필자는 이 `Covariance` `Contravariance`에 대한 이야기를 들었을 때 생소했다.  

*생각 해보는 시간을 가지자.*   
어떤 framework 를 사용하던 간에, framework 는 그저 우리의 소스를 가져가다 쓸 뿐이다.   
어떤 framework 를 사용하던 간에, 우리의 소스는 객체지향이어야 좋다.  
framework가 그렇게 강제하기 때문이다.   
따라서 어떤 framework 를 사용하던 간에, 항상 그 언어에 걸맞는 객체생성과 관련된 라이브러리가 존재한다.  
`autofac`, `IoC Container(C#)`, `Windsor`, `Guice`, `Avalon`...

어떻게 그런것들이 `Application`에서 생성되는 여러 `클래스 타입`에 대해 문제들을 해결하나 고민하면서 마주친게 `Generic` 이고, `covariance` `Contravariance` 였다. 

부록페이지엔 간단히 `Generic, covariance, contravariance`에 대해 기술한다.  

#### Start from Generic  

`Generic`을 사용하는 이유는 크게 2 가지다.  

1. 재 사용성이 좋다.   
````C#
public class MyQueue<T>  : IEnumerable // naive Queue
{
    static private int Size = 20;
    private int head = 0;
    private int tail = 0;
    private T[] q = new T[Size];
    public T Pop()
    {
        return q[head++];
    }
    public void Add(T data)
    {
        q[tail++] = data;
    }

    public IEnumerator GetEnumerator()
    {
        return q.GetEnumerator();
    }
}
````

2. 타입에 대해 안전하다.   

````C#
var integers = new ArrayList();
integers.Add(1);
integers.Add("1");
integers.Add("kimchi");
foreach( var i in integers) Console.WriteLine(i); // fine with Implicit type
foreach( int i in integers) Console.WriteLine(i); // <--- runtime Error BOOM!

var T_integers = new MyQueue<int>();
T_integers.Add(1);
T_integers.Add(5);
T_integers.Add(11);
foreach( int i in T_integers) Console.WriteLine(i); // type safe
````

`Generic`은 재사용성이 좋다.     
하지만 다음 예를 보자.  

````C#
var PersonQueue = new MyQueue<Person>();
PersonQueue.Add(new Student("Kimchi")); // fine  

var StudentQueue = new MyQueue<Student>();
PersonQueue = StudentQueue; // compile error.
````

`MyQueue<Person>` 는 `Student` 타입의 객체들을 담을 수 있는데, 왜 `MyQueue<Student>` 객체 는 가르킬 수 없을까?   

#### Problem 

이 문제는 `C#` 언어만이 안고 있는 문제가 아니다. 보다 일반적인 `type` 관점의 문제이다.   
`Person`은 `Student`의 상위 클래스이다. `Person`을 가르키는 변수는 `Student` 또한 가르킬 수 있다.   
그런데 위 예제에서 `Queue<Person>`이 `Queue<Student>`를 가르키려고 할때 컴파일 에러가 뜬 것을 확인할 수 있다.   
`Queue<Person>`은 `Queue<Student>`의 상위 클래스가 아니라는 말인가?    

아래에서 대립하는 두 의견을 보자.
* 위 예제에서 볼 수 있듯 `Queue<Person>`은 `Student` 객체를 담을 수 있다. 만약 `Student`만 가득 채운 `Queue<Person>`을 생각해보자.     
`Queue<Student>`와 다른게 무엇인가? 이때 왜 `Queue<Person>` 객체는 `Queue<Student>`객체를 가르킬 수 없는가? 

* `Person`과 `Student`가 밀접한 연관이 있다는 건 사실이다. 그런데 `Queue<Student>`는 `Queue<Person>`의 하위타입인가? 
`MyQueue` 클래스를 구현할때 사용한 인터페이스 `IEnumerable`을 생각해보자. `Queue<Student>`는 `IEnumerable<Student>`의 하위 타입아닐까? 

나는 두 번째 의견에 손을 들어 주고 싶다.     
하지만 첫 번째 의견도 공감가는 내용이 있다.   
적당한 지점에서 `Queue<Person>`은 `Queue<Student>`를 가르키는게 정당하다고 생각한다.

### covariance and contravariance    

이 경우 `Covariance` 개념이 필요하다. 우선 `Co`를 제외한 `Variance`란 다음을 뜻한다.  
어떤 복잡한 타입의 두 원소가 있다하자, 그 원소를 구성하는 구성원소가 서로 같은 타입으로 치환 가능할때(부모-자식클래스/인터페이스) 두 원소를 같은 타입으로 치환하는 방법을 가르킨다.  

다시 `MyQueue` 예제로 돌아가보자. 만약 `MyQueue<Person>`로 `MyQueue<Student>`를 가르키고 싶다.  
이 경우 `MyQueue` 원소가 부모-자식 클래스 구성을 하고, `Student` < `Person` 이므로 `Covariance`에 속한다.  
만약 `MyQueue<Student>`가 `MyQueue<Person>`을 가르키고 싶다면, 그 경우 `Contravariance`에 속한다.   
두 경우 다 불가능한 것은 `Invariance`라 한다. 기존 `Generic`을 생각하면 된다.  
두 경우 다 가능한 것은 `Bivariance`라 한다. 이 경우 `C#`에서는 지원하지 않는다.   

`covariance`의 사용법은 다음과 같다.  

````C#
public interface QueueInterface<out T> : IEnumerable
{
}
public class MyQueue<T> : QueueInterface<T>
{
//...정의부분 .. 위 예제와 같음 
}
QueueInterface<Student> students = new MyQueue<Student>();
QueueInterface<Person> persons = students; //안전하게 가르킬 수 있다. 
````

만약 `Contravariance`를 하고 싶다면, `<in T>`라고 정의한 인터페이스를 이용한다.  

#### 참고 

`.Net`에 `built-in` 된 `Generic Interface` 들 중 `Covariance` 를 지원하는 대표적 `Interface`는 `IEnumerable<out T>`, `IComparable<out T>` 이다.  

### delegate에서의 Covariance, Contravariance   

`delegate`에서의 `Covariance, Contravariance` 사용은 좀 더 제한적이며, `in, out Keyword` 가 생략된다.  
`covariance`는 `return` 값에 한정한다.   
`Func<out TResult>` 와 같이 `return value`가 있는`delegate type`에 사용된다.  
즉, `return` 타입에 대해서 `Covariant`를 지원한다. 

반대로 `Contravariance` 에 대해선 `Parameter Type`에 사용을 제한한다.    
대표적으로 `Func<in T , out TResult>` , `Action<in T>`, `EventHandler<TEventargs>` 등이 있다.   
다음은 `delegate`에서의 `Covariance`, `Contravariance` 예제다.   

````C#
public class Person { public int age; }
public class Student : Person { }

public class Time<T> where T : Person
{
    public EventHandler<Student> delegateHandler; // kind of delegate type
    public Action<Student> actionHandler; // kind of delegate type
    public Func<Student, Person> funcHandler; // kind of delegate type 
}

~~end of class def~~
~~start of main class def~~

/* delegate/EventHandler Contravariant : Params Type */
private static void Timer_delegateHandler(object sender, Student e)=> e.age++; // naive
private static void Timer_delegateHandler2(object sender, Person e)=> e.age++; // Contravariant

/* action Contravariant : Params Type */
private static void Timer_actionHandler(Student obj) => obj.age++; // naive
private static void Timer_actionHandler2(Person obj) => obj.age++; // Contravariant

/* Func Covariant : Return Type */
private static Person Timer_funcHandler(Student arg) // naive
{
    arg.age++;
    Person ret = new Person();
    ret.age = arg.age;
    return ret;
}
private static Student Timer_funcHandler2(Person arg) {  // return : Covariant ,  params : contravariant  
    // return Type은 Person, params Type은 Student 여야하지만 역전되어도 Func에 붙일 수 있다.  
    arg.age++;
    Student ret = new Student();
    ret.age = arg.age;
    return ret;
}

static void Main(string[] args)
{
    Time<Student> timer = new Time<Student>();
    Student kimchi = new Student();

    /* delegate/EventHandler Contravariant : Params Type */
    kimchi.age = 13;
    timer.delegateHandler += Timer_delegateHandler; 
    timer.delegateHandler += Timer_delegateHandler2; // Contravariant : Params Type  
    timer.delegateHandler.Invoke(null, kimchi);
    Console.WriteLine(kimchi.age);

    /* action Contravariant : Params Type */
    kimchi.age = 13;
    timer.actionHandler += Timer_actionHandler;
    timer.actionHandler += Timer_actionHandler2; // Contravariant : Params Type 
    timer.actionHandler.Invoke(kimchi);

    Console.WriteLine(kimchi.age);

    /* Func Covariant : Return Type */
    kimchi.age = 13;
    timer.funcHandler += Timer_funcHandler; 
    timer.funcHandler += Timer_funcHandler2; // Covariant : Return Type and Covariant : Params Type
    timer.funcHandler.Invoke(kimchi); // Func은 return value를 받을 수 있지만.. 생략하겠다.  

    Console.WriteLine(kimchi.age);  
}
````

#### 결과물 

````
15
15
15
````


### delegate type, covariance, contravariance에 대한 문서 

이론적으로 `covariance`, `contravariance`는 수학분야에서 파생된 용어이다.  
그리고 `C#`은 `Interface`에서의 `covariance`, `contravariance`을 지원한다.  
그러나 `delegate` 에서의 사용은 조금 제한적인 느낌을 받을 수 있다.  
`Return Type`에 대해선 `Covariance`만 가능하며, `Parameter Type`에선 `Contravariance`만 가능한것이다.  

그에 대한 `.NET` 시스템 개발자의 솔직한 이야기를 볼 수 있다. 

[Eric Lippert's answers on Stack Overflow.](https://stackoverflow.com/questions/54907236/why-wasnt-teventargs-made-contravariant-in-the-standard-event-pattern-in-the-n)

부족한 영어로 정확한 뜻을 전달할 순 없지만, 개발에 있어서의 해당 기능 필요성에 대한 서술이 주를 이룬다.  
한번 생각해볼 것이 만약 `Covariance` `Contravarinace`를 동시에 지원하는 `Bivariance` 기능이 생긴다하자.  
프로그래밍을 하는 어느 시점에서 그것이 필요하겠는가? 충분히 생각해 볼 가치가 있다.  

### 정리하며

`아 이런게 있구나` 끝.



