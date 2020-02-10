using System;
using System.Linq.Expressions;
using System.Security.Claims;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Services;
using RapidCMS.Core.Enums;

namespace RapidCMS.Core.Abstractions.Forms
{
    public interface IEditContext<TEntity> where TEntity : IEntity
    {
        ClaimsPrincipal CurrentUser { get; }
        IAuthService AuthService { get; }

        UsageType UsageType { get; }
        EntityState EntityState { get; }

        TEntity Entity { get; }
        IParent? Parent { get; }

        IRelationContainer GetRelationContainer();
        bool? IsModified<TValue>(Expression<Func<TEntity, TValue>> property);
        bool? IsModified(string propertyName);

        bool? IsValid<TValue>(Expression<Func<TEntity, TValue>> property);
        bool? IsValid(string propertyName);

        bool? WasValidated<TValue>(Expression<Func<TEntity, TValue>> property);
        bool? WasValidated(string propertyName);
    }
}
