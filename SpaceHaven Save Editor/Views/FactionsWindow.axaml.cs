using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.ViewModels;

namespace SpaceHaven_Save_Editor.Views
{
    public class FactionsWindow : ReactiveWindow<FactionsViewModel>
    {
        public FactionsWindow(IEnumerable<Faction> interactionInput)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new FactionsViewModel(interactionInput);

            this.WhenActivated(d => d(ViewModel!.SaveAndExit.Subscribe(Close)));
        }

        public FactionsWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new FactionsViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}