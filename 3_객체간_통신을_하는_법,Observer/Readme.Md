## 객체간 통신을 하는 법, Observer Pattern 

### 1 introduction  

어느날, 취미개발자 K씨는 구인광고 프로그램을 제작해달라는 의뢰를 받았다.  
아래는 의뢰의 내용이다.
````
안녕하세요, 의뢰내용입니다.   
회사는 입사희망자들에게 자신들이 구인광고를 내걸었다고 알려야 하구요.
그리고 입사희망자들은 해당 회사의 구인광고가 올라온 것을 알게되면, 
입사희망자들은 해당 회사에 지원을 하는거에요. 제작가능할까요?
````

K씨는 꽤나 잘 훈련된 취미개발자이기 때문에 이미 어떻게 구현해야할지 잘 알고 있다.  
그리고 K씨는 객체지향을 좋아하기 때문에, 회사 클래스와 사람 클래스를 작성하였다.  
아래는 K씨가 작성한 소스이다.  

````C#
//Company class
public class Company
{
    public List<Person> subscriber = new List<Person>();

    public string Name { get; }

    public Company(string name)
    {
        Name = name;
    }

    public void register(Person p)
    {
        subscriber.Add(p);
    }

    public void NotifyNewCrewWanted()
    {

        Console.WriteLine($"{Name} company Notify that they want New Crew.");
        foreach (var p in subscriber)
        {
            p.apply();
        }
    }
}

//Person class
public class Person
{
    public string Name { get; set; }
    public Person(string name)
    {
        Name = name;
    }
    public void apply()
    {
        Console.WriteLine($"I'm {Name}. I apply your company");
    }
}

//Main
class Program
{
    static void Main(string[] args)
    {
        Company samsung = new Company("samsung");
        samsung.register( new Person("YoHan"));
        samsung.register( new Person("Gaelim"));
        samsung.register( new Person("Kimchi"));

        Company LG = new Company("LG");

        samsung.NotifyNewCrewWanted();
        LG.NotifyNewCrewWanted();
    }
}
````

결과물
````
samsung company Notify that they want New Crew.
I'm YoHan. I apply your company
I'm Gaelim. I apply your company
I'm Kimchi. I apply your company
LG company Notify that they want New Crew.
````

K는 기쁜마음으로 소스를 의뢰인분에게 전달하였다. 

### 2 요구사항1(절망편)  

한 1주일 후 의뢰인에게 연락이 왔다.  

````
예, K씨 소스 잘받았습니다. 동작 잘 되는데요?  
저희가 지금 지원자분들이랑 회사한테 서비스를 제공하고 있잖아요? 
그런데 헤드헌터 분들한테도 저희 서비스를 제공하고 싶거든요. 
헤드헌터분들도 지원자분들이랑 똑같이 회사한테서 입사공고 받게하고 싶습니다.  
````
 
K씨는 인내심이 좋은 개발자이다. 카페에 앉아 커피 한 잔을 하면서, 그는 객체지향적으로 문제해결방법에 대해 생각했다.  
그는 `헤드헌터`와 `지원자`는 모두 `회사`에 대해 귀를 기울이는 형태를 하고 있으니, 그 행위자체를 하나의 인터페이스로 구현하기로 결심했다.  
그렇게 하니 소스가 훨씬 간편해졌고 재사용성이 좋았다.  
인터페이스에 대해서는 미리 `ingyeoking13` 의 `github`의 `1챕터`를  참고해보길 잘 했다고 생각하면서 말이다.  
아래는 K씨가 작성한 소스이다.    

````C#
//Company
public class Company
{
    public List<Observer> subscriber = new List<Observer>();

    public string Name { get; }

    public Company(string name)
    {
        Name = name;
    }

    public void register(Observer p)
    {
        subscriber.Add(p);
    }

    public void NotifyNewCrewWanted()
    {

        Console.WriteLine($"{Name} company Notify that they want New Crew.");
        foreach (var p in subscriber)
        {
            p.update();
        }
    }
}

public interface Observer
{
    void update();
}

public class HeadHunter : Observer
{
    public string Name { get; }
    public HeadHunter(string name)
    {
        Name = name;
    }

    public void update()
    {
        Console.WriteLine($"I'm {Name} HeadHunter. I receive your company notification.");
    }
}

public class Person : Observer
{
    public string Name { get; }
    public Person(string name)
    {
        Name = name;
    }

