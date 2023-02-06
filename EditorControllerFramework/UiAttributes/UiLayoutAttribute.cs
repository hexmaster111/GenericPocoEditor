namespace EditorControllerFramework.UiAttributes;

public class UiLayoutLine0 : UiLayoutLineAttribute
{
    public UiLayoutLine0(params string[] elementsName) : base(0, elementsName)
    {
    }
}

public class UiLayoutLine1 : UiLayoutLineAttribute
{
    public UiLayoutLine1(params string[] elementsName) : base(1, elementsName)
    {
    }
}

public class UiLayoutLine2 : UiLayoutLineAttribute
{
    public UiLayoutLine2(params string[] elementsName) : base(2, elementsName)
    {
    }
}

public class UiLayoutLineAttribute : Attribute
{
    public readonly string[] ElementsName;
    public readonly int LineNumber;

    public int GetMemberIndex(string nameOfElement)
    {
        for (var i = 0; i < ElementsName.Length; i++)
        {
            if (ElementsName[i] == nameOfElement)
                return i;
        }

        return -1;
    }

    protected UiLayoutLineAttribute(int lineNumber, params string[] elementsName)
    {
        ElementsName = elementsName;
        LineNumber = lineNumber;
    }
}