using System.Security.Claims;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Services;

namespace RapidCMS.Core.Abstractions.Forms
{
    public interface IButtonContext
    {
        IParent? Parent { get; }
        object? CustomData { get; }

        ClaimsPrincipal CurrentUser { get; }
        IAuthService AuthService { get; }
    }
}
