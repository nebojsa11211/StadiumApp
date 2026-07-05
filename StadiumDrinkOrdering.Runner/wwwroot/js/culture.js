// Blazor WebAssembly persists the chosen UI culture in localStorage. Program.cs reads this at
// startup (blazorCulture.get) and applies it before the first render; the LanguageSwitcher writes
// it (blazorCulture.set) and reloads the app so the new culture takes effect.
window.blazorCulture = {
    get: () => window.localStorage['BlazorCulture'],
    set: (value) => window.localStorage['BlazorCulture'] = value
};
