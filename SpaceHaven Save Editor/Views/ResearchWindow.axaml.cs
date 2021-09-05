using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.ViewModels;
namespace SpaceHaven_Save_Editor.Views
{
    public class ResearchWindow : ReactiveWindow<ResearchViewModel>
    {
        public ResearchWindow(Research research)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new ResearchViewModel(research);
            
            this.WhenActivated(d => d(ViewModel!.SaveAndExit.Subscribe(Close)));
        }
        
        public ResearchWindow()
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