# Quick Start Guide - UI Components
## Stadium Drink Ordering Customer Application

**Quick Reference for Developers**

---

## 🚀 Getting Started

### 1. Include Required Files

The components are automatically available once these files are included in `_Layout.cshtml`:

```html
<link href="css/site.css" rel="stylesheet" />
<link href="css/components.css" rel="stylesheet" />
<script src="js/ui-enhancements.js"></script>
```

---

## 📦 Component Quick Reference

### Toast Notifications

```razor
@inject ToastNotification Toast

Toast.ShowSuccess("Success message!", 5000);
Toast.ShowError("Error message!", 7000);
Toast.ShowWarning("Warning message!", 6000);
Toast.ShowInfo("Info message!", 5000);
```

**Parameters:**
- `message` (string) - Text to display
- `duration` (int) - Milliseconds to show (default: 5000)

---

### Loading Spinner

```razor
<LoadingSpinner IsLoading="@isLoading"
                Message="Loading events..."
                Fullscreen="true" />
```

**Parameters:**
- `IsLoading` (bool) - Show/hide spinner
- `Message` (string) - Loading text (default: "Loading...")
- `Fullscreen` (bool) - Full viewport overlay (default: false)
- `ComponentId` (string) - Unique ID (auto-generated)

---

### Empty State

```razor
<EmptyState Icon="📭"
            Title="No Orders Yet"
            Description="Start ordering from our menu!"
            ActionText="Browse Menu"
            ActionCallback="@(() => Navigation.NavigateTo("/menu"))" />
```

**Parameters:**
- `Icon` (string) - Emoji or icon
- `Title` (string) - Main heading
- `Description` (string) - Explanatory text
- `ActionText` (string) - Button text (optional)
- `ActionCallback` (EventCallback) - Button click handler
- `CssClass` (string) - Additional classes (optional)
- `ComponentId` (string) - Unique ID (auto-generated)

---

## 🎨 CSS Classes Quick Reference

### Animations

```html
<div class="fade-in">Fades in when visible</div>
<div class="slide-up">Slides up when visible</div>
```

### Button Enhancements

```html
<button class="btn btn-primary btn-ripple">Ripple Effect</button>
<button class="btn btn-primary btn-animated">Animated Button</button>
```

### Card Enhancements

```html
<div class="card card-hover-lift">Lifts on hover</div>
<div class="card card-hover-glow">Glows on hover</div>
```

### Loading States

```html
<div class="skeleton skeleton-text"></div>
<div class="skeleton skeleton-card"></div>
```

---

## 🛠️ JavaScript Utilities

### Available Functions

```javascript
// Show confetti celebration
StadiumUI.showConfetti();

// Add loading overlay to element
StadiumUI.addLoadingOverlay('element-id');

// Remove loading overlay
StadiumUI.removeLoadingOverlay('element-id');

// Trap focus in modal (accessibility)
StadiumUI.trapFocus('modal-id');
```

---

## 💡 Common Patterns

### Pattern 1: Form Submission with Feedback

```razor
<EditForm Model="@model" OnValidSubmit="@HandleSubmit">
    <button type="submit" class="btn btn-primary btn-ripple"
            disabled="@isSubmitting">
        Submit
    </button>
</EditForm>

<LoadingSpinner IsLoading="@isSubmitting"
                Message="Processing..." />

@code {
    private bool isSubmitting;

    private async Task HandleSubmit()
    {
        isSubmitting = true;
        try
        {
            await Service.SubmitAsync(model);
            Toast.ShowSuccess("Submitted successfully!");
        }
        catch (Exception ex)
        {
            Toast.ShowError($"Failed: {ex.Message}");
        }
        finally
        {
            isSubmitting = false;
        }
    }
}
```

---

### Pattern 2: List with Empty State

```razor
@if (isLoading)
{
    <LoadingSpinner IsLoading="true" Message="Loading data..." />
}
else if (items.Count == 0)
{
    <EmptyState Icon="📭"
                Title="No Items Found"
                Description="Try adjusting your filters"
                ActionText="Clear Filters"
                ActionCallback="@ClearFilters" />
}
else
{
    <div class="row g-4">
        @foreach (var item in items)
        {
            <div class="col-md-4 fade-in">
                <div class="card card-hover-lift">
                    @* Card content *@
                </div>
            </div>
        }
    </div>
}
```

---

### Pattern 3: Success Celebration

```razor
private async Task PlaceOrder()
{
    try
    {
        await OrderService.CreateAsync(order);
        Toast.ShowSuccess("Order placed!");
        await JSRuntime.InvokeVoidAsync("StadiumUI.showConfetti");
        Navigation.NavigateTo("/orders");
    }
    catch (Exception ex)
    {
        Toast.ShowError($"Order failed: {ex.Message}");
    }
}
```

---

## ♿ Accessibility Checklist

### Required Elements

✅ Add ARIA labels to all interactive elements:
```html
<button aria-label="Close notification">×</button>
<a href="/events" aria-label="View available events">Events</a>
```

✅ Use semantic HTML:
```html
<header role="banner">
<nav role="navigation" aria-label="Main navigation">
<main role="main">
<footer role="contentinfo">
```

✅ Ensure keyboard navigation works:
- Tab through all interactive elements
- Enter/Space activates buttons
- Escape closes modals

✅ Test with screen reader:
- NVDA (Windows)
- JAWS (Windows)
- VoiceOver (macOS/iOS)

---

## 📱 Mobile Best Practices

### Touch Targets
Minimum size: **48x48 pixels**

```css
.btn {
    min-height: 48px;
    min-width: 48px;
}
```

### Responsive Breakpoints

```css
/* Mobile: < 576px */
/* Small tablets: >= 576px */
@media (min-width: 576px) { ... }

/* Tablets: >= 768px */
@media (min-width: 768px) { ... }

/* Desktops: >= 992px */
@media (min-width: 992px) { ... }

/* Large desktops: >= 1200px */
@media (min-width: 1200px) { ... }
```

---

## 🎯 Animation Guidelines

### Timing
- Fast actions: **200-300ms**
- Normal: **300-400ms**
- Slow/dramatic: **500-600ms**
- Never exceed: **800ms**

### Easing Functions
```css
ease-out    /* Best for entrances */
ease-in     /* Best for exits */
ease-in-out /* Best for transitions */
```

### Respect User Preferences
```css
@media (prefers-reduced-motion: reduce) {
    * {
        animation-duration: 0.01ms !important;
        transition-duration: 0.01ms !important;
    }
}
```

---

## 🐛 Common Issues & Solutions

### Issue: Toast not showing
**Solution:** Ensure ToastNotification component is in MainLayout.razor

### Issue: Loading spinner behind content
**Solution:** Check parent element has `position: relative`

### Issue: Animations not working
**Solution:** Verify components.css is loaded and no CSS conflicts

### Issue: Focus not visible
**Solution:** Check `:focus-visible` styles are not overridden

---

## 📚 Additional Resources

- **Full Documentation:** `UI_PATTERN_GUIDE.md`
- **Implementation Summary:** `UI_ENHANCEMENTS_SUMMARY.md`
- **Component Source:** `Components/Shared/`
- **Styles:** `wwwroot/css/components.css`
- **Scripts:** `wwwroot/js/ui-enhancements.js`

---

## 🆘 Need Help?

Check the comprehensive guides:
1. `UI_PATTERN_GUIDE.md` - Complete component documentation
2. `UI_ENHANCEMENTS_SUMMARY.md` - Implementation details
3. Code comments in component files

---

**Last Updated:** 2025-10-11
**Version:** 2.0
