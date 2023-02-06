using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class ComboBoxController : IController
{
    private MemberInfo _memberInfo;
    private object _obj;
    public bool IsPlacedOnGrid => _layoutLineAttribute != null;
    public int Row => _layoutLineAttribute.LineNumber;
    public int Column => _layoutLineAttribute.GetMemberIndex(_memberInfo.Name);

    private readonly UiLayoutLineAttribute _layoutLineAttribute;

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


    public ComboBoxController(MemberInfo memberInfo, object obj, UiLayoutLineAttribute? layoutLineAttribute)
    {
        _memberInfo = memberInfo;
        _obj = obj;
        Label = _memberInfo.GetCustomAttributes<UiComboBox>().First().Label;
        _layoutLineAttribute = layoutLineAttribute;


        switch (memberInfo.MemberType)
        {
            case MemberTypes.Field:
            {
                var fieldInfo = memberInfo as FieldInfo;
                if (fieldInfo == null) throw new ArgumentException();
                if (!fieldInfo.FieldType.IsEnum)
                    throw new Exception("UiComboBox can only be applied to enums");
                var enumMembers = fieldInfo.FieldType
                    .GetMembers(BindingFlags.Static | BindingFlags.Public);
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


                var enumMembers = propertyInfo.PropertyType
                    .GetMembers(BindingFlags.Static | BindingFlags.Public);
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