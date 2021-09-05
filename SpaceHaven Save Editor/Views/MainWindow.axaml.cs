using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.ViewModels;

namespace SpaceHaven_Save_Editor.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            this.WhenActivated(d =>
            {
                if (ViewModel == null) return;
                d(ViewModel.ShowOpenFileDialog.RegisterHandler(ShowOpenFileDialog));

                ViewModel.SaveLoaded += () =>
                {
                    d(ViewModel.GameViewModel!.OpenStorageWindow.RegisterHandler(ShowStorageDialog));
                    d(ViewModel.GameViewModel!.CloneCharacterWindow.RegisterHandler(ShowCloneCharacterDialog));
                    d(ViewModel.GameViewModel!.EditCharacterWindow.RegisterHandler(ShowCharacterDialog));
                    d(ViewModel.GameViewModel!.OpenResearchWindow.RegisterHandler(ShowResearchDialog));
                };
            });
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private async Task ShowOpenFileDialog(InteractionContext<Unit, string?> interaction)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Open Save File",
                AllowMultiple = false
            };

            var fileNames = await dialog.ShowAsync(this);
            interaction.SetOutput(fileNames.FirstOrDefault());
        }

        private async Task ShowStorageDialog(InteractionContext<StorageFacility, StorageFacility?> interaction)
        {
            var storageWindow = new StorageWindow(interaction.Input);

            var result = await storageWindow.ShowDialog<StorageFacility?>(this);

            interaction.SetOutput(result);
        }

        private async Task ShowCharacterDialog(InteractionContext<Character, Character?> interaction)
        {
            var editCharacterWindow = new CharacterWindow(interaction.Input);

            var result = await editCharacterWindow.ShowDialog<Character?>(this);

            interaction.SetOutput(result);
        }

        private async Task ShowCloneCharacterDialog(InteractionContext<Unit, string?> interactionContext)
        {
            var cloneCharacterWindow = new CloneCharacterPromptWindow();

            var result = await cloneCharacterWindow.ShowDialog<string?>(this);

            interactionContext.SetOutput(result);
        }

        private async Task ShowResearchDialog(InteractionContext<Research, Research?> interaction)
        {
            var researchWindow = new ResearchWindow(interaction.Input);

            var result = await researchWindow.ShowDialog<Research?>(this);

            interaction.SetOutput(result);
        }
    }
}