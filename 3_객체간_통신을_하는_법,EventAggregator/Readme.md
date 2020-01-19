# 이벤트 노티피케이션, EventAggregator    

## intro.

클래스간 통신을 하기 위해선 여러가지 방법이 있습니다. 
객체간 통신을 하는 법, Observer Pattern에서는 클래스간 다이렉트 하게 `이벤트를 Listen`하는 구조를 구현하였습니다.  
실제 개발에서 사용하게 되면 굉장히 소스가 난잡하겠지요. 프로그램-특히, 어플리케이션-에서 이벤트를 관리는 주로 한 클래스가 도맡아서 하게 됩니다.  

```
리스너 ->( 나 'A' 이벤트에 대해 들을래요, Regist/Subscribe ) -> 이벤트 중개 클래스   
이벤트 중개 클래스 -> (오키오키, 너도 'A'이벤트 구독자 중 한명으로 등록 해둘게)  

퍼블리셔 -> ( 나 'A' 이벤트 구독자들한테 알릴래요 , Post/Publish ) -> 이벤트 중개 클래스 
이벤트 중개 클래스 -> (오키오키, 'A' 이벤트 구독자들 한테 알릴게요)  
```

예제 상황과 그에 대응한 프로그램, 그리고 두 가지 이벤트 클래스 내용에 대해 공유해보겠습니다.    

우선 `상황`에 대해 제시하겠습니다. 

## 상황

대학에서 사용될 강의 과목 관련 프로그램 제작한다고 가정합시다. 
학생, 교수님, 대학 교직원이 이 프로그램을 사용하게 될 것입니다. 

1. 학생은 본인의 미출석에 대해 통보를 할 수 있고 대상은 교수님입니다. 사유를 제공할 수 있습니다.   
2. 교수님은 다음과 같은 정보를 제공할 수 있습니다.   
    2.1 교수 자신의 미출석에 대해 학생들에게 통보를 할 수 있고, 사유 및 보강 수업 시간표를 제공할 수 있습니다.  
    2.2 학생들에게 시험 일정에 대해 통보 할 수 있고, 시험 과목 범위와 시험 일정을 제공할 수 있습니다.   
    2.3 몇 몇 학생들에게 수업에서 쫓겨남을 통보 할 수 있고, 과 사무실 직원 및 해당 학생들이 듣게 됩니다. 사유를 제공할 수 있습니다.  
3. 과 사무실 직원은 과목에 대한 정보들을 제공할 수 있습니다. 정보의 종류는 다음과 같습니다.   
    3.1 해당 수업이 폐지 됨을 알릴 수 있습니다. 학생들 및 교수에게 알릴 수 있습니다.   

작은 시연 상황인데도, 꽤 복잡하지요? 실제 어플리케이션에서 이벤트의 구성은 더 촘촘하고 복잡합니다.       
이 작은 예제를 이용해 이벤트 관리 클래스 두 개를 비교하여, 어떤 것이 더 우위에 있는지 비교해볼 것입니다.  
압도적으로 후자의 솔루션을 더 좋아하게 될 겁니다.    

우선은, 어떻게 이벤트를 관리하게 될지 `설계`를 제안할 것입니다.  

## 설계 

강의는 여러 학생을, 한 교수를, 한 직원을 가질 수 있다고 합시다. 
단, 상황의 편의를 위해 한 학생은 단 하나의 수업만 들을 수 있고, 그리고 단 하나의 강의만 존재하는 상황을 봅시다.    
우선은 큰 설계는 다음과 같습니다.  (스도코드)  

