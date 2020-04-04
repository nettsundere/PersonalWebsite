using Microsoft.Data.Sqlite;

namespace PersonalWebsite.Tests.Helpers
{
    /// <summary>
    /// In-memory db (testing) connection helper.
    /// </summary>
    public static class InMemoryConnectionHelper
    {
        /// <summary>
        /// Setup an in-memory database connection.
        /// </summary>
        /// <returns>The connection to the in-memory db</returns>
        public static SqliteConnection SetupConnection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return connection;
        }
    }
}
