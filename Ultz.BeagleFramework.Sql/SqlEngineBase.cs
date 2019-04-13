#region

using System;
using System.Data.Common;
using Ultz.BeagleFramework.Core;
using Ultz.BeagleFramework.Core.Structure;

#endregion

namespace Ultz.BeagleFramework.Sql
{
    public abstract class SqlEngineBase : IStorageEngine
    {
        protected SqlEngineBase(string connectionString)
        {
        }
        public DbConnection Connection { get; protected set; }
        public IQuery Execute(Query query)
        {
            return new SqlQuery(this, query);
        }
        public abstract DbCommand Translate(Query query);

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
