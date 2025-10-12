# Stadium Drink Ordering - UI Pattern Guide

## Overview
This guide documents the enhanced UI/UX patterns, components, and best practices implemented in the Stadium Drink Ordering Customer application.

---

## Table of Contents
1. [New Components](#new-components)
2. [CSS Architecture](#css-architecture)
3. [JavaScript Enhancements](#javascript-enhancements)
4. [Accessibility Features](#accessibility-features)
5. [Animation Patterns](#animation-patterns)
6. [Usage Examples](#usage-examples)
7. [Best Practices](#best-practices)

---

## New Components

### 1. Toast Notification Component
**File:** `Components/Shared/ToastNotification.razor`

**Purpose:** Provides real-time feedback for user actions with animated toast notifications.

**Features:**
- Success, Error, Warning, and Info states
- Auto-dismissal with configurable duration
- Slide-in animation from top-right
- Manual close button
- Stacking support for multiple toasts
- Time ago display

**Usage:**
```razor
@inject ToastNotification Toast

// In your component
Toast.ShowSuccess("Order placed successfully!", 5000);
Toast.ShowError("Failed to process payment", 7000);
Toast.ShowWarning("Low stock on selected items", 6000);
Toast.ShowInfo("New event added to your favorites", 5000);
```

**CSS Classes:**
- `.toast-success` - Green gradient for success messages
- `.toast-error` - Red gradient for error messages
- `.toast-warning` - Orange gradient for warnings
- `.toast-info` - Blue gradient for information

---

### 2. Loading Spinner Component
**File:** `Components/Shared/LoadingSpinner.razor`

**Purpose:** Beautiful stadium-themed loading indicator with soccer ball animation.

**Features:**
- Stadium-themed loader with rotating soccer ball
- Customizable loading message
- Fullscreen or inline overlay modes
- Smooth fade-in/fade-out
- Optional backdrop blur

**Usage:**
```razor
<LoadingSpinner IsLoading="@isLoading"
                Message="Loading events..."
                Fullscreen="true"
                ComponentId="events-loader" />
```

**Parameters:**
- `IsLoading` (bool) - Shows/hides the loader
- `Message` (string) - Loading text displayed below spinner
- `Fullscreen` (bool) - Full viewport overlay vs inline
- `ComponentId` (string) - Unique identifier for the loader

**CSS Classes:**
- `.loading-overlay` - Backdrop overlay
- `.stadium-loader` - Main loader container
- `.stadium-ball` - Animated soccer ball emoji
- `.loading-message` - Message text styling

---

### 3. Empty State Component
**File:** `Components/Shared/EmptyState.razor`

**Purpose:** User-friendly empty state display for no data scenarios.

**Features:**
- Large animated icon
- Descriptive title and message
- Optional call-to-action button
- Floating icon animation
- Customizable styling

**Usage:**
```razor
<EmptyState Icon="📭"
            Title="No Orders Yet"
            Description="You haven't placed any orders. Browse our menu to get started!"
            ActionText="Browse Menu"
            ActionCallback="@(() => Navigation.NavigateTo("/menu"))"
            ComponentId="orders-empty" />
```

**Parameters:**
- `Icon` (string) - Emoji or icon to display
- `Title` (string) - Main heading
- `Description` (string) - Explanatory text
- `ActionText` (string) - Button text (optional)
- `ActionCallback` (EventCallback) - Button click handler
- `CssClass` (string) - Additional CSS classes
- `ComponentId` (string) - Unique identifier

**CSS Classes:**
- `.empty-state` - Container styling
- `.empty-state-icon` - Icon with float animation
- `.empty-state-title` - Title typography
- `.empty-state-description` - Description text

---

## CSS Architecture

### File Structure
```
wwwroot/css/
├── site.css                 # Main styles (existing, enhanced)
├── components.css           # New component styles
├── auth.css                 # Authentication page styles
└── stadium-components.css   # Stadium-specific components
```

### Key CSS Files

#### components.css
Contains styles for all new UI components:
- Toast notifications
- Loading spinners
- Empty states
- Success animations
- Button enhancements
- Card interactions
- Progress indicators
- Accessibility improvements

**Key Features:**
- CSS custom properties for theming
- Responsive breakpoints
- Animation keyframes
- Reduced motion support
- High contrast mode support

### CSS Custom Properties
```css
:root {
    /* Primary Colors */
    --stadium-primary: #10B981;
    --stadium-primary-dark: #0A5F38;
    --stadium-primary-light: #34D399;

    /* Accent Colors */
    --stadium-accent: #FBBF24;
    --stadium-danger: #EF4444;
    --stadium-success: #22C55E;

    /* Shadows */
    --stadium-shadow: 0 1px 3px 0 rgb(0 0 0 / 0.1);
    --stadium-shadow-md: 0 4px 6px -1px rgb(0 0 0 / 0.1);
    --stadium-shadow-lg: 0 10px 15px -3px rgb(0 0 0 / 0.1);

    /* Border Radius */
    --stadium-radius: 8px;
    --stadium-radius-lg: 12px;
    --stadium-radius-xl: 16px;
}
```

---

## JavaScript Enhancements

### File: `wwwroot/js/ui-enhancements.js`

**Purpose:** Provides interactive UI enhancements without frameworks.

**Features:**

#### 1. Button Ripple Effect
Adds material-design-inspired ripple effect to buttons on click.

**Usage:**
```html
<button class="btn btn-primary btn-ripple">Click Me</button>
<button class="btn btn-secondary btn-animated">Animated Button</button>
```

#### 2. Smooth Scroll
Enables smooth scrolling for all anchor links.

**Automatic:** Works on all `<a href="#section">` links.

#### 3. Lazy Loading Images
Improves performance by lazy-loading images with IntersectionObserver.

**Usage:**
```html
<img data-src="image.jpg" alt="Description" class="lazy-load" />
```

#### 4. Animate on Scroll
Triggers animations when elements enter viewport.

**Usage:**
```html
<div class="fade-in">Fades in when scrolled into view</div>
<div class="slide-up">Slides up when visible</div>
```

#### 5. Skip to Content
Accessibility feature for keyboard navigation.

**Automatic:** Skip link added to MainLayout.razor.

### Utility Functions (window.StadiumUI)

```javascript
// Show confetti animation (success celebration)
StadiumUI.showConfetti();

// Add loading overlay to specific element
StadiumUI.addLoadingOverlay('element-id');

// Remove loading overlay
StadiumUI.removeLoadingOverlay('element-id');

// Trap focus in modal (accessibility)
StadiumUI.trapFocus('modal-id');
```

---

## Accessibility Features

### WCAG 2.1 Level AA Compliance

#### 1. Skip to Content Link
Allows keyboard users to skip navigation and jump directly to main content.

**Implementation:**
```html
<a href="#main-content" class="skip-to-content">Skip to main content</a>
```

**Behavior:**
- Hidden by default (positioned off-screen)
- Visible when focused via keyboard
- Smooth scroll to main content area

#### 2. ARIA Labels
All interactive elements have descriptive ARIA labels.

**Examples:**
```html
<nav role="navigation" aria-label="Main navigation">
<button aria-label="Close notification">×</button>
<a href="/events" aria-label="View available events">Events</a>
```

#### 3. Focus Management
Enhanced focus indicators for keyboard navigation.

**Features:**
- 3px solid outline on focus-visible
- High contrast mode support
- Focus trap for modals
- Logical tab order

**CSS:**
```css
*:focus-visible {
    outline: 3px solid var(--stadium-primary);
    outline-offset: 2px;
    border-radius: 4px;
}
```

#### 4. Semantic HTML
Proper use of semantic elements for screen readers.

**Structure:**
```html
<header role="banner">
<nav role="navigation" aria-label="Main navigation">
<main role="main">
<footer role="contentinfo">
```

#### 5. Color Contrast
All text meets WCAG AA contrast ratios (4.5:1 minimum).

#### 6. Reduced Motion Support
Respects user's motion preferences.

```css
@media (prefers-reduced-motion: reduce) {
    *,
    *::before,
    *::after {
        animation-duration: 0.01ms !important;
        animation-iteration-count: 1 !important;
        transition-duration: 0.01ms !important;
    }
}
```

---

## Animation Patterns

### 1. Fade In
Smooth opacity transition for content appearance.

**Usage:**
```html
<div class="fade-in">Content</div>
```

**Animation:**
```css
@keyframes fadeIn {
    from { opacity: 0; }
    to { opacity: 1; }
}
```

### 2. Slide Up
Content slides up from bottom with fade.

**Usage:**
```html
<div class="slide-up">Content</div>
```

**Animation:**
```css
@keyframes slideUp {
    from {
        opacity: 0;
        transform: translateY(20px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}
```

### 3. Pulse
Attention-grabbing pulse effect for live indicators.

**Usage:**
```html
<span class="live-pulse"></span>
```

**Animation:**
```css
@keyframes pulse {
    0%, 100% {
        opacity: 1;
        transform: scale(1);
    }
    50% {
        opacity: 0.5;
        transform: scale(1.3);
    }
}
```

### 4. Bounce
Playful bounce animation for soccer ball icon.

**Usage:**
```html
<span class="stadium-icon">⚽</span>
```

**Animation:**
```css
@keyframes bounce {
    0%, 100% { transform: translateY(0) rotate(0deg); }
    25% { transform: translateY(-10px) rotate(-5deg); }
    50% { transform: translateY(0) rotate(0deg); }
    75% { transform: translateY(-5px) rotate(5deg); }
}
```

### 5. Shimmer (Loading)
Skeleton loading shimmer effect.

**Usage:**
```html
<div class="skeleton skeleton-text"></div>
<div class="skeleton skeleton-card"></div>
```

**Animation:**
```css
@keyframes shimmer {
    0% { background-position: -200% 0; }
    100% { background-position: 200% 0; }
}
```

### 6. Confetti
Success celebration animation.

**Usage:**
```javascript
StadiumUI.showConfetti();
```

**Animation:**
```css
@keyframes confetti-fall {
    0% {
        transform: translateY(0) rotate(0deg);
        opacity: 1;
    }
    100% {
        transform: translateY(100vh) rotate(720deg);
        opacity: 0;
    }
}
```

---

## Usage Examples

### Complete Form with Feedback

```razor
@page "/example"
@inject ToastNotification Toast

<h2>Order Form</h2>

<LoadingSpinner IsLoading="@isSubmitting" Message="Processing order..." />

<EditForm Model="@orderModel" OnValidSubmit="@HandleSubmit">
    <DataAnnotationsValidator />

    <div class="form-group">
        <label>Customer Name</label>
        <InputText class="form-control" @bind-Value="orderModel.Name" />
        <ValidationMessage For="@(() => orderModel.Name)" />
    </div>

    <button type="submit" class="btn btn-primary btn-ripple" disabled="@isSubmitting">
        Submit Order
    </button>
</EditForm>

@if (orders.Count == 0)
{
    <EmptyState Icon="📭"
                Title="No Orders"
                Description="Start by adding items to your cart"
                ActionText="Browse Menu"
                ActionCallback="@(() => Navigation.NavigateTo("/menu"))" />
}

@code {
    private bool isSubmitting;
    private List<Order> orders = new();

    private async Task HandleSubmit()
    {
        isSubmitting = true;
        StateHasChanged();

        try
        {
            await OrderService.CreateOrderAsync(orderModel);
            Toast.ShowSuccess("Order placed successfully!");
            await JSRuntime.InvokeVoidAsync("StadiumUI.showConfetti");
        }
        catch (Exception ex)
        {
            Toast.ShowError($"Failed to place order: {ex.Message}");
        }
        finally
        {
            isSubmitting = false;
            StateHasChanged();
        }
    }
}
```

### Event Cards with Animations

```razor
<div class="row g-4">
    @foreach (var evt in events)
    {
        <div class="col-md-6 col-lg-4 fade-in">
            <div class="match-fixture-card card-hover-lift">
                <div class="match-header">
                    <h3>@evt.Name</h3>
                    <span class="badge badge-primary">@evt.Type</span>
                </div>
                <div class="match-pitch-details">
                    <p>@evt.Date.ToString("MMM dd, yyyy")</p>
                    <p>@evt.Location</p>
                </div>
                <div class="match-cta">
                    <button class="btn-get-tickets btn-ripple"
                            @onclick="() => ViewEvent(evt.Id)">
                        <span class="btn-icon">🎟️</span>
                        <span class="btn-text">GET TICKETS</span>
                        <span class="btn-arrow">→</span>
                    </button>
                </div>
            </div>
        </div>
    }
</div>
```

---

## Best Practices

### 1. Component Usage

**DO:**
- Use `LoadingSpinner` for async operations
- Show `Toast` notifications for user feedback
- Use `EmptyState` for no-data scenarios
- Apply `btn-ripple` for interactive buttons
- Add ARIA labels to all interactive elements

**DON'T:**
- Don't use generic "Loading..." text without context
- Don't show errors without user-friendly messages
- Don't create custom loaders when component exists
- Don't ignore accessibility requirements

### 2. Animation Guidelines

**DO:**
- Keep animations under 400ms for responsiveness
- Use `ease-out` for entrances, `ease-in` for exits
- Respect `prefers-reduced-motion` setting
- Apply animations to enhance, not distract

**DON'T:**
- Don't animate continuously (causes motion sickness)
- Don't use more than 2-3 animations simultaneously
- Don't ignore reduced motion preferences
- Don't make animations required for functionality

### 3. Accessibility

**DO:**
- Provide descriptive ARIA labels
- Maintain logical tab order
- Ensure color contrast ratios
- Test with keyboard navigation
- Support screen readers

**DON'T:**
- Don't rely solely on color to convey information
- Don't hide focusable elements without proper handling
- Don't forget alt text for images
- Don't prevent keyboard access

### 4. Performance

**DO:**
- Lazy load images below the fold
- Use CSS transforms for animations (GPU accelerated)
- Debounce expensive operations
- Minimize DOM manipulation

**DON'T:**
- Don't load all images immediately
- Don't animate layout properties (width, height)
- Don't create memory leaks with event listeners
- Don't render hidden content unnecessarily

### 5. Mobile Optimization

**DO:**
- Use touch-friendly target sizes (min 48x48px)
- Test on real devices
- Optimize for portrait and landscape
- Provide haptic feedback where appropriate

**DON'T:**
- Don't rely on hover states for mobile
- Don't use tiny touch targets
- Don't forget about one-handed usage
- Don't block scrolling unnecessarily

---

## Browser Support

### Minimum Requirements
- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile Safari 14+
- Chrome Mobile 90+

### Progressive Enhancement
- Core functionality works in all modern browsers
- Advanced animations require CSS animation support
- Intersection Observer used with fallback
- Service Workers for offline (future enhancement)

---

## Testing Checklist

### Visual Testing
- [ ] Test on multiple screen sizes (320px - 2560px)
- [ ] Verify animations work smoothly
- [ ] Check color contrast ratios
- [ ] Test in light and dark system themes
- [ ] Verify touch targets on mobile

### Functional Testing
- [ ] Toast notifications appear and dismiss
- [ ] Loading spinners show/hide correctly
- [ ] Empty states display properly
- [ ] Buttons respond to clicks
- [ ] Forms validate and submit

### Accessibility Testing
- [ ] Navigate entire site with keyboard only
- [ ] Test with screen reader (NVDA/JAWS/VoiceOver)
- [ ] Verify focus indicators visible
- [ ] Check ARIA labels are descriptive
- [ ] Test skip-to-content link

### Performance Testing
- [ ] Check Lighthouse score (aim for 90+)
- [ ] Verify lazy loading works
- [ ] Test on slow 3G connection
- [ ] Check bundle size
- [ ] Monitor memory usage

---

## Troubleshooting

### Animations Not Working
**Issue:** Animations don't play or appear choppy.

**Solutions:**
1. Check if `prefers-reduced-motion` is enabled
2. Verify CSS file is loaded correctly
3. Ensure element has animation class
4. Check browser DevTools for errors

### Loading Spinner Not Showing
**Issue:** `LoadingSpinner` component doesn't appear.

**Solutions:**
1. Verify `IsLoading` parameter is set to `true`
2. Check parent element has `position: relative`
3. Ensure component CSS is loaded
4. Verify no z-index conflicts

### Toast Notifications Overlapping
**Issue:** Multiple toasts stack incorrectly.

**Solutions:**
1. Check `.toast-container` has proper z-index
2. Verify positioning is fixed, not absolute
3. Ensure toasts have unique IDs
4. Check for CSS conflicts

### Accessibility Issues
**Issue:** Screen reader announces incorrectly.

**Solutions:**
1. Verify ARIA labels are present
2. Check semantic HTML structure
3. Test live regions are working
4. Ensure dynamic content updates announce

---

## Future Enhancements

### Planned Features
1. **Dark Mode Support** - Full dark theme with toggle
2. **Advanced Animations** - Page transitions with Blazor
3. **Offline Support** - Service Worker for offline mode
4. **Push Notifications** - Real-time order updates
5. **Voice Commands** - Voice-activated navigation
6. **Gesture Controls** - Swipe navigation for mobile
7. **PWA Support** - Install as mobile app
8. **Analytics Dashboard** - User behavior insights

### Component Roadmap
- [ ] Modal/Dialog component
- [ ] Dropdown menu component
- [ ] Carousel/Slider component
- [ ] Tab navigation component
- [ ] Accordion component
- [ ] Tooltip component
- [ ] Progress bar component
- [ ] Pagination component

---

## Credits & Resources

### Design Inspiration
- Material Design (Google)
- Fluent Design (Microsoft)
- Apple Human Interface Guidelines
- Sports stadium branding aesthetics

### Accessibility Resources
- WCAG 2.1 Guidelines: https://www.w3.org/WAI/WCAG21/quickref/
- A11Y Project: https://www.a11yproject.com/
- WebAIM: https://webaim.org/

### Animation Resources
- Animista: https://animista.net/
- Easings.net: https://easings.net/
- Cubic Bezier: https://cubic-bezier.com/

---

## Support & Contact

For questions, issues, or contributions:
- Project Repository: (Your repo URL)
- Documentation: This guide
- Issue Tracker: (Your issue tracker URL)

---

**Last Updated:** 2025-10-11
**Version:** 2.0
**Author:** Claude Code Assistant
