using System.Text;
using System.Text.RegularExpressions;
using Backbone.Documentations.TextTemplates.Abstractions.Models;
using Backbone.Documentations.TextTemplates.Abstractions.Services;
using Backbone.Documentations.TextTemplates.Basic.Models;
using Backbone.Documentations.TextTemplates.Basic.Settings;
using Backbone.General.Validations.FluentValidation.Exceptions;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace Backbone.Documentations.TextTemplates.Basic.Services;

/// <summary>
/// Provides the foundation service functionality for rendering templates.
/// </summary>
public class BasicTextTemplateRenderingService(IOptions<TemplateRenderingSettings> templateRenderingSettingsOptions) : ITextTemplateRenderingService
{
    private readonly TemplateRenderingSettings _templateRenderingSettings = templateRenderingSettingsOptions.Value;

    /// <inheritdoc />
    public string Render(string template, Dictionary<string, string>? variables = default)
    {
        if (string.IsNullOrWhiteSpace(template))
            return template;

        // Prepare regex for placeholder and variables extraction
        var placeholderRegex = new Regex(_templateRenderingSettings.PlaceholderRegexPattern,
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(_templateRenderingSettings.RegexMatchTimeoutInSeconds));

        var placeholderValueRegex = new Regex(_templateRenderingSettings.PlaceholderValueRegexPattern,
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(_templateRenderingSettings.RegexMatchTimeoutInSeconds));

        var matches = placeholderRegex.Matches(template);

        if (matches.Count != 0 && (variables is null || variables.Count == 0))
            throw new AppFluentValidationException([new ValidationFailure(nameof(variables), "Variables are required for this template.")]);

        // Extract placeholders and their values
        var templatePlaceholders = matches.Select(ITemplatePlaceholder (match) =>
            {
                var placeholder = match.Value;
                var placeholderValue = placeholderValueRegex.Match(placeholder).Groups[1].Value;
                var valid = variables!.TryGetValue(placeholderValue, out var value);

                return new BasicTemplatePlaceholder
                {
                    Placeholder = placeholder,
                    PlaceholderValue = placeholderValue,
                    Value = value,
                    IsValid = valid
                };
            })
            .ToList();

        // Validate placeholders
        var placeholdersValidationResult = ValidatePlaceholders(templatePlaceholders);
        if (!placeholdersValidationResult.IsValid)
            throw new AppFluentValidationException(placeholdersValidationResult.Errors);

        // Replace placeholders with their values
        var messageBuilder = new StringBuilder(template);
        templatePlaceholders.ForEach(placeholder => messageBuilder.Replace(placeholder.Placeholder, placeholder.Value));

        return messageBuilder.ToString();
    }

    /// <inheritdoc />
    public ValidationResult ValidatePlaceholders(ICollection<ITemplatePlaceholder> templatePlaceholders)
    {
        var validationErrors = templatePlaceholders
            .Where(placeholder => !placeholder.IsValid)
            .Select(placeholder =>
            {
                var errorMessage = $"Variable for placeholder '{placeholder.PlaceholderValue}' is not found";
                return new ValidationFailure(placeholder.PlaceholderValue, errorMessage);
            }).ToList();

        return new ValidationResult(validationErrors);
    }
}