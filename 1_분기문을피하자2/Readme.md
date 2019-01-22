## 상황 1.2 분기문을 피하자2

### 상황 (절망편)

**설명**   

외주를 통해, 여러분은 고등학교에서 점수를 매기는 프로그램을 작성한다고 하자.   
이과계열의 반의 총 점수의 합을 구하는 프로그램을 우선적으로 작성해야했다.  

한 학생의 점수를 도출하는 방식은 다음과 같다.  
무작정 점수를 합계내는 것과 약간 다른데, 학생이 원하는 과목을 한개 선택하여  
그 과목에 대해선 원점수*2 로 계산하는 것이다.  

개발자인 당신은 다음과 같은 소스를 작성하였다.  

```` C# 
public enum StudentDesired
{
    korean, math, english
};

class Student
{
    public int korean { get; set; }
    public int math { get; set; }
    public int english { get; set; }
    public StudentDesired studentDesired { get; set; }
}

class classRoom
{
    private readonly List<Student> students;

    public classRoom()
    {
        students = new List<Student>();
    }

    public int getTotalScore()
    {
        int score=0;

        foreach (var s in students)
        {

            if (s.studentDesired == StudentDesired.english) // 만약의 자신의 목표와 같다면
            {
                score += s.english * 2 + s.math + s.korean; // 해당 점수 가중치
            }
            else if (s.studentDesired == StudentDesired.korean)
            {
                score += s.korean * 2 + s.math + s.english;
            }
            else score += s.math * 2 + s.english + s.korean;

        }

        return score;
    }
}

````

작성한 코드는 잘 동작 하였고, 만족하고 제품을 보내었다.  
다음날 전화가 한 통 걸려온다.  

**요청사항**  
````
예, 잘 동작하는데요. 그런데 학생들 점수 두배해주는 것에 대해서 추가사항이 있어서요.  
현재에는 학생 점수 2배해주는게 국어, 수학, 영어 만 있는데요. 앞으로 학칙이 변경되어 어떤게 추가되냐면 두개씩 선택할 수 있는  
<영어, 수학>, <수학, 국어>, <국어, 영어> 가 추가되서요.  
각각 원 점수에 1.3배씩 해주는 조건을 달고 싶거든요.   
그리고 문과반에 대해서 총 점수 구하는 것은 기존 조건으로만 해서 점수를 받고 싶거든요.  
언제쯤 완성 될 수 있을까요? 
````  

아래는 어떻게 작성할지에 대한 예제 코드이다.  

````C#
public enum StudentDesired
{
    korean, math, english,
    korean_math, math_english, english_korean // added
};

class classRoom
{
    public int getTotalScore()
    { 
        //기존 점수 합계 함수는 문과에서 호출 할것이기 때문에 나둠.
    }

    public int getTotalScore_IGwa()
    {
        int score = 0;

        foreach (var s in students)
        {

            if (s.studentDesired == StudentDesired.english) // 만약의 자신의 목표와 같다면
            {
                score += s.english * 2 + s.math + s.korean; // 해당 점수 가중치
            }
            else if (s.studentDesired == StudentDesired.korean)
            {
                score += s.korean * 2 + s.math + s.english;
            }
            //else  if .... added 4개 더 등록
        }

        return score;
    }
}
````

아마 여러분은 `classRoom` 클래스를 `abstract`로 두고 `문과, 이과classRoom`으로 분리하고 싶어 할 수 있다.  
그렇게 하더라도, 여전히 문제인 점이 있다.  

점수를 계산하는 것에 대해 규칙이 새로 추가 될 때마다 기존에 작성한 `getTotal`함수를 계속하여 수정하여야 한다는 점이 있다.   
<국어, 수학, 영어>, <아무것도 가산하지 않음> 조건 따위가 또 추가 되지 말라는 법은 없으니까 말이다.  
따라서, 함수는 조건문으로 계속 길어질 것이다.  

