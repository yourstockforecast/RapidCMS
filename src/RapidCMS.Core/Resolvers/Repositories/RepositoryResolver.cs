﻿using System;
using Microsoft.Extensions.DependencyInjection;
using RapidCMS.Core.Abstractions.Repositories;
using RapidCMS.Core.Abstractions.Resolvers;
using RapidCMS.Core.Abstractions.Setup;

namespace RapidCMS.Core.Resolvers.Repositories
{
    internal class RepositoryResolver : IRepositoryResolver
    {
        private readonly ISetupResolver<ICollectionSetup> _collectionResolver;
        private readonly IServiceProvider _serviceProvider;

        public RepositoryResolver(ISetupResolver<ICollectionSetup> collectionResolver, IServiceProvider serviceProvider)
        {
            _collectionResolver = collectionResolver;
            _serviceProvider = serviceProvider;
        }

        IRepository IRepositoryResolver.GetRepository(ICollectionSetup collection)
        {
            return (IRepository)_serviceProvider.GetRequiredService(collection.RepositoryType);
        }

        IRepository IRepositoryResolver.GetRepository(string collectionAlias)
        {
            return (this as IRepositoryResolver).GetRepository(_collectionResolver.ResolveSetup(collectionAlias));
        }

        IRepository IRepositoryResolver.GetRepository(Type repositoryType)
        {
            return (IRepository)_serviceProvider.GetRequiredService(repositoryType);
        }
    }
}
