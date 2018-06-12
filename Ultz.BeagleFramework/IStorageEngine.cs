using System;
using System.Collections.Generic;

namespace Ultz.BeagleFramework
{
    /// <summary>
    /// Represents a storage engine used to access a database
    /// </summary>
    public interface IStorageEngine : IDisposable
    {
        /// <summary>
        /// A unique ID for this implementation of <see cref="IStorageEngine"/>
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Initializes this <see cref="IStorageEngine"/> with the given initialization parameters.
        /// </summary>
        /// <param name="initializationParameters">the parameters used to initialize this storage engine</param>
        void Initialize(string initializationParameters);
        /// <summary>
        /// Returns an <see cref="ITable"/> representing a table with the given name, from the underlying source
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ITable GetTable(string name);
        /// <summary>
        /// Returns a <see cref="Column"/> from the given table name and index.
        /// </summary>
        /// <param name="table">the table to get the column from</param>
        /// <param name="col">the column's index</param>
        /// <returns>a <see cref="Column"/> from the given table name and index.</returns>
        Column GetColumn(string table, int col);
        /// <summary>
        /// Returns a <see cref="Column"/> from the given table name and column name.
        /// </summary>
        /// <param name="table">the table to get the column from</param>
        /// <param name="col">the column's name</param>
        /// <returns>a <see cref="Column"/> from the given table name and column name.</returns>
        Column GetColumn(string table, string col);
        /// <summary>
        /// Creates a table with the given name and columns
        /// </summary>
        /// <param name="table">the desired table name</param>
        /// <param name="cols">the columns to initialize the table with</param>
        /// <returns>the created table</returns>
        ITable CreateTable(string table, string[] cols);
        /// <summary>
        /// Deletes a table with the given name
        /// </summary>
        /// <param name="table">the table name</param>
        void DeleteTable(string table);
    }
}