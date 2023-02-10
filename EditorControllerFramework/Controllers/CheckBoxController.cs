using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class CheckBoxController : IController
{
    public string Label;

    public bool IsPlacedOnGrid => _layoutLineAttribute != null;
    public int Row => _layoutLineAttribute.LineNumber;
    public int Column => _layoutLineAttribute.GetMemberIndex(_propertyInfo.Name);

    public bool IsChecked
    {
        get
        {
            var propertyInfo = _propertyInfo;
            var value = propertyInfo.GetValue(_obj);
            return value != null && (bool)value;
        }
        set => _propertyInfo.SetValue(_obj, value);
    }

    private readonly UiLayoutLineAttribute? _layoutLineAttribute;
    private readonly PropertyInfo _propertyInfo;
    private readonly object _obj;


    public CheckBoxController(MemberInfo propertyInfo, object obj, UiLayoutLineAttribute? layoutLineAttribute)
    {
        if (propertyInfo.MemberType != MemberTypes.Property)
            throw new UiBuilderException("UiCheckBox attribute can only be used on properties");
        _propertyInfo = (PropertyInfo)propertyInfo;
        _obj = obj;
        Label = _propertyInfo.GetCustomAttributes<UiCheckBox>().First().Label;
        _layoutLineAttribute = layoutLineAttribute;
    }
}