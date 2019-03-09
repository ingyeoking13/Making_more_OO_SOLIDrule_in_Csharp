## 상황 1.3 분기문을 피하자3

### 익명의 블로그에 있는 예를 가지고 오다. (절망편)

**설명**   

아래 소스는 음식 클래스에 대한 것이다. 음식 클래스의 동작은 단순하다.  
만약 아침이라면, 아침을 먹고 점심이라면 점심을 먹고, 저녁이라면 저녁을 먹는다.  

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
소스는 간단해서 보기가 쉽다. 하지만 조금 더 상황을 악화시켜 Meal 클래스를 다음과 같다고 하자.  

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

어때요, 조금 더 복잡한 것 같지 않나요? 

**요청사항**  
````
어... 음, K씨. 내가 Meal 클래스에 수정할 일이 있는데 말이야.  
eating() 함수에 사용된 멤버 변수들은 내가 사용하면 안되는거 맞지?   
````  
### 이렇게 작성하면 된다. (희망편)

만약 모든 문제가 잘 정의되어있고 모듈별로 각자의 책임을 다 하게끔 작성하였다면 `(SRP)`  
이런질문은 받지 않았을 것이다. 그렇지만 항상 잘 정의될 수 없다.   
아래는 어떻게 작성할지에 대한 예제 코드이다.  

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
} //.. Night 생략
````

`Meal` 클래스의 `primitive(bool)` 타입의 변수들을 개념적으로 묶어 객체화 하였기 때문에 좋다.  
그리고 다른 개발자가 보았을 때, `ITimeState` 타입의 변수만 건들지 않으면 되기에 좋다.  

위 예제는 사실 `State Pattern` 의 한 형태이다.  
하지만 `Pattern`을 외우는 것보다, 내 소스에서 어떤 smell이 난다고 생각할 때 사용해야 좋은 판단이라고 생각하기 때문에 여기서 서술하였다.  
