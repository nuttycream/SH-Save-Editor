namespace SpaceHaven_Save_Editor.ViewModels
{
    public class LogViewModel : ViewModelBase
    {
        public LogViewModel(string text, string logLocation)
        {
            LogLocation = "Log Location: " + logLocation;
            LogText = text;
        }

        public LogViewModel()
        {
            LogLocation = "";
            LogText = "";
        }
        
        public string LogLocation { get; init; }
        public string LogText { get; init; }
    }
}