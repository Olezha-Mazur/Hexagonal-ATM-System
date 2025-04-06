using Contracts;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection, string password)
    {
        collection.AddScoped<IAccountService, AccountService>();
        collection.AddScoped<IOperationHistoryService, OperationHistoryService>();
        collection.AddScoped<IAdminService, AdminService>();
        collection.AddScoped<CurrentAccountManager>();
        collection.AddSingleton<IAdminService>(new AdminService(new SystemPassword(password)));
        collection.AddScoped<ICurrentAccountService>(
            p => p.GetRequiredService<CurrentAccountManager>());
        return collection;
    }
}