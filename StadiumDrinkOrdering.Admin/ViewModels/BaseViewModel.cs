using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StadiumDrinkOrdering.Admin.ViewModels;

/// <summary>
/// Base ViewModel class providing common functionality for all ViewModels in the Admin application.
/// Implements INotifyPropertyChanged, error handling, loading states, and resource management.
/// </summary>
public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly Dictionary<string, object?> _properties = new();
    private bool _isLoading;
    private bool _hasError;
    private string? _errorMessage;
    private DateTime _lastRefresh;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private bool _disposed;

    /// <summary>
    /// Event raised when a property value changes
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    #region Common Properties

    /// <summary>
    /// Indicates whether the ViewModel is currently loading data
    /// </summary>
    public bool IsLoading
    {
        get => _isLoading;
        protected set => SetProperty(ref _isLoading, value);
    }

    /// <summary>
    /// Indicates whether the ViewModel has encountered an error
    /// </summary>
    public bool HasError
    {
        get => _hasError;
        protected set => SetProperty(ref _hasError, value);
    }

    /// <summary>
    /// The current error message, if any
    /// </summary>
    public string? ErrorMessage
    {
        get => _errorMessage;
        protected set => SetProperty(ref _errorMessage, value);
    }

    /// <summary>
    /// The timestamp of the last data refresh
    /// </summary>
    public DateTime LastRefresh
    {
        get => _lastRefresh;
        protected set => SetProperty(ref _lastRefresh, value);
    }

    /// <summary>
    /// Indicates whether data is considered stale and needs refreshing
    /// </summary>
    public virtual bool IsStale => DateTime.UtcNow - LastRefresh > TimeSpan.FromMinutes(5);

    /// <summary>
    /// Cancellation token for async operations
    /// </summary>
    protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

    #endregion

    #region Property Management

    /// <summary>
    /// Gets a property value with optional default
    /// </summary>
    /// <typeparam name="T">Property type</typeparam>
    /// <param name="defaultValue">Default value if property not set</param>
    /// <param name="propertyName">Property name (auto-filled by compiler)</param>
    /// <returns>Property value or default</returns>
    protected T? GetProperty<T>([CallerMemberName] string propertyName = "", T? defaultValue = default)
    {
        if (string.IsNullOrEmpty(propertyName))
            throw new ArgumentException("Property name cannot be null or empty", nameof(propertyName));

        return _properties.TryGetValue(propertyName, out var value) && value is T typedValue
            ? typedValue
            : defaultValue;
    }

    /// <summary>
    /// Sets a property value and raises PropertyChanged event if value changes
    /// </summary>
    /// <typeparam name="T">Property type</typeparam>
    /// <param name="storage">Reference to backing field</param>
    /// <param name="value">New value</param>
    /// <param name="propertyName">Property name (auto-filled by compiler)</param>
    /// <returns>True if property was changed</returns>
    protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
    {
        if (string.IsNullOrEmpty(propertyName))
            throw new ArgumentException("Property name cannot be null or empty", nameof(propertyName));

        if (EqualityComparer<T>.Default.Equals(storage, value))
            return false;

        storage = value;
        _properties[propertyName] = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Sets a property value using dictionary storage and raises PropertyChanged event if value changes
    /// </summary>
    /// <typeparam name="T">Property type</typeparam>
    /// <param name="value">New value</param>
    /// <param name="propertyName">Property name (auto-filled by compiler)</param>
    /// <returns>True if property was changed</returns>
    protected virtual bool SetProperty<T>(T value, [CallerMemberName] string propertyName = "")
    {
        if (string.IsNullOrEmpty(propertyName))
            throw new ArgumentException("Property name cannot be null or empty", nameof(propertyName));

        var currentValue = GetProperty<T>(propertyName);
        if (EqualityComparer<T>.Default.Equals(currentValue, value))
            return false;

        _properties[propertyName] = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Raises the PropertyChanged event for the specified property
    /// </summary>
    /// <param name="propertyName">Property name that changed</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region Error Handling

    /// <summary>
    /// Sets error state with message
    /// </summary>
    /// <param name="message">Error message</param>
    protected virtual void SetError(string message)
    {
        ErrorMessage = message;
        HasError = !string.IsNullOrEmpty(message);
        IsLoading = false;
    }

    /// <summary>
    /// Sets error state from exception
    /// </summary>
    /// <param name="exception">Exception that occurred</param>
    /// <param name="userFriendlyMessage">Optional user-friendly message</param>
    protected virtual void SetError(Exception exception, string? userFriendlyMessage = null)
    {
        var message = userFriendlyMessage ?? GetUserFriendlyMessage(exception);
        SetError(message);

        // Log the full exception for debugging (would integrate with logging service)
        Console.WriteLine($"ViewModel Error ({GetType().Name}): {exception}");
    }

    /// <summary>
    /// Clears error state
    /// </summary>
    protected virtual void ClearError()
    {
        ErrorMessage = null;
        HasError = false;
    }

    /// <summary>
    /// Converts technical exceptions to user-friendly messages
    /// </summary>
    /// <param name="exception">Exception to convert</param>
    /// <returns>User-friendly error message</returns>
    protected virtual string GetUserFriendlyMessage(Exception exception) => exception switch
    {
        UnauthorizedAccessException => "Authentication required. Please log in to continue.",
        HttpRequestException => "Network error occurred. Please check your connection and try again.",
        TaskCanceledException => "Request timed out. Please try again.",
        ArgumentException => "Invalid input provided. Please check your data and try again.",
        InvalidOperationException => "Operation cannot be performed at this time. Please try again later.",
        _ => "An unexpected error occurred. Please try again or contact support."
    };

    #endregion

    #region Loading State Management

    /// <summary>
    /// Executes an async operation with automatic loading state management and error handling
    /// </summary>
    /// <param name="operation">Async operation to execute</param>
    /// <param name="errorMessage">Custom error message if operation fails</param>
    /// <returns>Task representing the operation</returns>
    protected async Task ExecuteAsync(Func<CancellationToken, Task> operation, string? errorMessage = null)
    {
        if (_disposed) return;

        try
        {
            ClearError();
            IsLoading = true;

            await operation(CancellationToken);
            LastRefresh = DateTime.UtcNow;
        }
        catch (OperationCanceledException) when (CancellationToken.IsCancellationRequested)
        {
            // Operation was cancelled - don't treat as error
        }
        catch (Exception ex)
        {
            SetError(ex, errorMessage);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Executes an async operation with return value, automatic loading state management and error handling
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="operation">Async operation to execute</param>
    /// <param name="errorMessage">Custom error message if operation fails</param>
    /// <returns>Operation result or default value if error occurred</returns>
    protected async Task<T?> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, string? errorMessage = null)
    {
        if (_disposed) return default(T);

        try
        {
            ClearError();
            IsLoading = true;

            var result = await operation(CancellationToken);
            LastRefresh = DateTime.UtcNow;
            return result;
        }
        catch (OperationCanceledException) when (CancellationToken.IsCancellationRequested)
        {
            // Operation was cancelled - return default
            return default(T);
        }
        catch (Exception ex)
        {
            SetError(ex, errorMessage);
            return default(T);
        }
        finally
        {
            IsLoading = false;
        }
    }

    #endregion

    #region Data Freshness

    /// <summary>
    /// Abstract method for refreshing ViewModel data - must be implemented by derived classes
    /// </summary>
    /// <returns>Task representing the refresh operation</returns>
    public abstract Task RefreshAsync();

    /// <summary>
    /// Refreshes data if it's stale, otherwise returns immediately
    /// </summary>
    /// <returns>Task representing the refresh operation</returns>
    public virtual async Task RefreshIfStaleAsync()
    {
        if (IsStale && !IsLoading)
        {
            await RefreshAsync();
        }
    }

    #endregion

    #region Validation

    /// <summary>
    /// Validation errors dictionary
    /// </summary>
    private readonly Dictionary<string, List<string>> _validationErrors = new();

    /// <summary>
    /// Indicates whether the ViewModel has validation errors
    /// </summary>
    public bool HasValidationErrors => _validationErrors.Any();

    /// <summary>
    /// Gets validation errors for a specific property
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <returns>List of validation errors for the property</returns>
    public IEnumerable<string> GetValidationErrors(string propertyName)
    {
        return _validationErrors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<string>();
    }

    /// <summary>
    /// Sets validation errors for a property
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <param name="errors">Validation errors</param>
    protected void SetValidationErrors(string propertyName, IEnumerable<string> errors)
    {
        var errorList = errors.ToList();

        if (errorList.Any())
        {
            _validationErrors[propertyName] = errorList;
        }
        else
        {
            _validationErrors.Remove(propertyName);
        }

        OnPropertyChanged(nameof(HasValidationErrors));
    }

    /// <summary>
    /// Clears validation errors for a property
    /// </summary>
    /// <param name="propertyName">Property name</param>
    protected void ClearValidationErrors(string propertyName)
    {
        if (_validationErrors.Remove(propertyName))
        {
            OnPropertyChanged(nameof(HasValidationErrors));
        }
    }

    /// <summary>
    /// Clears all validation errors
    /// </summary>
    protected void ClearAllValidationErrors()
    {
        if (_validationErrors.Any())
        {
            _validationErrors.Clear();
            OnPropertyChanged(nameof(HasValidationErrors));
        }
    }

    #endregion

    #region Disposal

    /// <summary>
    /// Disposes resources used by the ViewModel
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected dispose method for derived classes to override
    /// </summary>
    /// <param name="disposing">True if disposing managed resources</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _properties.Clear();
            _validationErrors.Clear();
            _disposed = true;
        }
    }

    /// <summary>
    /// Throws ObjectDisposedException if the ViewModel is disposed
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);
    }

    #endregion
}

