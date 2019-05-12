#region

#endregion

namespace Ultz.BeagleFramework.Core
{
    public abstract class Constraint
    {
        public class PrimaryKey : Constraint
        {
        }

        public class ForeignKey : Constraint
        {
            public string ForeignColumn { get; set; }
            public string ForeignTable { get; set; }
        }

        public class NotNull : Constraint
        {
        }

        public class Unique : Constraint
        {
        }

        public class Check : Constraint
        {
            public Query Condition { get; set; }
        }

        public class Default : Constraint
        {
            public object Value { get; set; }
        }
    }
}
