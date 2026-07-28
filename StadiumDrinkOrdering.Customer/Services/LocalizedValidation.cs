using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace StadiumDrinkOrdering.Customer.Services;

/// <summary>
/// Attribute arguments must be compile-time constants, so a validation attribute can never receive an
/// injected <see cref="IStringLocalizer"/>. The localizer is handed to this holder once at startup
/// instead. That is safe to share: the localizer carries no culture of its own — it resolves against
/// <see cref="System.Globalization.CultureInfo.CurrentUICulture"/> on every lookup — so the same
/// instance yields Croatian or English per request/circuit exactly as an injected one would.
/// </summary>
public static class ValidationLocalizer
{
    private static IStringLocalizer? _localizer;

    public static void Configure(IStringLocalizer localizer) => _localizer = localizer;

    /// <summary>Resolves a resource key. Falls back to the key itself if startup wiring was skipped
    /// (e.g. in a unit test) so validation still reports something rather than throwing.</summary>
    public static string Get(string key) => _localizer is null ? key : _localizer[key];
}

/// <summary>Localized <see cref="RequiredAttribute"/>. Pass a SharedResources key, not a message.</summary>
public sealed class LocRequiredAttribute : RequiredAttribute
{
    public LocRequiredAttribute(string resourceKey) => ErrorMessage = resourceKey;
    public override string FormatErrorMessage(string name) => ValidationLocalizer.Get(ErrorMessage!);
}

/// <summary>Localized <see cref="StringLengthAttribute"/>. Pass a SharedResources key, not a message.</summary>
public sealed class LocStringLengthAttribute : StringLengthAttribute
{
    public LocStringLengthAttribute(int maximumLength, string resourceKey) : base(maximumLength)
        => ErrorMessage = resourceKey;
    public override string FormatErrorMessage(string name) => ValidationLocalizer.Get(ErrorMessage!);
}

/// <summary>Localized email check. <see cref="EmailAddressAttribute"/> is sealed, so the real check is
/// delegated to it rather than inherited. Pass a SharedResources key, not a message.</summary>
public sealed class LocEmailAddressAttribute : ValidationAttribute
{
    private static readonly EmailAddressAttribute Inner = new();

    public LocEmailAddressAttribute(string resourceKey) => ErrorMessage = resourceKey;
    public override bool IsValid(object? value) => Inner.IsValid(value);
    public override string FormatErrorMessage(string name) => ValidationLocalizer.Get(ErrorMessage!);
}

/// <summary>Localized phone check. <see cref="PhoneAttribute"/> is sealed, so the real check is
/// delegated to it rather than inherited. Pass a SharedResources key, not a message.</summary>
public sealed class LocPhoneAttribute : ValidationAttribute
{
    private static readonly PhoneAttribute Inner = new();

    public LocPhoneAttribute(string resourceKey) => ErrorMessage = resourceKey;
    public override bool IsValid(object? value) => Inner.IsValid(value);
    public override string FormatErrorMessage(string name) => ValidationLocalizer.Get(ErrorMessage!);
}

/// <summary>Localized <see cref="RegularExpressionAttribute"/>. Pass a SharedResources key, not a message.</summary>
public sealed class LocRegularExpressionAttribute : RegularExpressionAttribute
{
    public LocRegularExpressionAttribute(string pattern, string resourceKey) : base(pattern)
        => ErrorMessage = resourceKey;
    public override string FormatErrorMessage(string name) => ValidationLocalizer.Get(ErrorMessage!);
}

/// <summary>Localized <see cref="CompareAttribute"/>. Pass a SharedResources key, not a message.</summary>
public sealed class LocCompareAttribute : CompareAttribute
{
    public LocCompareAttribute(string otherProperty, string resourceKey) : base(otherProperty)
        => ErrorMessage = resourceKey;
    public override string FormatErrorMessage(string name) => ValidationLocalizer.Get(ErrorMessage!);
}