/// <summary>
/// Generic base ViewModel for ViewModels that work with collections of data
/// </summary>
/// <typeparam name="T">Type of data items</typeparam>
public abstract class BaseCollectionViewModel<T> : BaseViewModel where T : class
{
    private List<T> _items = new();
    private List<T> _filteredItems = new();
    private int _totalCount;
    private int _currentPage = 1;
    private int _pageSize = 20;
    private string _searchText = string.Empty;

    /// <summary>
    /// All items in the collection
    /// </summary>
    public List<T> Items
    {
        get => _items;
        protected set => SetProperty(ref _items, value ?? new List<T>());
    }

    /// <summary>
    /// Filtered items based on current filter criteria
    /// </summary>
    public List<T> FilteredItems
    {
        get => _filteredItems;
        protected set => SetProperty(ref _filteredItems, value ?? new List<T>());
    }

    /// <summary>
    /// Total count of items (for pagination)
    /// </summary>
    public int TotalCount
    {
        get => _totalCount;
        protected set => SetProperty(ref _totalCount, value);
    }

    /// <summary>
    /// Current page number (1-based)
    /// </summary>
    public int CurrentPage
    {
        get => _currentPage;
        set => SetProperty(ref _currentPage, Math.Max(1, value));
    }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => SetProperty(ref _pageSize, Math.Max(1, value));
    }

    /// <summary>
    /// Search text for filtering
    /// </summary>
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value ?? string.Empty);
    }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages => TotalCount > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    /// <summary>
    /// Indicates whether there's a previous page
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// Indicates whether there's a next page
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;

    /// <summary>
    /// Indicates whether the collection is empty
    /// </summary>
    public bool IsEmpty => !Items.Any();

    /// <summary>
    /// Applies filtering to the items collection
    /// </summary>
    protected abstract void ApplyFiltering();

    /// <summary>
    /// Applies sorting to the filtered items
    /// </summary>
    protected abstract void ApplySorting();

    /// <summary>
    /// Updates filtered and sorted items when items or filters change
    /// </summary>
    protected virtual void UpdateFilteredItems()
    {
        ApplyFiltering();
        ApplySorting();
    }

    /// <summary>
    /// Moves to the next page if available
    /// </summary>
    public virtual async Task NextPageAsync()
    {
        if (HasNextPage)
        {
            CurrentPage++;
            await RefreshAsync();
        }
    }

    /// <summary>
    /// Moves to the previous page if available
    /// </summary>
    public virtual async Task PreviousPageAsync()
    {
        if (HasPreviousPage)
        {
            CurrentPage--;
            await RefreshAsync();
        }
    }

    /// <summary>
    /// Goes to a specific page
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    public virtual async Task GoToPageAsync(int page)
    {
        var newPage = Math.Max(1, Math.Min(page, TotalPages));
        if (newPage != CurrentPage)
        {
            CurrentPage = newPage;
            await RefreshAsync();
        }
    }
}