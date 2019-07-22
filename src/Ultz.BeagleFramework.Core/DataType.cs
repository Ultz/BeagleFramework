#region

using System.Data;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public class DataType
    {
        public static DataType AnsiString => new DataType {Type = DbType.AnsiString};
        public static DataType Binary => new DataType {Type = DbType.Binary};
        public static DataType Byte => new DataType {Type = DbType.Byte};
        public static DataType Boolean => new DataType {Type = DbType.Boolean};
        public static DataType Currency => new DataType {Type = DbType.Currency};
        public static DataType Date => new DataType {Type = DbType.Date};
        public static DataType DateTime => new DataType {Type = DbType.DateTime};
        public static DataType Decimal => new DataType {Type = DbType.Decimal};
        public static DataType Double => new DataType {Type = DbType.Double};
        public static DataType Guid => new DataType {Type = DbType.Guid};
        public static DataType Int16 => new DataType {Type = DbType.Int16};
        public static DataType Int32 => new DataType {Type = DbType.Int32};
        public static DataType Int64 => new DataType {Type = DbType.Int64};
        public static DataType Object => new DataType {Type = DbType.Object};
        public static DataType SByte => new DataType {Type = DbType.SByte};
        public static DataType Single => new DataType {Type = DbType.Single};
        public static DataType String => new DataType {Type = DbType.String};
        public static DataType Time => new DataType {Type = DbType.Time};
        public static DataType UInt16 => new DataType {Type = DbType.UInt16};
        public static DataType UInt32 => new DataType {Type = DbType.UInt32};
        public static DataType UInt64 => new DataType {Type = DbType.UInt64};
        public static DataType VarNumeric => new DataType {Type = DbType.VarNumeric};
        public static DataType Xml => new DataType {Type = DbType.Xml};
        public static DataType DateTime2 => new DataType {Type = DbType.DateTime2};
        public static DataType DateTimeOffset => new DataType {Type = DbType.DateTimeOffset};

        public static DataType AnsiStringFixedLength(int length)
        {
            return new DataType {Type = DbType.AnsiStringFixedLength, Length = length};
        }

        public static DataType StringFixedLength(int length)
        {
            return new DataType {Type = DbType.StringFixedLength, Length = length};
        }

        public DbType Type { get; set; }
        public int Length { get; set; } = -1;
    }
}
