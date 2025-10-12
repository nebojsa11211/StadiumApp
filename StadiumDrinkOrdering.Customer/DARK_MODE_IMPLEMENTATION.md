# Dark Mode Implementation - Stadium Drink Ordering Customer App

## Overview
Comprehensive dark mode support has been implemented for the Customer application with automatic system preference detection, user preference persistence, and smooth theme transitions.

## Features Implemented

### 1. Complete Dark Theme Color Palette
- **Background Colors**: Dark slate gradients (#0f172a to #1e293b)
- **Text Colors**: High contrast text with proper WCAG AA compliance
- **Brand Colors**: Emerald green primary color (#10b981)
- **Status Colors**: Success, warning, error, info with dark variants
- **Accessibility**: Proper contrast ratios and focus indicators

### 2. Theme Switcher Component
- **Location**: Top navigation bar (right side, next to language switcher)
- **Icons**: Sun icon for light mode, Moon icon for dark mode
- **Animation**: Smooth rotation and scale transitions
- **Accessibility**: Proper ARIA labels and keyboard navigation

### 3. Automatic System Preference Detection
- Detects `prefers-color-scheme` media query
- Automatically applies dark mode if system preference is dark
- Watches for system preference changes in real-time

### 4. User Preference Persistence
- Saves theme preference to localStorage
- Persists across browser sessions
- Overrides system preference when user makes explicit choice

### 5. Smooth Transitions
- 300ms transition duration for all theme changes
- No flash of unstyled content (FOUC) on page load
- Smooth color transitions for all elements

## Files Created

### 1. `wwwroot/css/dark-mode.css`
Complete dark theme styles including:
- CSS variables for all colors
- Dark mode styles for all components
- Navigation, cards, buttons, forms, tables, modals
- Alerts, badges, dropdowns, list groups
- Custom scrollbar styling
- Accessibility features (high contrast, reduced motion)

**Key CSS Variables:**
```css
:root[data-theme="dark"] {
    --bg-primary: #0f172a;
    --bg-secondary: #1e293b;
    --text-primary: #f1f5f9;
    --brand-primary: #10b981;
    /* ... more variables */
}
```

### 2. `wwwroot/js/theme-manager.js`
JavaScript theme management system with:
- Theme initialization without FOUC
- System preference detection
- localStorage persistence
- Theme toggle functionality
- Event dispatching for theme changes
- Mobile browser meta theme-color support

**Key Functions:**
```javascript
ThemeManager.init()           // Initialize theme
ThemeManager.toggle()         // Toggle between themes
ThemeManager.setTheme(theme)  // Set specific theme
ThemeManager.getCurrentTheme() // Get current theme
ThemeManager.isDarkMode()     // Check if dark mode active
```

### 3. `Components/Shared/ThemeSwitcher.razor`
Blazor component with:
- Sun/moon icon toggle button
- Smooth animations
- Proper ARIA labels
- Event handling for theme changes
- Responsive design

## Integration Points

### MainLayout.razor
Theme switcher added to navigation bar:
```razor
<!-- Theme Switcher -->
<ThemeSwitcher />
```

### _Layout.cshtml
CSS and JavaScript files included:
```html
<!-- In <head> -->
<link href="css/dark-mode.css" rel="stylesheet" />

<!-- Before </body> -->
<script src="js/theme-manager.js"></script>
```

## Usage

### For Users
1. **Automatic**: Dark mode applies automatically based on system preference
2. **Manual**: Click sun/moon icon in top navigation to toggle theme
3. **Persistent**: Theme preference saved and restored on next visit

### For Developers

#### Check Current Theme
```javascript
// JavaScript
const isDark = window.ThemeManager.isDarkMode();
const theme = window.ThemeManager.getCurrentTheme(); // 'light' or 'dark'
```

#### Toggle Theme Programmatically
```javascript
// JavaScript
window.ThemeManager.toggle();
window.ThemeManager.setTheme('dark');
```

#### Listen for Theme Changes
```javascript
window.addEventListener('themechange', (e) => {
    console.log('Theme changed to:', e.detail.theme);
});
```

#### Blazor Interop
```csharp
// In Razor component
@inject IJSRuntime JSRuntime

// Get current theme
var isDarkMode = await JSRuntime.InvokeAsync<bool>("eval", "window.ThemeManager.isDarkMode()");

// Toggle theme
await JSRuntime.InvokeAsync<string>("eval", "window.ThemeManager.toggle()");
```

## Styling Guidelines

### Adding Dark Mode Support to New Components

1. **Use CSS Variables**:
```css
/* Light mode (default) */
.my-component {
    background-color: #ffffff;
    color: #000000;
}

/* Dark mode */
[data-theme="dark"] .my-component {
    background-color: var(--bg-secondary);
    color: var(--text-primary);
}
```

2. **Common Color Variables**:
- `var(--bg-primary)` - Main background
- `var(--bg-secondary)` - Card/surface background
- `var(--bg-tertiary)` - Input/control background
- `var(--text-primary)` - Main text
- `var(--text-secondary)` - Secondary text
- `var(--brand-primary)` - Brand color (emerald)
- `var(--border-primary)` - Border color

3. **Transitions**:
```css
* {
    transition: background-color 300ms ease-in-out,
                border-color 300ms ease-in-out,
                color 300ms ease-in-out;
}
```

## Accessibility Features

### WCAG AA Compliance
- Minimum contrast ratio of 4.5:1 for normal text
- Minimum contrast ratio of 3:1 for large text
- Proper focus indicators with emerald outline

### High Contrast Mode Support
```css
@media (prefers-contrast: high) {
    [data-theme="dark"] {
        --bg-primary: #000000;
        --text-primary: #ffffff;
        --border-primary: #ffffff;
    }
}
```

### Reduced Motion Support
```css
@media (prefers-reduced-motion: reduce) {
    [data-theme="dark"] * {
        transition: none !important;
    }
}
```

### Keyboard Navigation
- Tab order maintained
- Focus indicators visible
- Enter/Space to toggle theme

### Screen Reader Support
- Proper ARIA labels on theme toggle button
- Dynamic aria-label based on current theme
- Semantic HTML structure

## Browser Support

### Minimum Requirements
- Chrome/Edge 76+
- Firefox 67+
- Safari 12.1+
- Opera 63+

### Features by Browser
- **Modern browsers**: Full support including system preference detection
- **Older browsers**: Manual toggle works, may not detect system preference
- **IE11**: Not supported (uses light mode fallback)

## Performance Considerations

### Page Load
- Theme applied immediately on page load (no FOUC)
- Minimal JavaScript execution (~10ms)
- CSS loaded asynchronously

### Runtime
- Smooth 300ms transitions
- GPU-accelerated animations where possible
- Debounced system preference monitoring

### Storage
- Single localStorage key: `customer-theme-preference`
- ~5 bytes storage used
- Automatic cleanup not required

## Testing

### Manual Testing Checklist
- [ ] Default theme matches system preference
- [ ] Theme toggle button works correctly
- [ ] Theme persists after page reload
- [ ] All pages render correctly in both themes
- [ ] Navigation and footer styled properly
- [ ] Forms and inputs readable in dark mode
- [ ] Cards and modals display correctly
- [ ] Tables and lists styled appropriately
- [ ] Alerts and badges visible
- [ ] Buttons have proper contrast

### Browser Testing
- [ ] Chrome (Windows/Mac)
- [ ] Firefox (Windows/Mac)
- [ ] Safari (Mac/iOS)
- [ ] Edge (Windows)
- [ ] Mobile browsers (Chrome, Safari)

### Accessibility Testing
- [ ] Keyboard navigation works
- [ ] Screen reader announces theme changes
- [ ] Focus indicators visible
- [ ] Contrast ratios meet WCAG AA
- [ ] High contrast mode supported
- [ ] Reduced motion respected

## Troubleshooting

### Theme Not Applying
**Issue**: Dark mode not activating
**Solution**:
1. Clear browser cache and localStorage
2. Check browser console for JavaScript errors
3. Verify theme-manager.js is loaded
4. Check data-theme attribute on <html> element

### FOUC (Flash of Unstyled Content)
**Issue**: Brief flash of light theme before dark mode applies
**Solution**:
- Ensure theme-manager.js loads before Blazor
- Check that .no-transition class is applied during init
- Verify CSS order in _Layout.cshtml

### Theme Not Persisting
**Issue**: Theme resets to default after page reload
**Solution**:
1. Check localStorage is enabled in browser
2. Verify customer-theme-preference key exists
3. Check browser console for localStorage errors
4. Clear cookies and try again

### Icons Not Showing
**Issue**: Sun/moon icons not visible
**Solution**:
1. Verify Bootstrap Icons or custom SVGs loaded
2. Check ThemeSwitcher.razor SVG paths
3. Inspect CSS for icon color/size issues

## Future Enhancements

### Planned Features
- [ ] Theme preview before applying
- [ ] Multiple dark theme variants (OLED, high contrast)
- [ ] Scheduled theme switching (auto dark at night)
- [ ] Theme sync across tabs
- [ ] Custom color palette editor
- [ ] Theme export/import

### Optimization Opportunities
- [ ] Preload dark mode CSS for faster switching
- [ ] Use CSS color-scheme property
- [ ] Implement CSS Houdini for smooth transitions
- [ ] Add theme transition animations

## Resources

### Documentation
- [MDN: prefers-color-scheme](https://developer.mozilla.org/en-US/docs/Web/CSS/@media/prefers-color-scheme)
- [WCAG Color Contrast](https://www.w3.org/WAI/WCAG21/Understanding/contrast-minimum.html)
- [CSS Custom Properties](https://developer.mozilla.org/en-US/docs/Web/CSS/--*)

### Tools
- [Contrast Checker](https://webaim.org/resources/contrastchecker/)
- [Color Palette Generator](https://coolors.co/)
- [Dark Mode Design Guide](https://material.io/design/color/dark-theme.html)

## Support

For issues or questions:
1. Check this documentation first
2. Review browser console for errors
3. Test in different browsers
4. Check localStorage and cookies
5. Contact development team

---

**Version**: 1.0.0
**Last Updated**: 2025-01-11
**Author**: Stadium Drink Ordering Development Team
