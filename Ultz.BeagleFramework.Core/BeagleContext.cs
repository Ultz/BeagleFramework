#region

using Ultz.BeagleFramework.Core.Structure;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public class BeagleContext
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
    }
}
