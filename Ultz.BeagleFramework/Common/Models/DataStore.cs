using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ultz.BeagleFramework.Common.Models
{
    public abstract class DataStore : IEnumerable<Table>, IDataContainer
    {

        private IStorageEngine _engine;
        public void AssignTo<T>(DataContext<T> store) where T : DataStore
        {
            _engine = store.Engine;
        }
        /// <summary>
        /// Initializes this <see cref="DataStore"/>
        /// </summary>
        public void Fill()
        {
            // iterates through all types that are Table<>
            foreach (var x in GetType().GetProperties().Where(x => x.PropertyType.IsAssignableToGenericType(typeof(Table<>))))
            {
                x.SetValue(this,Activator.CreateInstance(x.PropertyType,_engine.GetTable(x.Name.ToNamePreservation()) ?? _engine.CreateTable(x.Name.ToNamePreservation(),x.PropertyType.GenericTypeArguments.First())));
            }
        }

        /// <inheritdoc />
        public IEnumerator<Table> GetEnumerator()
        {
            return GetType().GetProperties().Where(x => x.PropertyType.IsAssignableToGenericType(typeof(Table<>))).Select(x => (Table)x.GetValue(this)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void SaveChanges()
        {
            
        }

        /// <inheritdoc />
        public void Refresh()
        {
            Fill();
        }
    }
}
