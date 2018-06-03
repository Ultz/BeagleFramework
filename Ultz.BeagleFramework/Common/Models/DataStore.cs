using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ultz.BeagleFramework.Common.Models
{
    public abstract class DataStore : IEnumerable<Table>
    {

        private IStorageEngine _engine;
        public void AssignTo<T>(DataContext<T> store) where T : DataStore
        {
            _engine = store.Engine;
        }

        public Dictionary<string,Table> GetTables()
        {
            return GetType().GetProperties().Where(x => x.PropertyType.IsAssignableToGenericType(typeof(Table<>)))
                .ToDictionary(x => x.Name, x => (Table) x.GetValue(this));
        }
        
        public void Fill()
        {
            // iterates through all types that are Table<>
            foreach (var x in GetType().GetProperties().Where(x => x.PropertyType.IsAssignableToGenericType(typeof(Table<>))))
            {
                Console.WriteLine("Initiating "+x.Name);
                x.SetValue(this,Activator.CreateInstance(x.PropertyType,_engine.GetTable(x.Name.ToNamePreservation()) ?? _engine.CreateTable(x.Name.ToNamePreservation(),x.PropertyType.GenericTypeArguments.First())));
            }
        }

        public IEnumerator<Table> GetEnumerator()
        {
            return GetTables().Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