이 상황을 한 단계 더 악화 시켜보자.  
계산 규칙 문제와 별개로, **다른 개발자가 프로젝트에 달라붙어 `특수반classRoom`에 대한 `getTotal`함수를 작성한다고 생각해보자.**   
그 사람은 여러분이 미리 작성해놓은 `getTotal` 소스를 사용할 수 있을까? 절대 그럴 수 없다. (논리구조를 참고할 수는 있겠지만...)  
기존 소스는 `classRoom`에서부터 자신이 보유한 학생들의 점수의 합계를 구하는 함수 `getTotal`까지 너무나도 tight하게 coupling이 되어있는 것이다.  
coupling이 되어있는 것은 `classRoom`, `규칙enum`, `학생`, `getTotal` 이다.   
~~(작성 된 것 중에 거르는 타선이 없다. 모든게 하드 코딩이 되었다. )~~  
그 개발자 또한 `getTotal` 함수에 점수 규칙에 따라 if문을 추가하면서 자신의 시간을 보내야 할 것이다.  


### 옳게 된 해결법 (희망편)  

**문제정의**  
1 한 학생에 대해 점수를 계산하는 규칙을 객체화한다.  
2 규칙들의 묶음을 객체화 한다.  `be OO!`   

**기술적인 방법을 말하다.**  
학생의 `studentDesired`에 따라 점수를 계산하는 규칙을 인터페이스화 한다.  
그리고 각 규칙 클래스들을 그 인터페이스로 구현한다.  
이과, 문과에 대해 적용되는 규칙들이 다르므로, 그 규칙들의 묶음을 다룰 것을 객체화 해준다.  
이렇게 작성해놓으면, 특수반에 대해서도 규칙들의 셋을 넣어주기만 하면 간단히 구현될 수 있다. 


````C# 
public interface ICalc
{
    int getScore(Student student);
}

public interface IRule
{
    bool isMatch(Student student);
    int myScore(Student student);
}

public class mathRule : IRule
{
    public bool isMatch(Student student)
    {
        return (student.studentDesired == StudentDesired.math);
    }

    public int myScore(Student student)
    {
        return student.math * 2 + student.korean + student.english;
    }
}

public class englishRule : IRule
{
    public bool isMatch(Student student)
    {
        return (student.studentDesired == StudentDesired.english);
    }

    public int myScore(Student student)
    {
        return student.english * 2 + student.korean + student.math;
    }
} // rule 중략 .... 

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
 
class mGwaCalc : ICalc
{
    private readonly List<IRule> rules;
    public mGwaCalc()
    {
        rules = new List<IRule>();

        rules.Add(new koreanRule());
        rules.Add(new englishRule());
        rules.Add(new koreanRule());
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

**개발자**  
````
음... 제가 규칙은 최대한 만들어 놓았는데, 또 추가 될 규칙은 IRule 인터페이스로 구현하시면 되구요.  
아, 그리고 이번에 C씨가 특수반 쪽 점수합계 작성하신다고 했잖아요?   
그러면 ICalc 인터페이스 구현해놓았으니, 그 인터페이스로 특수반 쪽 작성하세요.  
````  
:smile:

무엇이 달라졌는가?  
점수의 규칙이 추가되면, `이과, 문과, 특수반 .... etc ClassRoom`와 관계없이  `IRule` 인터페이스로 새 규칙 클래스를 구현하면 된다.  
`XX반 ClassRoom`가 추가되면, 규칙 클래스들만 참조하여, 해당 `XX반 ClassRoom`의 계산 조건에 맞는 규칙 클래스들을 추가해주면 된다.   
생성자에 넘겨주는 것은 잊지말고.

즉, 기존의 `classRoom`, `계산enum`, `학생`, `getTotal` 모두가 느슨하게 연결되었다.  

### 힌트!

첫 번째 예제와 두 번째 예제의 공통점을 알겠는가?   
class는 자신의 특정 함수의 동작을 정의하는 부분(알고리즘)을 필드로 가지고 있게끔하여 동적으로 결정하게 하였다.  
이를 `strategy pattern` 이라고 한다. 

그렇지만 시작부터 여러분의 머리를 패턴의 이름으로 가득 채우게 하고 싶지 않다.   
그리고 `design pattern`이라는 제목의 책을 읽으면 그런 내용들이 잘 설명 되어있다.    
이 예제들이 여러분이 동일한 상황에 마주쳤을 때, *아! 규칙에 대해 객체를 만들어야겠구나*라고 상기했으면 좋겠다.   

