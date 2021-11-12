namespace SuperHeroSearch_App.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }

        public ErrorViewModel(ErrorType type, string message)
        {
            Type = type;
            Message = message;
        }
    }

    public enum ErrorType
    {
        None,
        Warning,
        Error
    }
}
