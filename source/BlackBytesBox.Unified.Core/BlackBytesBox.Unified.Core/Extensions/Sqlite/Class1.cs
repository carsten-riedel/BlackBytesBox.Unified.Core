using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlackBytesBox.Unified.Core.Extensions.Sqlite
{
    /// <summary>
    /// Provides extension methods for setting up and initializing a SQLite-based DbContext.
    /// </summary>
    /// <remarks>
    /// These extensions register a generic DbContext with SQLite and perform initialization by ensuring the database is created and setting the Write-Ahead Logging (WAL) journal mode.
    /// </remarks>
    public static partial class SqliteDbContextExtensions
    {
        /// <summary>
        /// Registers the generic DbContext with the required SQLite connection string.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the DbContext to register.</typeparam>
        /// <param name="services">The service collection to add the context to.</param>
        /// <param name="password">The desired database password; set to an empty string if no encryption is required.</param>
        /// <returns>The updated IServiceCollection.</returns>
        /// <example>
        /// <code>
        /// // In Program.cs or Startup.cs:
        /// builder.Services.AddSqliteDbContext&lt;TDbContext&gt;();
        /// </code>
        /// </example>
        public static IServiceCollection AddSqliteDbContext<TDbContext>(this IServiceCollection services, string password = "")
        where TDbContext : DbContext
        {
            // Determine the absolute path to the database file in the output directory.
            var outputDir = AppContext.BaseDirectory;
            var dataSource = Path.Combine(outputDir, "app.db");

            // Build the SQLite connection string.
            var connectionString = $"Data Source={dataSource};Cache=Shared;";
            if (!string.IsNullOrEmpty(password))
            {
                connectionString += $"Password={password};";
            }

            // Register the DbContext with the SQLite connection.
            services.AddDbContext<TDbContext>(options => options.UseSqlite(connectionString));

            return services;
        }
    }
}