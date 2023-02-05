namespace ControllerFramework.UiAttributes;

public class UiLabel : Attribute
{
    public readonly string Text;

    public UiLabel(string text)
    {
        Text = text;
    }
}