using Tekoding.KoIdentity.Abstraction.Models;

namespace Tekoding.KoIdentity.Abstraction.Test.Helper;

internal class BaseEntity : Entity
{
#nullable disable
    internal string TestProp { get; set; } = "Default";
#nullable restore
}