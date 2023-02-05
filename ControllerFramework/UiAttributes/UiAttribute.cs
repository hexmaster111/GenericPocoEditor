namespace ControllerFramework;

public class UiAttribute : Attribute
{
    public readonly string Label;

    protected UiAttribute(string label)
    {
        Label = label;
    }
}