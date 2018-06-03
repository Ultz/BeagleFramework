using System;

namespace Ultz.BeagleFramework.Common.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    [Obsolete("ColumnAttribute has been deprecated as Beagle Framework now automatically determines column names from the property names.",true)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }
}