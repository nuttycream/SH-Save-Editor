using System;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpaceHaven_Save_Editor.ViewModels;

namespace SpaceHaven_Save_Editor.Views
{
    public class CloneCharacterPromptWindow : ReactiveWindow<CloneCharacterPromptViewModel>
    {
        public CloneCharacterPromptWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            DataContext = new CloneCharacterPromptViewModel();

            this.WhenActivated(d => { d(ViewModel!.Continue.Subscribe(Close)); });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}