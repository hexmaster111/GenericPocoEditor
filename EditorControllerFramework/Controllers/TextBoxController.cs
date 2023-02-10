using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class TextBoxController : Controller
{
    public string Label;

    public string Text
    {
        get => (string)PropertyInfo.GetValue(_obj);
        set => PropertyInfo.SetValue(_obj, value);
    }

    public TextBoxController(MemberInfo memberInfo, object instance, UiLayoutLineAttribute? layoutLineAttribute)
        : base(memberInfo, instance, layoutLineAttribute)
    {
        if (PropertyInfo.PropertyType != typeof(string))
            throw new UiBuilderException("UiTextBox attribute can only be used on string properties");


        Label = PropertyInfo.GetCustomAttributes<UiTextBox>().First().Label;
    }
}