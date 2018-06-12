using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Ultz.BeagleFramework.Common.Models
{
    public abstract class DataModel : IDataContainer
    {
        internal Row Row { get; set; }
        /// <inheritdoc />
        public void SaveChanges()
        {
            if (Row == null)
                throw new InvalidOperationException("This object doesn't belong to a table yet, please Add it to a table before calling this method.");
            Row.Replace(this);
        }

        /// <inheritdoc />
        public void Refresh()
        {
            if (Row == null)
                throw new InvalidOperationException("This object doesn't belong to a table yet, please Add it to a table before calling this method.");
            var columns = GetType().GetProperties().ToDictionary(
                x =>
                    x.Name.ToNamePreservation(),
                x => x);
            foreach (var col in columns)
            {
                col.Value.SetValue(this,
                    JsonConvert.DeserializeObject(Row[col.Key].Value, col.Value.PropertyType));
            }
        }
    }
}