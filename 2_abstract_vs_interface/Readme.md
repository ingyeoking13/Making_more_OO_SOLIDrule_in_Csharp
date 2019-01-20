## �߻� vs �������̽�  

### introduction
��ü������ �ϱ����ؼ� Ŭ������ ����Ѵ�. �ǰ� ������ Ŭ������ �׻� ȿ�����̰� ȿ�������� �ٰ��´�. (�б�� ���������.)  
������ Ŭ������ �ƹ��� ������ ���� ���� ���� �ƴϴ�.  

�� �� �����ϰ� Ŭ������ �������ؼ� ������ �ѷ��� �ؾ��ϴµ�,  
�����ϱ� ������ ������ �߻�Ŭ���� vs �������̽� �̶�� �����ߴ�.  

### �츮 �������̽� ���� �� �ƴ� �� �ƴϿ���?

**�׷��鼭 �׷��� �ʴ�!**  
�� �������� �������̽��� Ȱ�뿡 ���� ���캸�Ҵ�. �� �߿� �������̽��� ĸ��ȭ �Ͽ� ����ϴ� �͵� �־���.  
�������̽��� ���� �� �� �ִ� �͵� �� �������̸鼭�� ������ �͵��̾���.   
����, �� �� �������� ������ ������ �������̽��� �� �� �ִ� ���ҿ� ���� ����� ������ �� ����.   
�׸��� �������� ������ �� é�Ϳ����� �и��� �ٷ� ���̴�!.

### �׷���, �� ���� �������� �߻� Ŭ������ ������� �ʳ���?  
 
�߻� Ŭ������ �������̽��� �������� �ִ�.  

�߻� Ŭ������ �ʵ�(�������)�� ���� �� ������, Ư�� �������̽��� ���� �� �� �ִ�.  
�ݸ鿡 �������̽��� �ʵ带 ���� �� ������, �߻�Ŭ������ ����� �� ����.  

�׷����� �̷� �������� ������ ���û��׿��� �ѷ��ϰ� �����ϰ� õ������ ������ �����Բ� �������� �ʴ´�.  

����,   
������ Ŭ������ Ư�� ��ɿ� �پ��� ������ �����ϰ� �ϱ� ���ؼ��� 
�������̽��� �����ϴ���, �߻�Ŭ������ �߻�޼���� ������ �� �ִ�.   
1_�б⹮�� ������2 ���� `Calculator`�� ����.    

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

classRoom Ŭ������ ����� iGawCalc�� ������ ��� ������ �����ϰԲ� �ߴ�.  
��... �߻�Ŭ�����δ� �ȵǳ���? `�ȵǴ� ���� �����ϴ�.`    
�߻�Ŭ������ Ȱ���� ���� �ڵ带 �ۼ��غ��Ҵ�!  

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

���ôٽ��� ��Ե� �䱸 ������ �� �� �ְԲ� ������ �� �ִ�.  
�߻� Ŭ������ �������̽� �� �ƹ��ų� �����ؼ� �����ϸ� �ȴ�!.  

### �׷��� ������ ����� å���� �����ϴ�.  

���û��׿� �־� �ǻ������ ������ �ش� ��ü�� ��� Ȯ��� �� ����� ������ ���߾�����.  
������, ������ ���谡 ��� �ٲ��� �����Ͽ� ���Ҹ� �������� �ʾƵ� �ȴ�.   

�ڵ� ������ ���ų� �̹� �� �˰� �ִ� ���� ������ ��, �� �� �����ؾ��Ѵ�.   


