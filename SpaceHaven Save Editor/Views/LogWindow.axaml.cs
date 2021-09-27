using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SpaceHaven_Save_Editor.ViewModels;

namespace SpaceHaven_Save_Editor.Views
{
    public class LogWindow : ReactiveWindow<LogViewModel>
    {
        public LogWindow(string text, string logLocation)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            DataContext = new LogViewModel(text, logLocation);
        }
        
        public LogWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}