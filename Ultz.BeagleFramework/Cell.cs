﻿namespace Ultz.BeagleFramework
{
    public class Cell
    {
        public Column Column { get; }
        public Row Row { get; }
        private readonly Row _row;

        public Cell(Column col, Row row)
        {
            Column = col;
            Row = row;
        }

        public string Value
        {
            get => Row.GetValue(Column.Index);
            set => Row.SetValue(Column.Index, value);
        }
    }
}