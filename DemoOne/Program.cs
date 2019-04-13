#region

using System;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

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
                using (var reader = new SqlCommand("select * from " + Input("Table: "), conn))
                {
                    foreach (var c in reader.ExecuteReader(CommandBehavior.KeyInfo).GetColumnSchema())
                        Console.WriteLine(JsonConvert.SerializeObject(c, Formatting.Indented));
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
