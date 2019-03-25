## Abstract Factory Pattern

***객체를 만드는 이유가 객체를 만들기 위해서라고요?***

개발자 A씨는 공부를 꾸준히 했기에 `objected-oriented design`에서 큰 도약을 할 수 있었다.   
A씨는 개념적으로 동일한 것들이라는 느낌을 받을 때는 동일한 수행을 인터페이스로 묶어주는 작업을 수행 하였다.  
이전보다는 큰 도약이었으리라.  

하지만 A씨는 자신의 코드에서 어떤 냄새를 느꼈다.  
분명히 클라이언트에서 `inertface`, `abstract class`와 같은 높은 차원의 변수명으로 해당 객체들을 지시하고 있음에도,  
여전히 클래스를 생성하는데에는 구체적인 코드를 수행해야했다. 객체를 생성하는 시점에서 쓰이는 코드 `interface object = new concreteClass()` 이다.   

객체를 생성하는 것이 어떤 문제를 동반하는지 살펴볼것이다. 

### 1. Introduction

다음 의뢰를 한번 살펴보자.  

````
안녕하세요. OO학교 전산실에서 연락드립니다.   
교실출입을 체크하는 시스템을 구축하려고 합니다.  
학생, 선생님 둘 다 출입을 체크할 수 있어야하거든요.   
가능할까요?   
````

A 씨는 출입을 하는 행위들을 `ClassRoomUser` 인터페이스를 통해 구현하였다.  
그리고 그는 자신이 짜놓은 클래스를 클라이언트에서 어떻게 활용할 수 있는지도 테스트 해보았다.  
클라이언트에서 호출할 수 있는 함수의 사용법 `DoJoinExit`와 같다.  

````C#
public interface IClassRoom
{
    void Join();
    void Exit();
}

public class Student : IClassRoom
{
    public void Join() {}
    public void Exit() {}
}

public class Teacher : IClassRoom
{
    public void Join() {}
    public void Exit() {}
}

static void Main(string[] args)
{

    doJoinAndExit("teacher");
    doJoinAndExit("student");
}

static void doJoinAndExit(string args)
{
    IClassRoomUser person;
    if (args == "teacher") person = new Teacher();
    else if (args == "student") person = new Student();
    else throw new Exception();

    person.Join();
    person.Exit();
}
````

A씨는 테스트 까지 안전하게 마치고, 의뢰인에게 전달했다.  
돌아온 답변은 이렇다.  

```` 
정말 빠른 시간 안에 잘하셨군요.  
그런데요, 일단 테스트해야할 게 DoJoinAndExit 뿐만이 아니라,   
DoJoinAndStay 와 DoExit 도 필요해요.  
아! 그리고 학생, 선생님 말고 학부모님도 추가로 필요하거든요.     
구현 가능할까요?   
````

A씨는 당연 그렇다고 했다.   

### 2. Factory의 도입  

A씨는 우선 `doJoinAndStay(...)` 메서드를 생성했다. 그리고 그는 메서드를 리뷰했다.  

````
static void doJoinAndStay(string args)
{
    IClassRoomUser person;
    if (args == "teacher") person = new Teacher();
    else if (args == "student") person = new Student();
    else if (args == "parent") person = new Parent();
    else throw new Exception();

    person.Join();
}
````

각 메서드들 `doJoinAndExit`, `doJoinAndStay`, `doExit` 내부에서 객체를 생성하는데 if문이 반복되는 것을 발견하였다.   
이를 해결하기 위해 그에겐 몇 가지의 접근법이 있는데 다음과 같다.  

* `doJoinAndExit` 와 같은 메서드들에 인자로 객체를 넘긴다. 즉, `string` 말고, `IClassRoomUser`를 넘긴다. 
* 객체 생성 알고리즘을 객체화 한다.    

사실, 첫번째 방법은 복잡도를 전혀 개선시키지 않는다.  
왜냐하면 클라이언트 단에서 `doJoinAndStay` 등 계열의 메서드를 호출할 때마다 여전히 `if... else if.. else` 문을 수행해 객체를 얻어 함수에 전달해야하기 때문이다.   

다음 함수를 보자.  
````C#
Method(string arg1, string arg2)
{
    IClassRoomUser person = new nullPerson();
    if (arg1== "student") person = new Student(); 
    else if ( arg1 == "teacher") person =  new Teacher();
    else if ( arg1 == "parent") person = new Parent();

    if ( arg2 == "DoJoinAndStay" ) DoJoinAndStay(person);
    else if (arg2 == "DoJoinAndExit") DoJoinAndExit(person);
    else if ( arg2 == "DoExit") DoExit(person);
}
````
이 방법은 얼핏 보기에 좋아보이지만, `DoJoinAndStay` 등 함수의 개별적인 호출에 대해서도 항상 `person`에 대한 전처리 `if else syntax`가 우선되어야한다.        
이해가 가지 않는다면 아래의 `Method2`를 보자.

