namespace Tekoding.KoIdentity.Abstraction.Extensions;

/// <summary>
/// Provides extension methods used directly on <see cref="Type"/>s.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Gets the default type, without the generic arity.
    /// </summary>
    /// <param name="type">The type to get the name from.</param>
    /// <returns>Returning only the type without the generic arity.</returns>
    public static string GetNameWithoutGenericArity(this Type type)
    {
        var name = type.Name;
        var index = name.IndexOf('`');
        return index == -1 ? name : name.Substring(0, index);
    }
}