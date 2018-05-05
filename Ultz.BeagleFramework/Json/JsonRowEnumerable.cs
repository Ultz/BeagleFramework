using System.Collections;
using System.Collections.Generic;

namespace Ultz.BeagleFramework.Json
{
    public class JsonRowEnumerable : IEnumerable<Row>
    {
        private readonly JsonTable _parent;

        public JsonRowEnumerable(JsonTable parent)
        {
            _parent = parent;
        }

        public IEnumerator<Row> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<Row>
        {
            private int _current = -1;
            private JsonRowEnumerable _parent;
            private SerializableTable _table;

            public Enumerator(JsonRowEnumerable parent)
            {
                _parent = parent;
                _table = _parent._parent.Get();
            }

            public bool MoveNext()
            {
                _current++;
                if (_current >= _table.Rows.Count) return false;
                Current = new JsonRow(_table, _current,_parent._parent._engine);
                return true;
            }

            public void Reset()
            {
                Current = null;
                _current = -1;
            }

            public Row Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _parent = null;
                _table = null;
            }
        }
    }
}