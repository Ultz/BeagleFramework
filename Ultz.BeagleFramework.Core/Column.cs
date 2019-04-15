#region

using System.Collections.Generic;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public class Column
    {
        public Column()
        {
            
        }

        public Column(string name, DataType type, bool primaryKey = false)
        {
            Name = name;
            Type = type;
            Constraints = new List<Constraint>(primaryKey ? new []{new Constraint.PrimaryKey()} : new Constraint[0]);
        }
        public string Name { get; set; }
        public DataType Type { get; set; }
        public List<Constraint> Constraints { get; set; }
    }
}
