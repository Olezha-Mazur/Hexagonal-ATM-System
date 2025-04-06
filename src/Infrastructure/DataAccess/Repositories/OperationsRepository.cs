﻿using Abstractions.Repositories;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Models.Operations;
using Npgsql;

namespace DataAccess.Repositories;

public class OperationsRepository : IOperationRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public OperationsRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<IEnumerable<Operation>> GetOperationsByAccountId(long accountId)
    {
        const string sql = """
                           select * 
                           from operations
                           where account_id = @account_id;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(sql, connection);
        command
            .AddParameter("account_id", accountId);

        using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        var operationList = new List<Operation>();
        while (await reader.ReadAsync())
        {
            operationList.Add(new Operation(
                Date: reader.GetDateTime(0),
                AccountId: reader.GetInt32(1),
                AmountBefore: reader.GetDecimal(2),
                AmountAfter: reader.GetDecimal(3),
                Type: reader.GetFieldValue<OperationType>(4)));
        }

        return operationList;
    }

    public async Task AddOperation(long accountId, decimal? amountBefore, decimal? amountAfter, OperationType type)
    {
        const string sql = """
                           insert into operations (operation_date, account_id, amount_before, amount_after, operation_type)
                           values (now(), @account_id, @amount_before, @amount_after, @operation_type);
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(sql, connection);
        command
            .AddParameter("account_id", accountId)
            .AddParameter("amount_before", amountBefore)
            .AddParameter("amount_after", amountAfter)
            .AddParameter("operation_type", type);

        using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
    }
}