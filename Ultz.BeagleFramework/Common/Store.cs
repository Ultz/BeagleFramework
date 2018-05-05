using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;
using Ultz.BeagleFramework.Common.Models;

namespace Ultz.BeagleFramework.Common
{
    public static class Store
    {
        public static ITable CreateTable<T>(this IStorageEngine engine,string name)
        {
            var type = typeof(T);
            if (!(type.GetCustomAttributes(
                typeof(ModelAttribute), true
            ).FirstOrDefault() is ModelAttribute))
                throw new InvalidModelException();
            var columns = type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() != null).Select(x =>
                    ((ColumnAttribute) x.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault()).Name)
                .ToArray();
            return engine.CreateTable(name, columns);
        }

        public static Row Put<T>(this ITable table,T obj)
        {
            var type = typeof(T);
            if (!(type.GetCustomAttributes(
                typeof(ModelAttribute), true
            ).FirstOrDefault() is ModelAttribute attribute))
                throw new InvalidModelException();
            var columns = type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() != null).ToDictionary(
                    x =>
                        ((ColumnAttribute) x.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault()).Name,
                    x => x);
            var values = columns.Keys.Select(x =>
               JsonConvert.SerializeObject(columns.Values.FirstOrDefault(y =>
                        ((ColumnAttribute) y.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault())
                        .Name == x)
                    .GetValue(obj)));
            return table.Put(values);
        }

        internal static T ToObject<T>(Row row)
        {
            var type = typeof(T);
            if (!(type.GetCustomAttributes(
                typeof(ModelAttribute), true
            ).FirstOrDefault() is ModelAttribute attribute))
                throw new InvalidModelException();
            var columns = type.GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() != null).ToDictionary(
                    x =>
                        ((ColumnAttribute) x.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault()).Name,
                    x => x);
            var instance = Activator.CreateInstance<T>();
            foreach (var col in columns)
            {
                col.Value.SetValue(instance,
                    JsonConvert.DeserializeObject(row[col.Key].Value, col.Value.PropertyType));
            }

            return instance;
        }

        public static IEnumerable<T> ConvertAll<T>(this IEnumerable<Row> enumerable)
        {
            return enumerable.Select(ToObject<T>);
        }
    }
}