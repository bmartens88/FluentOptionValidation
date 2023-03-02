namespace FluentOptionValidation.Abstractions.Interfaces;

/// <summary>
/// Marker interface which must be implemented by an Options class.
/// </summary>
public interface IOptions
{
    /// <summary>
    /// Property which returns the name of the section from configuration which contains the option value(s).
    /// </summary>
    static abstract string SectionName { get; }
}