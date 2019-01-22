## ��Ȳ 1.2 �б⹮�� ������2

### ��Ȳ (������)

**����**   

���ָ� ����, �������� ����б����� ������ �ű�� ���α׷��� �ۼ��Ѵٰ� ����.   
�̰��迭�� ���� �� ������ ���� ���ϴ� ���α׷��� �켱������ �ۼ��ؾ��ߴ�.  

�� �л��� ������ �����ϴ� ����� ������ ����.  
������ ������ �հ賻�� �Ͱ� �ణ �ٸ���, �л��� ���ϴ� ������ �Ѱ� �����Ͽ�  
�� ���� ���ؼ� ������*2 �� ����ϴ� ���̴�.  

�������� ����� ������ ���� �ҽ��� �ۼ��Ͽ���.  

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

            if (s.studentDesired == StudentDesired.english) // ������ �ڽ��� ��ǥ�� ���ٸ�
            {
                score += s.english * 2 + s.math + s.korean; // �ش� ���� ����ġ
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

�ۼ��� �ڵ�� �� ���� �Ͽ���, �����ϰ� ��ǰ�� ��������.  
������ ��ȭ�� �� �� �ɷ��´�.  

**��û����**  
````
��, �� �����ϴµ���. �׷��� �л��� ���� �ι����ִ� �Ϳ� ���ؼ� �߰������� �־��.  
���翡�� �л� ���� 2�����ִ°� ����, ����, ���� �� �ִµ���. ������ ��Ģ�� ����Ǿ� ��� �߰��ǳĸ� �ΰ��� ������ �� �ִ�  
<����, ����>, <����, ����>, <����, ����> �� �߰��Ǽ���.  
���� �� ������ 1.3�辿 ���ִ� ������ �ް� �Ͱŵ��.   
�׸��� �����ݿ� ���ؼ� �� ���� ���ϴ� ���� ���� �������θ� �ؼ� ������ �ް� �Ͱŵ��.  
������ �ϼ� �� �� �������? 
````  

�Ʒ��� ��� �ۼ������� ���� ���� �ڵ��̴�.  

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
        //���� ���� �հ� �Լ��� �������� ȣ�� �Ұ��̱� ������ ����.
    }

    public int getTotalScore_IGwa()
    {
        int score = 0;

        foreach (var s in students)
        {

            if (s.studentDesired == StudentDesired.english) // ������ �ڽ��� ��ǥ�� ���ٸ�
            {
                score += s.english * 2 + s.math + s.korean; // �ش� ���� ����ġ
            }
            else if (s.studentDesired == StudentDesired.korean)
            {
                score += s.korean * 2 + s.math + s.english;
            }
            //else  if .... added 4�� �� ���
        }

        return score;
    }
}
````

�Ƹ� �������� `classRoom` Ŭ������ `abstract`�� �ΰ� `����, �̰�classRoom`���� �и��ϰ� �;� �� �� �ִ�.  
�׷��� �ϴ���, ������ ������ ���� �ִ�.  

������ ����ϴ� �Ϳ� ���� ��Ģ�� ���� �߰� �� ������ ������ �ۼ��� `getTotal`�Լ��� ����Ͽ� �����Ͽ��� �Ѵٴ� ���� �ִ�.   
<����, ����, ����>, <�ƹ��͵� �������� ����> ���� ������ �� �߰� ���� ����� ���� �����ϱ� ���̴�.  
����, �Լ��� ���ǹ����� ��� ����� ���̴�.  