    public void update()
    {
        Console.WriteLine($"I'm {Name}. I apply your company");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Company samsung = new Company("samsung");
        samsung.register( new Person("YoHan"));
        samsung.register( new Person("Gaelim"));
        samsung.register( new Person("Kimchi"));
        samsung.register( new HeadHunter("SaramIn"));
        samsung.register( new HeadHunter("Jasoseol"));
        samsung.register( new HeadHunter("JobKorea"));

        Company LG = new Company("LG");
        LG.register( new HeadHunter("SaramIn"));

        samsung.NotifyNewCrewWanted();
        LG.NotifyNewCrewWanted();
    }
}
````
결과물

````
samsung company Notify that they want New Crew.
I'm YoHan. I apply your company
I'm Gaelim. I apply your company
I'm Kimchi. I apply your company
I'm SaramIn HeadHunter. I receive your company notification.
I'm Jasoseol HeadHunter. I receive your company notification.
I'm JobKorea HeadHunter. I receive your company notification.
LG company Notify that they want New Crew.
I'm SaramIn HeadHunter. I receive your company notification.
````

작성하는데 불과 30분도 걸리지 않았다.   
취미개발자 K씨는 의외로 취미 이상의 능력을 가지고 있었던 것이다.  

### 3 요구사항2(절망편)

하루 뒤, 불길한 느낌의 전화 한 통화가 온다.  

````
예, K씨 잘 확인했습니다.  
그런데 헤드헌터도 회사처럼 옵저버들에게 개별적으로 연락할 수 있거든요.  
그 부분을 구현해주시겠어요? 
얼마의 시간이 걸릴까요?   
````

K씨는 굉장히 인내심이 좋고, 객체지향을 잘 알고있다.     
그는 알린다는 `notify` 행위를 인터페이스로 구현하기로 했다. 그리고 자신이 참조해야할 `observer` 행위를 하는 객체들을 등록하는 행위 또한 인터페이스에 녹였다.   
그리고 기존의 `Company`와 `HeadHunter`를 해당 행위를 할 수있게끔 위 행위를 인터페이스화한 `ISubject`로 구현하였다.   

````C#
//Isubject
public interface ISubject
{
    void register(IObserver o);
    void notify();
}

//company
public class Company : ISubject
{
    public List<IObserver> subscriber = new List<IObserver>();

    public string Name { get; }

    public Company(string name)
    {
        Name = name;
    }

    public void register(IObserver p)
    {
        subscriber.Add(p);
    }

    public void NotifyNewCrewWanted()
    {
        Console.WriteLine($"*** {Name} company Notify that they want New Crew.");
        notify();
        Console.WriteLine();
    }

    public void notify()
    {
        foreach (var p in subscriber) p.update();
    }
}

//Headhunter
public class HeadHunter : IObserver, ISubject
{
    List<IObserver> observers = new List<IObserver>();
    public string Name { get; }
    public HeadHunter(string name)
    {
        Name = name;
    }

    public void update()
    {
        Console.WriteLine($"I'm {Name} HeadHunter. I received your notification.");
    }

    public void notify()
    {
        Console.WriteLine($"*** {Name} HeadHunter notify that he has something to you");
        foreach (var o in observers) o.update();
        Console.WriteLine();
    }

    public void register(IObserver o)
    {
        observers.Add(o);
    }
}
//Person update 메서드만 수정
public void update()
{
    Console.WriteLine($"I'm {Name}. I reveived message");
}

//Program
class Program
{
    static void Main(string[] args)
    {
        //initializing
        Company samsung = new Company("samsung");
        Company LG = new Company("LG");
        HeadHunter SaramIn = new HeadHunter("SaramIn");
        HeadHunter Jasoseol = new HeadHunter("Jasoseol");
        HeadHunter JobKorea = new HeadHunter("JobKorea");

        //register
        samsung.register(new Person("YoHan"));
        samsung.register(new Person("Gaelim"));
        samsung.register(new Person("Kimchi"));
        samsung.register(JobKorea);
        samsung.register(Jasoseol);
        samsung.register(SaramIn);

        SaramIn.register(new Person("adkb"));
        SaramIn.register(new Person("BlueBird"));

        Jasoseol.register(JobKorea);

        //notify
        samsung.NotifyNewCrewWanted();
        LG.NotifyNewCrewWanted();

        SaramIn.notify();
        JobKorea.notify();
        Jasoseol.notify();

    }
}
````
결과물
````
*** samsung company Notify that they want New Crew.
I'm YoHan. I reveived message
I'm Gaelim. I reveived message
I'm Kimchi. I reveived message
I'm JobKorea HeadHunter. I received your notification.
I'm Jasoseol HeadHunter. I received your notification.
I'm SaramIn HeadHunter. I received your notification.

*** LG company Notify that they want New Crew.

*** SaramIn HeadHunter notify that he has something to you
I'm adkb. I reveived message
I'm BlueBird. I reveived message

