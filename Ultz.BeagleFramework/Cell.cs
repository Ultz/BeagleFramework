namespace Ultz.BeagleFramework
{
    /// <summary>
    /// Represents a field in a table.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Returns the <see cref="Column"/> that this <see cref="Cell"/> belongs to.
        /// </summary>
        public Column Column { get; }
        /// <summary>
        /// Returns the <see cref="Row"/> that this <see cref="Cell"/> belongs to.
        /// </summary>
        public Row Row { get; }

        /// <summary>
        /// Creates an instance of <see cref="Cell"/> from a Column and Row
        /// </summary>
        /// <param name="col">the column that the cell belongs to</param>
        /// <param name="row">the row that the cell belongs to</param>
        public Cell(Column col, Row row)
        {
            Column = col;
            Row = row;
        }

        /// <summary>
        /// Gets or sets the value of this <see cref="Cell"/>
        /// </summary>
        public string Value
        {
            get => Row.GetValue(Column.Index);
            set => Row.SetValue(Column.Index, value);
        }
    }
}