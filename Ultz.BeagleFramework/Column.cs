namespace Ultz.BeagleFramework
{
    /// <summary>
    /// Represents a column in a table
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Creates a Column with the given name and index
        /// </summary>
        /// <param name="index">the index of the column</param>
        /// <param name="name">the name of the column</param>
        public Column(int index, string name)
        {
            Index = index;
            Name = name;
        }

        /// <summary>
        /// The index of this <see cref="Column"/> in its table
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// The name of this <see cref="Column"/>
        /// </summary>
        public string Name { get; }
    }
}