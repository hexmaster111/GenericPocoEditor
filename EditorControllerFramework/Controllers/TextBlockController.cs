using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class TextBlockController : Controller
{
    public string Label;
    public string Text => (string)PropertyInfo.GetValue(_obj);


    public TextBlockController(MemberInfo memberInfo, object instance, UiLayoutLineAttribute? layoutLineAttribute)
        : base(memberInfo, instance, layoutLineAttribute)
    {
        if (PropertyInfo.PropertyType != typeof(string))
            throw new UiBuilderException("UiTextBlock attribute can only be used on string properties");

        Label = PropertyInfo.GetCustomAttributes<UiTextBlock>().First().Label;
    }
}