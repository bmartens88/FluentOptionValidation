using FluentOptionValidation.Abstractions;
using FluentOptionValidation.Abstractions.Interfaces;

namespace OptionValidationTestProject.Options;

public sealed class JwtOptions : IOptions
{
    public static string SectionName => "Jwt";

    public required string Key { get; init; }

    public required string Issuer { get; init; }

    public required string Audience { get; init; }
}