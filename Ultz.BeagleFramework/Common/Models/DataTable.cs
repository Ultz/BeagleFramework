using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultz.BeagleFramework.Common.Models
{
    public class Table<T> : Table, IList<T>, IDataContainer where T: DataModel
    {
        /// <inheritdoc />
        public new IEnumerator<T> GetEnumerator()
        {
            return Rows.Select(Store.ToObject<T>).GetEnumerator();
        }
        /// <inheritdoc />
        public void Add(T item)
        {
            Mirror.Put(item);
        }
        /// <inheritdoc />
        public bool Contains(T item)
        {
            return Rows.Select(Store.ToObject<T>).Contains(item);
        }
        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            var l = ((T[])array).ToList();
            l.InsertRange(arrayIndex, Rows.Select(Common.Store.ToObject<T>));
            array = l.ToArray();
        }
        /// <inheritdoc />
        public bool Remove(T item)
        {
            if (!Contains(item))
                return false;
            try
            {
                Remove(IndexOf(item));
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return Mirror.Rows.Select(Common.Store.ToObject<T>).ToList().IndexOf(item);
        }
        /// <inheritdoc />
        [Obsolete("Due to design restrictions of Beagle Framework, Insert is not supported.", true)]
        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Due to design restrictions of Beagle Framework, Insert is not supported.");
        }
        /// <inheritdoc />
        public new T this[int index]
        {
            get => Common.Store.ToObject<T>(Rows.ElementAt(index));
            set => Rows.ElementAt(index).Replace(value);
        }
        /// <summary>
        /// Creates a modelled table from an <see cref="ITable"/>
        /// </summary>
        /// <param name="mirror"></param>
        public Table(ITable mirror) : base(mirror)
        {
            RunModifications(Compare());
        }

        private void RunModifications(IEnumerable<string> modifications)
        {
            foreach (var mod in modifications)
            {
                if (mod.StartsWith("R:"))
                {
                    Mirror.TryRemoveColumn(mod.Substring(2));
                }
                else
                {
                    Mirror.TryAddColumn(mod.Substring(2));
                }
            }
        }
        
        private IEnumerable<string> Compare()
        {
            var updatedColumns = typeof(T).GetProperties().Select(x =>
                    x.Name.ToNamePreservation().ToUpper())
                .ToArray();
            var columns = Mirror.Columns.Select(x => x.ToUpper()).ToList();
            return (from column in updatedColumns where !columns.Contains(column) select "A:" + column).Concat(
                (from column in columns where !updatedColumns.Contains(column) select "R:" + column));
        }
    }

    public class Table : ITable, IList, IDataContainer
    {
        protected Table(ITable mirror)
        {
            Mirror = mirror;
        }
        /// <inheritdoc />
        public void Dispose()
        {
            Mirror.Dispose();
        }

        /// <inheritdoc />
        public IEnumerator GetEnumerator()
        {
            return Mirror.Rows.Select(Common.Store.ToObject).GetEnumerator();
        }

        /// <inheritdoc />
        public int Add(object value)
        {
            return Mirror.Put(value).Index;
        }

        /// <inheritdoc />
        public void Clear()
        {
            int i = -1;
            while (i++ != Count)
                Remove(0);
        }

        /// <inheritdoc />
        public bool Contains(object value)
        {
            return Mirror.Rows.Select(Common.Store.ToObject).Contains(value);
        }

        /// <inheritdoc />
        public int IndexOf(object value)
        {
            return Mirror.Rows.Select(Common.Store.ToObject).ToList().IndexOf(value);
        }
        /// <inheritdoc />
        [Obsolete("Due to design restrictions of Beagle Framework, Insert is not supported.",true)]
        public void Insert(int index, object value)
        {
            throw new NotSupportedException("Due to design restrictions of Beagle Framework, Insert is not supported.");
        }

        /// <inheritdoc />
        public void Remove(object value)
        {
            Remove(IndexOf(value));
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            Remove(index);
        }

        /// <inheritdoc />
        public bool IsFixedSize => false;
        /// <inheritdoc />
        public bool IsReadOnly => false;
        /// <inheritdoc />
        object IList.this[int index]
        {
            get => Common.Store.ToObject(Rows.ElementAt(index));
            set => Rows.ElementAt(index).Replace(value);
        }

        /// <inheritdoc />
        public Row this[int index] => Rows.ElementAt(index);

        /// <inheritdoc />
        public string Name => Mirror.Name;
        /// <inheritdoc />
        public string[] Columns => Mirror.Columns;
        /// <inheritdoc />
        public IEnumerable<Row> Rows => Mirror.Rows;
        /// <inheritdoc />
        public bool TryAddColumn(string column)
        {
            return Mirror.TryAddColumn(column);
        }

        /// <inheritdoc />
        public bool TryRemoveColumn(string column)
        {
            return Mirror.TryRemoveColumn(column);
        }

        /// <inheritdoc />
        public Row Put(IEnumerable<string> row)
        {
            return Add(row);
        }

        /// <inheritdoc />
        public Row Add(IEnumerable<string> row)
        {
            return Mirror.Add(row);
        }

        /// <inheritdoc />
        public void Remove(Row row)
        {
            Mirror.Remove(row);
        }
        /// <inheritdoc />

        public void Remove(int row)
        {
            Mirror.Remove(row);
        }

        
        /// <inheritdoc />
        public void SaveChanges()
        {
            
        }

        /// <inheritdoc />
        public void Refresh()
        {
            Mirror.Refresh();
        }

        /// <inheritdoc />
        public void CopyTo(Array array, int index)
        { 
        
            var l = ((object[])array).ToList();
            l.InsertRange(index,Rows.Select(Common.Store.ToObject));
            array = l.ToArray();
        }

        /// <inheritdoc />
        public int Count => Rows.Count();
        /// <inheritdoc />
        public bool IsSynchronized => true;
        /// <inheritdoc />
        public object SyncRoot => Mirror;

        protected ITable Mirror { get; }
    }
}
