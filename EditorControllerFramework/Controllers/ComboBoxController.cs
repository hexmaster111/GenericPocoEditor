using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class ComboBoxController : Controller
{
    public EnumCbItem[] Items { get; set; }

    public EnumCbItem SelectedItem
    {
        get { return Items.First(x => x.Value == (int)PropertyInfo.GetValue(_obj)); }
        set => PropertyInfo.SetValue(_obj, value.Value);
    }

    public string Label;


    public class EnumCbItem
    {
        public EnumCbItem(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string Name;
        public int Value;
    }


    public ComboBoxController(MemberInfo memberInfo, object obj, UiLayoutLineAttribute? layoutLineAttribute)
        : base(memberInfo, obj, layoutLineAttribute)
    {
        Label = memberInfo.GetCustomAttributes<UiComboBox>().First().Label;


        var propertyInfo = memberInfo as PropertyInfo;
        if (propertyInfo == null) throw new ArgumentException();
        if (!propertyInfo.PropertyType.IsEnum)
            throw new Exception("UiComboBox can only be applied to enums");


        var enumMembers = propertyInfo.PropertyType
            .GetMembers(BindingFlags.Static | BindingFlags.Public);
        Items = new EnumCbItem[enumMembers.Length];

        for (int i = 0; i < enumMembers.Length; i++)
        {
            var enumMember = enumMembers[i];
            var enumValue = (int)Enum.Parse(propertyInfo.PropertyType, enumMember.Name);

            var enumNameLabelText = "";
            var enumNameLabel =
                enumMember.GetCustomAttributes(typeof(UiLabel), false)
                    .FirstOrDefault() as UiLabel;

            enumNameLabelText = enumNameLabel == null ? enumMember.Name : enumNameLabel.Text;

            Items[i] = new EnumCbItem(enumNameLabelText, enumValue);
        }
    }
}