## 객체를 만드는 이유

이전 주제는 상당히 길었다. 이번에는 간단한 주제에 해보려고 한다.  
객체를 생성하는 이유는 뭘까?   
이제까지 살펴 본 것들은 어떤 행위를 객체화 하기위해 ([`strategy pattern`](../1_분기문을피하자/Readme.Md),[`strategy pattern2`](../1_분기문을피하자2/Readme.Md))     
또는 어떤 객체의 상태를 표현하기위해 ([`state pattern`](../1_분기문을피하자3/Readme.md)) 였다.  

이번에는 객체의 다형성을 연쇄적으로 사용해보겠다.  

### 나는 누구인가요?

나를 표현하기 위해 클래스를 하나 생성 해 보았다.  
나를 일반화 하여 표현하기 위해, 사람이라는 클래스를 생성하였다.  
이름과 자기 소개 메서드가 있다.  

````C#
public class Person
{
	public string Name { get; }

	public Person(string name) {
		Name = name;
	}
	public virtual string description { get => "사람"; }
	public string Introduce()=> "이름이 " + Name + "이며, "  + description + "입니다";
}
````

### 그런데 나는 사람이면서 ... OO 입니다.  

사람들은 보통 여러 사회적 위치를 가지고 있다.   
나는 사람이면서 학생이다. 또, 프리터이기도 하다. (실제로 두 가지가 동시에 가능하진 않겠지만. 일단은. )     
그리고 프로그래밍을 좋아한다.   

이런 사람들이 한 둘이 아니기 때문에, 나는 일반화 하기 위해 클래스를 만들어야 한다.  
그리고 각각 직업마다 또는 취향 마다 종종 하는 행위들이 있을것이다. 그런 행위들을 메서드로 만든다고 가정하자.  

### 첫번째 시도 

나는 우선 `Person` 클래스를 제일 위 클래스로 두고 나머지 클래스들은 그 `Person` 을 상속하기로 한다.   
그래서 다음과 같은 소스가 나온다.  

````C#
class Person {}
class Student : Person {}
class Programming : Person {}
class Freeter : Person {} 
````

그런데 이렇게 작성하고 난 뒤 더 이상 작성할 수 가 없다.   
`나`라는 `ojbect`는 `Person` 이자 `Student`이자 `Programming`(을좋아하는)이자 `Freeter`인데,   
이 객체를 사실상 만들 수 있는가? C#은 다중 상속을 제공해주지 않는다.    
아래는 내가 표현하고자 하는 예이다.     
````C# 
class 나같은놈(학생이며,프리터이며,프로그래밍을 좋아하는) : Programming, Freeter, Student {} 
````
설령 개발환경에서 다중 상속이 된다고 하자. `(Remember C++)`      
그리고 여러분이 위와 같은 솔루션을 생각했다면, 다음 예제를 보자.  

````C# 
class 너같은놈(직장인이며, 프로그래밍을 흥미있어하는) : LikeProgramming, Employee {} 
````
다음과 같은 클래스도 만들어 주어야하는가?   
만약 그렇다면, 한명이 최대 10개의 성격을 띈다고 할 때 클래스가 생성되는 총 가지수는 2^10개이다.   

그러나 개발에서 사이즈가 작을 땐 나쁘진 않을 것 이다. (다중 상속이 된다면)

### 두번째 시도 
 
첫번째 시도가 썩 좋지 않은 결과로 이어질 수 있다는 건 어쩐지 두려운 일이다.   
처음부터 제대로 하기란 쉽진 않지만, 역시 클래스 간 설계를 어떻게 구성하는가에 따라  
차후에 발생할 결과를 조금은 예측하는 것이 중요하다. 라고 생각한다.   

두번째 시도엔 나는 이런 접근을 했다. 

사람은 사람인데, `A`, `B`, `C` 성격을 가지니 ...   
클래스가 변수를 가지는 것과 비슷하네 ...    
그럼 이런 성격(`프리터, 학생, 프로그래밍을 좋아함`) 을 인터페이스화 시키고 필드로 가지게 하자.  

````C#
class Person 
{
	public List<성격> 성격list {get;}
}
````

나쁘지 않다. 성격은 인터페이스 또는 클래스로 생성 작성하면 된다.   
그리고 세부적인 성격은 작성된 인터페이스로 구현하던 상속받아 구현하면 된다.   

