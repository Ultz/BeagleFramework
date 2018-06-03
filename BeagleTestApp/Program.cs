using System;
using System.Diagnostics;
using Ultz.BeagleFramework;
using Ultz.BeagleFramework.Common;
using Ultz.BeagleFramework.PostgreSql;
using Ultz.BeagleFramework.SQLite;

namespace BeagleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = Beagle.CreateContext<MyDatabase>(new SqliteEngine("Filename=./test3.db"));
            Debug.WriteLine(context.Store.People == null);
            context.Store.People.Add(new Person() {Name = "Dylan"});
            context.Store.Friends.Add(new Friend() {FriendName = "Dylan", Name = "Brad"});
            context.Store.Friends.Add(new Friend() { FriendName = "Dylan", Name = "Jacob" });
            foreach (var person in context.Store.People)
            {
                Console.WriteLine("[Person] Name="+person.Name);
            }
            foreach (var f in context.Store.Friends)
            {
                Console.WriteLine("[Friend] Name=" + f.Name,",FriendName="+f.Name);
            }

            Console.ReadLine();
        }
    }
}