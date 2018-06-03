using Ultz.BeagleFramework.Common.Models;

namespace BeagleTestApp
{
    public class MyDatabase : DataStore
    {
        public Table<Person> People { get; set; }
        public Table<Friend> Friends { get; set; }
    }

    public class Person : DataModel
    {
        public string Name { get; set; }
    }

    public class Friend : DataModel
    {
        public string FriendName { get; set; }
        public string Name { get; set; }
    }
}