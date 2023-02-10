using System;
using System.Windows;
using EditorControllerFramework;

namespace WpfPocoFrontend
{
    public static class ViewBuilder
    {
        public static Window BuildWindow(FormController formController)
        {
            return new Window();
        }
    }
}