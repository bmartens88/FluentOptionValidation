using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FluentOptionValidation.Validator;

public sealed class FluentValidationOptions<TOptions>
    : IValidateOptions<TOptions>
    where TOptions : class
{
    private readonly string? _name;
    private readonly IServiceProvider _serviceProvider;

    public FluentValidationOptions(string? name,
        IServiceProvider serviceProvider)
    {
        _name = name;
        _serviceProvider = serviceProvider;
    }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (_name != null && _name != name)
            return ValidateOptionsResult.Skip;

        ArgumentNullException.ThrowIfNull(options);

        using var scope = _serviceProvider.CreateScope();

        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();

        var result = validator.Validate(options);
        if (result.IsValid)
            return ValidateOptionsResult.Success;

        var typeName = options.GetType().Name;
        var errors = result.Errors.Select(error =>
                $"Fluent validation failed for '{typeName}.{error.PropertyName}' with the error: '{error.ErrorMessage}'.")
            .ToList();
        return ValidateOptionsResult.Fail(errors);
    }
}