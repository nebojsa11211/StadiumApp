using System;
using System.Collections.Generic;
using System.Linq;

namespace StadiumDrinkOrdering.Admin.Common;

/// <summary>
/// Client-side pagination state shared by admin data tables. A page assigns the
/// full, already filtered and sorted sequence to <see cref="Source"/>, renders
/// <see cref="PageItems"/> in its <c>@foreach</c>, and drops a
/// <c>&lt;TablePager&gt;</c> below the table to drive navigation. Keeping the logic
/// here means every table pages, counts and clamps identically.
/// </summary>
public class PagedView<T>
{
    /// <summary>Page-size choices offered by the pager's selector.</summary>
    public static readonly int[] PageSizeOptions = { 10, 20, 50, 100 };

    private IReadOnlyList<T> _source = Array.Empty<T>();
    private int _page = 1;
    private int _pageSize = 20;

    /// <summary>The complete sequence to page over (already filtered and sorted).</summary>
    public IReadOnlyList<T> Source
    {
        get => _source;
        set
        {
            _source = value ?? (IReadOnlyList<T>)Array.Empty<T>();
            ClampPage();
        }
    }

    /// <summary>Rows per page. Coerced to a sane positive value.</summary>
    public int PageSize
    {
        get => _pageSize;
        set
        {
            _pageSize = value <= 0 ? 20 : value;
            ClampPage();
        }
    }

    /// <summary>Total number of rows across all pages.</summary>
    public int TotalCount => _source.Count;

    /// <summary>Number of pages (never less than one).</summary>
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)_pageSize));

    /// <summary>Active page, always clamped into <c>[1, TotalPages]</c>.</summary>
    public int CurrentPage => Math.Clamp(_page, 1, TotalPages);

    /// <summary>The rows that belong on the current page.</summary>
    public IEnumerable<T> PageItems => _source.Skip((CurrentPage - 1) * _pageSize).Take(_pageSize);

    /// <summary>1-based index of the first row shown (0 when the table is empty).</summary>
    public int FirstItem => TotalCount == 0 ? 0 : (CurrentPage - 1) * _pageSize + 1;

    /// <summary>1-based index of the last row shown.</summary>
    public int LastItem => Math.Min(CurrentPage * _pageSize, TotalCount);

    /// <summary>Navigates to <paramref name="page"/>, clamped to the valid range.</summary>
    public void GoToPage(int page) => _page = Math.Clamp(page, 1, TotalPages);

    /// <summary>Jumps back to the first page (call after a filter/search changes the data).</summary>
    public void Reset() => _page = 1;

    private void ClampPage() => _page = Math.Clamp(_page, 1, TotalPages);
}
