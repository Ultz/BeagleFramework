using System;
using System.Collections.Generic;

namespace Ultz.BeagleFramework.Core
{
    public interface IColumnCollection : IReadOnlyList<Column>
    {
        int IndexOf(string col);
    }
    public interface IRowCollection : IReadOnlyList<Row>
    {
    }
}
