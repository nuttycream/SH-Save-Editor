using ReactiveUI;

namespace SpaceHaven_Save_Editor.Data
{
    public class DataProp : ReactiveObject
    {
        private int _value;

        public DataProp()
        {
            Name = "def";
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}