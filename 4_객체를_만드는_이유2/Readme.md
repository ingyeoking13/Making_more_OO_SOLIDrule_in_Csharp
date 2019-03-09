## ��ü�� ����� ����2 

***��ü�� ����� ������ ��ü�� ����� ���ؼ�����?***

������ A���� ���θ� ������ �߱⿡ `objected-oriented design`���� ū ������ �� �� �־���.   
A���� ���������� ������ �͵��̶�� ������ ���� ���� ������ ������ �������̽��� �����ִ� �۾��� ���� �Ͽ���.  
�������ٴ� ū �����̾�������.  

������ A���� �ڽ��� �ڵ忡�� � ������ ������.  
�и��� Ŭ���̾�Ʈ���� `inertface`, `abstract class`�� ���� ���� ������ ���������� �ش� ��ü���� �����ϰ� ��������,  
������ Ŭ������ �����ϴµ����� ��ü���� �ڵ带 �����ؾ��ߴ�. ��ü�� �����ϴ� �������� ���̴� �ڵ� `interface object = new concreteClass()` �̴�.   

��ü�� �����ϴ� ���� � ������ �����ϴ��� ���캼���̴�. 

### 1. ����

���� �Ƿڸ� �ѹ� ���캸��.  

````
�ȳ��ϼ���. OO�б� ����ǿ��� �����帳�ϴ�.   
���������� üũ�ϴ� �ý����� �����Ϸ��� �մϴ�.  
�л�, ������ �� �� ������ üũ�� �� �־���ϰŵ��.   
�����ұ��?   
````

A ���� ������ �ϴ� �������� `ClassRoomUser` �������̽��� ���� �����Ͽ���.  
�׸��� �״� �ڽ��� ¥���� Ŭ������ Ŭ���̾�Ʈ���� ��� Ȱ���� �� �ִ����� �׽�Ʈ �غ��Ҵ�.  
Ŭ���̾�Ʈ���� ȣ���� �� �ִ� �Լ��� ���� `DoJoinExit`�� ����.  

````C#
public interface IClassRoom
{
    void Join();
    void Exit();
}

public class Student : IClassRoom
{
    public void Join() {}
    public void Exit() {}
}

public class Teacher : IClassRoom
{
    public void Join() {}
    public void Exit() {}
}

static void Main(string[] args)
{

    doJoinAndExit("teacher");
    doJoinAndExit("student");
}

static void doJoinAndExit(string args)
{
    IClassRoomUser person;
    if (args == "teacher") person = new Teacher();
    else if (args == "student") person = new Student();
    else throw new Exception();

    person.Join();
    person.Exit();
}
````

A���� �׽�Ʈ ���� �����ϰ� ��ġ��, �Ƿ��ο��� �����ߴ�.  
���ƿ� �亯�� �̷���.  

```` 
���� ���� �ð� �ȿ� ���ϼ̱���.  
�׷�����, �ϴ� �׽�Ʈ�ؾ��� �� DoJoinAndExit �Ӹ��� �ƴ϶�,   
DoJoinAndStay �� DoExit �� �ʿ��ؿ�.  
��! �׸��� �л�, ������ ���� �кθ�Ե� �߰��� �ʿ��ϰŵ��.     
���� �����ұ��?   
````

A���� �翬 �׷��ٰ� �ߴ�.   

### 2. Factory�� ����  

A���� �켱 `doJoinAndStay(...)` �޼��带 �����ߴ�. �׸��� �״� �޼��带 �����ߴ�.  

````
static void doJoinAndStay(string args)
{
    IClassRoomUser person;
    if (args == "teacher") person = new Teacher();
    else if (args == "student") person = new Student();
    else if (args == "parent") person = new Parent();
    else throw new Exception();

    person.Join();
}
````

�� �޼���� `doJoinAndExit`, `doJoinAndStay`, `doExit` ���ο��� ��ü�� �����ϴµ� if���� �ݺ��Ǵ� ���� �߰��Ͽ���.   
�̸� �ذ��ϱ� ���� �׿��� �� ������ ���ٹ��� �ִµ� ������ ����.  

* `doJoinAndExit` �� ���� �޼���鿡 ���ڷ� ��ü�� �ѱ��. ��, `string` ����, `IClassRoomUser`�� �ѱ��. 
* ��ü ���� �˰����� ��üȭ �Ѵ�.    

