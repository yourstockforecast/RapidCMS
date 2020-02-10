using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Setup;
using RapidCMS.Core.Enums;
using RapidCMS.Core.Forms;

namespace RapidCMS.Core.Abstractions.Services
{
    public interface IAuthService
    {
        internal Task<bool> IsUserAuthorizedAsync(UsageType usageType, IEntity entity);
        public Task<bool> IsUserAuthorizedAsync(OperationAuthorizationRequirement operation, IEntity entity);
        internal Task<bool> IsUserAuthorizedAsync(EditContext editContext, IButtonSetup button);

        internal Task EnsureAuthorizedUserAsync(UsageType usageType, IEntity entity);
        internal Task EnsureAuthorizedUserAsync(OperationAuthorizationRequirement operation, IEntity entity);
        internal Task EnsureAuthorizedUserAsync(EditContext editContext, IButtonSetup button);
    }
}
