namespace Backbone.Documentations.TextTemplates.Abstractions.Models;

/// <summary>
/// Defines template placeholder properties.
/// </summary>
public interface ITemplatePlaceholder
{
    /// <summary>
    /// Gets or sets placeholder of the value 
    /// </summary>
    string Placeholder { get; }

    /// <summary>
    /// Gets or sets value of placeholder
    /// </summary>
    string PlaceholderValue { get; }

    /// <summary>
    /// Gets or sets value of the template
    /// </summary>
    string? Value { get; }

    /// <summary>
    /// Gets or sets if the placeholder is valid or not
    /// </summary>
    bool IsValid { get; }
}
