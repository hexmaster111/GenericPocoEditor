using System.Windows.Controls;
using EditorControllerFramework.Controllers;

namespace WpfPocoFrontend.Views;

public partial class TextBoxView : UserControl
{
    public TextBoxView(TextBoxController textBoxController)
    {
        InitializeComponent();
        DataContext = textBoxController;
    }
}