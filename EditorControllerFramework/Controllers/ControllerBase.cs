namespace EditorControllerFramework.Controllers;

public interface IController
{
    public bool IsPlacedOnGrid { get; }
    public int Row { get; }
    public int Column { get; }
}