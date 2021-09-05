using ReactiveUI;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class Cargo : ReactiveObject
    {
        private readonly int _cargoId;
        private readonly string _cargoName;
        private int _cargoAmount;

        public Cargo(int cargoId, int cargoAmount)
        {
            if (IdCollection.DefaultItemIDs.TryGetValue(cargoId, out var cargoName))
            {
                _cargoId = cargoId;
                _cargoName = cargoName;
                CargoAmount = cargoAmount;
            }
            else
            {
                _cargoId = -1;
                _cargoName = "Invalid Cargo Id for" + cargoName;
                CargoAmount = -1;
            }
        }

        public int CargoId => _cargoId;

        public string CargoName => _cargoName;

        public int CargoAmount
        {
            get => _cargoAmount;
            set => this.RaiseAndSetIfChanged(ref _cargoAmount, value);
        }

        public override string ToString()
        {
            return CargoName + " in storage: " + CargoAmount;
        }
    }
}