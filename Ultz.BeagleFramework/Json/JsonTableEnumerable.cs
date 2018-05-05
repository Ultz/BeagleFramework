using System.Collections;
using System.Collections.Generic;

namespace Ultz.BeagleFramework.Json
{
    public class JsonTableEnumerable : IEnumerable<ITable>
    {
        private readonly JsonStorageEngine _parent;

        public JsonTableEnumerable(JsonStorageEngine parent)
        {
            _parent = parent;
        }

        public IEnumerator<ITable> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<ITable>
        {
            private int _current = -1;
            private SerializableDatabase _database;
            private JsonTableEnumerable _parent;

            public Enumerator(JsonTableEnumerable parent)
            {
                _parent = parent;
                _database = _parent._parent.Get();
            }

            public bool MoveNext()
            {
                _current++;
                if (_current >= _database.Tables.Count) return false;
                Current = new JsonTable(_database.Tables[_current].File,_parent._parent);
                return true;
            }

            public void Reset()
            {
                Current = null;
                _current = -1;
            }

            public ITable Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _parent = null;
                _database = null;
            }
        }
    }
}