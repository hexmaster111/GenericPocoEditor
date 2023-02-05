namespace EditorControllerFramework.UiAttributes;

public class UiLayoutLine0 : UiLayoutLineAttribute
{
    public UiLayoutLine0(params string[] nameOfElements) : base(0, nameOfElements)
    {
    }
}

public class UiLayoutLine1 : UiLayoutLineAttribute
{
    public UiLayoutLine1(params string[] nameOfElements) : base(1, nameOfElements)
    {
    }
}

public class UiLayoutLine2 : UiLayoutLineAttribute
{
    public UiLayoutLine2(params string[] nameOfElements) : base(2, nameOfElements)
    {
    }
}

public class UiLayoutLineAttribute : Attribute
{
    public string[] NameOfElement;
    public int LineNumber;

    protected UiLayoutLineAttribute(int lineNumber, params string[] nameOfElements)
    {
        NameOfElement = nameOfElements;
        LineNumber = lineNumber;
    }
}

