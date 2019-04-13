#region

using System;
using Ultz.BeagleFramework.Core.Structure;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public interface IStorageEngine : IDisposable
    {
        IQuery Execute(Query query);
    }
}
