using MudBlazor;

#pragma warning disable 8618

namespace Pamaxie.Website
{
    public partial class App
    {
        private readonly MudTheme _darkTheme = new()
        {
            Palette = new Palette()
            {
                Black = "#111111",
                Background = "#212121",
                BackgroundGrey = "#212121",
                Surface = "#313131",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 1)",
                DrawerIcon = "rgba(255,255,255, 1)",
                AppbarBackground = "#292b2c",
                AppbarText = "rgba(255,255,255, 1)",
                Primary = "#6637a1",
                Secondary = "#275c5a",
                Tertiary = "#983448",
                TextPrimary = "rgba(255,255,255, 1)",
                TextSecondary = "rgba(255,255,255, 1)",
                TextDisabled = "rgba(255, 255, 255, 0.5)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.5)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                PrimaryContrastText = "rgba(255,255,255, 1)",
                SecondaryContrastText = "rgba(255,255,255, 1)",
                InfoContrastText = "rgba(255,255,255, 1)",
                WarningContrastText = "rgba(255,255,255, 1)",
            
                Error = "#DA4E4B",
                Warning = "#DAA932"
            }
        };
    }
}