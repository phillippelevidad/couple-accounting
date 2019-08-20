using System.Data;
using System.Data.SQLite;

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
            var builder = new SQLiteConnectionStringBuilder(connectionString.Value) { BinaryGUID = true };
            var connStr = builder.ToString();
            return new SQLiteConnection(connStr);
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
