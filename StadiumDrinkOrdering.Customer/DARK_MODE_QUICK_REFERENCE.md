# Dark Mode Quick Reference Guide

## Quick Start

### Enable Dark Mode
1. Open the Customer app: https://localhost:7020
2. Look for the theme toggle button in the top navigation (moon/sun icon)
3. Click the button to toggle between light and dark modes

### Theme Toggle Location
```
[Logo]  [Home] [Events] [Menu]  [Language ▼] [🌙/☀️] [User Menu]
                                              ↑
                                         Theme Toggle
```

## Color Reference

### Light Mode (Default)
| Element | Color | Hex |
|---------|-------|-----|
| Background | White | #ffffff |
| Text | Dark Gray | #1f2937 |
| Primary Button | Blue | #2563eb |
| Cards | White | #ffffff |
| Navbar | White | #ffffff |

### Dark Mode
| Element | Color | Hex | CSS Variable |
|---------|-------|-----|--------------|
| Background | Dark Slate | #0f172a | var(--bg-primary) |
| Surface (Cards) | Slate 800 | #1e293b | var(--bg-secondary) |
| Tertiary (Inputs) | Slate 700 | #334155 | var(--bg-tertiary) |
| Text Primary | Slate 100 | #f1f5f9 | var(--text-primary) |
| Text Secondary | Slate 400 | #94a3b8 | var(--text-secondary) |
| Primary (Buttons) | Emerald 500 | #10b981 | var(--brand-primary) |
| Border | Slate 700 | #334155 | var(--border-primary) |
| Success | Emerald 500 | #10b981 | var(--success) |
| Error | Red 500 | #ef4444 | var(--error) |
| Warning | Amber 500 | #f59e0b | var(--warning) |
| Info | Blue 500 | #3b82f6 | var(--info) |

## CSS Variables

### Using in Your Code
```css
/* Component styling with CSS variables */
.my-component {
    background-color: var(--bg-secondary);
    color: var(--text-primary);
    border: 1px solid var(--border-primary);
}

/* Hover state */
.my-component:hover {
    background-color: var(--bg-hover);
}

/* Primary action */
.my-button {
    background-color: var(--brand-primary);
    color: white;
}
```

### Available Variables
```css
/* Backgrounds */
--bg-primary          /* Main background */
--bg-secondary        /* Cards, surfaces */
--bg-tertiary         /* Inputs, controls */
--bg-hover            /* Hover states */
--bg-active           /* Active states */

/* Text */
--text-primary        /* Main text */
--text-secondary      /* Secondary text */
--text-tertiary       /* Tertiary text */
--text-muted          /* Muted text */

/* Borders */
--border-primary      /* Default borders */
--border-secondary    /* Secondary borders */
--border-hover        /* Hover borders */

/* Brand */
--brand-primary       /* Primary brand color */
--brand-primary-dark  /* Darker brand color */
--brand-secondary     /* Secondary brand */
--brand-accent        /* Accent color */

/* Status */
--success             /* Success color */
--success-bg          /* Success background */
--error               /* Error color */
--error-bg            /* Error background */
--warning             /* Warning color */
--warning-bg          /* Warning background */
--info                /* Info color */
--info-bg             /* Info background */

/* Shadows */
--shadow-sm           /* Small shadow */
--shadow-md           /* Medium shadow */
--shadow-lg           /* Large shadow */
--shadow-xl           /* Extra large shadow */
```

## JavaScript API

### Basic Usage
```javascript
// Get current theme
window.ThemeManager.getCurrentTheme()
// Returns: 'light' or 'dark'

// Check if dark mode
window.ThemeManager.isDarkMode()
// Returns: true or false

// Toggle theme
window.ThemeManager.toggle()
// Returns: new theme ('light' or 'dark')

// Set specific theme
window.ThemeManager.setTheme('dark')
window.ThemeManager.setTheme('light')

// Clear saved preference
window.ThemeManager.clearPreference()
```

### Advanced Usage
```javascript
// Get saved preference
window.ThemeManager.getSavedTheme()
// Returns: 'light', 'dark', or null

// Get system preference
window.ThemeManager.getSystemPreference()
// Returns: 'light' or 'dark'

// Listen for theme changes
window.addEventListener('themechange', (e) => {
    console.log('New theme:', e.detail.theme);
});
```

## Blazor Interop

### In Razor Components
```csharp
@inject IJSRuntime JSRuntime

@code {
    private bool isDarkMode = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Get current theme
            isDarkMode = await JSRuntime.InvokeAsync<bool>("eval",
                "window.ThemeManager.isDarkMode()");
            StateHasChanged();
        }
    }

    private async Task ToggleTheme()
    {
        var newTheme = await JSRuntime.InvokeAsync<string>("eval",
            "window.ThemeManager.toggle()");
        isDarkMode = (newTheme == "dark");
        StateHasChanged();
    }
}
```

## Component Examples

### Card Component
```css
/* Light mode (default) */
.card {
    background-color: white;
    color: #000;
    border: 1px solid #e5e7eb;
}

/* Dark mode */
[data-theme="dark"] .card {
    background-color: var(--bg-secondary);
    color: var(--text-primary);
    border: 1px solid var(--border-primary);
}
```

### Button Component
```css
/* Light mode */
.btn-primary {
    background-color: #2563eb;
    color: white;
}

/* Dark mode */
[data-theme="dark"] .btn-primary {
    background-color: var(--brand-primary);
    color: white;
}
```

### Form Input
```css
/* Light mode */
.form-control {
    background-color: white;
    color: #000;
    border: 1px solid #d1d5db;
}

/* Dark mode */
[data-theme="dark"] .form-control {
    background-color: var(--bg-tertiary);
    color: var(--text-primary);
    border: 1px solid var(--border-primary);
}

/* Focus state */
[data-theme="dark"] .form-control:focus {
    border-color: var(--brand-primary);
    box-shadow: 0 0 0 0.2rem rgba(16, 185, 129, 0.25);
}
```

