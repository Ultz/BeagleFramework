namespace Ultz.BeagleFramework.Common.Models
{
    /// <summary>
    /// Represents a data access context with modelled tables and a storage engine.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DataContext<T> where T:DataStore
    {
        /// <summary>
        /// Gets the <see cref="DataStore"/> attached to this instance
        /// </summary>
        public T Store { get; internal set; }
        /// <summary>
        /// Gets the <see cref="IStorageEngine"/> attached to this instance
        /// </summary>
        public IStorageEngine Engine { get; internal set; }
    }
}