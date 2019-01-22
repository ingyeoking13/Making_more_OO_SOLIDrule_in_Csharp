## �߻� vs �������̽�  

### introduction
��ü������ �ϱ����ؼ� Ŭ������ ����Ѵ�. �ǰ� ������ Ŭ������ ȿ�����̰� ȿ�������� �ٰ��´�.  
�Ʒõ��� ���� ����鿡�Դ� �б� ���������, �� ������ Ŭ������ �ƹ��� ������ ������ �ʴ�.  

���� �������� �ۼ��ϰ� �ִ� �ҽ����� �� �� �����ϰ� Ŭ������ ��� �ϱ� �ٶ���.  
�׷����ϱ� ���ؼ� Ŭ������ ���� �̽Ű� ���� �κ��� ���־� �Ѵٰ� �����Ͽ���.  
�����ϱ� ������ ������ �߻�Ŭ���� vs �������̽� �̶�� �����ߴ�.  

### �츮 �������̽��� ���ؼ���� �̹� �������� �ʳ���?  

**��� ������ �׷���!**  
�� �������� �������̽��� Ȱ���غ��Ҵ�. �Ϻ� �������̽��� ĸ��ȭ �Ͽ� ����Ͽ���.  
�������̽��� ���� �� �� �ִ� �͵� �� �������̸鼭�� �����Ͽ���.   
����, �� �� ������ �� �ִ� ������ ������ �������̽��� �� �� �ִ� ���ҿ� ���� ����� ������ �� ����.      
�׸��� �������� ������ �� é�Ϳ����� �и��� �ٷ� ���̴�!.  

### �׷���, �� ���� �������� �߻� Ŭ������ ������� �ʳ���?  
 
�߻� Ŭ������ �������̽��� �������� �ִ�. �� �� ������ ����� �ſ� ���������� �������ش�.      

�ݸ鿡, �������� ������ ����.    
�߻� Ŭ������ �ʵ�(�������)�� ���� �� ������, Ư�� �������̽��� ���� �� �� �ִ�.  
�ݸ鿡 �������̽��� �ʵ带 ���� �� ������, �߻�Ŭ������ ����� �� ����.  

�׷����� �̷� �������� ���߿����� ���û��׿��� *���������� �ѷ��� ����*�� �����Բ� �������� �ʴ´�.    

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
*��å���� �������� �׷���, �������̽��� �߻�Ŭ������ ����ؾ� �ϳ� ����ϴ� �͸����ε� ��ü���� ���迡 �� ���� ����� �ȴ�. �ű⿡ ���Ǹ� ����.*    

### �׷��� ������ ����� å���� �����ϴ�.  

���û��׿� �־� �ǻ������ ������ �ش� ��ü�� ��� Ȯ��� �� ����� ������ ���߾�����.  
������, ������ ���谡 ��� �ٲ��� �����Ͽ� ���Ҹ� �������� �ʾƵ� �ȴ�.   

�ڵ� ������ ���ų� �̹� ������ ��� ���� �� �� �� �˰� �ִ� ���� ������ ��, �� �� �����ؾ��Ѵ�.   


