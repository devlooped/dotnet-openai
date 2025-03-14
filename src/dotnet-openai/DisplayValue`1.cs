using System.Reflection;

namespace Devlooped.OpenAI;

static class DisplayValue<T> where T : struct, Enum
{
    static readonly Dictionary<T, string> valueToDisplay = [];
    static readonly Dictionary<string, T> displayToValue = [];
    static readonly Dictionary<string, T> displayToValueIgnoreCase = new(StringComparer.OrdinalIgnoreCase);

    static DisplayValue()
    {
        var type = typeof(T);
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var value = (T)field.GetValue(null)!;
            foreach (var display in field.GetCustomAttributes<DisplayValueAttribute>(false))
            {
                // Ensures first one wins.
                valueToDisplay.TryAdd(value, display.Value);
                // We can have multiple display values for the same enum value.
                displayToValue[display.Value] = value;
                displayToValueIgnoreCase[display.Value] = value;
            }
        }
    }

    public static string GetString(T value) => valueToDisplay.TryGetValue(value, out var str) ? str : value.ToString();

    public static T Parse(string? value, bool ignoreCase = false)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var map = ignoreCase ? displayToValueIgnoreCase : displayToValue;
        if (map.TryGetValue(value, out var cached))
            return cached;

        // Fallback to Enum.Parse if no overridden value matches
        return Enum.Parse<T>(value, ignoreCase);
    }

    public static bool TryParse(string? value, out T result) => TryParse(value, false, out result);

    public static bool TryParse(string? value, bool ignoreCase, out T result)
    {
        if (value == null)
        {
            result = default;
            return false;
        }

        var map = ignoreCase ? displayToValueIgnoreCase : displayToValue;
        if (map.TryGetValue(value, out result))
            return true;

        return Enum.TryParse(value, ignoreCase, out result);
    }
}