using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class CheckBoxController : Controller
{
    public string Label;

    public bool IsChecked
    {
        get
        {
            var propertyInfo = PropertyInfo;
            var value = propertyInfo.GetValue(_obj);
            return value != null && (bool)value;
        }
        set => PropertyInfo.SetValue(_obj, value);
    }

    public CheckBoxController(
        MemberInfo memberInfo,
        object obj,
        UiLayoutLineAttribute? layoutLineAttribute) : base(memberInfo, obj, layoutLineAttribute)
    {
        Label = PropertyInfo.GetCustomAttributes<UiCheckBox>().First().Label;
    }
}