```c#  
-- 객체 명세서 
List<Student>   
Professor  
Employee  
-- 명세서 끝

-- 이벤트 정의서 
- 학생의 결석 "STUDENT-ABSENT"  
Student.OnAbsent()
{
    Publish("STUDENT-ABSENT", 
            // 학생 아이디
            this.myId, 
            // 결석 사유 
            "I'M SICK");
}

Prof.constructor()
{
    Subscribe(
        "STUDENT-ABSENT", (id, reason)=> { 
            Write($"{id} is ABSENT, Reason is {reason}"});
}

- 교수의 결석 "PROF-ABSENT"  
Prof.OnAbsent()
{
    Publish("PROF-ABSENT", 
            // 결석 사유 
            "I'M SICK", 
            // 보충 수업 시간 
            new DateTime(2020, 01, 20, 13, 50));
}

Student.constructor()
{
    Subscribe(
        "PROF-ABSENT", (reason, dateTime) =>{
            Wrtie($"Professor is absent. Reason is {reason}. Alternate lecture time is {dateTime}"});
}

- 교수의 시험범위 알림, "PROF-TESTNOTIFY"
Prof.OnTestNotify()
{
    Publish("PROF-TESTNOTIFY", 
            // new TestRange(시작구간, 끝구간(열린 구간))
            new TestRange(rangeStart, rangeEnds);
            new DateTime(2020, 01, 21, 13, 50));
}

Student.constructor()
{
    Subscribe(
        "PROF-TESTNOTIFY", 
         (testRange, dateTime) =>{
            Write($"test Range is {testRange.start} ~ {testRange.rangeEnds}. Date is {dateTime}");
        });
}

- 교수의 퇴출 통보 "PROF-BANNOTIFY" 
Prof.OnBan()
{
    Publish("PROF-BANNOTIFY", 
            //퇴출 당하는 학생들의 아이디 리스트 
            new List<integer>{ 1, 3 });
}

Student.constructor()
{
    Subscribe("PROF-BANNOTIFY", (list )=>{ 
              if ( list.FindOrDefuault(studentId=>studentId == myId) != 0)
                  Write(" .... IM BANNED SAD ... "); 
    });
}

Employee.constructor()
{
    Subscribe("PROF-BANNOTIFY", (list)=>{
        foreach(var st in list)
            Write($"OKAY, {st.myId} is Out"); });
}

- 직원의 폐강 통보 "EMPLOYEE-CLOSE"
Employee.OnClose()
{
    Publish("EMPLOYEE-CLOSE");
}

Student.constructor()
{
    Subscribe("EMPLYOEE-CLOSE", ()=>{ Write("I'M SAD");});
}

Prof.constructor()
{
    Subscribe("EMPLOYEE-CLOSE", ()=>{Write("I'M SAD,TOO");});
}
```  

이제, `Naive`한 이벤트 관리 클래스를 보겠습니다.  

## Naive EventCenter.   

바로 소스를 보기전에 소스의 맥락을 설명하겠습니다.  
우선 이벤트 관리 클래스는 기본적으로 어플리케이션에 단 하나만 존재할 것 입니다. 
따라서, 이벤트 관리 클래스는 싱글톤 또는 스태틱으로 제작합니다.   
이벤트가 발생하면서 넘어가는 정보(인자)들은 모두 object를 

```c#
public sealed class EventCenter
{
    static EventCenter _instance = null;
    static readonly object _padlock = new object();

    public delegate void standardEventHandler(object sender, NotifyEventArgs e);

    // 이벤트name당 여러 개의 이벤트핸들러를 등록시킬 수 있다.
    public static Dictionary<string, List<standardEventHandler>> _dicEventHandlers 
        =new Dictionary<string, List<standardEventHandler>>();
    EventCenter()
    {
    }

    public static EventCenter defaultCenter
    {
        get
        {
            lock (_padlock)
            {
                if (null == _instance)
                {
                    _instance = new EventCenter();
                }
                return _instance;
            }
        }
    }

    public void registerHandler(string opname, standardEventHandler eventHandler)
    {
        if (null == eventHandler)
            return;

        List<standardEventHandler> listHandler = null;
        if (_dicEventHandlers.ContainsKey(opname))
            listHandler = _dicEventHandlers[opname];

        if (null == listHandler)
        {
            listHandler = new List<standardEventHandler>();
            _dicEventHandlers.Add(opname, listHandler);
        }
        if (!listHandler.Contains(eventHandler))
            listHandler.Add(eventHandler);
    }


    public void unregisterHandler(string opname, standardEventHandler eventHandler)
    {
        if (null == eventHandler)
            return;

        List<standardEventHandler> listHandler = null;
        if (_dicEventHandlers.ContainsKey(opname))
            listHandler = _dicEventHandlers[opname];

        if (null == listHandler)
            return;

        listHandler = new List<standardEventHandler>();
        listHandler.Remove(eventHandler);
    }

    public void postNotification(string opname, object sender, NotifyEventArgs e)
    {
        if (_dicEventHandlers.ContainsKey(opname))
        {
            List<standardEventHandler> listHandler = _dicEventHandlers[opname];
            // notification실행 도중 unregister호출하는 경우에 대비하기 위해 foreach --> for 수정(예: GraphicHTMLTextBox);
            int cnt = listHandler.Count;
            for (int i = cnt - 1; i >= 0; i--)
            {
                standardEventHandler handler = listHandler[i];
                try
                {
                    handler(sender, e);
                }
                catch (Exception ex)
                {
                    string st = "event save exception : " + ex.ToString();
                    Console.WriteLine(st);
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
```


## 구현 with Naive EventCenter   

## EventAggregator /Based Prism EventAggregator




작성하기 너무귀찮내... 뒤로미룸 


