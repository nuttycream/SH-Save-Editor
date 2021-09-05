using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.ViewModels;

namespace SpaceHaven_Save_Editor.Views
{
    public class StorageWindow : ReactiveWindow<StorageViewModel>
    {
        public StorageWindow(StorageFacility storageFacility)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            DataContext = new StorageViewModel(storageFacility);

            this.WhenActivated(d => d(ViewModel!.SaveAndExit.Subscribe(Close)));
        }
        
        public StorageWindow()
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

        public void DataGridCargoItems_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid) sender!;
            ViewModel!.SelectedCargoIndex = dataGrid.SelectedIndex;
        }
    }
}