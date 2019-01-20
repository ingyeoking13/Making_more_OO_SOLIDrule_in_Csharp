## 추상 vs 인터페이스  

### introduction
객체지향을 하기위해서 클래스를 써야한다. 옳게 쓰여진 클래스는 항상 효과적이고 효율적으로 다가온다. (읽기는 힘들겠지만.)  
적절한 클래스는 아무리 많더라도 절대 나쁜 것이 아니다.  

좀 더 현명하게 클래스를 쓰기위해선 개념을 뚜렷히 해야하는데,  
시작하기 적절한 주제는 추상클래스 vs 인터페이스 이라고 생각했다.  

### 우리 인터페이스 거의 다 아는 거 아니에요?

**그러면서 그렇지 않다!**  
앞 예제에서 인터페이스의 활용에 대해 살펴보았다. 그 중엔 인터페이스를 캡슐화 하여 사용하는 것도 있었다.  
인터페이스를 통해 할 수 있는 것들 중 기초적이면서도 강력한 것들이었다.   
아직, 좀 더 응용적인 주제는 있지만 인터페이스가 할 수 있는 역할에 대해 충분히 설명한 것 같다.   
그리고 응용적인 주제는 뒤 챕터에서도 분명히 다룰 것이다!.

### 그래서, 왜 저번 예제에서 추상 클래스를 사용하지 않나요?  
 
추상 클래스와 인터페이스는 공통점이 있다.  

추상 클래스는 필드(멤버변수)를 가질 수 있으며, 특정 인터페이스로 구현 할 수 있다.  
반면에 인터페이스는 필드를 가질 수 없으며, 추상클래스를 상속할 수 없다.  

그렇지만 이런 차이점은 개발의 선택사항에서 뚜렷하고 현명하고 천재적인 결정을 내리게끔 도와주진 않는다.  

예로,   
임의의 클래스에 특정 기능에 다양한 구현을 가능하게 하기 위해서는 
인터페이스로 구현하던지, 추상클래스의 추상메서드로 구현할 수 있다.   
1_분기문을 피하자2 에서 `Calculator`를 보자.    

````C#
public interface ICalc
{
    int getScore(Student student);
}

public class iGawCalc : ICalc
{
    private readonly List<IRule> rules;
    public iGawCalc()
    {
        rules = new List<IRule>();
        rules.Add(new koreanRule());
        rules.Add(new englishRule());
        rules.Add(new koreanRule());
        rules.Add(new korean_mathRule());
    }

    public int getScore(Student student)
    {
        return rules.First(rules => rules.isMatch(student)).myScore(student);
    }
}

class classRoom
{
    private readonly List<Student> students;
    private readonly ICalc calc;

    public classRoom(ICalc calcc)
    {
        students = new List<Student>();
        calc = calcc;
    }

    public int getTotalScore()
    {
        int score = 0;
        foreach (var student in students)
        {
            score += calc.getScore(student);
        }
        return score;
    }
}


````  

classRoom 클래스는 멤버로 iGawCalc를 가져서 계산 동작을 수행하게끔 했다.  
음... 추상클래스로는 안되나요? `안되는 것은 없습니다.`    
추상클래스를 활용해 다음 코드를 작성해보았다!  

````C#
public abstract class Igwacalculatorabstract
{
    public abstract void getscore();
    protected readonly List<IRule> rules;
    public calculatorabstract()
    {
        rules = new List<IRule>();
        rules.Add(new koreanRule());
        rules.Add(new englishRule());
        rules.Add(new koreanRule());
        rules.Add(new korean_mathRule());
    }
}

class inheritedClassRoom  : Igwacalculatorabstract
{
    private readonly List<Student> students;

    public inheritedClassRoom()
    {
        students = new List<Student>();
    }

    public override void getscore()
    {
        int score = 0;
        foreach (var student in students)
        {
            score += 
            rules.First(rules => rules.isMatch(student)).myScore(student);
        }
    }
}
````

보시다시피 어떻게든 요구 사항을 할 수 있게끔 구현할 수 있다.  
추상 클래스나 인터페이스 중 아무거나 선택해서 개발하면 된다!.  

### 그러나 선택의 결과엔 책임이 따릅니다.  

선택사항에 있어 의사결정은 앞으로 해당 객체가 어떻게 확장될 수 있을까에 초점이 맞추어진다.  
하지만, 앞으로 설계가 어떻게 바뀔지 유추하여 섣불리 결정하지 않아도 된다.   

코딩 스멜이 나거나 이미 잘 알고 있는 설계 과정일 때, 그 때 선택해야한다.   


