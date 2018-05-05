using System;
using System.Data;

namespace Ultz.BeagleFramework.Sql
{
    public class SqlRow : Row
    {
        private SqlTable _parent;
        private DataRow _row;
        public SqlRow(SqlTable @for,DataRow @base,IStorageEngine @engine)
        {
            _parent = @for;
            _row = @base;
            Engine = @engine;
        }

        protected override IStorageEngine Engine { get; }

        protected override string GetTable()
        {
            return _parent.Name;
        }

        protected override int GetIndex()
        {
            return _parent._table.Rows.IndexOf(_row);
        }

        internal override string GetValue(int col)
        {
            return (string)_row[col+1];
        }

        internal override void SetValue(int col, string val)
        {
            _row.BeginEdit();
            _row[col] = val;
            _row.AcceptChanges();
            _parent.CreateCommands();
            _parent._adapter.Update(_parent._table);
        }

        internal void Remove()
        {
            _row.Delete();
            _parent.CreateCommands();
            _parent._adapter.Update(_parent._table);
        }
    }
}