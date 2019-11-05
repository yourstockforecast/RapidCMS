﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using RapidCMS.Common.Authorization;
using RapidCMS.Common.Extensions;
using TestLibrary.Entities;

namespace TestLibrary.Authorization
{
    public class CountryEntityAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, CountryEntity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, CountryEntity resource)
        {
            // if (requirement.In(Operations.Add, Operations.Remove))
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}