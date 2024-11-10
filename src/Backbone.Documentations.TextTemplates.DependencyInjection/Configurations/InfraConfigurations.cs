using Backbone.Documentations.TextTemplates.Abstractions.Services;
using Backbone.Documentations.TextTemplates.Basic.Services;
using Backbone.Documentations.TextTemplates.Basic.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Backbone.Documentations.TextTemplates.DependencyInjection.Configurations;

/// <summary>
/// Provides extension methods to configure the text template services.
/// </summary>
public static class InfraConfigurations
{
    /// <summary>
    /// Registers the text template infrastructure services.
    /// </summary>
    private static IServiceCollection AddBasicTextTemplatesInfrastructureWithDefaultConfiguration(this IServiceCollection services)
    {
        // Register settings
        var templateRenderingSettings = new TemplateRenderingSettings
        {
            PlaceholderRegexPattern = @"\\{\\{([^\\{\\}]+)\\}\\}",
            PlaceholderValueRegexPattern = @"{{(.*?)}}",
            RegexMatchTimeoutInSeconds = 5  
        };
        
        services
            .AddSingleton(_ => Options.Create(templateRenderingSettings));
        
        // Register foundation services
        services
            .AddSingleton<ITextTemplateRenderingService, BasicTextTemplateRenderingService>();

        return services;
    }
    
    /// <summary>
    /// Registers the text template infrastructure services.
    /// </summary>
    private static IServiceCollection AddBasicTextTemplatesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register settings
        services
            .Configure<TemplateRenderingSettings>(configuration.GetSection(nameof(TemplateRenderingSettings)));

        // Register foundation services
        services
            .AddSingleton<ITextTemplateRenderingService, BasicTextTemplateRenderingService>();

        return services;
    }
}