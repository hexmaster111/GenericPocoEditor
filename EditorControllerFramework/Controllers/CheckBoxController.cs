using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class CheckBoxController : IController
{
    public string Label;

    public bool IsPlacedOnGrid => _layoutLineAttribute != null;
    public int Row => _layoutLineAttribute.LineNumber;
    public int Column => _layoutLineAttribute.GetMemberIndex(_memberInfo.Name);

    private readonly UiLayoutLineAttribute? _layoutLineAttribute;

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


    public CheckBoxController(MemberInfo memberInfo, object obj, UiLayoutLineAttribute? layoutLineAttribute)
    {
        _memberInfo = memberInfo;
        _obj = obj;
        Label = _memberInfo.GetCustomAttributes<UiCheckBox>().First().Label;
        _layoutLineAttribute = layoutLineAttribute;
    }
}