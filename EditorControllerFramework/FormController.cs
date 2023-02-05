using EditorControllerFramework.Controllers;

namespace EditorControllerFramework;

public struct FormController
{
    public ButtonController[] ButtonControllers;
    public TextBlockController[] TextBlockControllers;
    public TextBoxController[] TextBoxControllers;
    public CheckBoxController[] CheckBoxControllers;
    public ComboBoxController[] ComboBoxControllers;
    public SliderController[] SliderControllers;
}