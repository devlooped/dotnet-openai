namespace Devlooped.OpenAI;

/// <summary>
/// Allows defining an alternative display value for an enum value.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class DisplayValueAttribute(string value) : Attribute
{
    /// <summary>
    /// Gets the display value.
    /// </summary>
    public string Value => value;
}
