#region

using System.Collections.Generic;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public class Column
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public DataType Type { get; set; }
        public List<Constraint> Constraints { get; set; }
    }
}
