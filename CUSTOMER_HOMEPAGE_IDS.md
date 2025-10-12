# Customer Homepage - Element IDs Reference

Complete list of IDs added to the Customer Index (homepage) page for testing and automation.

## Naming Convention
All IDs follow the pattern: `customer-index-{section}-{element}`

---

## Hero Section

### Container Elements
- `customer-index-hero-section` - Main hero section
- `customer-index-hero-overlay` - Hero background overlay
- `customer-index-hero-container` - Hero container
- `customer-index-hero-content` - Hero content wrapper

### Text Elements
- `customer-index-hero-title` - Main hero title
- `customer-index-hero-accent` - Accent text "Delivered to Your Seat"
- `customer-index-hero-subtitle` - Hero subtitle/description

### Buttons & CTAs
- `customer-index-hero-cta-group` - CTA buttons container
- `customer-index-order-now-btn` - "Order Now" primary button
- `customer-index-order-now-icon` - SVG icon in Order Now button
- `customer-index-view-menu-btn` - "View Menu" secondary button

---

## Categories Section

### Container Elements
- `customer-index-categories-section` - Main categories section
- `customer-index-categories-container` - Categories container
- `customer-index-categories-header` - Section header wrapper
- `customer-index-categories-row` - Categories grid row

### Header Elements
- `customer-index-categories-title` - "What's Popular" title
- `customer-index-categories-subtitle` - Section subtitle

### Beer Category
- `customer-index-category-beer-col` - Beer column wrapper
- `customer-index-category-beer-card` - Beer card container
- `customer-index-category-beer-icon` - Beer icon (🍺)
- `customer-index-category-beer-title` - "Beer" title
- `customer-index-category-beer-desc` - "Craft & Premium" description
- `customer-index-category-beer-link` - "Browse Selection" link

### Cocktails Category
- `customer-index-category-cocktails-col` - Cocktails column wrapper
- `customer-index-category-cocktails-card` - Cocktails card container
- `customer-index-category-cocktails-icon` - Cocktails icon (🍹)
- `customer-index-category-cocktails-title` - "Cocktails" title
- `customer-index-category-cocktails-desc` - "Signature Mixes" description
- `customer-index-category-cocktails-link` - "Browse Selection" link

### Soft Drinks Category
- `customer-index-category-softdrinks-col` - Soft drinks column wrapper
- `customer-index-category-softdrinks-card` - Soft drinks card container
- `customer-index-category-softdrinks-icon` - Soft drinks icon (🥤)
- `customer-index-category-softdrinks-title` - "Soft Drinks" title
- `customer-index-category-softdrinks-desc` - "Refreshing Choices" description
- `customer-index-category-softdrinks-link` - "Browse Selection" link

### Snacks Category
- `customer-index-category-snacks-col` - Snacks column wrapper
- `customer-index-category-snacks-card` - Snacks card container
- `customer-index-category-snacks-icon` - Snacks icon (🍟)
- `customer-index-category-snacks-title` - "Snacks" title
- `customer-index-category-snacks-desc` - "Game Day Favorites" description
- `customer-index-category-snacks-link` - "Browse Selection" link

---

## How It Works Section

### Container Elements
- `customer-index-how-it-works-section` - Main section
- `customer-index-how-it-works-container` - Container wrapper
- `customer-index-how-it-works-header` - Header wrapper
- `customer-index-steps-row` - Steps grid row

### Header Elements
- `customer-index-how-it-works-title` - "How It Works" title
- `customer-index-how-it-works-subtitle` - Section subtitle

### Step 1: Browse Menu
- `customer-index-step-1-col` - Step 1 column wrapper
- `customer-index-step-1-card` - Step 1 card container
- `customer-index-step-1-number` - Number "1"
- `customer-index-step-1-title` - "Browse Menu" title
- `customer-index-step-1-desc` - Step 1 description

### Step 2: Place Order
- `customer-index-step-2-col` - Step 2 column wrapper
- `customer-index-step-2-card` - Step 2 card container
- `customer-index-step-2-number` - Number "2"
- `customer-index-step-2-title` - "Place Order" title
- `customer-index-step-2-desc` - Step 2 description

