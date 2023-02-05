using System;

namespace Editor.EditorTags;

public class UiTextBlock : UiAttribute
{
    public UiTextBlock(string label) : base(label)
    {
    }
}

public class UiTextBox : UiAttribute
{
    public UiTextBox(string label) : base(label)
    {
    }
}

public class UiButton : UiAttribute
{
    public UiButton(string label) : base(label)
    {
    }
}

public class UiComboBox : UiAttribute
{
    public UiComboBox(string label) : base(label)
    {
    }
}

public class UiCheckBox : UiAttribute
{
    public UiCheckBox(string label) : base(label)
    {
    }
}

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

public class UiAttribute : Attribute
{
    public readonly string Label;

    protected UiAttribute(string label)
    {
        Label = label;
    }
}

public class Label : Attribute
{
    public readonly string Text;

    public Label(string text)
    {
        Text = text;
    }
}

public class DEBUG : Attribute
{
}

public class UiBuilderException : Exception
{
    public UiBuilderException(string message) : base(message)
    {
    }
}