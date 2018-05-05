using System;
using Ultz.BeagleFramework;
using Ultz.BeagleFramework.Common;
using Ultz.BeagleFramework.PostgreSql;

namespace BeagleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new NpgsqlEngine("Server=localhost;Port=5432;Database=test;User ID=test;Password=Test12345;");
            engine.CreateTable<Person>("people").Put<Person>(new Person(){Name="Dylan"});
            engine.Dispose();
        }
    }
}