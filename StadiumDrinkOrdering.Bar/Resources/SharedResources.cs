namespace StadiumDrinkOrdering.Bar
{
    // Marker class MUST live in the root namespace (not .Resources). With ResourcesPath="Resources"
    // set in Program.cs, .NET builds the resource base name as
    // "<RootNamespace>.<ResourcesPath>.<TypeNameWithoutRootNamespace>". If this class were in the
    // ".Resources" namespace, that would double up to "...Resources.Resources.SharedResources" and
    // fail to match the embedded "StadiumDrinkOrdering.Bar.Resources.SharedResources.*.resx" files,
    // silently disabling all translations.
    public class SharedResources
    {
        // This class serves as a marker for resource files
        // The actual localization is handled by IStringLocalizer<SharedResources>
    }
}