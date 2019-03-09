## ��Ȳ 1.3 �б⹮�� ������3

### �͸��� ��α׿� �ִ� ���� ������ ����. (������)

**����**   

�Ʒ� �ҽ��� ���� Ŭ������ ���� ���̴�. ���� Ŭ������ ������ �ܼ��ϴ�.  
���� ��ħ�̶��, ��ħ�� �԰� �����̶�� ������ �԰�, �����̶�� ������ �Դ´�.  

```` C# 
public class Meal
{
    bool morning = true;
    bool evening = false;
    bool night = false;

    public void eating()
    {

        if (morning)
        {
            DoMorning();
        }
        else if (evening) DoEvening();
        else DoNight();
    }
    public void DoMorning() { }
    public void DoEvening() { }
    public void DoNight() { }
}
```` 
�ҽ��� �����ؼ� ���Ⱑ ����. ������ ���� �� ��Ȳ�� ��ȭ���� Meal Ŭ������ ������ ���ٰ� ����.  

```` C# 
public class Meal
{
    //.......
    bool morning = true;
    bool evening = false;
    bool night = false;
    //.......

    public void eating()
    {

        if (morning)
        {
            DoMorning();
        }
        else if (evening) DoEvening();
        else DoNight();

    }

    //.......
    public void DoMorning() { }
    public void DoEvening() { }
    public void DoNight() { }
    //.......
}
```` 

���, ���� �� ������ �� ���� �ʳ���? 

**��û����**  
````
��... ��, K��. ���� Meal Ŭ������ ������ ���� �ִµ� ���̾�.  
eating() �Լ��� ���� ��� �������� ���� ����ϸ� �ȵǴ°� ����?   
````  
### �̷��� �ۼ��ϸ� �ȴ�. (�����)

���� ��� ������ �� ���ǵǾ��ְ� ��⺰�� ������ å���� �� �ϰԲ� �ۼ��Ͽ��ٸ� `(SRP)`  
�̷������� ���� �ʾ��� ���̴�. �׷����� �׻� �� ���ǵ� �� ����.   
�Ʒ��� ��� �ۼ������� ���� ���� �ڵ��̴�.  

````C#
public class Meal
{
    ITimeState nowState { get; set; }

    public Meal(ITimeState nowState)
    {
        this.nowState = nowState;
    }
    public void eating()
    {
        nowState.eating();
    }

    public void TimeProceed()
    {
        nowState = nowState.TimeProceed();
    }

}

public interface ITimeState
{
    void eating();
    ITimeState SetTimeMorning();
    ITimeState SetTimeEvening();
    ITimeState SetTimeNight();
    ITimeState TimeProceed();
}

public class Morning : ITimeState
{
    public ITimeState SetTimeMorning() => this;
    public ITimeState SetTimeEvening() => new Evening();
    public ITimeState SetTimeNight() => new Night();
    public ITimeState TimeProceed() => new Evening();
    public void eating()
    {
        //Do MorningEating();
    }
}

public class Evening : ITimeState
{
    public ITimeState SetTimeMorning() => new Morning();
    public ITimeState SetTimeEvening() => this;
    public ITimeState SetTimeNight() => new Night();
    public ITimeState TimeProceed() => new Night();
    public void eating()
    {
        //Do EveningEating();
    }
} //.. Night ����
````

`Meal` Ŭ������ `primitive(bool)` Ÿ���� �������� ���������� ���� ��üȭ �Ͽ��� ������ ����.  
�׸��� �ٸ� �����ڰ� ������ ��, `ITimeState` Ÿ���� ������ �ǵ��� ������ �Ǳ⿡ ����.  

�� ������ ��� `State Pattern` �� �� �����̴�.  
������ `Pattern`�� �ܿ�� �ͺ���, �� �ҽ����� � smell�� ���ٰ� ������ �� ����ؾ� ���� �Ǵ��̶�� �����ϱ� ������ ���⼭ �����Ͽ���.  
