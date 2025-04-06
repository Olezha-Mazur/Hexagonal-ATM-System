﻿using Abstractions.Repositories;
using Contracts;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Models;
using Npgsql;

namespace DataAccess.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public AccountRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<long> AddAccount(string password)
    {
        const string sql = """
                           insert into accounts (account_password, account_amount)
                           values (@account_password, 0)
                           returning account_id;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(sql, connection);
        command.AddParameter("account_password", password);

        using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return reader.GetInt64(0);
    }

    public async Task ChangeTotalMoneyAccount(long accountId, decimal totalMoney)
    {
        const string sql = """
                           update accounts
                           set account_amount = account_amount + @account_amount
                           where account_id = @account_id;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(sql, connection);
        command.AddParameter("account_id", accountId).AddParameter("account_amount", totalMoney);

        using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
    }

    public async Task<FindAccountResult> TryFindAccount(long accountId, string password)
    {
        const string sql = """
                           select * 
                           from accounts 
                           where account_id = @account_id;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(sql, connection);
        command.AddParameter("account_id", accountId);

        using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync() is false)
            return new FindAccountResult.NotFoundAccount();
        if (reader.GetString(1) == password)
        {
            var account = new Account(
                Id: reader.GetInt64(0),
                Password: reader.GetString(1),
                TotalMoney: reader.GetDecimal(2));
            return new FindAccountResult.Success(account);
        }
        else
        {
            return new FindAccountResult.WrongPassword();
        }
    }

    public async Task<Account?> TryGetAccount(long accountId)
    {
        const string sql = "select * from accounts where account_id = @account_id;";

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(sql, connection);
        command.AddParameter("account_id", accountId);

        using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync() is false)
            return null;

        return new Account(
            Id: reader.GetInt64(0),
            Password: reader.GetString(1),
            TotalMoney: reader.GetDecimal(2));
    }
}