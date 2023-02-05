using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

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