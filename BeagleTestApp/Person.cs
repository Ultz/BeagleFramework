using Ultz.BeagleFramework.Common.Models;

namespace BeagleTestApp
{
    [Model]
    public class Person
    {
        [Column("name")]
        public string Name { get; set; }
    }
}