using System.Diagnostics;
using System.Reflection;
using EditorControllerFramework.Controllers;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework;

public static class ControllerBuilder
{
    public static FormController BuildFormController(object obj, Type type)
    {
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


        var typeLayoutAttributes =
            type
                .GetCustomAttributes<UiLayoutLineAttribute>()
                .OrderBy(x => x.LineNumber);

        Debugger.Break();


        var controllers = new FormController()
        {
            //The layout attribute is the typeLayoutAttribute that contains the button
            ButtonControllers = uiButtons.Select(x => new ButtonController(x, obj,
                typeLayoutAttributes.FirstOrDefault(y => y.GetMemberIndex(x.Name) != -1))).ToArray(),
            TextBlockControllers = uiTextBlocks.Select(x => new TextBlockController(x, obj,
                typeLayoutAttributes.FirstOrDefault(y => y.GetMemberIndex(x.Name) != -1))).ToArray(),
            TextBoxControllers = uiTextBoxes.Select(x => new TextBoxController(x, obj,
                typeLayoutAttributes.FirstOrDefault(y => y.GetMemberIndex(x.Name) != -1))).ToArray(),
            CheckBoxControllers = uiCheckBoxes.Select(x => new CheckBoxController(x, obj,
                typeLayoutAttributes.FirstOrDefault(y => y.GetMemberIndex(x.Name) != -1))).ToArray(),
            ComboBoxControllers = uiComboBoxes.Select(x => new ComboBoxController(x, obj,
                typeLayoutAttributes.FirstOrDefault(y => y.GetMemberIndex(x.Name) != -1))).ToArray(),
            SliderControllers = uiSliders.Select(x => new SliderController(x, obj,
                typeLayoutAttributes.FirstOrDefault(y => y.GetMemberIndex(x.Name) != -1))).ToArray()
        };


        return controllers;
    }
}