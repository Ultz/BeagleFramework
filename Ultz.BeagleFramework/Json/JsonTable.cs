using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Ultz.BeagleFramework.Json
{
    public class JsonTable : ITable
    {
        internal string _filepath;
        internal IStorageEngine _engine;

        public JsonTable(string file,IStorageEngine engine)
        {
            _filepath = file;
            _engine = engine;
        }

        public Row this[int index] => Rows.ElementAt(index);

        public string Name => Get().Name;
        public string[] Columns => Get().Columns.ToArray();
        public IEnumerable<Row> Rows => new JsonRowEnumerable(this);

        public bool TryAddColumn(string column)
        {
            Get().AddColumn(column.ToUpper(), out var result).Set();
            return result;
        }

        public bool TryRemoveColumn(string column)
        {
            Get().RemoveColumn(column.ToUpper(), out var result).Set();
            return result;
        }

        public Row Put(IEnumerable<string> row)
        {
            return Add(row);
        }

        public Row Add(IEnumerable<string> row)
        {
            var enumerable = row.ToList();
            Get().AddRow(enumerable).Set();
            return new JsonRow(Get(), Get().Rows.IndexOf(enumerable), _engine);
        }

        public void Remove(Row row)
        {
            Get().RemoveRow(row.Index).Set();
        }

        public void Remove(int row)
        {
            Get().RemoveRow(row).Set();
        }

        public void Refresh()
        {
            
        }

        public SerializableTable Get()
        {
            return JsonConvert.DeserializeObject<SerializableTable>(File.ReadAllText(_filepath,
                ((JsonStorageEngine) _engine)._encoding)).For(this);
        }

        public IEnumerator<Row> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            // lol there's nothing to dispose
        }
    }
}