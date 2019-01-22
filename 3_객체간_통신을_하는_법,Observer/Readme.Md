## 객체간 통신을 하는 법, Observer Pattern 

### introduction  

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

### 요구사항1(절망편)  

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

### 요구사항2(절망편)

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

*** Jasoseol HeadHunter notify that he has something to you
I'm JobKorea HeadHunter. I received your notification.
````

취미개발자 K씨... 당신은 대체....    

### 요구사항3(절망편)  

그러나 아직 취미개발자 K씨를 찬양하기는 이르다.  
그는 전화한통을 받았다.     

````
안녕하세요. K씨, 요구사항에 유연하게 대응해주셔서 정말 감사합니다.  
그런데 말이죠. 회사에서 구인광고를 낼 때 그 회사 이름과 평균연봉에 대한 정보를 제공해주고 싶거든요.  
그리고 헤드헌터에게는 
추가해줄 수 있을까요?   
````