�� ��Ȳ�� �� �ܰ� �� ��ȭ ���Ѻ���.  
��� ��Ģ ������ ������, **�ٸ� �����ڰ� ������Ʈ�� �޶�پ� `Ư����classRoom`�� ���� `getTotal`�Լ��� �ۼ��Ѵٰ� �����غ���.**   
�� ����� �������� �̸� �ۼ��س��� `getTotal` �ҽ��� ����� �� ������? ���� �׷� �� ����. (�������� ������ ���� �ְ�����...)  
���� �ҽ��� `classRoom`�������� �ڽ��� ������ �л����� ������ �հ踦 ���ϴ� �Լ� `getTotal`���� �ʹ����� tight�ϰ� coupling�� �Ǿ��ִ� ���̴�.  
coupling�� �Ǿ��ִ� ���� `classRoom`, `��Ģenum`, `�л�`, `getTotal` �̴�.   
~~(�ۼ� �� �� �߿� �Ÿ��� Ÿ���� ����. ���� �ϵ� �ڵ��� �Ǿ���. )~~  
�� ������ ���� `getTotal` �Լ��� ���� ��Ģ�� ���� if���� �߰��ϸ鼭 �ڽ��� �ð��� ������ �� ���̴�.  


### �ǰ� �� �ذ�� (�����)  

**��������**  
1 �� �л��� ���� ������ ����ϴ� ��Ģ�� ��üȭ�Ѵ�.  
2 ��Ģ���� ������ ��üȭ �Ѵ�.  `be OO!`   

**������� ����� ���ϴ�.**  
�л��� `studentDesired`�� ���� ������ ����ϴ� ��Ģ�� �������̽�ȭ �Ѵ�.  
�׸��� �� ��Ģ Ŭ�������� �� �������̽��� �����Ѵ�.  
�̰�, ������ ���� ����Ǵ� ��Ģ���� �ٸ��Ƿ�, �� ��Ģ���� ������ �ٷ� ���� ��üȭ ���ش�.  
�̷��� �ۼ��س�����, Ư���ݿ� ���ؼ��� ��Ģ���� ���� �־��ֱ⸸ �ϸ� ������ ������ �� �ִ�. 


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
} // rule �߷� .... 

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

**������**  
````
��... ���� ��Ģ�� �ִ��� ����� ���Ҵµ�, �� �߰� �� ��Ģ�� IRule �������̽��� �����Ͻø� �Ǳ���.  
��, �׸��� �̹��� C���� Ư���� �� �����հ� �ۼ��ϽŴٰ� ���ݾƿ�?   
�׷��� ICalc �������̽� �����س�������, �� �������̽��� Ư���� �� �ۼ��ϼ���.  
````  
:smile:

������ �޶����°�?  
������ ��Ģ�� �߰��Ǹ�, `�̰�, ����, Ư���� .... etc ClassRoom`�� �������  `IRule` �������̽��� �� ��Ģ Ŭ������ �����ϸ� �ȴ�.  
`XX�� ClassRoom`�� �߰��Ǹ�, ��Ģ Ŭ�����鸸 �����Ͽ�, �ش� `XX�� ClassRoom`�� ��� ���ǿ� �´� ��Ģ Ŭ�������� �߰����ָ� �ȴ�.   
�����ڿ� �Ѱ��ִ� ���� ��������.

��, ������ `classRoom`, `���enum`, `�л�`, `getTotal` ��ΰ� �����ϰ� ����Ǿ���.  

### ��Ʈ!

ù ��° ������ �� ��° ������ �������� �˰ڴ°�?   
class�� �ڽ��� Ư�� �Լ��� ������ �����ϴ� �κ�(�˰���)�� �ʵ�� ������ �ְԲ��Ͽ� �������� �����ϰ� �Ͽ���.  
�̸� `strategy pattern` �̶�� �Ѵ�. 

�׷����� ���ۺ��� �������� �Ӹ��� ������ �̸����� ���� ä��� �ϰ� ���� �ʴ�.   
�׸��� `design pattern`�̶�� ������ å�� ������ �׷� ������� �� ���� �Ǿ��ִ�.    
�� �������� �������� ������ ��Ȳ�� �������� ��, *��! ��Ģ�� ���� ��ü�� �����߰ڱ���*��� ��������� ���ڴ�.   

