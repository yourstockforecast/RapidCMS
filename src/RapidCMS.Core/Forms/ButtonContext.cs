using System;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Forms;
using RapidCMS.Core.Abstractions.Services;

namespace RapidCMS.Core.Forms
{
    internal class ButtonContext : IButtonContext
    {
        private readonly IServiceProvider _serviceProvider;

        public ButtonContext(IParent? parent, object? customData, ClaimsPrincipal claimsPrincipal, IServiceProvider serviceProvider)
        {
            Parent = parent;
            CustomData = customData;
            CurrentUser = claimsPrincipal;
            _serviceProvider = serviceProvider;
        }

        public IParent? Parent { get; set; }
        public object? CustomData { get; set; }

        public ClaimsPrincipal CurrentUser { get; private set; }
        public IAuthService AuthService => _serviceProvider.GetService<IAuthService>();
    }
}
