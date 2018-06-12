namespace Ultz.BeagleFramework.Common.Models
{
    public interface IDataContainer
    {
        /// <summary>
        /// Saves all changes to the underlying source
        /// </summary>
        void SaveChanges();
        /// <summary>
        /// Gets all changes from the underlying source, if external modifications have been made
        /// </summary>
        void Refresh();
    }
}