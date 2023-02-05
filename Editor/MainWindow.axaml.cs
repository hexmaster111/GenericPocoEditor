using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using EditorControllerFramework;
using EditorControllerFramework.UiAttributes;

namespace Editor;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var a = new TestPoco(name: "Test", description: "Test Description", someField: "Yes", someOtherField: "sneeky")
        {
            SomeOtherProperty = "Words"
        };

        var res = ControllerBuilder.BuildFormController(a, a.GetType());
        Debugger.Break();
    }

    public static void DoExecute(string message = "", [CallerMemberName] string name = "")
    {
        Console.WriteLine($"{name}: {message}");
    }


    public class TestPoco
    {
        [UiButton("Run static method")]
        public static void StaticUserMethod()
        {
            DoExecute();
        }

        [UiTextBlock(nameof(Name))] public string Name { get; set; }
        public string Description { get; set; }

        [UiTextBlock("Some Field")] public string SomeField;

        [UiTextBox(nameof(SomeOtherField))] public string SomeOtherField;
        [UiTextBox(nameof(SomeOtherProperty))] public string SomeOtherProperty { get; set; }

        [UiCheckBox(nameof(SomeBool))] public bool SomeBool { get; set; }

        private string _someOtherField;

        public TestPoco(TestEnum testEnum)
        {
            switch (testEnum)
            {
                case TestEnum.Test1:
                    Name = "Test1";
                    Description = "Test1 Description";
                    SomeField = "Test1";
                    _someOtherField = "Test1";
                    break;
                case TestEnum.Test2:
                    Name = "Test2";
                    Description = "Test2 Description";
                    SomeField = "Test2";
                    _someOtherField = "Test2";
                    break;
                case TestEnum.Test3:
                    Name = "Test3";
                    Description = "Test3 Description";
                    SomeField = "Test3";
                    _someOtherField = "Test3";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(testEnum), testEnum, null);
            }

            MyEnum = testEnum;
        }

        public TestPoco(string name, string description, string someField, string someOtherField)
        {
            Name = name;
            Description = description;
            SomeField = someField;
            _someOtherField = someOtherField;
            MyEnum = TestEnum.Test1;
        }

        [UiComboBox(nameof(MyEnum))] public TestEnum MyEnum;
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
            [EditorControllerFramework.UiAttributes.UiLabel("Test 1")] Test1,
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
            DoExecute($"SomeDataClass Built {buildEnum} {@string}");
        }

        public SomeBasicDataClass(int someInt, string someString)
        {
            SomeInt = someInt;
            SomeString = someString;
        }

        public int SomeInt { get; set; }
        public string SomeString { get; set; }
    }

    private void EditSomething_OnClick(object? sender, RoutedEventArgs e)
    {
    }
}