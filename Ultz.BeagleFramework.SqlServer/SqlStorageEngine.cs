using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Ultz.BeagleFramework.Core.Structure;
using Ultz.BeagleFramework.Sql;

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
            var str = "";
            var values = new List<SqlParameter>();
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
                        str += "DELETE";
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
                        str += string.Join(", ", cols.Columns);
                        break;
                }

                str += " ";
            }
            var cmd = new SqlCommand(str, (SqlConnection)Connection);
            values.ForEach(x => cmd.Parameters.Add(x));
            return cmd;
        }
    }
}
