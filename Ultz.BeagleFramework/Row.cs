using System.Collections;
using System.Collections.Generic;

namespace Ultz.BeagleFramework
{
    public abstract class Row : IEnumerable<Cell>
    {
        public int Index => GetIndex();
        public ITable Table => Engine.GetTable(GetTable());

        protected abstract IStorageEngine Engine { get; }
        
        public Cell this[int col] => new Cell(Engine.GetColumn(GetTable(), col), this);
        public Cell this[string col] => new Cell(Engine.GetColumn(GetTable(), col), this);

        protected abstract string GetTable();
        protected abstract int GetIndex();
        internal abstract string GetValue(int col);
        internal abstract void SetValue(int col, string val);
        public IEnumerator<Cell> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<Cell>
        {
            private Row _row;
            private int _current = -1;
            public Enumerator(Row row)
            {
                _row = row;
            }

            public bool MoveNext()
            {
                _current++;
                if (_row.Table.Columns.Length <= _current)
                    return false;
                Current = _row[_current];
                return true;
            }

            public void Reset()
            {
                _current = -1;
            }

            public Cell Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}