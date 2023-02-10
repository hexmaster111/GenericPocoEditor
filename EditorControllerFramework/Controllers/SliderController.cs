using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class SliderController : Controller
{
    public string Label => _uiSlider.Label;
    public double Min => _uiSlider.Min;
    public double Max => _uiSlider.Max;

    private readonly UiSlider _uiSlider;

    public double Value
    {
        get => (double)PropertyInfo.GetValue(_obj);
        set => PropertyInfo.SetValue(_obj, value);
    }


    public SliderController(MemberInfo memberInfo, object obj, UiLayoutLineAttribute? layoutLineAttribute)
        : base(memberInfo, obj, layoutLineAttribute)
    {
        if (PropertyInfo.PropertyType != typeof(double))
            throw new UiBuilderException("UiSlider attribute can only be used on double properties");

        _uiSlider = PropertyInfo.GetCustomAttributes<UiSlider>().First();
    }
}