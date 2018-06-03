using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultz.BeagleFramework.Common.Models
{
    public class Table<T> : Table, IList<T> where T: DataModel
    {
        public new IEnumerator<T> GetEnumerator()
        {
            return Rows.Select(Store.ToObject<T>).GetEnumerator();
        }

        public void Add(T item)
        {
            Mirror.Put(item);
        }

        public bool Contains(T item)
        {
            return Rows.Select(Store.ToObject<T>).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var l = ((T[])array).ToList();
            l.InsertRange(arrayIndex, Rows.Select(Common.Store.ToObject<T>));
            array = l.ToArray();
        }

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

        public int IndexOf(T item)
        {
            return Mirror.Rows.Select(Common.Store.ToObject<T>).ToList().IndexOf(item);
        }

        [Obsolete("Due to design restrictions of Beagle Framework, Insert is not supported.", true)]
        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Due to design restrictions of Beagle Framework, Insert is not supported.");
        }

        public new T this[int index]
        {
            get => Common.Store.ToObject<T>(Rows.ElementAt(index));
            set => Rows.ElementAt(index).Replace(value);
        }


        public Table(ITable mirror) : base(mirror)
        {

        }
    }

    public class Table : ITable, IList
    {
        public Table(ITable mirror)
        {
            Mirror = mirror;
        }
        public void Dispose()
        {
            Mirror.Dispose();
        }

        public IEnumerator GetEnumerator()
        {
            return Mirror.Rows.Select(Common.Store.ToObject).GetEnumerator();
        }

        public int Add(object value)
        {
            return Mirror.Put(value).Index;
        }

        public void Clear()
        {
            int i = -1;
            while (i++ != Count)
                Remove(0);
        }

        public bool Contains(object value)
        {
            return Mirror.Rows.Select(Common.Store.ToObject).Contains(value);
        }

        public int IndexOf(object value)
        {
            return Mirror.Rows.Select(Common.Store.ToObject).ToList().IndexOf(value);
        }
        [Obsolete("Due to design restrictions of Beagle Framework, Insert is not supported.",true)]
        public void Insert(int index, object value)
        {
            throw new NotSupportedException("Due to design restrictions of Beagle Framework, Insert is not supported.");
        }

        public void Remove(object value)
        {
            Remove(IndexOf(value));
        }

        public void RemoveAt(int index)
        {
            Remove(index);
        }

        public bool IsFixedSize => false;
        public bool IsReadOnly => false;
        object IList.this[int index]
        {
            get => Common.Store.ToObject(Rows.ElementAt(index));
            set => Rows.ElementAt(index).Replace(value);
        }

        public Row this[int index] => Rows.ElementAt(index);

        public string Name => Mirror.Name;
        public string[] Columns => Mirror.Columns;
        public IEnumerable<Row> Rows => Mirror.Rows;
        public bool TryAddColumn(string column)
        {
            return Mirror.TryAddColumn(column);
        }

        public bool TryRemoveColumn(string column)
        {
            return Mirror.TryRemoveColumn(column);
        }

        public Row Put(IEnumerable<string> row)
        {
            return Add(row);
        }

        public Row Add(IEnumerable<string> row)
        {
            return Mirror.Add(row);
        }

        public void Remove(Row row)
        {
            Mirror.Remove(row);
        }

        public void Remove(int row)
        {
            Mirror.Remove(row);
        }

        public void Refresh()
        {
            Mirror.Refresh();
        }

        public void CopyTo(Array array, int index)
        { 
        
            var l = ((object[])array).ToList();
            l.InsertRange(index,Rows.Select(Common.Store.ToObject));
            array = l.ToArray();
        }

        public int Count => Rows.Count();
        public bool IsSynchronized => true;
        public object SyncRoot => Mirror;

        protected ITable Mirror { get; }
    }
}
