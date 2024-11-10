using Backbone.Documentations.TextTemplates.Abstractions.Models;
using FluentValidation.Results;

namespace Backbone.Documentations.TextTemplates.Abstractions.Services;

/// <summary>
/// Defines the template rendering foundation service functionality.
/// </summary>
public interface ITextTemplateRenderingService
{
    /// <summary>
    /// Renders a template with given variables for placeholders.
    /// </summary>
    /// <param name="template">The template with placeholders to render</param>
    /// <param name="placeholderValues">A dictionary of variables to replace placeholders</param>
    /// <returns>The rendered template content.</returns>
    string Render(string template, Dictionary<string, string>? placeholderValues = default);

    /// <summary>
    /// Validates placeholders in the template
    /// </summary>
    /// <param name="templatePlaceholders">A collection of template placeholders</param>
    /// <returns>The validation result</returns>
    ValidationResult ValidatePlaceholders(ICollection<ITemplatePlaceholder> templatePlaceholders);
}