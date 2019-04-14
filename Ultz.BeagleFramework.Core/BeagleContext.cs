#region

using System;
using Ultz.BeagleFramework.Core.Structure;

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

        public Table CreateTable(string name)
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

        public void Dispose()
        {
            StorageEngine?.Dispose();
        }
    }
}
