#region

using System;
using System.Data.SqlClient;
using System.Linq;
using Ultz.BeagleFramework.Core;
using Ultz.BeagleFramework.SqlServer;

#endregion

namespace DemoOne
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var conn = new SqlConnection
            (
                $"Server={Input("Server: ")};Database={Input("Database: ")};User Id={Input("User ID: ")};Password={Input("Password: ")};"
            ))
            {
                conn.Open();
                using (var beagle = new BeagleContext(new SqlStorageEngine(conn)))
                {
                    using (var table = beagle.GetTable(Input("Table: ")))
                    {
                        table.Add(new Column("blargh", DataType.Boolean));
                        Console.WriteLine(string.Join(", ", table.Columns.Select(x => x.Name)));
                    }
                }
            }
        }

        private static string Input(string msg)
        {
            Console.Write(msg);
            return Console.ReadLine();
        }
    }
}