````C#
public interface 성격 
{
	TimeSpan 성격의기간 {get; set;}
	void doMethod();
	// and so on
}
public class programming : 성격 { }
public class freeter : 성격 { }
````

소스를 통해 좋은 결과가 나올 것 이다.      

이전 소스를 보자. `성격`과 `Person` 클래스는 어떤 성격을 갖는가?   
`성격`이 `Person`을 꾸며주는 역할을 한다.    
`List<성격>` 의 `size`가 많으면 많을수록 많은 성격을 가지고 있는 것이다.   

그리고 성격을 추가하거나 단일 성격에 대해 수정하려고하면, 해당 성격 클래스만 수정하면 된다.  

이번엔 재밌는 것을 보여주려고 한다.   

### 세번째 시도

`성격`은 `Person`을 꾸며주는데, `Person`은 `List`로 성격을 가지고 있다.    
여러가지의 성격을 다른 리스트로 치환할 수 있을까?   
아래의 소스를 보자.  

````C#
public class Person
{
	public string Name { get; }

	public Person(string name) {
		Name = name;
	}

	public virtual string description { get => "사람"; }
	public string Introduce()=> "이름이 " + Name + "이며, "  + description + "입니다";
}

public class Freeter : Person
{
	public Person A { get; }
	public Freeter(Person a) : base(a.Name)
	{
		A = a;
	}
	public override string description => "프리터이고 " + A.description;
}

public class Programming : Person
{
	public Person A { get; }
	public Programming(Person a) : base(a.Name)
	{
		A = a;
	}

	public override string description => "프로그래밍 좋아하는 " + A.description;
}
````

각 성격 클래스는 `Person` 에서 상속하며, `Person` 타입의 필드를 가지고 있다.   
이 것은 링크드 리스트와 같은 결과를 낼 수 있다. `Person`에서 유래되는 모든 클래스들을 일렬로 세울 수 있는 점이다.      

처음에 무슨 성격을 가지든, 그 성격의 클래스로 작성한다.   
만약 새 성격을 덧대어 부여하고 싶으면 이전 그 클래스의 성격의 정보를 그대로 가르키는 객체를 필드로 가지게끔 하면된다.  

````C#
static void Main(string[] args)
{ 
	Person kimchi = new Person("김치");
	Console.WriteLine(kimchi.Introduce());

	kimchi = new Student(kimchi);
	Console.WriteLine(kimchi.Introduce());

	kimchi = new Programming(kimchi);
	Console.WriteLine(kimchi.Introduce());

	kimchi = new Freeter(kimchi);
	Console.WriteLine(kimchi.Introduce());

	Person yohan = new Freeter(new Student(new Programming(new Person("요한"))));
	Console.WriteLine(yohan.Introduce());
}
````

이 방법도 나쁘지 않다.    
이 전략을 `decrator Pattern`이라고 한다. 
책에서 소개되는 `decorator pattern` 은 좀 더 클래스 구성이 다양하다.  
그리고 이것보다 더 복잡할 것이다.  
하지만 원리는 이 것과 같다.

아래 예를 보자. 
````
A (abstract)
ㄴ B
   ㄴ Ba
      ㄴ Baa
      ㄴ Bab
   ㄴ Bb
   ㄴ Bc
ㄴ C (decorator)
   ㄴ Ca
   ㄴ Cb
   ㄴ Cc
````

만약 위에서 서술 했던 2번째 시도로 접근한다면 C를 A트리 바깥으로 꺼낼 수 있을 것이다.  
그 접근 또한 나쁘지 않다고 생각한다.  

`decorator pattern`으로 작성한다면, 좋은 것은 C는 A에 작성된 메서드 행위에 대해 공유 하고 있는 점이다.  
따라서, 소스 작성자가 이 C 클래스의 책임에 대해 **"분리해서 작성된 것"** 보다 명확하게 알 수 있을 것이다.    

위 트리에서 C 클래스는 A 클래스 필드를 가지고 있기만 하면 어떤 B 타입에 대해 가르킬 수 있다.    
그리고 계속해서 C 클래스를 덧댈 수 있는 것이다.  

책에서는 위 클래스 트리 보다 쉬운 클래스 개요를 썼다.   
하지만 원리는 같다.    
:smile:




