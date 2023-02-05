namespace EditorControllerFramework.UiAttributes;

public class UiSlider : UiAttribute
{
    public double Min { get; set; }
    public double Max { get; set; }

    public UiSlider(string label, double min = Double.MinValue, double max = Double.MaxValue) : base(label)
    {
        Min = min;
        Max = max;
    }
}