````C# 
Method2(string arg1)
{
    someOperation(); // 어떤 임의의 작업
    SomeOperation2(); // 어떤 임의의 작업2

    IClassRoomUser person = new nullPerson();
    if (arg1== "student") person = new Student(); 
    else if ( arg1 == "teacher") person =  new Teacher();
    else if ( arg1 == "parent") person = new Parent();

    DoJoinAndStay(person);
}
````
보시다시피 복잡도는 전혀 나아지지 않는다.    
`StudentDoJoin....`, `TeacherDoJoin...` 등의 전용 메서드 들을 만드는 것도 다를바 없이, 좋은 선택이 아니다.   

좋은 선택은, 두 번째 선택이다. A씨는 이미 객체를 생성하는 것을 객체화하는 `Factory Pattern`에 대해 조금은 알고 있었다.     
아래 소스는 그가 작성한 소스이다. 어떻게 작성하였는지 보자.   

````C#
public static IClassRoomUser createClassRoomUser(string args)
{
    IClassRoomUser person;
    if (args == "teacher") person = new Teacher();
    else if (args == "student") person = new Parent();
    else if (args == "parent") person = new Student();
    else throw new Exception();
    return person;
}

static void doJoinAndExit(string args)
{
    IClassRoomUser person = 
        ClassRoomUserFactory.createClassRoomUser(args);

    person.Join();
    person.Exit();
}
static void doJoinAndStay(string args)
{

    IClassRoomUser person = 
        ClassRoomUserFactory.createClassRoomUser(args);

    person.Join();
}
````

클라이언트 단에서의 소스 복잡도가 훨씬 줄어들었다.    
A씨는 성공적으로 프로젝트를 마무리 할 수 있었다.   

### 3. abstract Factory의 도입   

A씨는 다음과 같은 메일을 받았다.   

````
의뢰 결과 만족합니다.
저희 재단이 다른 계열의 학교도 운영중입니다. 
그 학교는 국제학교라서 학생, 선생님, 부모님이 기존에 작성된 것과 공통적인 부분도 있지만,  
다른 부분도 있거든요. 그래서 다르게 취급하고 싶습니다.  
따라서 새로운 클래스를 생성 해주셔야 할것 같은데요?   
````

A씨는 각각의 `Ordianry, InternationalStudent`를 생성하고, `Student`를 공통적으로 상속하였다.    
`Parent` `Teacher` 모두 그렇게 해주었다.  

````C#
public abstract class Student : IClassRoomUser
{
    public void Join() { }
    public void Exit() { }
}

public class OrdinaryStudent : Student { }
public class InternationalStudent : Student { }

public abstract class Teacher : IClassRoomUser
{
    public void Join() {}
    public void Exit() {}
}

public class OrdinaryTeacher : Teacher { }
public class InternationalTeacher : Teacher { }

public abstract class Parent : IClassRoomUser
{
    public void Join() {}
    public void Exit() {}
}

public class OrdinaryParent : Parent { }
public class InternationalParent : Parent { }
````

그리고 이전 `Factory` 에서 `Student`, `Parent`, `Teacher`를 반환했다.   
이번에는 `AbstractFactory`를 생성해서, 각각의 `Student`, `Parent`, `Teacher`를 반환하는 `Factory`를 만들어준다.  

````C#
public abstract class AbstractClassRoomUserFactory
{
    public abstract IClassRoomUser createClassRoomUser(string args);
}

public class OrdinaryClassRoomUserFactory : AbstractClassRoomUserFactory
{
    public override IClassRoomUser createClassRoomUser(string args)
    {
        IClassRoomUser ret = null;
        if (args == "student") ret= new OrdinaryStudent();
        else if (args == "teacher") ret= new OrdinaryTeacher();
        else if (args == "parent") ret= new OrdinaryParent();
        return ret;
    }
}

public class InternationalClassRoomUserFactory : AbstractClassRoomUserFactory
{
    public override IClassRoomUser createClassRoomUser(string args)
    {
        IClassRoomUser ret = null;
        if (args == "student") ret= new InternationalStudent();
        else if (args == "teacher") ret= new InternationalTeacher();
        else if (args == "parent") ret= new InternationalParent();
        return ret;
    }
}
````

