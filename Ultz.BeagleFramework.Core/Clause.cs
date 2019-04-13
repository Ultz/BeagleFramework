#region

using System.Collections.Generic;

#endregion

namespace Ultz.BeagleFramework.Core.Structure
{
    public abstract class Clause
    {
        public class Select : Clause
        {
        }

        public class Wildcard : Clause
        {
        }

        public class ColumnName : Clause
        {
            public string Column { get; set; }
        }

        public class ColumnNames : Clause
        {
            public List<string> Columns { get; set; }
        }

        public class Set : Clause
        {
            public List<(string, object)> Values { get; set; }
        }

        public class From : Clause
        {
        }

        public class TableName : Clause
        {
            public string Table { get; set; }
        }

        public class Where : Clause
        {
        }

        public class And : Clause
        {
        }

        public class Not : Clause
        {
        }

        public class Or : Clause
        {
        }

        public class Equal : Clause
        {
        }

        public class NotEqual : Clause
        {
        }

        public class Update : Clause
        {
        }

        public class GreaterThan : Clause
        {
            public bool OrEqualTo { get; set; }
        }

        public class LessThan : Clause
        {
            public bool OrEqualTo { get; set; }
        }

        public class Between : Clause
        {
        }

        public class Like : Clause
        {
        }

        public class In : Clause
        {
        }

        public class Insert : Clause
        {
        }

        public class Into : Clause
        {
        }

        public class Delete : Clause
        {
        }

        public class RowNumber : Clause
        {
        }

        public class Value : Clause
        {
            public object Item { get; set; }
        }

        public class Values : Clause
        {
            public object[] Items { get; set; }
        }

        public class ValuesGroup : Clause
        {
            public Values[] Items { get; set; }
        }
    }
}
