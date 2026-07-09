using System;
using System.Collections.Generic;
using System.Linq;

namespace StadiumDrinkOrdering.Admin.Common;

/// <summary>
/// Tracks the active sort column and direction for a data table and applies the
/// sort to an in-memory sequence. Shared by all admin tables so that clicking a
/// column header behaves consistently across the whole app.
/// </summary>
public class TableSortState
{
    /// <summary>Key of the column currently sorted, or <c>null</c> when unsorted.</summary>
    public string? Column { get; private set; }

    /// <summary>True when the active column is sorted descending.</summary>
    public bool Descending { get; private set; }

    /// <summary>
    /// Toggles sorting for a column. Clicking a new column sorts ascending;
    /// clicking the already-active column flips the direction.
    /// </summary>
    public void Toggle(string column)
    {
        if (Column == column)
        {
            Descending = !Descending;
        }
        else
        {
            Column = column;
            Descending = false;
        }
    }

    /// <summary>Seeds an initial sort without a user click.</summary>
    public void Set(string column, bool descending = false)
    {
        Column = column;
        Descending = descending;
    }

    /// <summary>True when <paramref name="column"/> is the active sort column.</summary>
    public bool IsSorted(string column) => Column == column;

    /// <summary>
    /// Arrow glyph to render next to a header. Empty when the column is not the
    /// active sort column.
    /// </summary>
    public string Indicator(string column)
        => Column != column ? string.Empty : (Descending ? "▼" : "▲");

    /// <summary>
    /// Applies the active sort to <paramref name="source"/> using the supplied key
    /// selectors. Returns the source unchanged when no column is active or the
    /// active column has no selector.
    /// </summary>
    public IEnumerable<T> Apply<T>(IEnumerable<T> source, IReadOnlyDictionary<string, Func<T, object?>> selectors)
    {
        if (Column is null || !selectors.TryGetValue(Column, out var selector))
            return source;

        return Descending
            ? source.OrderByDescending(selector, TableSortComparer.Instance)
            : source.OrderBy(selector, TableSortComparer.Instance);
    }
}

/// <summary>
/// Comparer used for table sorting. Compares strings case-insensitively, sorts
/// nulls first, and otherwise defers to the default comparer (numbers, dates,
/// enums, booleans).
/// </summary>
internal sealed class TableSortComparer : IComparer<object?>
{
    public static readonly TableSortComparer Instance = new();

    public int Compare(object? x, object? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;
        if (x is string sx && y is string sy)
            return string.Compare(sx, sy, StringComparison.OrdinalIgnoreCase);
        return Comparer<object>.Default.Compare(x, y);
    }
}
