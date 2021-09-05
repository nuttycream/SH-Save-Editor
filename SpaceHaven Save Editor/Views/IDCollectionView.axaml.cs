using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpaceHaven_Save_Editor.ViewModels;

namespace SpaceHaven_Save_Editor.Views
{
    public class IdCollectionView : ReactiveWindow<IdCollectionViewModel>
    {
        public IdCollectionView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new IdCollectionViewModel();

            this.WhenActivated(d =>
            {
                if (ViewModel != null) d(ViewModel.ShowSaveFileDialog.RegisterHandler(ShowSaveFileDialog));
            });
        }

        public async Task ShowSaveFileDialog(InteractionContext<Unit, string?> interaction)
        {
            SaveFileDialog saveFileBox = new()
            {
                Title = "Save ID List as...",
                DefaultExtension = "txt",
                InitialFileName = "id list"
            };

            var fileNames = await saveFileBox.ShowAsync(this);
            interaction.SetOutput(fileNames);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}