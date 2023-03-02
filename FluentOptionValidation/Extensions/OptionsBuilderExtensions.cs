using FluentOptionValidation.Validator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FluentOptionValidation.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="OptionsBuilder{TOptions}"/>.
/// </summary>
public static class OptionsBuilderExtensions
{
    /// <summary>
    /// Adds options validation using Fluent Validation to the DI container.
    /// </summary>
    /// <param name="optionsBuilder"><see cref="OptionsBuilder{TOptions}"/> for which to setup option validation.</param>
    /// <typeparam name="TOptions">Type of options for which to configure Fluent option validation.</typeparam>
    /// <returns><see cref="OptionsBuilder{TOptions}"/> with option validation configured.</returns>
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(
        this OptionsBuilder<TOptions> optionsBuilder)
        where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(provider =>
            new FluentValidationOptions<TOptions>(optionsBuilder.Name, provider));
        return optionsBuilder;
    }
}