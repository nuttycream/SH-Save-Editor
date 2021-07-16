using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SpaceHaven_Save_Editor.Views
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var sInfo = new ProcessStartInfo(e.Uri.ToString())
            {
                UseShellExecute = true
            };
            Process.Start(sInfo);
        }
    }
}