���, ù��° ����� ���⵵�� ���� ������Ű�� �ʴ´�.  
�ֳ��ϸ� Ŭ���̾�Ʈ �ܿ��� `doJoinAndStay` �� �迭�� �޼��带 ȣ���� ������ ������ `if... else if.. else` ���� ������ ��ü�� ��� �Լ��� �����ؾ��ϱ� �����̴�.   

���� �Լ��� ����.  
````C#
Method(string arg1, string arg2)
{
    IClassRoomUser person = new nullPerson();
    if (arg1== "student") person = new Student(); 
    else if ( arg1 == "teacher") person =  new Teacher();
    else if ( arg1 == "parent") person = new Parent();

    if ( arg2 == "DoJoinAndStay" ) DoJoinAndStay(person);
    else if (arg2 == "DoJoinAndExit") DoJoinAndExit(person);
    else if ( arg2 == "DoExit") DoExit(person);
}
````
�� ����� ���� ���⿡ ���ƺ�������, `DoJoinAndStay` �� �Լ��� �������� ȣ�⿡ ���ؼ��� �׻� `person`�� ���� ��ó�� `if else syntax`�� �켱�Ǿ���Ѵ�.        
���ذ� ���� �ʴ´ٸ� �Ʒ��� `Method2`�� ����.

````C# 
Method2(string arg1)
{
    someOperation(); // � ������ �۾�
    SomeOperation2(); // � ������ �۾�2

    IClassRoomUser person = new nullPerson();
    if (arg1== "student") person = new Student(); 
    else if ( arg1 == "teacher") person =  new Teacher();
    else if ( arg1 == "parent") person = new Parent();

    DoJoinAndStay(person);
}
````
���ôٽ��� ���⵵�� ���� �������� �ʴ´�.    
`StudentDoJoin....`, `TeacherDoJoin...` ���� ���� �޼��� ���� ����� �͵� �ٸ��� ����, ���� ������ �ƴϴ�.   

���� ������, �� ��° �����̴�. A���� �̹� ��ü�� �����ϴ� ���� ��üȭ�ϴ� `Factory Pattern`�� ���� ������ �˰� �־���.     
�Ʒ� �ҽ��� �װ� �ۼ��� �ҽ��̴�. ��� �ۼ��Ͽ����� ����.   

````C#
public static IClassRoomUser createClassRoomUser(string args)
{
    IClassRoomUser person;
    if (args == "teacher") person = new Teacher();
    else if (args == "student") person = new Parent();
    else if (args == "parent") person = new Student();
    else throw new Exception();
    return person;
}

static void doJoinAndExit(string args)
{
    IClassRoomUser person = 
        ClassRoomUserFactory.createClassRoomUser(args);

    person.Join();
    person.Exit();
}
static void doJoinAndStay(string args)
{

    IClassRoomUser person = 
        ClassRoomUserFactory.createClassRoomUser(args);

    person.Join();
}
````

Ŭ���̾�Ʈ �ܿ����� �ҽ� ���⵵�� �ξ� �پ�����.    
A���� ���������� ������Ʈ�� ������ �� �� �־���.   

### 3. abstract Factory�� ����   

A���� ������ ���� ������ �޾Ҵ�.   

````
�Ƿ� ��� �����մϴ�.
���� ����� �ٸ� �迭�� �б��� ����Դϴ�. 
�� �б��� �����б��� �л�, ������, �θ���� ������ �ۼ��� �Ͱ� �������� �κе� ������,  
�ٸ� �κе� �ְŵ��. �׷��� �ٸ��� ����ϰ� �ͽ��ϴ�.  
````

A���� ������ `Ordianry, InternationalStudent`�� `Student`�� �����־���.  
`Parent` `Teacher` ��� �׷��� ���־���.  

````C#
public abstract class Student : IClassRoomUser
{
    public void Join() { }
    public void Exit() { }
}

public class OrdinaryStudent : Student { }
public class InternationalStudent : Student { }

public abstract class Teacher : IClassRoomUser
{
    public void Join() {}
    public void Exit() {}
}

public class OrdinaryTeacher : Teacher { }
public class InternationalTeacher : Teacher { }

public abstract class Parent : IClassRoomUser
{
    public void Join() {}
    public void Exit() {}
}

public class OrdinaryParent : Parent { }
public class InternationalParent : Parent { }
````







