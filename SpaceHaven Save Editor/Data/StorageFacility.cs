using System.Collections.ObjectModel;
using ReactiveUI;

namespace SpaceHaven_Save_Editor.Data
{
    public class StorageFacility : ReactiveObject
    {
        private ObservableCollection<Cargo> _cargo;
        private ObservableCollection<Cargo> _unmodifiedCargo;

        public StorageFacility()
        {
            _cargo = new ObservableCollection<Cargo>();
            _unmodifiedCargo = new ObservableCollection<Cargo>();
        }

        public ObservableCollection<Cargo> Cargo
        {
            get => _cargo;
            set => this.RaiseAndSetIfChanged(ref _cargo, value);
        }

        public override string ToString()
        {
            return Cargo.Count switch
            {
                > 1 => "Storage with... [" + Cargo[0].CargoName + ", " + Cargo[1].CargoName + "]",
                1 => "Storage with... [" + Cargo[0].CargoName + "]",
                _ => "Empty Storage Facility"
            };
        }
    }
}