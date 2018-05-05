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
        private Dictionary<string, ITable> _tables = new Dictionary<string, ITable>();

        public SqlEngine(string connectionString)
        {
            Initialize(connectionString);
        }

        public abstract string Id { get; }
        public abstract SqlConnector Connector { get; }
        public IEnumerable<ITable> Tables => GetTables().Select(x => x.Value);
        public void Initialize(string initializationParameters)
        {
            Connector.Init(initializationParameters);
            _tables = GetTables().ToDictionary(x => x.Key, x => x.Value);
            /*foreach (DataRow row in Connector.Connection.GetSchema("Tables").Rows)
            {
                var tablename = ((string)row[2]).ToUpper();
                _tables.Add(tablename,(SqlTable)GetTable(tablename));
            }*/
        }

        public ITable GetTable(string name)
        {
            return _tables.ContainsKey(name.ToUpper()) ? _tables[name.ToUpper()] : new SqlTable(name,Connector,this);
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
            sb.AppendLine("    onultz_id int,");
            foreach (var col in cols)
            {
                sb.AppendLine("    `" + col.ToUpper() + "` "+Connector.VarCharMax+",");
            }

            sb.AppendLine("    PRIMARY KEY (onultz_id)");
            sb.Append(");");
            using (var connection = Connector.CreateConnection())
            {
                connection.Open();
                Connector.CreateCommand(Connector.ProcessMessage(sb.ToString()),connection).ExecuteReader().Close();
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

            _tables.Remove(table.ToUpper());
        }

        private IEnumerable<KeyValuePair<string,ITable>> GetTables()
        {
            using (var connection = Connector.CreateConnection())
            {
                connection.Open();
                foreach (var tbl in Connector.GetTables(connection))
                {
                    var tablename = tbl.ToUpper();
                    if (!_tables.ContainsKey(tablename))
                        _tables.Add(tablename, (SqlTable) GetTable(tablename));
                }
                connection.Close();
            }

            return _tables;
        }

        public void Dispose()
        {
            Connector.Dispose();
        }
    }
}