using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class TextBlockController : IController
{
    public string Label;

    public string Text => (string)_propertyInfo.GetValue(_obj);

    public bool IsPlacedOnGrid => _layoutLineAttribute != null;
    public int Row => _layoutLineAttribute.LineNumber;
    public int Column => _layoutLineAttribute.GetMemberIndex(_propertyInfo.Name);

    private readonly UiLayoutLineAttribute? _layoutLineAttribute;

    private readonly PropertyInfo _propertyInfo;
    private readonly object _obj;


    public TextBlockController(MemberInfo memberInfo, object instance, UiLayoutLineAttribute? layoutLineAttribute)
    {
        if (memberInfo.MemberType != MemberTypes.Property)
            throw new UiBuilderException("UiTextBlock attribute can only be used on properties");

        _propertyInfo = (PropertyInfo)memberInfo;

        if (_propertyInfo.PropertyType != typeof(string))
            throw new UiBuilderException("UiTextBlock attribute can only be used on string properties");

        _layoutLineAttribute = layoutLineAttribute;
        Label = _propertyInfo.GetCustomAttributes<UiTextBlock>().First().Label;
        _obj = instance;
    }
}