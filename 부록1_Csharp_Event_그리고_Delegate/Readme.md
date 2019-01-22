## Csharp���� Event�� Delegate��1 

### Introduction   

C# ���� `Event`�� ������ ���� ������ �����Ѵ�.  

**Event�� �̿��ϸ� �� Ŭ������ �ٸ� Ŭ������ ��ü���� ������ �� �ִ�**  
1 `Publisher`�� �̺�Ʈ�� �߻���Ű��   
2 �ϳ� �̻��� `Subscribers`�� �̺�Ʈ�� �����Ѵ�.   

### ���, Delegate�� ���� ���߰ھ� 

��� `event`�� ���� �ڼ��� ������ �������. (��� �� `event`�̾߱⸦ �������ݾƿ�?)   
`event`�� �����ϱ� ���� `delegate`�� ���� ���캸�°� �켱�Ǿ�� �� �� ����.   

�ֳ��ϸ� `event`�� ������ `delegate`�� �����ϱ� �����̴�!.   

`delegate`�� �޼��带 ����Ų��. �Լ� �����Ϳ� �ſ� �����ϴ�.     

### Delegate �� �׷� �޼����ΰ���? �����ΰ���?   

�� ��... ���� ����!  
�̹� *C/C++* �� �˰� �ִ� ����� C/C++���� ���Ǵ� �Լ� �����ʹ� ������ ���ٰ� �����Ѵ�.  
`Delegate` ���� �޼��尡 �ƴ� �����̴�. C# ������ `delegate type`�̶�� �Ѵ�. (`delegate`�� ��� �� `new` keyword�� �Ҵ�Ǿ���Ѵ�.)    
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


### `Delegate`�� ��� �������� ó�� ������ �� ���� �� �����ϴ�.  

���� ������ C# ���� ���ø����̼��� ���� �� ���� ���Ҵ� ��Ȳ�� �� �ִ�. ��������� ��� �ȴ�!   
�� ������ Person Class�� ���� �� ���پ� ���Ҵ�.  

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

    public void Speak(string syntax)
    {
        Console.WriteLine(syntax);
    }
}
````

���� `namespace`�ȿ� `NameChangedDelgate` �̸��� `delegate`�� �����ߴ�.  
��ȯ���� `void` �̰� `parameter`��  2���� `string`�� �޴´�.  
`Person` Ŭ�������� �ش� `delegate type`�� �ʵ带 �����Ͽ���.   
`_name` �ʵ带 ������ ������ `delegate` �ʵ忡 ����� �޼��带 �������ش�.  

�Ʒ��� `Main`���� `Person` Ŭ���� ��ü�� �����ϰ� `delegate` �ʵ忡 (����)�Լ��� �����ϰ� �ߴ�.   
`delegate`�� **+= ������**�� �������� �Լ��� ������ �� �ִ�. 


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
}
````

����� 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

C# �����Ϸ����� �Լ��� �ڵ����� �ش� delegate type���� ��ȯ�����ش�. ���� new UserDelegate �����ڴ� ������ �� �ִ�.   
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
yohan.Name = "yohan"; // second change
````

����� 
````
Name change from Yohan to Kimchi
Name change from Kimchi to gaelim
hmmm, Do Not change more
````

### ���� `event`�� ����غ���. 

`event`�� �ۼ����� �ϳ� �����ϴ�.    
`Person`�� delegate type �ʵ带 ������ ���� ��������.   
�׸��� `event`�� �׻� **-=, += ������**�� �����ϹǷ� Main�� �����Ѵ�.  
���� `event`�̱⿡ `= null` �� ���� �ʾƿ�!.

````C#
public event NameChangedDelegate OnNameChanged; // Person field

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

�׷��� ���ʻ� .Net Framework���� �̺�Ʈ�� �׻� ������ ���� ����� `delegate`�� ����.  
`sender`�� �ش� `event`�� ����Ų ��ü�� `this` Ű���带 ���� �ѱ��. `Person` �̶� ������ �� ������, ���ʻ� *(���� .NET ���ʻ��̴�.)* `object` Ÿ������ �Ѵ�.  
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

`event` �� ���ؼ� ���캸�Ҵ�. �� `event`��� ���� ��� `delegate`�� ū �ٸ��� ������ �˰ԵǾ���.   
�츮�� `Person`Ŭ������ `Name` ������Ƽ�� ���� `_name` �����̺� �ʵ尡 �ٲ�ٴ� ���� ����� ������, `delegate`�� ����Ű�� �޼��带 �����Ѵ�.     

### �׷��� �̷��� �����غ��Ƶ� �ɱ�? 

`event` �ʵ带 ������ �ִ� Ŭ���� `Person`�� �ڽ��� ������ �˸��� `Publisher�Խ���`��� �� �� �ִ�.   
`Program`Ŭ������ `Person`��  `_name` ��ȭ�� ���� ������ �ڽ��� ������ �޼��带 ������ �ִ�.  
�׷� `Program`�� `Subscribers������` �̴�.  

### ��Ʈ! 

��մ� ���� �� `event`��� ���� ��ü ���� ����� ���������� ���´�.   
�ڼ��� ������ 3é�� `��ü�� ����� �ϴ¹�, Observer` ���� Ȯ������.   

