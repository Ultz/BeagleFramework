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
                        table.Clear();
                        var cols = string.Join("\t\t", table.Columns.Select(x => x.Name));
                        Console.WriteLine(cols);
                        Console.WriteLine(new string('-', cols.Length));
                        var row = new Row(431L, "Hi There!");
                        table.Add(row);
                        table.Rows.Select(x => string.Join("\t\t", x.Select(y => y.Value.ToString())))
                            .ToList()
                            .ForEach(Console.WriteLine);
                        Console.WriteLine();
                        row[1].Value = "Goodbye.";
                        row.SaveChanges();
                        table.Rows.Select(x => string.Join("\t\t", x.Select(y => y.Value.ToString())))
                            .ToList()
                            .ForEach(Console.WriteLine);
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
