#region

using System.Collections.Generic;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public interface IQuery
    {
        Query Query { get; }
        int AsNonQuery();
        object AsScalar();
        Table AsTable(bool readOnly = true);
        (IReadOnlyList<Row>, IReadOnlyList<Column>) AsLists();
    }
}
