using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class TextBoxController : IController
{
    public string Label;

    public bool IsPlacedOnGrid => _layoutLineAttribute != null;
    public int Row => _layoutLineAttribute.LineNumber;
    public int Column => _layoutLineAttribute.GetMemberIndex(_memberInfo.Name);

    private readonly UiLayoutLineAttribute? _layoutLineAttribute;

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

    public TextBoxController(MemberInfo memberInfo, object instance, UiLayoutLineAttribute? layoutLineAttribute)
    {
        _layoutLineAttribute = layoutLineAttribute;
        _memberInfo = memberInfo;
        Label = _memberInfo.GetCustomAttributes<UiTextBox>().First().Label;
        _obj = instance;
    }
}