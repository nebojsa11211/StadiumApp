# PWA Icon Specifications for Stadium Drink Ordering

## Overview
This document provides complete specifications for the Progressive Web App icons needed for the Stadium Drink Ordering Customer application. All icons should maintain consistent branding with the stadium theme and blue color scheme (#2563eb).

---

## Required Icon Sizes

### Standard Icons (PNG Format)
Create the following icon sizes in **PNG format** with **transparent backgrounds**:

| Size | Filename | Purpose |
|------|----------|---------|
| 72x72 | `icon-72x72.png` | Android launcher (ldpi) |
| 96x96 | `icon-96x96.png` | Android launcher (mdpi) |
| 128x128 | `icon-128x128.png` | Android launcher (hdpi) |
| 144x144 | `icon-144x144.png` | Android launcher (xhdpi), Windows tile |
| 152x152 | `icon-152x152.png` | iOS home screen |
| 192x192 | `icon-192x192.png` | Android launcher (xxhdpi), Chrome install prompt |
| 384x384 | `icon-384x384.png` | Android launcher (xxxhdpi) |
| 512x512 | `icon-512x512.png` | Splash screen, high-res displays |

### Maskable Icons (PNG Format)
Special icons with **safe zone** for adaptive launchers:

| Size | Filename | Purpose |
|------|----------|---------|
| 192x192 | `icon-maskable-192x192.png` | Adaptive icon (minimum) |
| 512x512 | `icon-maskable-512x512.png` | Adaptive icon (recommended) |

**Maskable Icon Requirements:**
- Use the full canvas (no transparency around edges)
- Keep important content within the **80% safe zone** (center circle)
- Content outside safe zone may be cropped by different launcher shapes
- Background should be solid color (#2563eb blue) or subtle gradient

### Additional Icons

#### Badge Icon
- **Size**: 72x72 pixels
- **Filename**: `badge-72x72.png`
- **Purpose**: Notification badge overlay
- **Design**: Simplified monochrome version of main icon
- **Format**: PNG with transparency

#### Shortcut Icons (96x96)
Create three shortcut icons representing key features:

1. **Events Shortcut** (`shortcut-events.png`)
   - Icon: Calendar or ticket symbol
   - Color: Blue (#2563eb)
   - Size: 96x96 pixels

2. **Orders Shortcut** (`shortcut-orders.png`)
   - Icon: Receipt or shopping bag
   - Color: Blue (#2563eb)
   - Size: 96x96 pixels

3. **Menu Shortcut** (`shortcut-menu.png`)
   - Icon: Drink cup or menu symbol
   - Color: Blue (#2563eb)
   - Size: 96x96 pixels

#### Apple-Specific Icons
iOS devices require special touch icons:

| Size | Filename | Purpose |
|------|----------|---------|
| 180x180 | `apple-touch-icon.png` | iOS home screen (standard) |
| 120x120 | `apple-touch-icon-120x120.png` | iPhone retina |
| 152x152 | `apple-touch-icon-152x152.png` | iPad retina |
| 167x167 | `apple-touch-icon-167x167.png` | iPad Pro |

**Apple Icon Requirements:**
- **No transparency** - use solid background color
- Rounded corners will be applied automatically by iOS
- Save as PNG format
- No gloss effects (iOS adds automatically)

---

## Design Guidelines

### Main Icon Concept
**Recommended Design Elements:**
- Stadium silhouette or seating section outline
- Drink cup or beverage icon
- Ticket or seat number symbol
- Clean, modern minimalist style
- High contrast for visibility

### Color Scheme
**Primary Colors:**
- Brand Blue: `#2563eb` (Main brand color)
- Dark Blue: `#1d4ed8` (Accent/shadow)
- White: `#ffffff` (Contrast/text)
- Orange: `#f59e0b` (Optional accent for food/drinks)

**Background Options:**
1. Solid blue (`#2563eb`)
2. Blue gradient (linear: #2563eb to #1d4ed8)
3. White with blue icon (for light mode)

### Icon Style Guidelines

#### Standard Icons
- **Background**: Solid color or gradient
- **Padding**: 10-15% padding from edges
- **Shape**: Square with slightly rounded corners (8-12px radius)
- **Drop Shadow**: Subtle shadow for depth (optional)
- **Line Weight**: Medium to bold for visibility at small sizes

#### Maskable Icons
- **Background**: Full bleed, no transparency
- **Safe Zone**: Keep logo/text within center 80% circle
- **Padding**: Minimum 10% from all edges
- **Testing**: Use [Maskable.app](https://maskable.app) to verify safe zone

### Logo/Symbol Requirements
- **Simplicity**: Clear and recognizable at 48x48 pixels
- **Scalability**: Must work at both 72px and 512px
- **Contrast**: High contrast against background
- **Uniqueness**: Distinctive from generic food/sports apps
- **Brand Consistency**: Match existing Stadium Drink Ordering brand

---

## File Organization

### Directory Structure
```
StadiumDrinkOrdering.Customer/wwwroot/icons/
├── icon-72x72.png
├── icon-96x96.png
├── icon-128x128.png
├── icon-144x144.png
├── icon-152x152.png
├── icon-192x192.png
├── icon-384x384.png
├── icon-512x512.png
├── icon-maskable-192x192.png
├── icon-maskable-512x512.png
├── badge-72x72.png
├── shortcut-events.png
├── shortcut-orders.png
├── shortcut-menu.png
├── apple-touch-icon.png
├── apple-touch-icon-120x120.png
├── apple-touch-icon-152x152.png
└── apple-touch-icon-167x167.png
```

---

## Design Tools & Resources

### Recommended Tools
1. **Adobe Illustrator** - Vector design (export to PNG)
2. **Figma** - Collaborative design with icon plugins
3. **Sketch** - macOS icon design with export presets
4. **Inkscape** - Free vector graphics editor
5. **GIMP** - Free raster graphics editor

### Online Tools
- **[Realfavicongenerator.net](https://realfavicongenerator.net/)** - Generate all sizes from one source
- **[PWA Asset Generator](https://github.com/elegantapp/pwa-asset-generator)** - CLI tool for icon generation
- **[Maskable.app](https://maskable.app)** - Test maskable icons

### Icon Libraries (for inspiration)
- **Material Design Icons** - https://materialdesignicons.com/
- **Heroicons** - https://heroicons.com/
- **Font Awesome** - https://fontawesome.com/

---

## Export Settings

### PNG Export Settings
- **Color Mode**: RGB
- **Bit Depth**: 24-bit (with alpha channel)
- **Compression**: PNG-8 or PNG-24 (optimize for web)
- **Transparency**: Yes (except Apple touch icons)
- **DPI**: 72 DPI (web standard)

### Optimization
After exporting, optimize file sizes:
- **TinyPNG** - https://tinypng.com/
- **ImageOptim** (macOS) - https://imageoptim.com/
- **Squoosh** - https://squoosh.app/

**Target File Sizes:**
- 72x72: < 3 KB
- 192x192: < 10 KB
- 512x512: < 25 KB

---

## Testing & Validation

### Test Checklist
- [ ] All required sizes generated
- [ ] Icons display correctly on Android (Chrome)
- [ ] Icons display correctly on iOS (Safari)
- [ ] Icons display correctly on Windows (Edge)
- [ ] Maskable icons tested with [Maskable.app](https://maskable.app)
- [ ] File sizes optimized (< 25KB for largest icon)
- [ ] Transparent backgrounds work correctly
- [ ] Apple touch icons have solid backgrounds
- [ ] All files placed in `/wwwroot/icons/` directory
- [ ] manifest.json correctly references all icons

### Browser Testing
Test icon display in:
- Chrome (Android, Desktop)
- Safari (iOS, macOS)
- Edge (Windows, Android)
- Firefox (Desktop, Android)
- Samsung Internet (Android)

---

## Placeholder Generation (Temporary)

If final icons are not ready, create placeholder icons:

### Using ImageMagick (CLI)
```bash
# Create placeholder with text
convert -size 512x512 xc:#2563eb \
  -font Arial -pointsize 120 -fill white \
  -gravity center -annotate +0+0 "SD" \
  icon-512x512.png

# Resize to other sizes
convert icon-512x512.png -resize 192x192 icon-192x192.png
convert icon-512x512.png -resize 144x144 icon-144x144.png
# ... repeat for all sizes
```

### Using Online Tools
- **[Favicon.io](https://favicon.io/)** - Generate from text or image
- **[PWA Builder](https://www.pwabuilder.com/)** - Generate icon set

---

## Delivery Format

### Designer Deliverables
Please provide:
1. **All PNG files** in specified sizes (18 total files)
2. **Source files** (AI, SVG, PSD, Sketch, or Figma)
3. **Design variations** (2-3 concepts for approval)
4. **Color palette** used in designs
5. **Usage notes** for any special considerations

### File Naming Convention
- Use exact filenames specified above
- All lowercase
- Hyphens for spaces
- No special characters

---

## Budget & Timeline

### Recommended Budget
- **Icon Design**: $200-500 (freelancer)
- **Professional Agency**: $500-1500
- **DIY/Template**: $0-100

### Timeline
- **Design Concept**: 2-3 days
- **Revisions**: 1-2 days
- **Final Export**: 1 day
- **Total**: 4-6 days

---

## Contact & Approval

All icon designs should be submitted for approval before final implementation.

**Approval Criteria:**
- Matches Stadium Drink Ordering brand identity
- Works at all required sizes (readable at 72px)
- Follows PWA best practices
- Optimized file sizes
- Cross-browser compatible

---

## Additional Notes

### Favicon Generation
While not strictly PWA-related, also generate:
- **favicon.ico** (16x16, 32x32, 48x48 multi-resolution)
- **favicon-16x16.png**
- **favicon-32x32.png**

### Future Considerations
- Seasonal icon variations (e.g., holiday themes)
- Dynamic icon badges (unread count)
- Animated icons for special events
- Dark mode icon variants

---

**Last Updated**: 2025-10-11
**Version**: 1.0
**Author**: Claude Code - Stadium Drink Ordering Team