그리고 이 `Factory`를 사용하는 방법은 아래와 같다.  
해당 `Factory`를 쓰는 부분을 `Main` 함수보다는 `Factory`를 사용하는 클래스를 명확하게 하기위해 `Algorithm` 클래스를 만들었다.  
기존에 작성된 함수들은 `Algorithm` 클래스 내부로 들어가게된다.  

````C#
public class Algorithm
{
    public void doJoinAndExit(string arg0, string arg1)
    {
        AbstractClassRoomUserFactory factory = new OrdinaryClassRoomUserFactory();
        if (arg0 == "International") factory = new InternationalClassRoomUserFactory();

        IClassRoomUser person = factory.createClassRoomUser(arg1);
        person.Join();
        person.Exit();
    }

    public void doJoinAndStay(string arg0, string arg1)
    {
        AbstractClassRoomUserFactory factory = new OrdinaryClassRoomUserFactory();
        if (arg0 == "International") factory = new InternationalClassRoomUserFactory();

        IClassRoomUser person = factory.createClassRoomUser(arg1);
        person.Join();
    }
    //...
}
static void Main(string[] args)
{
    Algorithm algorithm = new Algorithm();
    algorithm.doJoinAndExit("Ordinary", "teacher");
    algorithm.doJoinAndExit("International", "student");
}
````

이 것이 가장 간단한 `Abstract Factory Pattern`이다.   
우선 먼저 어떤 클래스가 연관이 있는지 정리해보자.  

`AbstractFactory`는 두 가지 `concrete factory`를 가지고 있다.   
그리고 각 각의 `factory`는 `createClassRoomUser` 메서드를 통해 `IClassRoomUser` 객체를 반환한다.  
이 경우 각각의 `concrete factory`가 각각의 `Ordinary` - `International IClassRoomUser`를 반환하고 있다.  
이 것을 가장 간단한 `AbstractFactory`라 할 수 있다.  

### 소스 다듬기  

이 소스는 `Abstract Factory`에 대한 골격을 제공하지만, 조금 더 소스를 개선 시킬 방향이 있다.  

아래 소스를 보자.  

````C#
public class Algorithm
{
    public void doJoinAndExit(string arg0, string arg1)
    {
        AbstractClassRoomUserFactory factory = new OrdinaryClassRoomUserFactory();
        if (arg0 == "International") factory = new InternationalClassRoomUserFactory();
    }
}
````

위 소스에서 `Algorithm` 클래스는 `Factory`를 사용한다. 이 클래스를 `Factory Consumer` 라고도 한다.  
보통 `Factory Consumer` 들은 언제든지 `Factory`를 사용하게 준비되어 있어야하므로, `member field`로 가진다.  
이 것을 클래스의 `dependecy`라고 하며, 보통 클래스 생성자에서 주입하는 `dependency Injection`을 주로 사용한다. 

따라서 `Algorithm Class`는 다음과 같이 바뀐다.  

````C#
public class Algorithm
{
    private readonly AbstractClassRoomUserFactory factory;
    public Algorithm(AbstractClassRoomUserFactory factory)
    {
        this.factory = factory;
    }

    public void doJoinAndExit(string arg)
    {
        IClassRoomUser person = factory.createClassRoomUser(arg);
        person.Join();
        person.Exit();
    }
}
````

그리고 `Algorithm`을 사용하는 클라이언트 쪽에서는 다음과 같은 소스를 작성할 수 있다.  
````C#
static void Main(string[] args)
{
    Algorithm algorithm = new Algorithm(new OrdinaryClassRoomUserFactory());
    algorithm.doJoinAndExit("teacher");
    algorithm.doJoinAndExit("student");
}
````

여기까지가 단순한 `Abstract Factory` 이다.   

### 마무리하며 

`Object`를 만드는 패턴 중 하나인 `Abstract Factory`는 이번 예제만으로는 생산성이 좋아보이지만, 실제로 사용하기엔 여러 약점을 가지고 있다.   
이번 예제는 잘 정의된 문제를 가지고 사용했지만, 실제로 사용할 때엔 단순한 `Abstract Factory`를 쓰는 것만으로는 큰 생산성을 제공해주기가 힘들다.    
그리고 `teacher`, `student`를 `string` 타입으로 넘어가는 것은 좋은 방법이 아니다. 명확한 `spelling`을 요구하며 `Runtime Error`를 유발하기 때문이다.   
따라서 다음 포스팅에서는 `Abstract Factory`와 유사하게 클래스를 만드는 것에 집중한 디자인 패턴인, `Builder` 에 대해서 이야기를 해보고자 한다.  






