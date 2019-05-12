#region

using System;
using System.Linq;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public class BeagleContext : IDisposable
    {
        public BeagleContext(IStorageEngine engine)
        {
            StorageEngine = engine;
        }

        public IStorageEngine StorageEngine { get; set; }

        public Table GetTable(string name)
        {
            return StorageEngine.Execute
                (
                    new QueryBuilder()
                        .Select()
                        .Wildcard()
                        .From()
                        .TableName(name)
                        .Build()
                )
                .AsTable(false);
        }

        public Table CreateTable(string name, Column c1, params Column[] columns)
        {
            StorageEngine.Execute
                (
                    new QueryBuilder()
                        .Create()
                        .Table()
                        .TableName(name)
                        .PseudoColumns
                        (
                            new[] {c1}.Concat(columns).Select(x => (x.Name, x.Type, x.Constraints.ToArray())).ToArray()
                        )
                        .Build()
                )
                .AsNonQuery();
            return GetTable(name);
        }

        public void DeleteTable(string name)
        {
            StorageEngine.Execute(new QueryBuilder().Drop().Table().TableName(name).Build()).AsNonQuery();
        }

        public void Dispose()
        {
            StorageEngine?.Dispose();
        }
    }
}
