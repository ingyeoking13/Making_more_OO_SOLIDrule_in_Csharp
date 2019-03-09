## Csharp���� Event�� Delegate

### Introduction   

��ư�� Ŭ���ϰ�, Ű���带 ������ ������ ����̶��Ѵ�.   
���� `Application`�� ���� �� �� `Event`��� �Ϳ� �ſ� ����޾Ұ�, �� �׷����� `Event`�� ���� ���� ����.  
`Button.Click`, `TextBox.KeyDown` ���� �༮�鿡 �ſ� ����޾Ұ�, �������� �� ����Ǹ� ����� ���Ҿ���.  

�׷��� ���� `event`�� ������� �� ������. 

�� ���������� `delegate type`, `event keyword`, `EventHander`, `Action`�� �ٷ��.  

### Event���� ���� Delegate�� ���캸��

`event`�� ������ `delegate`�� **����**�ϱ� ������ `delegate`�� ���� ���캸�� ���� �����ϴ�.  
ȥ���ؼ��� �ȵǴ� ���� `delegate`�� `event`�� �����ϴ� ���谡 �ƴϸ�, `event`�� `delegate`�� �����ϴ� ���谡 �ƴϴ�. `(delegate != event)`  
ǥ�� �״�� `event keyword`�� `delegate`�� **����** �Ѵ�.    
�������� ����ϰ�����, `delegate type`�� �ܵ����� ���� �� ������, `event`�� �ܵ����� ���� �� ���� `delegate`�� **����** �ؾ��Ѵ�.        

