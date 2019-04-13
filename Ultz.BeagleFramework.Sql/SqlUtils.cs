#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Ultz.BeagleFramework.Core;
using Constraint = Ultz.BeagleFramework.Core.Constraint;

#endregion

namespace Ultz.BeagleFramework.Sql
{
    public static class SqlUtils
    {
        private static readonly Dictionary<Type, DbType> _typeMap = new Dictionary<Type, DbType>
        {
            {typeof(byte), DbType.Byte},
            {typeof(sbyte), DbType.SByte},
            {typeof(short), DbType.Int16},
            {typeof(ushort), DbType.UInt16},
            {typeof(int), DbType.Int32},
            {typeof(uint), DbType.UInt32},
            {typeof(long), DbType.Int64},
            {typeof(ulong), DbType.UInt64},
            {typeof(float), DbType.Single},
            {typeof(double), DbType.Double},
            {typeof(decimal), DbType.Decimal},
            {typeof(bool), DbType.Boolean},
            {typeof(string), DbType.String},
            {typeof(char), DbType.StringFixedLength},
            {typeof(Guid), DbType.Guid},
            {typeof(DateTime), DbType.DateTime},
            {typeof(DateTimeOffset), DbType.DateTimeOffset},
            {typeof(byte[]), DbType.Binary},
            {typeof(byte?), DbType.Byte},
            {typeof(sbyte?), DbType.SByte},
            {typeof(short?), DbType.Int16},
            {typeof(ushort?), DbType.UInt16},
            {typeof(int?), DbType.Int32},
            {typeof(uint?), DbType.UInt32},
            {typeof(long?), DbType.Int64},
            {typeof(ulong?), DbType.UInt64},
            {typeof(float?), DbType.Single},
            {typeof(double?), DbType.Double},
            {typeof(decimal?), DbType.Decimal},
            {typeof(bool?), DbType.Boolean},
            {typeof(char?), DbType.StringFixedLength},
            {typeof(Guid?), DbType.Guid},
            {typeof(DateTime?), DbType.DateTime},
            {typeof(DateTimeOffset?), DbType.DateTimeOffset},
            {typeof(TimeSpan?), DbType.Time},
            {typeof(object), DbType.Object}
        };

        public static DataType ConvertToDataType(DbColumn col)
        {
            return new DataType
            {
                Type = _typeMap.ContainsKey(col.DataType) ? _typeMap[col.DataType] : DbType.Object,
                Length = col.ColumnSize ?? -1
            };
        }

        public static IEnumerable<IDataRecord> Enumerate(this IDataReader reader)
        {
            while (reader.Read()) yield return reader;
        }

        public static IEnumerable<Constraint> GetConstraints(DbColumn dbColumn)
        {
            if (dbColumn.IsUnique.HasValue && dbColumn.IsUnique.Value) yield return new Constraint.Unique();

            if (dbColumn.AllowDBNull.HasValue && !dbColumn.AllowDBNull.Value) yield return new Constraint.NotNull();

            if (dbColumn.IsKey.HasValue && dbColumn.IsKey.Value) yield return new Constraint.PrimaryKey();
        }
    }
}
