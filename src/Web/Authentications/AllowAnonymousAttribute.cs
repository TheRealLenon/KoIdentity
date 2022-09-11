namespace Tekoding.KoIdentity.Examples.API.Authentications;

/// <summary>
/// The authorization Attribute for anonymous access.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class AllowAnonymousAttribute: Attribute
{
    
}