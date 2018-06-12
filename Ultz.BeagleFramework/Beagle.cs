using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultz.BeagleFramework.Common.Models;

namespace Ultz.BeagleFramework
{
    public static class Beagle
    {
        /// <summary>
        /// Creates a <see cref="DataContext{T}"/> from an <see cref="IStorageEngine"/> instance.
        /// </summary>
        /// <param name="engine">the engine to attach to the context</param>
        /// <typeparam name="T">a <see cref="DataStore"/> that will be filled with data from the <see cref="IStorageEngine"/></typeparam>
        /// <returns>a <see cref="DataContext{T}"/></returns>
        /// <exception cref="ArgumentNullException">thrown if the engine parameter is null</exception>
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

        /// <summary>
        /// Adds a row to a table with the given values.
        /// </summary>
        /// <param name="table">the target table</param>
        /// <param name="values">the values to add</param>
        /// <returns></returns>
        public static Row Add(this ITable table, params string[] values)
        {
            return table.Add(values);
        }

        /// <summary>
        /// Creates a table with the given name and columns
        /// </summary>
        /// <param name="engine">the engine</param>
        /// <param name="name">the name of the table</param>
        /// <param name="keys">the columns to initialize the table with</param>
        /// <returns>the created table</returns>
        public static ITable CreateTable(this IStorageEngine engine, string name, params string[] keys)
        {
            return engine.CreateTable(name, keys);
        }
    }
}
