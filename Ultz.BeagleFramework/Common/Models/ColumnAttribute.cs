using System;

namespace Ultz.BeagleFramework.Common.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }
}