namespace Ultz.BeagleFramework.Json
{
    public class JsonRow : Row
    {
        private readonly int _row;
        private readonly SerializableTable _table;

        public JsonRow(SerializableTable table, int index, IStorageEngine engine)
        {
            _table = table;
            _row = index;
            Engine = engine;
        }

        protected override IStorageEngine Engine { get; }

        protected override string GetTable()
        {
            return _table.Name;
        }

        protected override int GetIndex()
        {
            return _row;
        }

        protected internal override string GetValue(int col)
        {
            return _table.Rows[_row][col];
        }

        protected internal override void SetValue(int col, string val)
        {
            _table.Rows[_row][col] = val;
            _table.Set();
        }
    }
}
