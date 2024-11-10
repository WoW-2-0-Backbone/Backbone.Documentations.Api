using Backbone.Documentations.TextTemplates.Abstractions.Models;

namespace Backbone.Documentations.TextTemplates.Basic.Models;

/// <summary>
/// Represents basic template placeholder.
/// </summary>
public record BasicTemplatePlaceholder : ITemplatePlaceholder
{
    /// <inheritdoc />
    public string Placeholder { get; init; } = default!;

    /// <inheritdoc />
    public string PlaceholderValue { get; init; } = default!;

    /// <inheritdoc />
    public string? Value { get; init; }

    /// <inheritdoc />
    public bool IsValid { get; init; }
}