## 객체를 만드는 이유2 

***객체를 만드는 이유가 객체를 만들기 위해서라고요?***

개발자 A씨는 공부를 꾸준히 했기에 `objected-oriented design`에서 큰 도약을 할 수 있었다.   
A씨는 개념적으로 동일한 것들이라는 느낌을 받을 때는 동일한 수행을 인터페이스로 묶어주는 작업을 수행 하였다.  
이전보다는 큰 도약이었으리라.  

하지만 A씨는 자신의 코드에서 어떤 냄새를 느꼈다.  
분명히 클라이언트에서 `inertface`, `abstract class`와 같은 높은 차원의 변수명으로 해당 객체들을 지시하고 있음에도,  
여전히 클래스를 생성하는데에는 구체적인 코드를 수행해야했다. 객체를 생성하는 시점에서 쓰이는 코드 `interface object = new concreteClass()` 이다.   

객체를 생성하는 것이 어떤 문제를 동반하는지 살펴볼것이다. 

### 1. 서막

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
````

A씨는 각각의 `Ordianry, InternationalStudent`를 `Student`로 묶어주었다.  
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







