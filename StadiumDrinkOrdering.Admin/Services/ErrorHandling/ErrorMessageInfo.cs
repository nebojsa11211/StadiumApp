namespace StadiumDrinkOrdering.Admin.Services.ErrorHandling
{
    public class ErrorMessageInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public ErrorSeverity Severity { get; set; }
        public bool IsRetryable { get; set; }
        public bool RequiresAuth { get; set; }
        public TimeSpan? SuggestedRetryDelay { get; set; }
        public string? ActionButtonText { get; set; }
        public string? HelpLink { get; set; }

        public ErrorMessageInfo(string title, string message, ErrorSeverity severity = ErrorSeverity.Error,
            bool isRetryable = false, bool requiresAuth = false, TimeSpan? suggestedRetryDelay = null)
        {
            Title = title;
            Message = message;
            Severity = severity;
            IsRetryable = isRetryable;
            RequiresAuth = requiresAuth;
            SuggestedRetryDelay = suggestedRetryDelay;
        }
    }

    public enum ErrorSeverity
    {
        Info,     // Blue - informational messages
        Success,  // Green - positive confirmations
        Warning,  // Orange - warnings that don't block functionality
        Error,    // Red - errors that block functionality
        Critical  // Dark red - authentication, security, data loss issues
    }

    public class NotificationOptions
    {
        public int DurationMs { get; set; } = 5000;
        public bool AutoClose { get; set; } = true;
        public bool ShowCloseButton { get; set; } = true;
        public string? Icon { get; set; }
        public List<NotificationAction>? Actions { get; set; }
    }

    public class NotificationAction
    {
        public string Text { get; set; }
        public string ActionType { get; set; }
        public string? Url { get; set; }
        public Func<Task>? OnClick { get; set; }

        public NotificationAction(string text, string actionType, string? url = null, Func<Task>? onClick = null)
        {
            Text = text;
            ActionType = actionType;
            Url = url;
            OnClick = onClick;
        }
    }
}