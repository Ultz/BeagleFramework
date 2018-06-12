using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultz.BeagleFramework
{
    /// <summary>
    /// Represents a table in a database
    /// </summary>
    public interface ITable : IDisposable, IEnumerable
    {
        /// <summary>
        /// Gets a row at the given index
        /// </summary>
        /// <param name="index">the index of the row to get</param>
        Row this[int index] { get; }
        /// <summary>
        /// The name of this <see cref="ITable"/>
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the columns in this <see cref="ITable"/>
        /// </summary>
        string[] Columns { get; }
        /// <summary>
        /// Returns an enumerable for all of the rows in this table.
        /// </summary>
        IEnumerable<Row> Rows { get; }
        /// <summary>
        /// Attempts to add a column to this table.
        /// </summary>
        /// <param name="column">the desired column name</param>
        /// <returns>true if the operation was successful; otherwise false.</returns>
        bool TryAddColumn(string column);
        /// <summary>
        /// Attempts to remove a column to this table.
        /// </summary>
        /// <param name="column">the target column name</param>
        /// <returns>true if the operation was successful; otherwise false.</returns>
        bool TryRemoveColumn(string column);
        /// <summary>
        /// Adds a row to this table.
        /// </summary>
        /// <param name="row">values for the new row</param>
        /// <returns>the created <see cref="Row"/></returns>
        [Obsolete("To be consistent with naming, Put is being renamed to Add. Put will be removed in a future release.")]
        Row Put(IEnumerable<string> row);
        /// <summary>
        /// Adds a row to this table.
        /// </summary>
        /// <param name="row">values for the new row</param>
        /// <returns>the created <see cref="Row"/></returns>
        Row Add(IEnumerable<string> row);
        /// <summary>
        /// Removes a row from this table.
        /// </summary>
        /// <param name="row">the row to remove</param>
        void Remove(Row row);
        /// <summary>
        /// Removes the row at the given index from this table
        /// </summary>
        /// <param name="row">the index of the target row.</param>
        void Remove(int row);
        /// <summary>
        /// Forces the table to refresh its data, overriding any cache
        /// </summary>
        void Refresh();
    }
}