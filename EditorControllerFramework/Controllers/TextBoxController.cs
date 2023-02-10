using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class TextBoxController : IController
{
    public string Label;

    public bool IsPlacedOnGrid => _layoutLineAttribute != null;

    public int Row => _layoutLineAttribute?.LineNumber ?? throw new InvalidOperationException();

    public int Column => _layoutLineAttribute.GetMemberIndex(_propertyInfo.Name);

    private readonly UiLayoutLineAttribute? _layoutLineAttribute;

    public string Text
    {
        get => (string)_propertyInfo.GetValue(_obj);
        set => _propertyInfo.SetValue(_obj, value);
    }

    private readonly PropertyInfo _propertyInfo;
    private readonly object _obj;


    public TextBoxController(MemberInfo memberInfo, object instance, UiLayoutLineAttribute? layoutLineAttribute)
    {
        if (memberInfo.MemberType != MemberTypes.Property)
            throw new UiBuilderException("UiTextBox attribute can only be used on properties");


        _propertyInfo = (PropertyInfo)memberInfo;

        if (_propertyInfo.PropertyType != typeof(string))
            throw new UiBuilderException("UiTextBox attribute can only be used on string properties");

        _layoutLineAttribute = layoutLineAttribute;
        Label = _propertyInfo.GetCustomAttributes<UiTextBox>().First().Label;
        _obj = instance;
    }
}