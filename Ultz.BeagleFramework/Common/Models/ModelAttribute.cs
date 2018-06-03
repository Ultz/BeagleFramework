using System;

namespace Ultz.BeagleFramework.Common.Models
{
    /// <summary>
    /// This attribute enables a class to be used as a model for the Store class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [Obsolete("ModelAttribute is deprecated, inherit DataModel instead",true)]
    public class ModelAttribute : Attribute
    {
        
    }
}