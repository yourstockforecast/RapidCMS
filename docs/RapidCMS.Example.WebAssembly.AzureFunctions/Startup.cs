using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RapidCMS.Example.Shared.Data;
using RapidCMS.Example.Shared.DataViews;
using RapidCMS.Example.Shared.Handlers;
using RapidCMS.Repositories;

[assembly: FunctionsStartup(typeof(RapidCMS.Example.WebAssembly.AzureFunctions.Startup))]
namespace RapidCMS.Example.WebAssembly.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<JsonRepository<Person>>();
            builder.Services.AddScoped<JsonRepository<ConventionalPerson>>();
            builder.Services.AddScoped<JsonRepository<Country>>();
            builder.Services.AddScoped<JsonRepository<User>>();
            builder.Services.AddScoped<JsonRepository<TagGroup>>();
            builder.Services.AddScoped<JsonRepository<Tag>>();
            builder.Services.AddSingleton<MappedInMemoryRepository<MappedEntity, DatabaseEntity>>();
            builder.Services.AddSingleton<IConverter<MappedEntity, DatabaseEntity>, Mapper>();
            builder.Services.AddSingleton<DatabaseEntityDataViewBuilder>();

            builder.Services.AddTransient<Base64TextFileUploadHandler>();
            builder.Services.AddTransient<Base64ImageUploadHandler>();
            
            builder.AddRapidCMSApi(config =>
            {
                config.RegisterRepository<Person, JsonRepository<Person>>("person");
            });
        }
    }
}
