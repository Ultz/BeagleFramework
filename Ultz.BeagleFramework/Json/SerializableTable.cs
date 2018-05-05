using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Ultz.BeagleFramework.Json
{
    public class SerializableTable
    {
        private string _for;
        public string Name { get; set; }
        public List<string> Columns { get; set; }
        public List<List<string>> Rows { get; set; }

        public SerializableTable For(JsonTable table)
        {
            _for = table._filepath;
            return this;
        }

        public SerializableTable New(string file)
        {
            _for = file;
            Set();
            return this;
        }

        public void Set()
        {
            File.WriteAllText(_for, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public SerializableTable AddRow(List<string> row)
        {
            Rows.Add(row);
            return this;
        }

        public SerializableTable AddColumn(string column, out bool result)
        {
            if (Columns.Contains(column.ToUpper()))
            {
                result = false;
            }
            else
            {
                Columns.Add(column.ToUpper());
                result = true;
            }

            return this;
        }

        public SerializableTable RemoveRow(int index)
        {
            Rows.RemoveAt(index);
            return this;
        }

        public SerializableTable RemoveColumn(string column, out bool result)
        {
            result = Columns.Contains(column.ToUpper()) && Columns.Remove(column.ToUpper());

            return this;
        }
    }
}