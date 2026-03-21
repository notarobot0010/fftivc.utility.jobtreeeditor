using Microsoft.Win32;
using System.Windows;
using System.Windows.Media;

namespace fftivc.utility.jobtreeeditor.gui;

/// <summary>
/// Detects the Windows app theme (light/dark) and provides matching WPF resources.
/// Watches for live theme changes via the registry.
/// </summary>
public class ThemeManager
{
    private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string RegistryValueName = "AppsUseLightTheme";

    public static bool IsDarkMode { get; private set; }

    public static event Action? ThemeChanged;

    /// <summary>
    /// Call once at startup to detect the current theme and start watching for changes.
    /// </summary>
    public static void Initialize()
    {
        IsDarkMode = DetectDarkMode();
        ApplyTheme(Application.Current);
        WatchForChanges();
    }

    /// <summary>
    /// Apply the current theme's resource dictionary to the application.
    /// </summary>
    public static void ApplyTheme(Application app)
    {
        var dict = IsDarkMode ? CreateDarkTheme() : CreateLightTheme();

        var existing = app.Resources.MergedDictionaries
            .FirstOrDefault(d => d.Contains("IsThemeDictionary"));
        if (existing != null)
            app.Resources.MergedDictionaries.Remove(existing);

        app.Resources.MergedDictionaries.Add(dict);
    }

    private static bool DetectDarkMode()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath);
            var value = key?.GetValue(RegistryValueName);
            if (value is int intVal)
                return intVal == 0; // 0 = dark, 1 = light
        }
        catch
        {
            // Fall back to light if we can't read the registry
        }
        return false;
    }

    private static void WatchForChanges()
    {
        // Poll-free approach using SystemEvents
        SystemEvents.UserPreferenceChanged += (_, args) =>
        {
            if (args.Category == UserPreferenceCategory.General)
            {
                bool wasDark = IsDarkMode;
                IsDarkMode = DetectDarkMode();
                if (wasDark != IsDarkMode)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        ApplyTheme(Application.Current);
                        ThemeChanged?.Invoke();
                    });
                }
            }
        };
    }

    #region ThemeDefinitions
    private static ResourceDictionary CreateLightTheme()
    {
        var dict = new ResourceDictionary
        {
            { "IsThemeDictionary", true },

            // Backgrounds
            { "WindowBg", ToBrush("#FAFAFA") },
            { "PanelBg", ToBrush("#FFFFFF") },
            { "PanelBorder", ToBrush("#E0E0E0") },
            { "InputBg", ToBrush("#F5F5F5") },
            { "InputBorder", ToBrush("#D0D0D0") },

            // Grid
            { "GridBg", ToBrush("#FFFFFF") },
            { "GridAltRowBg", ToBrush("#FAFAFA") },
            { "GridLineBrush", ToBrush("#F0F0F0") },
            { "GridHeaderBg", ToBrush("#F5F5F5") },
            { "GridHeaderFg", ToBrush("#444444") },
            { "GridSelectionBg", ToBrush("#DBEAFE") },
            { "GridSelectionFg", ToBrush("#1E293B") },

            // Text
            { "TextPrimary", ToBrush("#1E1E1E") },
            { "TextSecondary", ToBrush("#666666") },
            { "TextMuted", ToBrush("#999999") },
            { "TextOnPrimary", ToBrush("#FFFFFF") },

            // Combo Box Item
            { "ComboBorder", ToBrush("#E0E0E0")},
            { "ComboBg", ToBrush("#FAFAFA")},
            { "ComboFg", ToBrush("#1E1E1E")},
            { "ComboHoverBg", ToBrush("#FFFFFF")},
            { "ComboHoverFg", ToBrush("#1E1E1E")},
            { "ComboSelectBg", ToBrush("#1D4ED8")},
            { "ComboSelectFg", ToBrush("#1E1E1E")},

            // Buttons
            { "BtnBg", ToBrush("#FFFFFF") },
            { "BtnBorder", ToBrush("#D0D0D0") },
            { "BtnFg", ToBrush("#333333") },
            { "BtnHoverBg", ToBrush("#F0F0F0") },
            { "PrimaryBtnBg", ToBrush("#2563EB") },
            { "PrimaryBtnBorder", ToBrush("#1D4ED8") },
            { "PrimaryBtnFg", ToBrush("#FFFFFF") },
            { "PrimaryBtnHoverBg", ToBrush("#1D4ED8") },

            // Accents
            { "ModifiedDot", ToBrush("#F59E0B") },
            { "StatusFg", ToBrush("#666666") }
        };

        return dict;
    }

    private static ResourceDictionary CreateDarkTheme()
    {
        var dict = new ResourceDictionary
        {
            { "IsThemeDictionary", true },

            // Backgrounds
            { "WindowBg", ToBrush("#1E1E1E") },
            { "PanelBg", ToBrush("#2D2D2D") },
            { "PanelBorder", ToBrush("#3E3E3E") },
            { "InputBg", ToBrush("#383838") },
            { "InputBorder", ToBrush("#505050") },

            // Grid
            { "GridBg", ToBrush("#2D2D2D") },
            { "GridAltRowBg", ToBrush("#333333") },
            { "GridLineBrush", ToBrush("#3E3E3E") },
            { "GridHeaderBg", ToBrush("#383838") },
            { "GridHeaderFg", ToBrush("#CCCCCC") },
            { "GridSelectionBg", ToBrush("#1E3A5F") },
            { "GridSelectionFg", ToBrush("#E0E0E0") },

            // Text
            { "TextPrimary", ToBrush("#E0E0E0") },
            { "TextSecondary", ToBrush("#AAAAAA") },
            { "TextMuted", ToBrush("#777777") },
            { "TextOnPrimary", ToBrush("#FFFFFF") },

            // Combo Box Item
            { "ComboBorder", ToBrush("#3E3E3E")},
            { "ComboBg", ToBrush("#48494B")},
            { "ComboFg", ToBrush("#E0E0E0")},
            { "ComboHoverBg", ToBrush("#444444")},
            { "ComboHoverFg", ToBrush("#E0E0E0")},
            { "ComboSelectBg", ToBrush("#3B82F6")},
            { "ComboSelectFg", ToBrush("#E0E0E0")},

            // Buttons
            { "BtnBg", ToBrush("#383838") },
            { "BtnBorder", ToBrush("#505050") },
            { "BtnFg", ToBrush("#E0E0E0") },
            { "BtnHoverBg", ToBrush("#444444") },
            { "PrimaryBtnBg", ToBrush("#2563EB") },
            { "PrimaryBtnBorder", ToBrush("#3B82F6") },
            { "PrimaryBtnFg", ToBrush("#FFFFFF") },
            { "PrimaryBtnHoverBg", ToBrush("#3B82F6") },

            // Accents
            { "ModifiedDot", ToBrush("#FBBF24") },
            { "StatusFg", ToBrush("#AAAAAA") }
        };

        return dict;
    }

    private static SolidColorBrush ToBrush(string hex)
    {
        var color = (Color)ColorConverter.ConvertFromString(hex);
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        return brush;
    }
    #endregion
}
