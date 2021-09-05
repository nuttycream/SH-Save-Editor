using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpaceHaven_Save_Editor.ViewModels;

namespace SpaceHaven_Save_Editor.Views
{
    public class NodeCollectionView : Window
    {
        public NodeCollectionView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new NodeCollectionViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}