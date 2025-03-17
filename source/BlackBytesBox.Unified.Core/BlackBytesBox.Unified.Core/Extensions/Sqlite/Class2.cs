using System;
using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        /// Initializes the generic DbContext by ensuring the SQLite database is created and setting the journal mode to Write-Ahead Logging (WAL).
        /// This extension method applies to IHost.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the DbContext to initialize.</typeparam>
        /// <param name="host">The generic host to initialize the database on.</param>
        /// <returns>The updated IHost.</returns>
        /// <example>
        /// <code>
        /// // After building the host:
        /// var host = builder.Build();
        /// host.InitializeSqliteDbContext&lt;TDbContext&gt;();
        /// </code>
        /// </example>
        public static IHost InitializeSqliteDbContext<TDbContext>(this IHost host)
            where TDbContext : DbContext
        {
            InitializeSqlite<TDbContext>(host.Services);
            return host;
        }

        /// <summary>
        /// Internal helper method to initialize the SQLite DbContext.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the DbContext to initialize.</typeparam>
        /// <param name="serviceProvider">The service provider used to resolve the DbContext.</param>
        private static void InitializeSqlite<TDbContext>(IServiceProvider serviceProvider)
        where TDbContext : DbContext
        {
            // Create a service scope to work with the DbContext.
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                // Create the database and apply the schema based on the EF Core model.
                dbContext.Database.EnsureCreated();

                // Retrieve the underlying SQLite database connection.
                var conn = dbContext.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                // Set the journal mode to Write-Ahead Logging (WAL).
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "PRAGMA journal_mode=WAL;";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}