`delegate type` ������ �����ϰ� ���ϸ� �޼��带 ����Ų��. �� �� ������ �����ڸ� �޼��带 `delegate`�� Wrapping �� ��ü�� ����Ų��. �� ��ü�� `delegate instance`�� �Ѵ�.  [C#2.0 ���������� delegate�� �׻� Wrapping �ؼ� ����ؾ��ߴ�.](https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/delegates/how-to-declare-instantiate-and-use-a-delegate)   

### Delegate �� �׷� �޼����ΰ���? �����ΰ���?   

�̹� *C/C++* �� �˰� �ִ� ����� *C/C++* ���� ���Ǵ� �Լ� �����ʹ� ������ ���ٰ� �����Ѵ�. ~~void (*function)() ���� ������ ��� �ƴѰ���?~~   
`Delegate` ���� �޼��尡 �ƴ� ���۷��� Ÿ���� ������. C# ������ `delegate type`�̶�� �Ѵ�.   
���������δ� `C#`�� `delegate`�� `C/C++`�� `function Pointer`�� �ٸ�����, ������ �ܿ����� ������ �����ϴ�.  
`javascript`�� ���� �Լ��� ������ ������ �Լ��� ����Ű�� ���� ���ο� ���� �ƴϴ�.   

�Ʒ��� `Delegate` ��� �����̴�.

````C#
class Program
{
    public delegate void HelloWorldDelegate(string helloWorld);
    public delegate void GoodByeWorldDelegate(string GoodByeWorld);

    static void Main(string[] args)
    {
        Person yohan = new Person();
        HelloWorldDelegate helloWorld = new HelloWorldDelegate(yohan.Speak);
        helloWorld("helloWorld");

        GoodByeWorldDelegate goodByeWorld = new GoodByeWorldDelegate(yohan.Speak);
        goodByeWorld = new GoodByeWorldDelegate(yohan.Speak);
        goodByeWorld("GoodByeWorld");
    }
}

public class Person
{
    public void Speak(string syntax)
    {
        Console.WriteLine(syntax);
    }
}
````

**�����**

````
helloWorld
GoodByeWorld
````

### `Delegate`�� ��� �� �����ϴ�.  

���� ������ `C#` ���� ���ø����̼��� ���� �� ���� ���Ҵ� ��Ȳ�� ���̴�.  
�� ������ `Person Class`�� ���� �� ���پ� ���Ҵ�.  

````C#
public delegate void NameChangedDelegate(string currentName, string newName);
public class Person
{
    private string _name;
    public string Name
    {
        get { return _name; }
        set {
            if (_name != value)
            {
                OnNameChanged?.Invoke(_name, value);
            }
            _name = value;
        }
    }

    public NameChangedDelegate OnNameChanged;
}
````

���� `namespace`�ȿ� `NameChangedDelgate` �̸��� `delegate`�� �����ߴ�.  
��ȯ���� `void` �̰� `parameter`��  2���� `string`�� �޴´�.  
`Person` Ŭ�������� �ش� `delegate type`�� �ʵ带 �����Ͽ���.   
`_name` �ʵ带 ������ ������ `delegate` �ʵ忡 ����� �޼��带 �������ش�.  

�Ʒ��� `Main`���� `Person` Ŭ���� ��ü�� �����ϰ� `delegate` �ʵ忡 (����ǥ����)�޼��带 �����ϰ� �ߴ�. (������ `delegate instance`���� �����ϴ� �������� ǥ���� �޼��带 �����Ѵٰ� �ϰ���.)    
`delegate`�� **+= ������**�� �������� �޼��带 ������ �� �ִ�. �Ǵ� **-= ������** �� ��ϵ� �޼��带 ���� �ִ�.  
�Ǵ� **= ������** �� �޼��带 �Ҵ��� �� �ִ�. **=null**�� ���� ��ϵ� ��� �޼���(�����ϰԴ� `delegate instance`)���� ������ �Ҵ´�.     

````C#
static void Main(string[] args)
{
    Person yohan = new Person();
    yohan.Name = "Yohan"; // set

    yohan.OnNameChanged = new NameChangedDelegate(
        (s, s2) => { Console.WriteLine($"Name change from {s} to {s2}"); }
        );

    yohan.Name = "Kimchi"; // first change

    yohan.OnNameChanged += new NameChangedDelegate(
        (s, s2) => { Console.WriteLine($"hmmm, Do Not change more"); }
        );

    yohan.Name = "gaelim"; // second change

    yohan.OnNameChanged = null; 
    yohan.Name = "yohan"; // third change, Invoke Nothing.
}
````

����� 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

**�� ���� ��**��, C#2.0 ���� �����Ϸ����� �Լ��� �ڵ����� �ش� `delegate instance`�� ��ȯ�����ش�. ���� `new NameChangedDelegate` �����ڴ� ������ �� �ִ�.   
������ �������� �ҽ���.

````C#
yohan.Name = "Yohan"; // set

yohan.OnNameChanged = 
    (s, s2) => { Console.WriteLine($"Name change from {s} to {s2}"); };

yohan.Name = "Kimchi"; // first change

yohan.OnNameChanged += 
    (s, s2) => { Console.WriteLine($"hmmm, Do Not change more"); };
yohan.Name = "gaelim"; // second change

yohan.OnNameChanged = null; 
yohan.Name = "yohan"; // third change, Invoke nothing.
````

����� 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

�̷ν�, �ڵ� �ܿ����� `delegate`�� ���� �޼��带 ����Ű�� ������ ��� �����ϸ� �� ������ ����������.  

### ���� `event`�� ����غ���. 

`event`�� �ۼ����� ����� �����ϴ�. �ۼ��� `delegate` �տ� `event` Ű���带 ���̸� �ȴ�.     
`Person`�� `delegate type` �ʵ带 ������ ���� ��������.   
````C#
public event NameChangedDelegate OnNameChanged; // Person field
````
�׸��� `event`�� �׻� **-=, += ������**�� �����ϹǷ� Main�� �����Ѵ�.  

������ �ҽ��� ������ ����.  
````C#
static void Main(string[] args)
{
    Person yohan = new Person();
    yohan.Name = "Yohan"; // set
    yohan.OnNameChanged += 
        (s, s2) => { Console.WriteLine($"Name change from {s} to {s2}"); };

    yohan.Name = "Kimchi"; // first change
    yohan.OnNameChanged += 
        (s, s2) => { Console.WriteLine($"hmmm, Do Not change more"); };

    yohan.Name = "gaelim"; // second change
}
````

����� 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

��뿡 �־� `delegate` �� `event delegate` �� �ٸ����� ũ�� ������ ����.  
* `event`Ű���带 ������ ��� **= �Ҵ翬����**�� ����� �� ����. ���� `= null`�� ���� ������ �� �� ����.   
* `event`Ű���尡 ���� ��� �ٸ� Ŭ�������� �ش� `event delegate` �ʵ带 ���������� ������ �� ����. **+=** **-=** ���۸� �����ϴ�.  

�� ��°�� ���� ���� ������ ����.  

````C#
// with pure delegate 
class OtherClass_Method()
{
    Person yohan = new Person();
    yohan.OnNameChanged.Invoke("PrevName", "NewName"); //<-- success. can be invoked from Other Class.
}

// with event delegate
class OtherClass_Method()
{
    Person yohan = new Person();
   // yohan.OnNameChanged.Invoke("PrevName", "NewName"); //<-- failed. can not be invoked from Other class.
}
````

`event` Ű���尡 �����ϴ� ������ �ý��� ������(�����Ϸ� ��)���� �� ������ �ܰ谡 ���� �� �ִ�.   
������ ���߿� �־�� `event keyword`�� ��������ν� �߻��ϴ� ������ �����̶� �����Ѵ�.   
�����ڴ� `event keyword`�� ���� Ŭ���� å�ӿ� ���� `design`�� ��Ȯ�� ���ִ� ô����� ����� �� �ִ�.  

�����ӿ�ũ������ ����, `Button.Click` �� �ش� `Class`������ `Invoke` �� �� �ְ� ����Ǿ���.   
��, `event keyword`�� �����ϸ� `Button` �� `Click`�Ǿ��� ���� ���������� `Invoke`�� �� �ְԲ� �ߴ�. ~~(�׷���)~~[^1]     

### (event) delegate ���� ����� ...

�� �������� �츮�� ������ ���� `delegate type`�� ����Ͽ���.  
````C#
public delegate void NameChangedDelegate(string currentName, string newName);
````
�׷��� ���� ��뿡 �־�� ���� ����, ���� Ÿ��(`string`, `int` ������)�� �̿��ϴ� `design`�� ���� ���õ��� �ʴ´�.   

���ʻ� `.Net Framework`���� `event delegate` �Ǵ� `delegate`�� �׻� ������ ���� ����� ����.    
````C#
public delegate void NameChangedDelegate(object sender, NameChangedEventArgs e);
````
`sender`�� �ش� `event`�� ����Ų ��ü�� `this` Ű���带 ���� �ѱ��. `Person` ���� Ư�� Ŭ������ ������ �� ������, ���ʻ� *(���� .NET ���ʻ��̴�.)* `object` Ÿ������ �Ѵ�. [^2][^3]   

�׸��� ������ ���Ǿ��� `currentName`, `newName` ������ `...EventArgs`�� Ŭ������ `Wrapping`�� �� `EventArgs`�� ����Ѵ�.

````C#
public delegate void NameChangedDelegate(object sender, NameChangedEventArgs e);

public class NameChangedEventArgs : EventArgs
{
    public readonly string currentName;
    public readonly string newName;

    public NameChangedEventArgs(string currentName, string newName)
    {
        this.currentName = currentName;
        this.newName = newName;
    }
}

//Main 
yohan.OnNameChanged += (s,e) => { Console.WriteLine($"Name change from {e.CurrentName} to {e.NewName}"); }; 
````

�������� `event` �� ���ؼ� ���캸�Ҵ�. �� `event` Ű������ ���� ��� `delegate`�� �ߴ��� �ٸ����� ������ �˰ԵǾ���.   
�� �߿� ������ ����ϵ簣�� �츮�� `Person`Ŭ������ `Name` ������Ƽ�� ���� `_name` �����̺� �ʵ尡 �ٲ�ٴ� ���� ����� ������, `delegate`�� ����Ű�� �޼��带 �����Ѵ�.     

### Depth in (Event) Delegate

`event delegate` �ʵ带 ������ �ִ� Ŭ���� `Person`�� �ڽ��� ������ �˸��� `Publisher�Խ���`��� �� �� �ִ�.   
`Program`Ŭ������ `Person`��  `_name` ��ȭ�� ���� ������ �ڽ��� ������ �޼��� ������ ������ �ִ�.  
�ٲ𶧸��� ����ϴ� `Console class`�� `Subscriber������`�� �� �� �ִ�.   

��մ� ���� �� `delegate`��� ���� ��ü ���� ����� ���������� ���´�. Ŭ���� ���� `������` `�Խ���` �� �����ϰ� �������� �� �ִ�.   
�ڼ��� ������ 3é�� `��ü�� ����� �ϴ¹�, Observer` é�� �Ĺݺ� ���� Ȯ������.   

### delegate, event�� �˰ڽ��ϴ�. EventHandler�� Action�� �����ΰ���?   

�� �� `degelgate void`�̸� �⺻������ ��� ������ �����ϴ�.   
`EventHandler`�� `Action` �� `Generic` ���� ���ڸ� �����Ѵ�. �Ʒ� ���� ����.  

````C# 
public event EventHandler<NameChangedEventArgs> OnNameChanged; // <-- EventHandler
public Action<Person,NameChangedEventArgs> OnNameChanged; // <-- Action
````

�츮�� �ۼ��� �ҽ��� `delegate` �ʵ带 �� �� ��������� �ٲپ� �ۼ��Ͽ��� �� �����Ѵ�.  

`EventHandler<T>`�� ���콺�� �����ٴ�� ������ ���� ������ �� �� �ִ�.    
`delegate void EventHandler<T> (object sender, T args)`  
�ݸ� `Action<T>` �� ���콺�� �����ٴ�� ������ ���� ������ �� �� �ִ�.   
`delegate void Action<in T1, in T2> (T1 args1,T2 args2)`   

**������ �ٸ���?**  

* `EventHandler<T>`�� �⺻������ `object sender` ��� ��ü�� ��������Ѵ�. ���� ���� `OnNameChanged(this, T)` ���� ����Ѵ�.   
* �׷��� `Action`�� ��������� `<classType(�Ǵ� object), T>` ������ ���������, `OnNameChanged(this, T)` �� ����� �� �ִ�.  
* `EventHandler`�� �̸��� �ǵ��� �°� `event keyword`�� ������ `event EventHandler<T>`�� ����ϴ� ���� ����. �ݴ�� `Action`�� `event`Ű���带 ������ �ʴ´�.  

**`<in T>`�� `<T>` �� �ٸ��ϴ�. EventArgs�� `<in>` Ű���尡 ������ `contravariant`�� ���� ���ϴ� �� �ƴѰ���?**  

�׷��� �ʴ�. �� �� `delegate type` �̹Ƿ� �⺻������ `Paramter`�� ���� `Contravariant`�� �����Ѵ�.  
`Action` ������ ��������� `<in T>` ��� ������ ���̴�.  

***�׷��� `Contravariant`�� ���� ����ΰ���? ���� �η� é��`Covariant and Contravariant`�� ������!***  

\[^1] : �׷���, `wpf/UWP` ��� `Button`�� `UIElement`�� ����ϰ� �����Ƿ� `UIElement.RaiseEvent()`�� �� �� �ִ�. ����, �ٸ� Ŭ���������� (�ַ� ������ �ڽ�, �θ�������, �ڽ�������)`Button.RaiseEvent`�� ���� `Click Event`�� �߻��ϰԲ� �� �� �ִ�. �ְ����� �Ǵ����� �̷� ���, Ư�� �����ӿ�ũ������ ������ ������� �����ϴ� ���� ����, �� �� �Ϲ����� �������� �޼���(`delegate instance`) ���� ���� �����صξ� �����ϴ� ������ �� �� �Ǽ����̶� ���� ���� �ʹ�.   
\[^2] : �ٽ� �ѹ� �������ڸ�, ���� ���ʻ��̴�. �̰��� �������� `publisher/Notifier�Խ���`, `subscriber/observer������` ������ �����ϱ� ����, �ַ� ���Ǵ� ��ü ��ü�� �ѱ�� `Pull` ����� �����̴�. �� ��������� `.NET`���� �����ϴ� ���� `EventHandler<TeventArgs>(object sender, TEventArgs TArgs)` �̸�, ������ `eventHandlerName.Invoke(this, myEventArgs)`�� �ַ� ���ȴ�. �� ������ ������ `event` Ű���带 �տ� �ٿ� `event EventHandler<TeventArgs>` �� ����ϸ�, �ش� �ʵ带 ������ �ִ� Ŭ���������� `Invoke`�Ѵ�.    
\[^3] : �ι�° ������, `delegate`�� `parameter`�� ������ ���� Ư�� Ŭ������ �����ϴ���, (��: `delegate void MYDeleagetType(Person p)`) ������ ���� �޼��带 ���� �� �ִ�. (��: `method(object sender)`). �̰��� �⺻������ `delegate`�� `Parameter`�� ���� `Contravariant`�� �����ϱ� �����̴�.  

