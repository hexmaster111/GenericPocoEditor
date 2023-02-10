using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class SliderController : IController
{
    public string Label => _uiSlider.Label;
    public double Min => _uiSlider.Min;
    public double Max => _uiSlider.Max;

    public bool IsPlacedOnGrid => _layoutLineAttribute != null;
    public int Row => _layoutLineAttribute.LineNumber;
    public int Column => _layoutLineAttribute.GetMemberIndex(_propertyInfo.Name);

    private readonly UiLayoutLineAttribute _layoutLineAttribute;

    public double Value
    {
        get => (double)_propertyInfo.GetValue(_obj);
        set => _propertyInfo.SetValue(_obj, value);
    }


    public SliderController(MemberInfo memberInfo, object obj, UiLayoutLineAttribute? layoutLineAttribute)
    {
        if (memberInfo.MemberType != MemberTypes.Property)
            throw new UiBuilderException("UiSlider attribute can only be used on properties");

        _propertyInfo = (PropertyInfo)memberInfo;

        if (_propertyInfo.PropertyType != typeof(double))
            throw new UiBuilderException("UiSlider attribute can only be used on double properties");

        _layoutLineAttribute = layoutLineAttribute;
        _obj = obj;
        _uiSlider = _propertyInfo.GetCustomAttributes<UiSlider>().First();
    }

    private readonly UiSlider _uiSlider;
    private readonly PropertyInfo _propertyInfo;
    private readonly object _obj;
}