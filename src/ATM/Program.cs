﻿using Application.Extensions;
using Console;
using Console.Extensions;
using DataAccess.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Lab5;

public class Program
{
    public static void Main(string[] args)
    {
        var collection = new ServiceCollection();

        collection
            .AddApplication("total_secret_password")
            .AddInfrastructureDataAccess(configuration =>
            {
                configuration.Host = "localhost";
                configuration.Port = 6432;
                configuration.Username = "postgres";
                configuration.Password = "postgres";
                configuration.Database = "postgres";
                configuration.SslMode = "Prefer";
            })
            .AddPresentationConsole();

        ServiceProvider provider = collection.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();

        scope.UseInfrastructureDataAccess();

        ScenarioRunner scenarioRunner = scope.ServiceProvider
            .GetRequiredService<ScenarioRunner>();
        while (true)
        {
            scenarioRunner.Run();
            AnsiConsole.Clear();
        }
    }
}