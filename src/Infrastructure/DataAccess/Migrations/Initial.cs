using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace DataAccess.Migrations;

[Migration(1, "Initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider)
        => """
           create type operation_type as enum
           (
               'replenishment',
               'withdrawal',
               'creation'
           );

           create table accounts
           (
               account_id bigint primary key generated always as identity ,
               account_password text not null ,
               account_amount money 
           );

           create table operations
           (
               operation_id bigint primary key generated always as identity ,
               operation_date timestamp not null ,
               account_id bigint not null references accounts(account_id) ,
               amount_before money ,
               amount_after money not null ,
               operation_type operation_type not null
           );
           """;

    protected override string GetDownSql(IServiceProvider serviceProvider)
        => """
           drop table accounts;
           drop table operations;
           drop type operation_type;
           """;
}