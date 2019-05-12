using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Ultz.BeagleFramework.Core;
using Ultz.BeagleFramework.Sql;
using Constraint = Ultz.BeagleFramework.Core.Constraint;
using DbTypeMapEntry = System.Tuple<System.Type, System.Data.DbType, System.Data.SqlDbType>;

namespace Ultz.BeagleFramework.SqlServer
{
    public class SqlStorageEngine : SqlEngineBase
    {
        public SqlStorageEngine(string connectionString) : base(connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        public SqlStorageEngine(SqlConnection connection) : base(connection)
        {
        }

        public override DbCommand Translate(Query query)
        {
            var values = new List<SqlParameter>();
            var str = ToString(query, values);
            Debug.WriteLine("sql: " + str);
            var cmd = new SqlCommand(str, (SqlConnection) Connection);
            values.ForEach(x => cmd.Parameters.Add(x));
            return cmd;
        }

        public string ToString(Query query, List<SqlParameter> values)
        {
            var str = "";
            foreach (var clause in query.Clauses)
            {
                switch (clause)
                {
                    case Clause.Value value:
                        values.Add(new SqlParameter("Parameter" + values.Count, value.Item));
                        str += "@Parameter" + (values.Count - 1);
                        break;
                    case Clause.RowNumber _:
                        str += "ROW_NUMBER()";
                        break;
                    case Clause.TableName tblName:
                        str += "[" + tblName.Table + "]";
                        break;
                    case Clause.ColumnName colName:
                        str += "[" + colName.Column + "]";
                        break;
                    case Clause.In _:
                        str += "IN";
                        break;
                    case Clause.Or _:
                        str += "OR";
                        break;
                    case Clause.And _:
                        str += "AND";
                        break;
                    case Clause.Not _:
                        str += "NOT";
                        break;
                    case Clause.Set _:
                        str += "SET";
                        break;
                    case Clause.From _:
                        str += "FROM";
                        break;
                    case Clause.Into _:
                        str += "INTO";
                        break;
                    case Clause.Like _:
                        str += "LIKE";
                        break;
                    case Clause.Equal _:
                        str += "=";
                        break;
                    case Clause.Where _:
                        str += "WHERE";
                        break;
                    case Clause.Delete _:
                        str += "DELETE";
                        break;
                    case Clause.Insert _:
                        str += "INSERT";
                        break;
                    case Clause.Select _:
                        str += "SELECT";
                        break;
                    case Clause.Update _:
                        str += "UPDATE";
                        break;
                    case Clause.Values vals:
                        str += "VALUES(";
                        var param = new List<string>();
                        foreach (var item in vals.Items)
                        {
                            values.Add(new SqlParameter("Parameter" + values.Count, item));
                            param.Add("@Parameter" + (values.Count - 1));
                        }

                        str += string.Join(", ", param);
                        str += ")";
                        break;
                    case Clause.Between _:
                        str += "BETWEEN";
                        break;
                    case Clause.Wildcard _:
                        str += "*";
                        break;
                    case Clause.ValuesGroup vals:
                        str += string.Join
                        (
                            ", ", vals.Items.Select
                            (
                                x =>
                                {
                                    var str2 = "VALUES(";
                                    var param2 = new List<string>();
                                    foreach (var item in vals.Items)
                                    {
                                        values.Add(new SqlParameter("Parameter" + values.Count, item));
                                        param2.Add("@Parameter" + (values.Count - 1));
                                    }

                                    str2 += string.Join(", ", param2);
                                    str2 += ")";
                                    return str2;
                                }
                            )
                        );
                        break;
                    case Clause.LessThan lt:
                        str += "<" + (lt.OrEqualTo ? "=" : "");
                        break;
                    case Clause.GreaterThan gt:
                        str += ">" + (gt.OrEqualTo ? "=" : "");
                        break;
                    case Clause.NotEqual _:
                        str += "!=";
                        break;
                    case Clause.ColumnNames cols:
                        if (cols.Group)
                            str += "(";
                        str += string.Join(", ", cols.Columns);
                        if (cols.Group)
                            str += ")";
                        break;
                    case Clause.PseudoColumnGroup cols:
                        str += "(" + string.Join
                               (
                                   ", ",
                                   GetWithConstraints
                                   (
                                       cols.Columns.Select
                                           (x => "[" + x.Name + "] " + GetType(x.Type)), cols.Columns, values
                                   )
                               ) + ")";
                        break;
                    case Clause.Drop _:
                        str += "DROP";
                        break;
                    case Clause.Table _:
                        str += "TABLE";
                        break;
                    case Clause.Create _:
                        str += "CREATE";
                        break;
                }

                str += " ";
            }

            return str;
        }

        private IEnumerable<string> GetWithConstraints
            (IEnumerable<string> q, Clause.PseudoColumn[] cols, List<SqlParameter> values)
        {
            var strs = q.ToArray();
            for (var index = 0; index < cols.Length; index++)
            {
                var col = cols[index];
                foreach (var con in col.Constraints)
                {
                    switch (con)
                    {
                        case Constraint.Check ch:
                            strs[index] += " CHECK (" + ToString(ch.Condition, values) + ")";
                            break;
                        case Constraint.Default df:
                            strs[index] += " DEFAULT @Parameter" + values.Count;
                            values.Add(new SqlParameter("Parameter" + values.Count, df.Value));
                            break;
                        case Constraint.ForeignKey fk:
                            strs[index] += " FOREIGN KEY REFERENCES " + fk.ForeignTable + "(" + fk.ForeignColumn + ")";
                            break;
                        case Constraint.NotNull _:
                            strs[index] += " NOT NULL";
                            break;
                        case Constraint.PrimaryKey _:
                            strs[index] += " PRIMARY KEY";
                            break;
                        case Constraint.Unique _:
                            strs[index] += " UNIQUE";
                            break;
                    }
                }
            }

            return strs;
        }

        private static string GetType(DataType type)
        {
            switch (type.Type)
            {
                case DbType.AnsiString:
                    return "varchar("+(type.Length == -1 ? "max" : type.Length.ToString())+")";
                case DbType.AnsiStringFixedLength:
                    if (type.Length < 1)
                        throw new ArgumentOutOfRangeException(nameof(type.Length),"A length must be specified for a fixed-length string.");
                    return "char("+type.Length+")";
                case DbType.Binary:
                    return "varbinary("+(type.Length == -1 ? "max" : type.Length.ToString())+")";
                case DbType.Boolean:
                    return "bit";
                case DbType.Byte:
                    return "tinyint";
                case DbType.Currency:
                    return "money";
                case DbType.Date:
                    return "date";
                case DbType.DateTime:
                    return "datetime";
                case DbType.DateTime2:
                    return "datetime2";
                case DbType.DateTimeOffset:
                    return "datetimeoffset";
                case DbType.Decimal:
                    return "decimal";
                case DbType.Double:
                    return "float";
                case DbType.Guid:
                    return "uniqueidentifier";
                case DbType.Int16:
                    return "smallint";
                case DbType.Int32:
                    return "int";
                case DbType.Int64:
                    return "bigint";
                case DbType.Object:
                    return "sql_variant";
                case DbType.SByte:
                    return "smallint";
                case DbType.Single:
                    return "real";
                case DbType.String:
                    return "nvarchar("+(type.Length == -1 ? "max" : type.Length.ToString())+")";
                case DbType.StringFixedLength:
                    if (type.Length < 1)
                        throw new ArgumentOutOfRangeException(nameof(type.Length),"A length must be specified for a fixed-length string.");
                    return "nchar("+type.Length+")";
                case DbType.Time:
                    return "time";
                case DbType.UInt16:
                    return "int";
                case DbType.UInt32:
                    return "bigint";
                case DbType.UInt64:
                    return "decimal(20,0)";
                case DbType.VarNumeric:
                    throw new ArgumentException("Couldn't map DbType VarNumeric", nameof(type.Type));
                case DbType.Xml:
                    return "xml";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
