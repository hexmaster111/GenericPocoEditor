using System.Diagnostics;
using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class ButtonController : Controller
{
    public Action ButtonMethod;
    public string Label { get; }


    private MemberInfo _memberInfo;

    private readonly UiLayoutLineAttribute? _layoutLineAttribute;

    public ButtonController(MemberInfo memberInfo, object instance, UiLayoutLineAttribute? layoutLineAttribute)
        : base(memberInfo, instance, layoutLineAttribute, true)
    {
        Label = memberInfo.GetCustomAttributes<UiButton>().First().Label;
        _memberInfo = memberInfo;

        ThrowIfNotMethod(memberInfo);

        var methodInfo = memberInfo as MethodInfo;

        ThrowIfTakesParamsOrNull(methodInfo);
        Debug.Assert(methodInfo != null, nameof(methodInfo) + " != null");

        _layoutLineAttribute = layoutLineAttribute;

        ButtonMethod = () => methodInfo.Invoke(instance, null);
    }

    private static void ThrowIfTakesParamsOrNull(MethodInfo? methodInfo)
    {
        if (methodInfo == null)
            throw new ArgumentException("MemberInfo is not a method");

        if (methodInfo.GetParameters().Length > 0)
            throw new UiBuilderException(
                $"UiButton attribute can only be used on methods with no parameters.");
    }

    private static void ThrowIfNotMethod(MemberInfo memberInfo)
    {
        if (memberInfo.MemberType != MemberTypes.Method)
            throw new UiBuilderException(
                $"UiButton attribute can only be used on methods. " +
                $"{memberInfo.Name} is not a method.");
    }
}