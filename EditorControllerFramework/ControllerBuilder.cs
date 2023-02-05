using System.Diagnostics;
using EditorControllerFramework.Controllers;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework;

public static class ControllerBuilder
{
    public static FormController BuildFormController(object obj, Type type)
    {
        var testTimer = Stopwatch.StartNew();


        var members = type.GetMembers();

        var uiButtons = members
            .Where(x => x.GetCustomAttributes(typeof(UiButton), false).Length > 0);
        var uiTextBlocks = members
            .Where(x => x.GetCustomAttributes(typeof(UiTextBlock), false).Length > 0);
        var uiTextBoxes = members
            .Where(x => x.GetCustomAttributes(typeof(UiTextBox), false).Length > 0);
        var uiCheckBoxes = members
            .Where(x => x.GetCustomAttributes(typeof(UiCheckBox), false).Length > 0);
        var uiComboBoxes = members
            .Where(x => x.GetCustomAttributes(typeof(UiComboBox), false).Length > 0);
        var uiSliders = members
            .Where(x => x.GetCustomAttributes(typeof(UiSlider), false).Length > 0);


        var controllers = new FormController()
        {
            ButtonControllers = uiButtons
                .Select(x => new ButtonController(x, obj)).ToArray(),
            TextBlockControllers = uiTextBlocks
                .Select(x => new TextBlockController(x, obj)).ToArray(),
            TextBoxControllers = uiTextBoxes
                .Select(x => new TextBoxController(x, obj)).ToArray(),
            CheckBoxControllers = uiCheckBoxes
                .Select(x => new CheckBoxController(x, obj)).ToArray(),
            ComboBoxControllers = uiComboBoxes
                .Select(x => new ComboBoxController(x, obj)).ToArray(),
            SliderControllers = uiSliders
                .Select(x => new SliderController(x, obj)).ToArray()
        };

        testTimer.Stop();
        Console.WriteLine(testTimer.ElapsedTicks);

        return controllers;
    }
}