#region

using System.Collections.Generic;
using Ultz.BeagleFramework.Core;
using Ultz.BeagleFramework.Core.Structure;

#endregion

namespace Ultz.BeagleFramework.Sql
{
    public class SqlQuery : IQuery
    {
        public SqlQuery(SqlEngineBase engine, Query query)
        {
            Engine = engine;
            Query = query;
        }

        internal SqlEngineBase Engine { get; }
        public Query Query { get; }

        public int AsNonQuery()
        {
            return Engine.Translate(Query).ExecuteNonQuery();
        }

        public object AsScalar()
        {
            return Engine.Translate(Query).ExecuteScalar();
        }

        public Table AsTable(bool readOnly = true)
        {
            var tbl = new SqlTable(Engine.Translate(Query));
            return new Table(Engine, tbl.Name, tbl, tbl, Query, readOnly);
        }

        public (IReadOnlyList<Row>, IReadOnlyList<Column>) AsLists()
        {
            var tbl = new SqlTable(Engine.Translate(Query));
            return (tbl, tbl);
        }
    }
}