### Step 3: Enjoy
- `customer-index-step-3-col` - Step 3 column wrapper
- `customer-index-step-3-card` - Step 3 card container
- `customer-index-step-3-number` - Number "3"
- `customer-index-step-3-title` - "Enjoy!" title
- `customer-index-step-3-desc` - Step 3 description

---

## Stats Section

### Container Elements
- `customer-index-stats-section` - Main stats section
- `customer-index-stats-container` - Stats container
- `customer-index-stats-row` - Stats grid row

### Happy Customers Stat
- `customer-index-stat-customers-col` - Customers column wrapper
- `customer-index-stat-customers-item` - Customers stat container
- `customer-index-stat-customers-number` - "10K+" number
- `customer-index-stat-customers-label` - "Happy Customers" label

### Menu Items Stat
- `customer-index-stat-menu-col` - Menu column wrapper
- `customer-index-stat-menu-item` - Menu stat container
- `customer-index-stat-menu-number` - "50+" number
- `customer-index-stat-menu-label` - "Menu Items" label

### Delivery Time Stat
- `customer-index-stat-delivery-col` - Delivery column wrapper
- `customer-index-stat-delivery-item` - Delivery stat container
- `customer-index-stat-delivery-number` - "15min" number
- `customer-index-stat-delivery-label` - "Avg Delivery" label

### Rating Stat
- `customer-index-stat-rating-col` - Rating column wrapper
- `customer-index-stat-rating-item` - Rating stat container
- `customer-index-stat-rating-number` - "4.8/5" number
- `customer-index-stat-rating-label` - "Rating" label

---

## CTA Section

### Container Elements
- `customer-index-cta-section` - Main CTA section
- `customer-index-cta-container` - CTA container

### Text Elements
- `customer-index-cta-title` - "Ready to Order?" title
- `customer-index-cta-subtitle` - CTA subtitle

### Buttons
- `customer-index-cta-buttons` - Buttons container
- `customer-index-get-started-btn` - "Get Started" button
- `customer-index-sign-up-btn` - "Sign Up Free" button

---

## Playwright Testing Examples

### Navigate to specific sections
```javascript
// Click Order Now button
await page.click('#customer-index-order-now-btn');

// Click Beer category
await page.click('#customer-index-category-beer-link');

// Click Get Started CTA
await page.click('#customer-index-get-started-btn');
```

### Check element visibility
```javascript
// Verify hero section is visible
await expect(page.locator('#customer-index-hero-section')).toBeVisible();

// Verify all category cards are present
await expect(page.locator('#customer-index-category-beer-card')).toBeVisible();
await expect(page.locator('#customer-index-category-cocktails-card')).toBeVisible();
await expect(page.locator('#customer-index-category-softdrinks-card')).toBeVisible();
await expect(page.locator('#customer-index-category-snacks-card')).toBeVisible();
```

### Test interactions
```javascript
// Hover over category card
await page.hover('#customer-index-category-beer-card');

// Check stat numbers
const customersCount = await page.textContent('#customer-index-stat-customers-number');
expect(customersCount).toBe('10K+');
```

### CSS Selector Patterns
```css
/* All homepage buttons */
[id^="customer-index-"][id$="-btn"] { }

/* All category cards */
[id^="customer-index-category-"][id$="-card"] { }

/* All step numbers */
[id^="customer-index-step-"][id$="-number"] { }

/* All stat numbers */
[id^="customer-index-stat-"][id$="-number"] { }
```

---

## Total Elements with IDs

- **Hero Section**: 9 elements
- **Categories Section**: 25 elements
- **How It Works Section**: 16 elements
- **Stats Section**: 17 elements
- **CTA Section**: 6 elements

**Total: 73 uniquely identifiable elements**

---

## Maintenance Notes

- All IDs follow the `customer-index-{section}-{element}` pattern
- IDs are stable and should not change unless the feature changes
- When adding new sections, follow the same naming convention
- Update this document when modifying the homepage structure