## Transition Effects

### Standard Transition
```css
* {
    transition: background-color 300ms ease-in-out,
                border-color 300ms ease-in-out,
                color 300ms ease-in-out,
                box-shadow 300ms ease-in-out;
}
```

### Disable Transitions (Initialization)
```javascript
// Add class to prevent transitions during init
document.documentElement.classList.add('no-transition');

// Apply theme
applyTheme(theme);

// Re-enable transitions
setTimeout(() => {
    document.documentElement.classList.remove('no-transition');
}, 100);
```

## Testing Commands

### Browser Console
```javascript
// Quick test
console.log('Theme:', window.ThemeManager.getCurrentTheme());
console.log('Dark mode?', window.ThemeManager.isDarkMode());
console.log('System prefers dark?',
    window.matchMedia('(prefers-color-scheme: dark)').matches);
console.log('Saved preference:',
    localStorage.getItem('customer-theme-preference'));

// Toggle test
window.ThemeManager.toggle();
```

### Check Theme Attribute
```javascript
document.documentElement.getAttribute('data-theme')
// Should return: 'light' or 'dark'
```

### Check CSS Loading
```javascript
document.querySelector('link[href*="dark-mode.css"]') !== null
// Should return: true
```

## Keyboard Shortcuts

| Action | Keys |
|--------|------|
| Navigate to theme toggle | Tab (until focused) |
| Toggle theme | Enter or Space |
| Navigate navigation | Arrow keys |

## Accessibility Features

### Focus Indicators
```css
[data-theme="dark"] :focus-visible {
    outline: 2px solid var(--brand-primary);
    outline-offset: 2px;
}
```

### Screen Reader Support
```html
<!-- Theme toggle button -->
<button aria-label="Switch to dark mode">
    <!-- Icon -->
</button>
```

### High Contrast Mode
```css
@media (prefers-contrast: high) {
    [data-theme="dark"] {
        --bg-primary: #000000;
        --text-primary: #ffffff;
        --border-primary: #ffffff;
    }
}
```

### Reduced Motion
```css
@media (prefers-reduced-motion: reduce) {
    [data-theme="dark"] * {
        transition: none !important;
    }
}
```

## Common Patterns

### Conditional Styling
```css
/* Light mode default */
.element {
    color: #000;
}

/* Dark mode override */
[data-theme="dark"] .element {
    color: var(--text-primary);
}
```

### Gradients
```css
/* Light mode */
.hero {
    background: linear-gradient(135deg, #ffffff 0%, #f3f4f6 100%);
}

/* Dark mode */
[data-theme="dark"] .hero {
    background: linear-gradient(135deg, #1e293b 0%, #0f172a 100%);
}
```

### Shadows
```css
/* Light mode */
.card {
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

/* Dark mode */
[data-theme="dark"] .card {
    box-shadow: var(--shadow-md);
}
```

## Troubleshooting

### Theme Not Applying
```javascript
// Debug checklist
console.log('1. ThemeManager loaded?',
    typeof window.ThemeManager !== 'undefined');
console.log('2. Data attribute set?',
    document.documentElement.getAttribute('data-theme'));
console.log('3. CSS loaded?',
    document.querySelector('link[href*="dark-mode.css"]') !== null);
```

### Not Persisting
```javascript
// Check localStorage
console.log('localStorage available?',
    typeof Storage !== 'undefined');
console.log('Can write?', (() => {
    try {
        localStorage.setItem('test', '1');
        localStorage.removeItem('test');
        return true;
    } catch(e) {
        return false;
    }
})());
```

### Button Not Working
```javascript
// Check button
const btn = document.querySelector('#customer-theme-switcher button');
console.log('Button exists?', btn !== null);
console.log('Button enabled?', !btn?.disabled);
```

## Best Practices

### Do's
- ✅ Use CSS variables for colors
- ✅ Test both themes for every component
- ✅ Verify contrast ratios
- ✅ Support keyboard navigation
- ✅ Test on mobile devices
- ✅ Check accessibility

### Don'ts
- ❌ Hardcode colors in components
- ❌ Forget to test dark mode
- ❌ Ignore contrast ratios
- ❌ Skip accessibility features
- ❌ Force theme without user preference
- ❌ Use images without dark mode variants

## Browser Support

| Browser | Version | Support |
|---------|---------|---------|
| Chrome | 76+ | Full ✅ |
| Firefox | 67+ | Full ✅ |
| Safari | 12.1+ | Full ✅ |
| Edge | 79+ | Full ✅ |
| Opera | 63+ | Full ✅ |
| Samsung Internet | 12+ | Full ✅ |
| IE11 | - | Light mode only ⚠️ |

## File Locations

```
StadiumDrinkOrdering.Customer/
├── wwwroot/
│   ├── css/
│   │   └── dark-mode.css          ← Dark mode styles
│   └── js/
│       └── theme-manager.js       ← Theme management
├── Components/
│   └── Shared/
│       └── ThemeSwitcher.razor    ← Toggle component
├── Pages/
│   └── _Layout.cshtml             ← Layout integration
└── Shared/
    └── MainLayout.razor           ← Component integration
```

## Related Files

- `DARK_MODE_IMPLEMENTATION.md` - Full technical documentation
- `DARK_MODE_TESTING_GUIDE.md` - Testing procedures
- `DARK_MODE_IMPLEMENTATION_SUMMARY.md` - High-level overview

---

**Quick Help**: For any issues, check the browser console first, verify CSS and JS are loaded, and ensure data-theme attribute is set correctly on the HTML element.
