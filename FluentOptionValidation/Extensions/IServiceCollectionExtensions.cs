using FluentOptionValidation.Abstractions;
using FluentOptionValidation.Abstractions.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FluentOptionValidation.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Register an Option class with Fluent validation to the DI container.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> for configuring the DI container.</param>
    /// <param name="configuration"><see cref="IConfiguration"/> which contains the option value(s).</param>
    /// <typeparam name="TOptions">Type of the Option class to configure in the DI container.</typeparam>
    /// <typeparam name="TValidator">Type of the validator to configure in the DI container.</typeparam>
    /// <returns></returns>
    public static IServiceCollection AddOptionsWithValidation<TOptions, TValidator>(this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : class, IOptions
        where TValidator : class, IValidator<TOptions>
    {
        services.AddScoped<IValidator<TOptions>, TValidator>();
        services.AddOptions<TOptions>()
            .Bind(configuration.GetSection(TOptions.SectionName))
            .ValidateFluentValidation()
            .ValidateOnStart();

        // No configure DI to have a singleton instance of the Option class.
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<TOptions>>().Value);
        return services;
    }
}