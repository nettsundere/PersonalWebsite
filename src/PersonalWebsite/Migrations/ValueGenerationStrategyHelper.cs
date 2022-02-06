using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsite.Migrations;

/// <summary>
/// A helper to make the autogenerate ID annotations database-agnostic.
/// Supports MSSQL Server and the SQLite.
/// </summary>
public static class ValueGenerationStrategyHelper
{
    /// <summary>Get database-agnostic values for .Annotation</summary>
    /// <param name="migrationBuilder">The migration builder</param>
    /// <returns>A pair of arguments for .Annotation that makes it database-agnostic</returns>
    /// <exception cref="NotImplementedException">The currently used DB is not supported</exception>
    public static (string Key, object Val) GetAutoIncrementGenerationStrategy(MigrationBuilder migrationBuilder)
    {
        if (migrationBuilder.IsSqlServer())
        {
            return ("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
        
        if (migrationBuilder.IsSqlite())
        {
            return ("Sqlite:Autoincrement", true);
        }

        throw new NotImplementedException("The database currently used is not supported");
    }
}