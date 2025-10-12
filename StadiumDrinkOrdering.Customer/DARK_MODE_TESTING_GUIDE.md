# Dark Mode Testing Guide

## Quick Start Testing

### 1. Start the Application
```bash
cd StadiumDrinkOrdering.Customer
dotnet run --launch-profile https
```

Access at: **https://localhost:7020**

### 2. Visual Verification

#### A. Theme Toggle Button
- **Location**: Top navigation bar, right side (next to language switcher)
- **Expected**: Circular button with moon icon (light mode) or sun icon (dark mode)
- **Test**: Click the button - icon should change and theme should toggle

#### B. Immediate Visual Changes
**Dark Mode Elements**:
- Background: Dark slate (#0f172a)
- Navigation bar: Dark gray (#1e293b)
- Text: Light gray (#f1f5f9)
- Cards: Dark gray with borders
- Buttons: Emerald green primary (#10b981)

**Light Mode Elements**:
- Background: White (#ffffff)
- Navigation bar: White
- Text: Dark gray/black
- Cards: White with shadows
- Buttons: Blue/standard colors

### 3. Browser DevTools Testing

#### Check Theme Attribute
```javascript
// Open browser console (F12)
document.documentElement.getAttribute('data-theme')
// Should return: 'light' or 'dark'
```

#### Check localStorage
```javascript
localStorage.getItem('customer-theme-preference')
// Should return: 'light', 'dark', or null (system default)
```

#### Test ThemeManager
```javascript
// Check current theme
window.ThemeManager.getCurrentTheme()

// Check if dark mode
window.ThemeManager.isDarkMode()

// Toggle theme
window.ThemeManager.toggle()

// Set specific theme
window.ThemeManager.setTheme('dark')
window.ThemeManager.setTheme('light')
```

### 4. System Preference Testing

#### Windows
1. Open Settings → Personalization → Colors
2. Choose "Light" or "Dark" under "Choose your mode"
3. Open app in new browser tab (no localStorage yet)
4. Theme should match system preference

#### macOS
1. Open System Preferences → General
2. Choose "Light" or "Dark" appearance
3. Open app in new browser tab
4. Theme should match system preference

#### Test Auto-Detection
```javascript
// Check if system prefers dark mode
window.matchMedia('(prefers-color-scheme: dark)').matches
// Returns: true or false
```

### 5. Page-by-Page Visual Test

#### Test All Pages
- [ ] **Home Page** (`/`)
  - Hero section background
  - Feature cards
  - CTA buttons
  - Footer

- [ ] **Events Page** (`/events`)
  - Event cards
  - Filters sidebar
  - Date pickers
  - Buy tickets buttons

- [ ] **Menu Page** (`/menu`)
  - Drink cards
  - Category filters
  - Add to cart buttons
  - Shopping cart modal

- [ ] **Login Page** (`/login`)
  - Login form card
  - Input fields
  - Submit button
  - Background gradient

- [ ] **Register Page** (`/register`)
  - Registration form
  - Input validation
  - Terms checkbox
  - Submit button

- [ ] **Orders Page** (`/orders`)
  - Order cards
  - Status badges
  - Order details
  - Empty state

- [ ] **Event Details** (`/event-details/{id}`)
  - Event information
  - Seat selection
  - Price display
  - Add to cart

- [ ] **Checkout** (`/checkout`)
  - Customer form
  - Payment details
  - Order summary
  - Submit button

### 6. Component Testing

#### Navigation Bar
- [ ] Logo and brand name readable
- [ ] Nav links visible and hover states work
- [ ] User dropdown menu styled correctly
- [ ] Theme switcher button visible
- [ ] Mobile menu (responsive)

#### Cards
- [ ] Card backgrounds dark/light
- [ ] Card borders visible
- [ ] Card headers styled
- [ ] Card hover effects work
- [ ] Card shadows appropriate

#### Forms
- [ ] Input fields readable
- [ ] Input focus states visible (emerald outline)
- [ ] Placeholder text readable
- [ ] Labels visible
- [ ] Validation messages visible

#### Buttons
- [ ] Primary buttons (emerald green)
- [ ] Secondary buttons styled
- [ ] Outline buttons visible
- [ ] Hover states work
- [ ] Disabled states visible

#### Alerts & Messages
- [ ] Success alerts (green background)
- [ ] Error alerts (red background)
- [ ] Warning alerts (orange background)
- [ ] Info alerts (blue background)

#### Tables
- [ ] Table headers readable
- [ ] Row borders visible
- [ ] Striped rows alternating
- [ ] Hover states work

#### Modals
- [ ] Modal background overlay
- [ ] Modal content visible
- [ ] Modal close button works
- [ ] Modal form fields readable

### 7. Accessibility Testing

#### Keyboard Navigation
```
1. Press Tab to navigate
2. Theme toggle button should be focusable
3. Focus indicator should be visible (emerald outline)
4. Press Enter/Space to toggle theme
```

#### Screen Reader Testing
**With NVDA/JAWS/VoiceOver**:
- [ ] Theme toggle announces "Switch to dark mode" or "Switch to light mode"
- [ ] Theme change is announced
- [ ] All text remains readable

#### Contrast Testing
**Use browser extension or online tool**:
- [ ] Text contrast ratio ≥ 4.5:1 (normal text)
- [ ] Large text contrast ratio ≥ 3:1
- [ ] Interactive elements contrast ≥ 3:1

Tools:
- [WAVE Browser Extension](https://wave.webaim.org/extension/)
- [axe DevTools](https://www.deque.com/axe/devtools/)

### 8. Performance Testing

#### Page Load
```javascript
// Measure theme initialization time
console.time('themeInit');
window.ThemeManager.init();
console.timeEnd('themeInit');
// Should be < 10ms
```

#### Theme Toggle Speed
```javascript
// Measure toggle performance
console.time('themeToggle');
window.ThemeManager.toggle();
console.timeEnd('themeToggle');
// Should be < 5ms
```

#### Check for FOUC
1. Open app with dark theme saved in localStorage
2. Hard refresh (Ctrl+Shift+R / Cmd+Shift+R)
3. Watch for any flash of light theme
4. Should apply dark theme immediately

### 9. Browser Compatibility

#### Test Browsers
- [ ] **Chrome** (Windows/Mac)
  - Theme toggle works
  - System preference detected
  - Smooth transitions

- [ ] **Firefox** (Windows/Mac)
  - Theme toggle works
  - System preference detected
  - Smooth transitions

- [ ] **Safari** (Mac/iOS)
  - Theme toggle works
  - System preference detected
  - Smooth transitions

- [ ] **Edge** (Windows)
  - Theme toggle works
  - System preference detected
  - Smooth transitions

- [ ] **Mobile Chrome** (Android)
  - Theme toggle works
  - Meta theme-color updates
  - Touch interactions smooth

- [ ] **Mobile Safari** (iOS)
  - Theme toggle works
  - Meta theme-color updates
  - Touch interactions smooth

### 10. Edge Cases

#### Test Scenarios

**A. Multiple Tabs**
1. Open app in two tabs
2. Toggle theme in tab 1
3. Check if theme persists to tab 2 (on reload)

**B. Private/Incognito Mode**
1. Open app in incognito window
2. Theme should default to system preference
3. Theme toggle should work
4. Theme won't persist after closing (no localStorage)

**C. localStorage Disabled**
1. Disable localStorage in browser settings
2. App should still work with system preference
3. Theme won't persist between sessions

**D. System Preference Change**
1. Open app with no saved preference
2. Change system dark/light mode
3. App should update automatically

**E. Rapid Toggling**
1. Click theme toggle rapidly 10 times
2. No visual glitches or errors
3. Final state should be stable

### 11. Console Error Check

#### Expected: No Errors
```javascript
// Open browser console (F12)
// Check for errors in:
// - Console tab (no red errors)
// - Network tab (all resources load)
// - Application → Local Storage (theme key exists)
```

#### Common Issues to Check
- [ ] No "ThemeManager is not defined" errors
- [ ] No CSS loading errors
- [ ] No JavaScript runtime errors
- [ ] No 404 errors for theme files

### 12. Automated Testing Commands

#### Run from project root
```bash
# Start the app first
cd StadiumDrinkOrdering.Customer
dotnet run --launch-profile https

# In another terminal, run Playwright tests
npx playwright test --headed
```

#### Create Test File
Create `tests/dark-mode-verification.spec.ts`:
```typescript
import { test, expect } from '@playwright/test';

test('Dark mode toggle works', async ({ page }) => {
  await page.goto('https://localhost:7020');

  // Check theme toggle exists
  const themeToggle = page.locator('#customer-theme-switcher button');
  await expect(themeToggle).toBeVisible();

  // Get initial theme
  const initialTheme = await page.evaluate(() =>
    document.documentElement.getAttribute('data-theme')
  );

  // Click toggle
  await themeToggle.click();
  await page.waitForTimeout(500); // Wait for transition

  // Check theme changed
  const newTheme = await page.evaluate(() =>
    document.documentElement.getAttribute('data-theme')
  );
  expect(newTheme).not.toBe(initialTheme);

  // Check localStorage
  const savedTheme = await page.evaluate(() =>
    localStorage.getItem('customer-theme-preference')
  );
  expect(savedTheme).toBe(newTheme);
});

test('Dark mode persists after reload', async ({ page }) => {
  await page.goto('https://localhost:7020');

  // Set dark mode
  await page.evaluate(() => window.ThemeManager.setTheme('dark'));

  // Reload page
  await page.reload();

  // Check theme is still dark
  const theme = await page.evaluate(() =>
    document.documentElement.getAttribute('data-theme')
  );
  expect(theme).toBe('dark');
});
```

### 13. Visual Regression Testing

#### Take Screenshots
```bash
# Light mode
npx playwright codegen https://localhost:7020

# Dark mode
npx playwright codegen https://localhost:7020
# Then toggle dark mode and capture screenshots
```

#### Compare Before/After
1. Take screenshots of all pages in light mode
2. Toggle to dark mode
3. Take screenshots of all pages in dark mode
4. Verify all elements properly styled

### 14. Mobile Responsive Testing

#### Viewport Sizes
Test at these breakpoints:
- [ ] Mobile: 375x667 (iPhone SE)
- [ ] Tablet: 768x1024 (iPad)
- [ ] Desktop: 1920x1080 (Full HD)

#### Mobile-Specific Tests
- [ ] Theme toggle button visible on mobile nav
- [ ] Theme toggle accessible with thumb
- [ ] No layout shifts when toggling theme
- [ ] Meta theme-color updates address bar color

### 15. Final Checklist

Before marking as complete:
- [ ] All pages render correctly in both themes
- [ ] Theme toggle visible and functional
- [ ] System preference detection works
- [ ] Theme persists after page reload
- [ ] No console errors
- [ ] Smooth transitions (300ms)
- [ ] Accessibility features work
- [ ] Mobile responsive
- [ ] All browsers tested
- [ ] Documentation complete

## Quick Test Script

Run this in browser console for rapid testing:
```javascript
// Quick theme test
console.log('=== Dark Mode Quick Test ===');
console.log('1. Current theme:', window.ThemeManager.getCurrentTheme());
console.log('2. Is dark mode?', window.ThemeManager.isDarkMode());
console.log('3. System prefers dark?', window.matchMedia('(prefers-color-scheme: dark)').matches);
console.log('4. Saved preference:', localStorage.getItem('customer-theme-preference'));

// Toggle test
console.log('\n=== Testing toggle ===');
const before = window.ThemeManager.getCurrentTheme();
window.ThemeManager.toggle();
const after = window.ThemeManager.getCurrentTheme();
console.log('Before:', before, '→ After:', after);

// Reset
window.ThemeManager.setTheme(before);
console.log('Reset to:', before);
console.log('\n✅ All tests passed!');
```

## Troubleshooting

### Issue: Theme not applying
```javascript
// Debug script
console.log('ThemeManager exists?', typeof window.ThemeManager !== 'undefined');
console.log('data-theme attr:', document.documentElement.getAttribute('data-theme'));
console.log('CSS loaded?', document.querySelector('link[href*="dark-mode.css"]') !== null);
```

### Issue: Toggle not working
```javascript
// Check button
const button = document.querySelector('#customer-theme-switcher button');
console.log('Button exists?', button !== null);
console.log('Button clickable?', button.disabled);
```

### Issue: Not persisting
```javascript
// Check localStorage
console.log('localStorage available?', typeof Storage !== 'undefined');
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

---

**Happy Testing!** 🎨🌙☀️
