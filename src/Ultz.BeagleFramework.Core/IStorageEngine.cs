#region

using System;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public interface IStorageEngine : IDisposable
    {
        IQuery Execute(Query query);
    }
}
