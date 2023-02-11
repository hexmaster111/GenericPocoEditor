// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Runtime.CompilerServices;
using EditorControllerFramework;
using EditorControllerFramework.UiAttributes;
using WpfPocoFrontend;


internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var a = new TestPoco()
        {
            Description = "Desc",
            Name = "Name",
            SomeBool = true,
            SomeDouble = 42.00,
            MyEnumProperty = TestPoco.TestEnum.Test1,
            SomeOtherProperty = "SomeOtherProperty",
            SomeBasicDataClasses = new List<SomeBasicDataClass>()
            {
                new(SomeBasicDataClass.SimpleBuildEnum.TypeOne, "one"),
                new(SomeBasicDataClass.SimpleBuildEnum.TypeThree, "three")
            },
            SomeBasicDataClassesArray = Array.Empty<SomeBasicDataClass>()
        };

        a.Name = "new name";
        
        var res = ControllerBuilder.BuildFormController(a, a.GetType());
        ViewBuilder.BuildWindow(res).ShowDialog();
    }
}


[UiLayoutLine0(nameof(Name), nameof(Description))]
[UiLayoutLine1(nameof(SomeBool), nameof(SomeDouble))]
[UiLayoutLine2(nameof(StaticUserMethod), nameof(DoSomethingWildWithNoArguments))]
public class TestPoco
{
    static void DoExecute(string message = "", [CallerMemberName] string name = "")
    {
        Console.WriteLine($"{name}: {message}");
    }

    [UiButton("Run static method")]
    public static void StaticUserMethod()
    {
        DoExecute();
    }

    [UiTextBlock(nameof(Name))] public string Name { get; set; }


    public string Description { get; set; }


    [UiTextBox(nameof(SomeOtherProperty))] public string SomeOtherProperty { get; set; }

    [UiCheckBox(nameof(SomeBool))] public bool SomeBool { get; set; }

    private string _someOtherField;


    [UiComboBox(nameof(MyEnumProperty))] public TestEnum MyEnumProperty { get; set; }

    [UiSlider(nameof(SomeDouble), -1, 1)] public double SomeDouble { get; set; }


    // [UiSlider(nameof(SomeDecimal))]
    // public decimal SomeDecimal { get; set; }


    [UiButton("Something")]
    public void DoSomethingWildWithNoArguments()
    {
        DoExecute();
    }

    public void NotSomethingTheUserCanDo()
    {
        DoExecute("THIS SHOULD NOT BE RAN FROM THE USER");
    }


    public enum TestEnum
    {
        [EditorControllerFramework.UiAttributes.UiLabel("Test 1")]
        Test1,
        Test2,
        Test3
    }

    public ICollection<SomeBasicDataClass> SomeBasicDataClasses { get; set; } = new List<SomeBasicDataClass>();
    public SomeBasicDataClass[] SomeBasicDataClassesArray { get; set; } = new SomeBasicDataClass[5];
}

public class SomeBasicDataClass
{
    public enum SimpleBuildEnum
    {
        TypeOne,
        TypeTwo,
        TypeThree
    }

    // [UserConstructor] TODO: tell the thing that it can use this constructor in the ui
    public SomeBasicDataClass(SimpleBuildEnum buildEnum, string @string)
    {
        SomeInt = (int)buildEnum;
        SomeString = @string;
    }

    public SomeBasicDataClass(int someInt, string someString)
    {
        SomeInt = someInt;
        SomeString = someString;
    }

    public int SomeInt { get; set; }
    public string SomeString { get; set; }
}