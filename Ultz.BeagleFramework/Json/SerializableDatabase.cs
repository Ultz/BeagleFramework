using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Ultz.BeagleFramework.Json
{
    public class SerializableDatabase
    {
        internal JsonStorageEngine _for;

        public List<SerializableDatabaseEntry> Tables { get; set; }

        public SerializableDatabase For(JsonStorageEngine engine)
        {
            _for = engine;
            return this;
        }

        public ITable GetTable(string name)
        {
            if (Tables.Count(x => string.Equals(x.TableName, name, StringComparison.CurrentCultureIgnoreCase)) != 0)
                return new JsonTable(Path.Combine(_for._path, Tables.First(x => x.TableName == name.ToUpper()).File),_for);

            throw new ArgumentException("Table not found: " + name.ToUpper(), nameof(name));
        }

        public SerializableDatabase RemoveTable(string name)
        {
            if (Tables.Count(x => x.TableName == name.ToUpper()) == 0)
                throw new ArgumentException("Table not found: " + name.ToUpper(), nameof(name));
            {
                Tables.RemoveAt(Tables.FindIndex(x => x.TableName == name.ToUpper()));
                return this;
            }
        }

        public SerializableDatabase CreateTable(string name, List<string> columns = null)
        {
            var file = Path.Combine(_for._path,
                           BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(name))).ToLower()
                               .Replace("-", "")) + ".json";
            new SerializableTable
            {
                Name = name.ToUpper(),
                Columns = columns ?? new List<string>(),
                Rows = new List<List<string>>()
            }.New(file);
            Tables.Add(new SerializableDatabaseEntry {File = file, TableName = name.ToUpper()});
            return this;
        }

        public void Set()
        {
            File.WriteAllText(_for._filepath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}