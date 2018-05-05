using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ultz.BeagleFramework.Common.Models
{
    public class ModelRowEnumerable<T> : IEnumerable<T>
    {
        private IEnumerable<Row> _rows;
        public ModelRowEnumerable(IEnumerable<Row> rows)
        {
            _rows = rows;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(_rows.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<T>
        {
            private IEnumerator<Row> _rows;
            public Enumerator(IEnumerator<Row> rows)
            {
                _rows = rows;
            }
            
            public bool MoveNext()
            {
                if (!_rows.MoveNext())
                    return false;
                Current = Store.ToObject<T>(_rows.Current);
                return true;
            }

            public void Reset()
            {
                _rows.Reset();
            }

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _rows.Dispose();
            }
        }
    }
}