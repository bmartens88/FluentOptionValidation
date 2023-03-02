using System.Reflection;
using FluentOptionValidation.Abstractions.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ext = FluentOptionValidation.Extensions.IServiceCollectionExtensions;

namespace FluentOptionValidation.AspNetCore.DependencyInjection.Extensions;

/// <summary>
/// Contains extension method(s) for the <see cref="IServiceCollection"/> DI container abstraction.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Reference to the method which is invoked to register an option type with a validator type.
    /// </summary>
    private static readonly MethodInfo Method = typeof(Ext).GetMethod(nameof(Ext.AddOptionsWithValidation))!;

    /// <summary>
    /// Register all Option validators found in the provided Assembly to the DI container.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> for DI container configuration.</param>
    /// <param name="configuration"><see cref="IConfiguration"/> which should contain option value(s).</param>
    /// <typeparam name="TAssembly">The <see cref="Assembly"/> to scan for option validator(s).</typeparam>
    /// <returns><see cref="IServiceCollection"/> with Fluent option validator(s) configured.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no validator is found for an Option type.</exception>
    public static IServiceCollection RegisterOptionsWithValidationFromAssembly<TAssembly>(
        this IServiceCollection services, IConfiguration configuration)
    {
        // All defined types of the Assembly.
        var definedTypes = typeof(TAssembly).Assembly.DefinedTypes.ToList();

        // Find all types which implement the FluentOptionValidation.Abstractions.Interfaces.IOptions interface.
        var optionTypes = definedTypes
            .Where(type => type.GetInterfaces().Contains(typeof(IOptions)))
            .ToList();

        // Get a collection of option type(s) with their validator type.
        var optionAndValidatorTypes = optionTypes.Select(option =>
        {
            var vt = typeof(AbstractValidator<>);
            var evt = vt.MakeGenericType(option);
            var validator = definedTypes.FirstOrDefault(t => t.IsSubclassOf(evt));
            if (validator is not null)
                return new ScanResult(option, validator);
            // No validator type found for an option type, so we throw an exception.
            throw new InvalidOperationException($"No validator provided for option type '{option.Name}'.");
        });

        // For each option with validator type, register in the DI container.
        foreach (var (optionType, validatorType) in optionAndValidatorTypes)
        {
            var generic = Method.MakeGenericMethod(optionType, validatorType);
            generic.Invoke(null, new object?[] { services, configuration });
        }

        return services;
    }

    private record ScanResult(Type OptionType, Type ValidatorType);
}