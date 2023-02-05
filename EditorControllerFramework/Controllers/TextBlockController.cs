using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

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