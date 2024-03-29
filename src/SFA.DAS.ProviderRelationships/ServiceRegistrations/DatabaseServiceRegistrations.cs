﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Extensions;

namespace SFA.DAS.ProviderRelationships.ServiceRegistrations;

public static class DatabaseServiceRegistrations
{
    public static IServiceCollection AddDatabaseRegistration(this IServiceCollection services, string databaseConnectionString, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        ArgumentNullException.ThrowIfNull(databaseConnectionString);

        services.AddDbContext<ProviderRelationshipsDbContext>(options => options.UseSqlServer(DatabaseExtensions.GetSqlConnection(databaseConnectionString)), serviceLifetime);

        services.AddTransient(provider => new Lazy<ProviderRelationshipsDbContext>(provider.GetService<ProviderRelationshipsDbContext>()));

        return services;
    }
}