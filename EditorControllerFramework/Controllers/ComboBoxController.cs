using System.Reflection;
using EditorControllerFramework.Exceptions;
using EditorControllerFramework.UiAttributes;

namespace EditorControllerFramework.Controllers;

public class ComboBoxController : IController
{
    private readonly PropertyInfo _propertyInfo;
    private readonly object _obj;
    public bool IsPlacedOnGrid => _layoutLineAttribute != null;
    public int Row => _layoutLineAttribute.LineNumber;
    public int Column => _layoutLineAttribute.GetMemberIndex(_propertyInfo.Name);

    private readonly UiLayoutLineAttribute _layoutLineAttribute;

    public EnumCbItem[] Items { get; set; }

    public EnumCbItem SelectedItem
    {
        get { return Items.First(x => x.Value == (int)_propertyInfo.GetValue(_obj)); }
        set => _propertyInfo.SetValue(_obj, value.Value);
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
    {
        if (memberInfo.MemberType != MemberTypes.Property)
            throw new UiBuilderException("UiComboBox attribute can only be used on properties");

        _propertyInfo = (PropertyInfo)memberInfo;
        _obj = obj;
        Label = _propertyInfo.GetCustomAttributes<UiComboBox>().First().Label;
        _layoutLineAttribute = layoutLineAttribute;


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