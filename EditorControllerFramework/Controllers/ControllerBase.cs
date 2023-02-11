using System.Reflection;
using System.Reflection.Emit;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class Controller
{
    public bool IsPlacedOnGrid => _layoutLineAttribute != null;
    public int Row => _layoutLineAttribute.LineNumber;
    public int Column => _layoutLineAttribute.GetMemberIndex(PropertyInfo.Name);

    private readonly UiLayoutLineAttribute? _layoutLineAttribute;
    public readonly PropertyInfo PropertyInfo;
    public readonly object _obj;

    private object _value;

    private void newSetMethod(object value)
    {
        _value = value;
    }

    private object newGetMethod()
    {
        return _value;
    }

    public Controller(MemberInfo memberInfo, object obj, UiLayoutLineAttribute? layoutLineAttribute,
        bool isMethod = false)
    {
        if (isMethod) return;
        if (memberInfo.MemberType != MemberTypes.Property)
            throw new UiBuilderException("Controllers only support properties");

        _layoutLineAttribute = layoutLineAttribute;
        PropertyInfo = (PropertyInfo)memberInfo;
        _obj = obj;
    }
}