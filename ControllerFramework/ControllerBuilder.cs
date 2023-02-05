using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ControllerFramework;
using ControllerFramework.Exceptions;
using ControllerFramework.UiAttributes;

namespace Editor.EditorParts;

public static class ControllerBuilder
{
    public static FormController BuildFormController(object obj, Type type)
    {
        var testTimer = Stopwatch.StartNew();


        var members = type.GetMembers();

        var uiButtons = members.Where(x => x.GetCustomAttributes(typeof(UiButton), false).Length > 0);
        var uiTextBlocks = members.Where(x => x.GetCustomAttributes(typeof(UiTextBlock), false).Length > 0);
        var uiTextBoxes = members.Where(x => x.GetCustomAttributes(typeof(UiTextBox), false).Length > 0);
        var uiCheckBoxes = members.Where(x => x.GetCustomAttributes(typeof(UiCheckBox), false).Length > 0);
        var uiComboBoxes = members.Where(x => x.GetCustomAttributes(typeof(UiComboBox), false).Length > 0);
        var uiSliders = members.Where(x => x.GetCustomAttributes(typeof(UiSlider), false).Length > 0);


        var controllers = new FormController()
        {
            ButtonControllers = uiButtons.Select(x => new ButtonController(x, obj)).ToArray(),
            TextBlockControllers = uiTextBlocks.Select(x => new TextBlockController(x, obj)).ToArray(),
            TextBoxControllers = uiTextBoxes.Select(x => new TextBoxController(x, obj)).ToArray(),
            CheckBoxControllers = uiCheckBoxes.Select(x => new CheckBoxController(x, obj)).ToArray(),
            ComboBoxControllers = uiComboBoxes.Select(x => new ComboBoxController(x, obj)).ToArray(),
            SliderControllers = uiSliders.Select(x => new SliderController(x, obj)).ToArray()
        };

        testTimer.Stop();
        Console.WriteLine(testTimer.ElapsedTicks);

        return controllers;
    }

    public struct FormController
    {
        public ButtonController[] ButtonControllers;
        public TextBlockController[] TextBlockControllers;
        public TextBoxController[] TextBoxControllers;
        public CheckBoxController[] CheckBoxControllers;
        public ComboBoxController[] ComboBoxControllers;
        public SliderController[] SliderControllers;
    }

    public class SliderController
    {
        public string Label => _uiSlider.Label;
        public double Min => _uiSlider.Min;
        public double Max => _uiSlider.Max;

        public double Value
        {
            get => (double)GetValueFromMember();
            set => SetMemberValue(value);
        }

