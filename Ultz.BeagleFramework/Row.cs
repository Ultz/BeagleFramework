using System.Collections;
using System.Collections.Generic;

namespace Ultz.BeagleFramework
{
    public abstract class Row : IEnumerable<Cell>
    {
        /// <summary>
        /// Gets the index of this row in its table.
        /// </summary>
        public int Index => GetIndex();
        /// <summary>
        /// Gets the table that this row is in.
        /// </summary>
        public ITable Table => Engine.GetTable(GetTable());

        protected abstract IStorageEngine Engine { get; }
        /// <summary>
        /// Gets the cell in the column at the given index.
        /// </summary>
        /// <param name="col">the index of the target column</param>
        public Cell this[int col] => new Cell(Engine.GetColumn(GetTable(), col), this);
        /// <summary>
        /// Gets the cell in the column with the given name.
        /// </summary>
        /// <param name="col">the name of the target column</param>
        public Cell this[string col] => new Cell(Engine.GetColumn(GetTable(), col), this);

        protected abstract string GetTable();
        protected abstract int GetIndex();
        protected internal abstract string GetValue(int col);
        protected internal abstract void SetValue(int col, string val);
        /// <summary>
        /// Returns an enumerator for all of the <see cref="Cell"/>s in this <see cref="Row"/>
        /// </summary>
        /// <returns>an enumerator for all of the <see cref="Cell"/>s in this <see cref="Row"/></returns>
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