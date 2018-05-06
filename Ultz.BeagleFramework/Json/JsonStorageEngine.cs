using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ultz.BeagleFramework.Json
{
    public class JsonStorageEngine : IStorageEngine
    {
        internal Encoding _encoding;
        internal string _filepath;
        internal string _path;
        public List<FileStream> Locks { get; set; }

        public JsonStorageEngine()
        {
            
        }
        
        public JsonStorageEngine(Encoding encoding, string rootDirectoryPath)
        {
            _path = rootDirectoryPath;
            _filepath = Path.Combine(_path, "store.json");
            _encoding = encoding;
            Locks = new List<FileStream>();
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            if (!File.Exists(_filepath))
                File.WriteAllText(_filepath,
                    JsonConvert.SerializeObject(new SerializableDatabase
                    {
                        Tables = new List<SerializableDatabaseEntry>(),
                        _for = this
                    }));
        }
        
        public static Dictionary<string, Encoding> Encoding => new Dictionary<string, Encoding>
        {
            {"UTF32", System.Text.Encoding.UTF32},
            {"UTF16", System.Text.Encoding.Unicode},
            {"UTF8", System.Text.Encoding.UTF8},
            {"UTF7", System.Text.Encoding.UTF7},
            {"OS_Default", System.Text.Encoding.Default},
            {"UTF16_Big_Endian", System.Text.Encoding.BigEndianUnicode},
            {"ASCII", System.Text.Encoding.ASCII}
        };


        public void Dispose()
        {
            // unlock files
            Locks.ForEach(x => x.Dispose());
        }

        public string Id => "json";

        public void Initialize(string initializationParameters)
        {
            var jinit = JsonConvert.DeserializeObject<JsonStorageParameters>(initializationParameters);
            _path = jinit.Store;
            _filepath = Path.Combine(_path, "store.json");
            _encoding = Encoding[jinit.Encoding];
            Locks = new List<FileStream>();
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            if (!File.Exists(_filepath))
                File.WriteAllText(_filepath,
                    JsonConvert.SerializeObject(new SerializableDatabase
                    {
                        Tables = new List<SerializableDatabaseEntry>(),
                        _for = this
                    }));
        }

        public ITable GetTable(string name)
        {
            return Get().GetTable(name);
        }

        public IEnumerable<ITable> Tables => new JsonTableEnumerable(this);

        public Column GetColumn(string table, int col)
        {
            var tbl = Get().GetTable(table);
            return new Column(col, tbl.Columns[col]);
        }

        public Column GetColumn(string table, string col)
        {
            var tbl = Get().GetTable(table);
            if (!tbl.Columns.Contains(table))
                throw new ArgumentException("Column not found: " + table + ", " + col, "col");
            return new Column(Array.IndexOf(tbl.Columns, col.ToUpper()), col.ToUpper());
        }

        public ITable CreateTable(string table, string[] cols)
        {
            Get().CreateTable(table, cols.ToList()).Set();
            return Get().GetTable(table);
        }

        public void DeleteTable(string table)
        {
            Get().RemoveTable(table).Set();
        }

        public SerializableDatabase Get()
        {
            return JsonConvert.DeserializeObject<SerializableDatabase>(File.ReadAllText(_filepath, _encoding))
                .For(this);
        }
    }
}