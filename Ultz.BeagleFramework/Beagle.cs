using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultz.BeagleFramework.Common.Models;

namespace Ultz.BeagleFramework
{
    public static class Beagle
    {
        public static DataContext<T> CreateContext<T>(IStorageEngine engine) where T : DataStore
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            var context = new DataContext<T>
            {
                Store = Activator.CreateInstance<T>(),
                Engine = engine
            };
            context.Store.AssignTo(context);
            context.Store.Fill();
            return context;
        }
        internal static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
            {
                return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }

        public static Row Add(this ITable table, params string[] values)
        {
            return table.Add(values);
        }

        public static ITable CrateTable(this IStorageEngine engine, string name, params string[] keys)
        {
            return engine.CreateTable(name, keys);
        }
    }
}
