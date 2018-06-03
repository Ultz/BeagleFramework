using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ultz.BeagleFramework.Sql
{
    public abstract class SqlEngine : IStorageEngine
    {
        protected SqlEngine(string connectionString)
        {
            Initialize(connectionString);
        }

        public abstract string Id { get; }
        public abstract SqlConnector Connector { get; }
        public void Initialize(string initializationParameters)
        {
            Connector.Init(initializationParameters);
            /*foreach (DataRow row in Connector.Connection.GetSchema("Tables").Rows)
            {
                var tablename = ((string)row[2]).ToUpper();
                _tables.Add(tablename,(SqlTable)GetTable(tablename));
            }*/
        }

        public ITable GetTable(string name)
        {
            try
            {
                return new SqlTable(name, Connector, this);
            }
            catch
            {
                return null;
            }
        }

        public Column GetColumn(string table, int col)
        {
            return new Column(col, GetTable(table.ToUpper()).Columns[col]);
        }

        public Column GetColumn(string table, string col)
        {
            return new Column(Array.IndexOf(GetTable(table.ToUpper()).Columns,col.ToUpper()),col);
        }

        public ITable CreateTable(string table, string[] cols)
        {
            var sb = new StringBuilder();
            sb.AppendLine("CREATE TABLE `" + table.ToUpper() + "` (");
            sb.AppendLine("    onultz_id "+Connector.Int+",");
            foreach (var col in cols)
            {
                sb.AppendLine("    `" + col.ToUpper() + "` "+Connector.VarCharMax+",");
            }

            sb.AppendLine("    PRIMARY KEY (onultz_id)");
            sb.Append(");");
            using (var connection = Connector.CreateConnection())
            {
                connection.Open();
                var msg = Connector.ProcessMessage(sb.ToString());
                Connector.CreateCommand(msg,connection).ExecuteReader().Close();
                connection.Close();
            }

            return GetTable(table.ToUpper());
        }

        public void DeleteTable(string table)
        {
            using (var connection = Connector.CreateConnection())
            {
                connection.Open();
                Connector.CreateCommand(Connector.ProcessMessage("DROP TABLE `" + table.ToUpper()+"`"),connection).ExecuteReader().Close();
                connection.Close();
            }
        }

        public void Dispose()
        {
            Connector.Dispose();
        }
    }
}