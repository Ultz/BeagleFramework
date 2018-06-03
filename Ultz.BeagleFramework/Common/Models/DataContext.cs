namespace Ultz.BeagleFramework.Common.Models
{
    public sealed class DataContext<T> where T:DataStore
    {
        public T Store { get; internal set; }
        public IStorageEngine Engine { get; internal set; }
    }
}