*** JobKorea HeadHunter notify that he has something to you

*** Jasoseol HeadHunter n`otify that he has something to you
I'm JobKorea HeadHunter. I received your notification.
````

취미개발자 K씨... 당신은 대체....    

### 4 요구사항3(절망편)  

그러나 아직 취미개발자 K씨를 찬양하기는 이르다.  
그는 전화한통을 받았다.     

````
안녕하세요. K씨, 요구사항에 유연하게 대응해주셔서 정말 감사합니다.  
그런데 말입니다. 저희 어플리케이션이 반응이 좋아서요. 
회사가 유저들에게 구인광고를 낼 때 그 회사 이름과 신입기준 연봉에 대한 정보를 제공해주고 싶거든요.  
회사가 헤드헌터들에게 구인광고를 낼 때는 그 회사 이름과 일하게 될 부서, 직급, 연봉에 대해 알리고 싶습니다.  
그리고 헤드헌터가 유저들에게 알리는 메커니즘에 조건이 생겼습니다. 헤드헌터는 회사한테 알림을 받으면 유저들에게 알리게 되는데요. 
헤드헌터가 유저들에게 헤드헌터 이름과 회사이름에 대한 정보를 알려줍니다.   
추가해줄 수 있을까요?   
````

K씨는 무척 인내심이 있는 사람이다. 그는 카페에서 향이 강한 커피를 함께하며 소스를 작성했다.   
K씨는 우선 notify를 수정하기로 했다. 알린다는 행위를 하는 ISubject는 이제, 세가지 유형의 알림 행위를 수행한다.  
첫 번째는 회사가 일반 유저들에게 알리는 `companyNotifyToPerson` 이다. 필요 정보는 회사 이름과 신입기준 연봉이다.   
두 번째는 회사가 헤드헌터들에게 알리는 `companyNotifyToHeadHunter`이다. 필요 정보는 이름, 부서, 직급, 연봉이다.  
세 번째는 헤드헌터가 일반 유저들에게 알리는 `headHunterNotifyToPerson` 이다. 필요 정보는 헤드헌터 이름과 회사이름이다.
그래서 K씨는 아래 after1 소스를 작성하였다.  

````C#
//before
//IObserver interface update
void update();

//after 1
void personGetNotifiedFromCompany(string Name, int newPersonWage);
void headhunterGetNotifiedFromCompany(string Name, string Department, string Level, int experiencedWage);
void personGetNotifiedFromHeadHunter(string Name, string companyName);
````

K씨는 after 1 처럼 `IObserver` 인터페이스의 함수를 작성하였다.   
K씨는 작성 중 Name과 같이 객체 자체의 필드를 이용할 땐 해당 객체 자체를 넘겨받기로했다.   
````C#
void ...(object sender, int newPersonWage /*, ... 인자*/); // (sender as Company).Name 이런식으로 말이다
````
`Observer` 쪽에서 객체의 해당 프로퍼티(필드의 get 메서드)에  접근하는 것이 `pull`방식이라 한다.  
그리고 유동적인 것들은 그대로 인자로 넘겨주기로 했다. 이를 `push`방식이라 한다.   
인자로 넘겨주는 이유는 한 객체가 다른 객체에 대해 너무 많은 필드에 대해 접근하는 것은 객체지향 관점에서 좋지 않았기 때문이다. 

````C#
//after2
void personGetNotifiedFromCompany(object sender, int newPersonWage);
void headhunterGetNotifiedFromCompany(object sender, string Department, string Level, int experiencedWage);
void personGetNotifiedFromHeadHunter(object sender, string companyName);
````

K씨는 IObserver 인터페이스는 이 정도로 마무리하고, `notify`에 대해 수정한다.  
````C#
//before
//ISubject interface
void notify();

//after
void companyNotifyObserver();
void headhunterNotifyPerson(string companyName, string department, string level, int experiencedWage);
````
자, 이 인터페이스 변경이 어떤 결과를 발생시키는지 살펴보자. 

아래는 생산된 소스이다.  


````C#
public class Company : ISubject
{
//생략
    public void NotifyNewCrewWanted() 
    { 
        companyNotifyObserver(); 
    }

    public void companyNotifyObserver()
    {
        foreach (var p in subscriber)
        {
            if (p is Person) p.personGetNotifiedFromCompany(this, (int)1e6);
            else p.headhunterGetNotifiedFromCompany(this, "IT", "CTO", (int)2e9);
        }
    }

    public void headhunterNotifyPerson(string companyName, string department, string level, int experiencedWage) { } // empty
}

public class Person : IObserver
{
//생략
    public void personGetNotifiedFromCompany(object sender, int newPersonWage)
    {
        Console.WriteLine($"I'm {Name}. I reveived message from {(sender as Company).Name}. {newPersonWage} KRW Job");
    }

    public void personGetNotifiedFromHeadHunter(object sender, string companyName, string Department, string Level, int experiecedWage)
    {
        Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {companyName} company");
        Console.WriteLine($"=={companyName}  : {Department}, {Level} {experiecedWage} KRW job==");
    }

    public void headhunterGetNotifiedFromCompany(object sender, string Department, string Level, int experiencedWage) { }//empty
}

public class HeadHunter : IObserver, ISubject
{
//생략
    public void headhunterGetNotifiedFromCompany(object sender, string Department, string Level, int experiencedWage)
    {
        Console.WriteLine($"I'm {Name} HeadHunter. I received {(sender as Company).Name} notification.");
        headhunterNotifyPerson((sender as Company).Name, Department, Level, experiencedWage);
    }

    public void headhunterNotifyPerson(string companyName, string department, string level, int experiencedWage)
    {
        Console.WriteLine($"*** {Name} HeadHunter notify that he has something to you");
        foreach (var o in observers) o.personGetNotifiedFromHeadHunter(this, companyName, department, level, experiencedWage);
        Console.WriteLine();
    }

    public void personGetNotifiedFromHeadHunter(object sender, string companyName, string Department, string Level, int experiecedWage)
    {
        Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {companyName} company");
        Console.WriteLine($"=={companyName}  : {Department}, {Level} {experiecedWage} KRW job==");
    }

    public void companyNotifyObserver() { } //empty
    public void personGetNotifiedFromCompany(object sender, int newPersonWage) { } //empty
}

// main
//initializing
Company samsung = new Company("samsung");
Company LG = new Company("LG");
HeadHunter SaramIn = new HeadHunter("SaramIn");
HeadHunter Jasoseol = new HeadHunter("Jasoseol");
HeadHunter JobKorea = new HeadHunter("JobKorea");

//register
samsung.register(new Person("YoHan"));
samsung.register(new Person("Gaelim"));
samsung.register(new Person("Kimchi"));
samsung.register(JobKorea);
samsung.register(Jasoseol);
samsung.register(SaramIn);

SaramIn.register(new Person("adkb"));
SaramIn.register(new Person("BlueBird"));

Jasoseol.register(JobKorea);

//notify
samsung.NotifyNewCrewWanted();
LG.NotifyNewCrewWanted();
````
결과물 
````
*** samsung company Notify that they want New Crew.
I'm YoHan. I reveived message from samsung. 1000000 KRW Job
I'm Gaelim. I reveived message from samsung. 1000000 KRW Job
I'm Kimchi. I reveived message from samsung. 1000000 KRW Job
I'm JobKorea HeadHunter. I received samsung notification.
*** JobKorea HeadHunter notify that he has something to you

I'm Jasoseol HeadHunter. I received samsung notification.
*** Jasoseol HeadHunter notify that he has something to you
I'm JobKorea.I received Message From Jasoseol about samsung company
==samsung  : IT, CTO 2000000000 KRW job==

I'm SaramIn HeadHunter. I received samsung notification.
*** SaramIn HeadHunter notify that he has something to you
I'm adkb.I received Message From SaramIn about samsung company
==samsung  : IT, CTO 2000000000 KRW job==
I'm BlueBird.I received Message From SaramIn about samsung company
==samsung  : IT, CTO 2000000000 KRW job==


*** LG company Notify that they want New Crew.

````

잘 동작한다. 하지만 어떤 느낌이 들지 않는가? 코딩 스멜....   
각자 클래스마다 원하는 함수가 있으니,인터페이스가 해당 메서드를 구현하게끔 강제하였다.  
`Observer`, `Subject` 에 대해 여러 클래스가 묶여있다보니 `Person` 과 `HeadHunter` 는 `getNotifed` 행위에대해 전혀 알필요 없는 메서드를 강제로 가지게 되었다.   
또 `Company`와 `HeadHunter`는 `Notify`에 대해 전혀 알필요없는 메서드를 알게되었다. 그런 함수들은 공백으로 남게 되었다.   
요약하자면, 각각 클래스는 인터페이스에 제시된 함수들 중 부분집합에만 관심이 있지만, 전체 함수 집합을 다 알고 있어야 하는 것이다.    

이전까지 K씨는 인터페이스가 마법의 도구처럼 느껴졌지만, 이번만큼은 아니다. 그도 어떤 구린 냄새를 맡고있는 것이다.  
어쨌든 그는 여기서 소스를 마무리 하기로 했다. 
그의 소스 작성시간은 1시간 정도가 걸렸다. 이전보다 2배가 걸린셈이다.     

### 5 요구사항4(BIG 절망편)  

```` 
의뢰인 : 
K씨 당신의 구현능력은 정말 최고예요!.  
서비스에 대해 고객들의 호응이 좋았습니다. 결과적으로, 
운영진에서 더 많은 고객층에 대해 서비스를 제공하기로 했습니다!  
이제는 '프리미엄 일반고객', '프리미엄 회사', '퇴사자', '검사'(??), '마법사'(????) 등을 추가하기로 했습니다.
그리고 각각 또 역할이 있는데요 ... 생략 ... 
가능할까요??

개발자 : 
음.. 힘들것 같은데요?

의뢰인 :
갑자기 다른분에게 의뢰를 맡기면 힘들것 같습니다. K님이 꼭 해주십시오.  

개발자 :
...
````

K씨는 얼마나 많은 함수를 만들어야 할지 머리가 띵해졌다.    
이 요청을 완성했을 때 몇 시간이 걸릴까 라는 생각도 들었다.      
카페가 문 닫을 때까지 완성할 수 있을까? 인터페이스에 얼마나 많은 함수를 작성해야할까?   

무엇보다 속에서 울리는 생각은 아래와 같았다.  
![](../짤방/why_jiral.jpg)   
~~K씨의 속생각~~

***
***K씨는 왜 이렇게 생각하는 지경에 도달했을까?***   

***의뢰인은 K씨를 싫어하는 것일까? 개발자가 사회생활을 하는 모습을 보기 싫어 하는 것일까?***   
***의뢰인은 개발자가 소스에 파묻혀살기를 바라는 싸이코패스일까? 취미개발자에게 더 냉정한 프로의 세계를 보여주려하기 때문이였을까?***      
***아니면 K씨가 단순히 큰 어플리케이션을 만들 역량이 부족하기 때문이였을까? K씨는 자신이 프로가 아니기에 근성이 부족한 것일까?***       
***

다 맞는 사실일지 모른다.  
가장 중요한 것은 이전 작업으로 인해 그의 소스엔 코딩 스멜이 나기 시작했다.  
K씨의 문제는 문제에 대해 조금만 더 고민을 하게되면 해결된다.   

우선 문제를 정의해보자.  
각 클래스는 `Observer`, `Subject` 역할을 한다. 그 역할에 맡게끔 인터페이스를 제공했다.  
프로젝트 초기에는 재사용성이 늘었다. 프로젝트가 진행되면 진행될수록 클래스는 많아지고,
각각의 `Notify`, `Observer` 행위를 구현하는 메서드를 제공하다보니 인터페이스에 포함되는 메서드가 갈수록 늘어났다.   
그리고 `Observer` 인터페이스로 구현된 클래스들은 사용하지 않는 메서드 까지 포함하게 되었다.   

### 5 희망편  

어떻게 인터페이스가 가지는 다양한 메서드들을 줄일 수 있을까?  
`Subject`가 알림을 받는 객체를 저장하는게 아니라, 알림을 받아서 수행되는 함수를 저장해두기로 하면 가능하다.  

문제 해결에 대한 것은 꽤나 명확하지만 해결에 대한 나는 `Zoran Horvat`의 `tactical design pattern` 강의에 있는 접근 방식을 사용할 것이다.   
왜냐하면 이 방식이 꽤 단계적으로 접근하기에 보기 좋기 때문이다.   

우선적으로 각 `observer` 마다 요구하는 다양한 인자들이 신경쓰인다.  
해결을 위해 다양한 인자들을 하나로 표현할 수 있는 `Generic`을 이용해보자.    

````C#
//before Observer
void personGetNotifiedFromCompany(object sender, int newPersonWage);
void headhunterGetNotifiedFromCompany(object sender, string Department, string Level, int experiencedWage);
void personGetNotifiedFromHeadHunter(object sender, string companyName, string Department, string Level, int experiecedWage);

//after Observer<T>
void getNotifed(object sender, T data);
````

이제 관찰하는 행위를 하는 `Person`, `HeadHunter` 는 `getNotifed`를 가지고 있게 한다.  
각자 `getNotifed` 행위가 요구하는 인자들은 `Generic T`로 일반화 하였다.  
`observer` 들은 자신들이 `getNofied`를 가지기 위해, 해당 `Observer<T>` 인터페이스로 직접 구현해도 되지만,  (예: `Person<T> : Observer<T>` )   
필드로 `Observer<T>` 인터페이스를 가지게 해두자. 이렇게 하면 한 객체가 하나 이상의 `Observer<T>`를 가질 수 있을 것이다.   
(직접적으로 인터페이스를 구현할 때는  Person 객체는 한 타입의 getNotified만 가질 수 있다는 걸 확인하자.)     
즉, 하나 이상의 Generic을 가질 수 있는 것이다. (예: Person 객체는 회사 알림은 int만 받지만, 헤드헌터 알림은 [string string string..]을 받는다 )  

````C#
public class ObserverDelegate<T> : fifth.IObserver<T>
{
    public delegate void observerDelegate(object sender ,T data);
    observerDelegate @delegate;

    public ObserverDelegate(observerDelegate method)
    {
         this.@delegate= method;
    }

    public void getNotified(object sender, T data)
    {
        @delegate?.Invoke(sender, data);
    }
}
````

`IObserver<T>` 인터페이스로 구현된 클래스이다. 이 클래스의 아래와 같은 역할을 수행한다.    
`getNotified` 함수는 자신의 `delegate` 가 가르키는 함수를 수행하는 것 뿐이다.   
아래는 `Observer` 클래스 중 하나인 `Person`에서 `ObserverDelegate<T>` 클래스가 어떻게 사용되는지 보여준다.  

````C#
public class Person 
{
    public ObserverDelegate<int> personGetNotifiedFromCompany;
    public ObserverDelegate<Tuple< string, string, string, int>> personGetNotifiedFromHeadHunter;
    public string Name { get; }

    public Person(string name)
    {
        Name = name;
        personGetNotifiedFromCompany = new ObserverDelegate<int>
            ( (s,d) => this.personGetNotifiedFromCompanyHandler(s, d) );

        personGetNotifiedFromHeadHunter = new ObserverDelegate<Tuple<string, string, string, int>>
            ( (s,d) => this.personGetNotifiedFromHeadHunterHandler(s, d) );
    }

    public void personGetNotifiedFromCompanyHandler(object sender, int newPersonWage)
    {
        Console.WriteLine($"I'm {Name}. I reveived message from {(sender as Company).Name}. {newPersonWage} KRW Job");
    }

    public void personGetNotifiedFromHeadHunterHandler(object sender, Tuple<string , string , string , int> data )
    {
        Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {data.Item1} company");
        Console.WriteLine($"=={data.Item1}  : {data.Item2}, {data.Item3} {data.Item4} KRW job==");
    }
}
````

`Person`은 자기가 호출받고 싶어하는 함수만 자신의 `ObserverDelegate<T>`라는 필드에 등록해놓으면 된다.  
그리고 자기 자신이 가지는 함수에 대해 필드로 노출시켜주었다.   

아래부터는 `Subject`에 관한 것이다. 
우선 `Subject`가 가질 `GenricNotifier<T>`를 살펴보자.   
`GenericNotifer<T>` 는 `IObserver<T>` 인터페이스로 구현된 클래스를 담는 리스트만 가지고 있다.  
그리고 `notify` 함수를 수행하면, 리스트를 순회하며 옵저버의 `getNotified` 함수를 수행한다.  

````C#
public class GenericNotifier<T>
{
    private List<IObserver<T>> notifyList;

    public GenericNotifier(IObserver<T>[] observer)
    {
        notifyList = new List<IObserver<T>>(observer);
    }

    public void notify(object sender, T data)
    {
        foreach (var o in notifyList) o.getNotified(sender, data);
    }
}

public class Company 
{
    public GenericNotifier<int> person;
    public GenericNotifier<Tuple<string, string, int>> HeadHunter; 

    public string Name { get; }

    public Company(string name)
    {
        Name = name;
    }

    public void NotifyNewCrewWanted()
    {
        Console.WriteLine($"*** {Name} company Notify that they want New Crew.");
        companyNotifyObserver();
        Console.WriteLine();
    }

    public void companyNotifyObserver()
    {
        person?.notify(this, (int)1e6);
        HeadHunter?.notify(this, Tuple.Create("IT", "CTO", (int)2e9));
    }
}
````

아래는 헤드헌터와 메인함수이다.  
````C#

public class HeadHunter 
{
    public GenericNotifier<Tuple<string, string, string, int>> person;
    public GenericNotifier<Tuple<string, string, string, int>> headHunter;
    public ObserverDelegate<Tuple<string, string, string, int>> personGetNotifiedFromHeadHunter;
    public ObserverDelegate<Tuple<string, string, int>> headHunterGetNotifiedFromCompany;

    public string Name { get; }
    public HeadHunter(string name)
    {
        Name = name;

        personGetNotifiedFromHeadHunter = new ObserverDelegate<Tuple<string, string, string, int>>
        ( (d, s) => personGetNotifiedFromHeadHunterHandler(d, s) );

        headHunterGetNotifiedFromCompany = new ObserverDelegate<Tuple<string, string, int>>
        ( (d, s) => headhunterGetNotifiedFromCompanyHandler(d, s) );
    }

    public void headhunterGetNotifiedFromCompanyHandler(object sender, Tuple<string, string, int> data)
    {
        Console.WriteLine($"I'm {Name} HeadHunter. I received {(sender as Company).Name} notification.");
        headhunterNotifyPerson((sender as Company).Name, data.Item1, data.Item2,data.Item3);
    }

    public void headhunterNotifyPerson(string companyName, string department, string level, int experiencedWage)
    {
        Console.WriteLine($"*** {Name} HeadHunter notify that he has something to you");
        person?.notify(this, Tuple.Create(companyName, department, level, experiencedWage));
        Console.WriteLine();
    }

    public void personGetNotifiedFromHeadHunterHandler(object sender, Tuple<string , string , string , int> data )
    {
        Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {data.Item1} company");
        Console.WriteLine($"=={data.Item1}  : {data.Item2}, {data.Item3} {data.Item4} KRW job==");
    }
}

//Main

static void Main(string[] args)
{
    //initializing
    Company samsung = new Company("samsung");
    Company LG = new Company("LG");
    HeadHunter SaramIn = new HeadHunter("SaramIn");
    HeadHunter Jasoseol = new HeadHunter("Jasoseol");
    HeadHunter JobKorea = new HeadHunter("JobKorea");
    Person yohan = new Person("YoHan");
    Person gaelim = new Person("Gaelim");
    Person kimchi = new Person("Kimchi");
    Person adkb = new Person("adkb");
    Person BlueBird = new Person("BlueBird");

    //register
    samsung.person = new GenericNotifier<int>(
        new IObserver<int>[]
        {
            yohan.personGetNotifiedFromCompany,
            gaelim.personGetNotifiedFromCompany,
            kimchi.personGetNotifiedFromCompany
        }
        );

    samsung.HeadHunter = new GenericNotifier<Tuple<string, string, int>>(
        new IObserver<Tuple<string, string, int>>[]
        {
            JobKorea.headHunterGetNotifiedFromCompany,
            Jasoseol.headHunterGetNotifiedFromCompany,
            SaramIn.headHunterGetNotifiedFromCompany
        }
        );

    SaramIn.person = new GenericNotifier<Tuple<string, string, string, int>>(
        new IObserver<Tuple<string, string, string, int>>[]
        {
            adkb.personGetNotifiedFromHeadHunter,
            BlueBird.personGetNotifiedFromHeadHunter
        }
        );

    Jasoseol.headHunter = new GenericNotifier<Tuple<string, string, string, int>>(
        new IObserver<Tuple<string, string, string, int>>[]
        {
            JobKorea.personGetNotifiedFromHeadHunter
        }
        );

    //notify
    samsung.NotifyNewCrewWanted();
    LG.NotifyNewCrewWanted();

}
````

결과물 
```` 
*** samsung company Notify that they want New Crew.
I'm YoHan. I reveived message from samsung. 1000000 KRW Job
I'm Gaelim. I reveived message from samsung. 1000000 KRW Job
I'm Kimchi. I reveived message from samsung. 1000000 KRW Job
I'm JobKorea HeadHunter. I received samsung notification.
*** JobKorea HeadHunter notify that he has something to you

I'm Jasoseol HeadHunter. I received samsung notification.
*** Jasoseol HeadHunter notify that he has something to you

I'm SaramIn HeadHunter. I received samsung notification.
*** SaramIn HeadHunter notify that he has something to you
I'm adkb.I received Message From SaramIn about samsung company
==samsung  : IT, CTO 2000000000 KRW job==
I'm BlueBird.I received Message From SaramIn about samsung company
==samsung  : IT, CTO 2000000000 KRW job==


*** LG company Notify that they want New Crew.
````

소스는 길어졌지만, `observer` 인터페이스는 간단명료해졌다.  
`Subject`는 이제 어떤 `Observer`를 기억하는 것이아니라, 어떤 `Observer`의 `Method`를 기억하는 것이다.   

````C#
//before

foreach (var p in subscriber) // subscriber 는 Observer 리스트이다.   
{
    if (p is Person) p.personGetNotifiedFromCompany(this, (int)1e6);
    else p.headhunterGetNotifiedFromCompany(this, "IT", "CTO", (int)2e9);
}

//after

public void companyNotifyObserver()
{
// Person과 HeadHunter 는 delegate 리스트이다.  
    person?.notify(this, (int)1e6); 
    HeadHunter?.notify(this, Tuple.Create("IT", "CTO", (int)2e9));
}
````

`Subject`의 함수만 보면 논리상 크게 달라진 점은 없어보인다.  
하지만 이 접근법을 통해   
1 `notify`의 인자를 일반화 시킬수 있었고,  
2 `Observer` 객체들은 자기가 수행하는 함수만 가질수 있게 되었다. (유지보수 좋음)  

### 6 .Net Solution

생각해보면 그냥 `Subject`에 `delegate`를 두고 해당 `Observer` 객체의 함수를 직접적으로 참조하면 될 것 이다.  
`EventHandler<TEventArgs>`를 사용해서 해결한다. 

````C#
public class Company 
{
    public event EventHandler<int> person;
    public event EventHandler<Tuple<string, string, int>> HeadHunter; 

    public string Name { get; }

    public Company(string name)
    {
        Name = name;
    }

    public void NotifyNewCrewWanted()
    {
        Console.WriteLine($"*** {Name} company Notify that they want New Crew.");
        companyNotifyObserver();
        Console.WriteLine();
    }

    public void companyNotifyObserver()
    {
        person?.Invoke(this, (int)1e6);
        HeadHunter?.Invoke(this, Tuple.Create("IT", "CTO", (int)2e9));
    }
}

 public class Person 
{
    public string Name { get; }

    public Person(string name)
    {
        Name = name;
    }

    public void personGetNotifiedFromCompanyHandler(object sender, int newPersonWage)
    {
        Console.WriteLine($"I'm {Name}. I reveived message from {(sender as Company).Name}. {newPersonWage} KRW Job");
    }

    public void personGetNotifiedFromHeadHunterHandler(object sender, Tuple<string , string , string , int> data )
    {
        Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {data.Item1} company");
        Console.WriteLine($"=={data.Item1}  : {data.Item2}, {data.Item3} {data.Item4} KRW job==");
    }
}

public class HeadHunter 
{
    public event EventHandler<Tuple<string, string, string, int>> person;
    public event EventHandler<Tuple<string, string, string, int>> headHunter;

    public string Name { get; }
    public HeadHunter(string name)
    {
        Name = name;
    }

    public void headhunterGetNotifiedFromCompanyHandler(object sender, Tuple<string, string, int> data)
    {
        Console.WriteLine($"I'm {Name} HeadHunter. I received {(sender as Company).Name} notification.");
        headhunterNotifyPerson((sender as Company).Name, data.Item1, data.Item2,data.Item3);
    }

    public void headhunterNotifyPerson(string companyName, string department, string level, int experiencedWage)
    {
        Console.WriteLine($"*** {Name} HeadHunter notify that he has something to you");
        person?.Invoke(this, Tuple.Create(companyName, department, level, experiencedWage));
        Console.WriteLine();
    }

    public void personGetNotifiedFromHeadHunterHandler(object sender, Tuple<string , string , string , int> data )
    {
        Console.WriteLine($"I'm {Name}.I received Message From {(sender as HeadHunter).Name} about {data.Item1} company");
        Console.WriteLine($"=={data.Item1}  : {data.Item2}, {data.Item3} {data.Item4} KRW job==");
    }
}

//initializing
Company samsung = new Company("samsung");
Company LG = new Company("LG");
HeadHunter SaramIn = new HeadHunter("SaramIn");
HeadHunter Jasoseol = new HeadHunter("Jasoseol");
HeadHunter JobKorea = new HeadHunter("JobKorea");
Person yohan = new Person("YoHan");
Person gaelim = new Person("Gaelim");
Person kimchi = new Person("Kimchi");
Person adkb = new Person("adkb");
Person BlueBird = new Person("BlueBird");

//register
samsung.person += yohan.personGetNotifiedFromCompanyHandler;
samsung.person += gaelim.personGetNotifiedFromCompanyHandler;
samsung.person += kimchi.personGetNotifiedFromCompanyHandler;

samsung.HeadHunter += JobKorea.headhunterGetNotifiedFromCompanyHandler;
samsung.HeadHunter += Jasoseol.headhunterGetNotifiedFromCompanyHandler;
samsung.HeadHunter += SaramIn.headhunterGetNotifiedFromCompanyHandler;

SaramIn.person += adkb.personGetNotifiedFromHeadHunterHandler;
SaramIn.person += BlueBird.personGetNotifiedFromHeadHunterHandler;

Jasoseol.headHunter += JobKorea.personGetNotifiedFromHeadHunterHandler;

//notify
samsung.NotifyNewCrewWanted();
LG.NotifyNewCrewWanted();
````
어떤가?   
소스가 훨씬 간편하고 가독성이 좋다.  
:smile:  





