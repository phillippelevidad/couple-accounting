using Microsoft.Data.Sqlite;
using System.Data;

namespace Queries
{
    public class ConnectionFactory
    {
        private readonly AccountingConnectionString connectionString;

        public ConnectionFactory(AccountingConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection Create()
        {
            return new SqliteConnection(connectionString.Value);
        }
    }

    public class AccountingConnectionString
    {
        public AccountingConnectionString(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