        void SetMemberValue(double value)
        {
            switch (_memberInfo)
            {
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(_obj, value);
                    break;
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(_obj, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        object GetValueFromMember()
        {
            switch (_memberInfo.MemberType)
            {
                case MemberTypes.Property:
                {
                    var propertyInfo = (PropertyInfo)_memberInfo;
                    var value = propertyInfo.GetValue(_obj);
                    return value;
                }
                case MemberTypes.Field:
                {
                    var fieldInfo = (FieldInfo)_memberInfo;
                    var value = fieldInfo.GetValue(_obj);
                    return value;
                }
                default:
                    throw new UiBuilderException("Member type not supported");
            }
        }

        public SliderController(MemberInfo memberInfo, object obj)
        {
            _memberInfo = memberInfo;
            _obj = obj;
            _uiSlider = _memberInfo.GetCustomAttributes<UiSlider>().First();
        }

        private readonly UiSlider _uiSlider;
        private MemberInfo _memberInfo;
        private object _obj;
    }

    public class ComboBoxController
    {
        private MemberInfo _memberInfo;
        private object _obj;

        public EnumCbItem[] Items { get; set; }

        public EnumCbItem SelectedItem
        {
            get
            {
                var currentValue = GetValueFromMember();
                return Items.First(x => x.Value == (int)currentValue);
            }
            set => SetMemberValue(value);
        }

        public string Label;


        public class EnumCbItem
        {
            public EnumCbItem(string name, int value)
            {
                Name = name;
                Value = value;
            }

            public string Name;
            public int Value;
        }


        public ComboBoxController(MemberInfo memberInfo, object obj)
        {
            _memberInfo = memberInfo;
            _obj = obj;
            Label = _memberInfo.GetCustomAttributes<UiComboBox>().First().Label;


            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                {
                    var fieldInfo = memberInfo as FieldInfo;
                    if (fieldInfo == null) throw new ArgumentException();
                    if (!fieldInfo.FieldType.IsEnum)
                        throw new Exception("UiComboBox can only be applied to enums");
                    var enumMembers = fieldInfo.FieldType.GetMembers(BindingFlags.Static | BindingFlags.Public);
                    Items = new EnumCbItem[enumMembers.Length];

                    for (int i = 0; i < enumMembers.Length; i++)
                    {
                        var enumMember = enumMembers[i];
                        var enumValue = (int)Enum.Parse(fieldInfo.FieldType, enumMember.Name);

                        var enumNameLabelText = "";
                        var enumNameLabel =
                            enumMember.GetCustomAttributes(typeof(UiLabel), false)
                                .FirstOrDefault() as UiLabel;

                        enumNameLabelText = enumNameLabel == null ? enumMember.Name : enumNameLabel.Text;

                        Items[i] = new EnumCbItem(enumNameLabelText, enumValue);
                    }
                }
                    break;
                case MemberTypes.Property:
                {
                    var propertyInfo = memberInfo as PropertyInfo;
                    if (propertyInfo == null) throw new ArgumentException();
                    if (!propertyInfo.PropertyType.IsEnum)
                        throw new Exception("UiComboBox can only be applied to enums");


                    var enumMembers = propertyInfo.PropertyType.GetMembers(BindingFlags.Static | BindingFlags.Public);
                    Items = new EnumCbItem[enumMembers.Length];

                    for (int i = 0; i < enumMembers.Length; i++)
                    {
                        var enumMember = enumMembers[i];
                        var enumValue = (int)Enum.Parse(propertyInfo.PropertyType, enumMember.Name);

                        var enumNameLabelText = "";
                        var enumNameLabel =
                            enumMember.GetCustomAttributes(typeof(UiLabel), false)
                                .FirstOrDefault() as UiLabel;

                        enumNameLabelText = enumNameLabel == null ? enumMember.Name : enumNameLabel.Text;

                        Items[i] = new EnumCbItem(enumNameLabelText, enumValue);
                    }
                }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void SetMemberValue(EnumCbItem value)
        {
            switch (_memberInfo)
            {
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(_obj, value.Value);
                    break;
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(_obj, value.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        object GetValueFromMember()
        {
            switch (_memberInfo.MemberType)
            {
                case MemberTypes.Property:
                {
                    var propertyInfo = (PropertyInfo)_memberInfo;
                    var value = propertyInfo.GetValue(_obj);
                    return value;
                }
                case MemberTypes.Field:
                {
                    var fieldInfo = (FieldInfo)_memberInfo;
                    var value = fieldInfo.GetValue(_obj);
                    return value;
                }
                default:
                    throw new UiBuilderException("Member type not supported");
            }
        }
    }

    public class CheckBoxController
    {
        public string Label;

        public bool IsChecked
        {
            get
            {
                switch (_memberInfo.MemberType)
                {
                    case MemberTypes.Property:
                    {
                        var propertyInfo = (PropertyInfo)_memberInfo;
                        var value = propertyInfo.GetValue(_obj);
                        return value != null && (bool)value;
                    }
                    case MemberTypes.Field:
                    {
                        var fieldInfo = (FieldInfo)_memberInfo;
                        var value = fieldInfo.GetValue(_obj);
                        return value != null && (bool)value;
                    }
                    default:
                        throw new UiBuilderException("Member type not supported");
                }
            }
            set
            {
                switch (_memberInfo)
                {
                    case PropertyInfo propertyInfo:
                        propertyInfo.SetValue(_obj, value);
                        break;
                    case FieldInfo fieldInfo:
                        fieldInfo.SetValue(_obj, value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        MemberInfo _memberInfo;
        object _obj;


        public CheckBoxController(MemberInfo memberInfo, object obj)
        {
            _memberInfo = memberInfo;
            _obj = obj;
            Label = _memberInfo.GetCustomAttributes<UiCheckBox>().First().Label;
        }
    }

    public class TextBoxController
    {
        public string Label;

        public string Text
        {
            get => GetTextFromMember();
            set => SetMemberText(value);
        }

        private readonly MemberInfo _memberInfo;
        private readonly object _obj;

        void SetMemberText(string text)
        {
            switch (_memberInfo)
            {
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(_obj, text);
                    break;
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(_obj, text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        string GetTextFromMember()
        {
            switch (_memberInfo.MemberType)
            {
                case MemberTypes.Property:
                {
                    var propertyInfo = (PropertyInfo)_memberInfo;
                    var value = propertyInfo.GetValue(_obj);
                    var text = value?.ToString();
                    return text ?? string.Empty;
                }
                case MemberTypes.Field:
                {
                    var fieldInfo = (FieldInfo)_memberInfo;
                    var value = fieldInfo.GetValue(_obj);
                    var text = value?.ToString();
                    return text ?? string.Empty;
                }
                default:
                    throw new UiBuilderException("Member type not supported");
            }
        }

        public TextBoxController(MemberInfo memberInfo, object instance)
        {
            _memberInfo = memberInfo;
            Label = _memberInfo.GetCustomAttributes<UiTextBox>().First().Label;
            _obj = instance;
        }
    }

    public class TextBlockController
    {
        public string Label;

        public string Text => GetTextFromMember();

        private readonly MemberInfo _memberInfo;
        private readonly object _obj;

        string GetTextFromMember()
        {
            switch (_memberInfo.MemberType)
            {
                case MemberTypes.Property:
                {
                    var propertyInfo = (PropertyInfo)_memberInfo;
                    var value = propertyInfo.GetValue(_obj);
                    var text = value?.ToString();
                    return text ?? string.Empty;
                }
                case MemberTypes.Field:
                {
                    var fieldInfo = (FieldInfo)_memberInfo;
                    var value = fieldInfo.GetValue(_obj);
                    var text = value?.ToString();
                    return text ?? string.Empty;
                }
                case MemberTypes.Constructor:
                case MemberTypes.Event:
                case MemberTypes.Method:
                case MemberTypes.TypeInfo:
                case MemberTypes.Custom:
                case MemberTypes.NestedType:
                case MemberTypes.All:
                default:
                    throw new UiBuilderException("Member type not supported");
            }
        }

        public TextBlockController(MemberInfo memberInfo, object instance)
        {
            _memberInfo = memberInfo;
            Label = _memberInfo.GetCustomAttributes<UiTextBlock>().First().Label;
            _obj = instance;
        }
    }

    public class ButtonController
    {
        public string Label;
        public Action ButtonMethod;

        public ButtonController(MemberInfo memberInfo, object instance)
        {
            Label = memberInfo.GetCustomAttributes<UiButton>().First().Label;

            ThrowIfNotMethod(memberInfo);

            var methodInfo = memberInfo as MethodInfo;

            ThrowIfTakesParamsOrNull(methodInfo);

            ButtonMethod = () => methodInfo.Invoke(instance, null);
        }

        private static void ThrowIfTakesParamsOrNull(MethodInfo? methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentException("MemberInfo is not a method");

            if (methodInfo.GetParameters().Length > 0)
                throw new UiBuilderException(
                    $"UiButton attribute can only be used on methods with no parameters.");
        }

        private static void ThrowIfNotMethod(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType != MemberTypes.Method)
                throw new UiBuilderException(
                    $"UiButton attribute can only be used on methods. {memberInfo.Name} is not a method.");
        }
    }
}