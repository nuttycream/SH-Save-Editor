using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SpaceHaven_Save_Editor.Views
{
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }
        

        private void ScrollViewer_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}