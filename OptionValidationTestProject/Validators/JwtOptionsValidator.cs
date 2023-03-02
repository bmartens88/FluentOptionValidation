// using FluentValidation;
// using OptionValidationTestProject.Options;
//
// namespace OptionValidationTestProject.Validators;
//
// public sealed class JwtOptionsValidator : AbstractValidator<JwtOptions>
// {
//     public JwtOptionsValidator()
//     {
//         RuleFor(x => x.Key)
//             .NotEmpty()
//             .MinimumLength(32);
//         RuleFor(x => x.Issuer)
//             .NotEmpty()
//             .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
//         RuleFor(x => x.Audience)
//             .NotEmpty()
//             .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
//     }
// }