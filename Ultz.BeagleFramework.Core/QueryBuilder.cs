#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Ultz.BeagleFramework.Core.Structure
{
    public class QueryBuilder
    {
        private List<Clause> Clauses { get; } = new List<Clause>();

        public Query Build()
        {
            return new Query(Clauses.ToArray());
        }

        public QueryBuilder Select()
        {
            Clauses.Add(new Clause.Select());
            return this;
        }

        public QueryBuilder Wildcard()
        {
            Clauses.Add(new Clause.Wildcard());
            return this;
        }

        public QueryBuilder Column(string name)
        {
            Clauses.Add(new Clause.ColumnName {Column = name});
            return this;
        }

        public QueryBuilder Columns(params string[] names)
        {
            Clauses.Add(new Clause.ColumnNames {Columns = names.ToList()});
            return this;
        }

        public QueryBuilder From()
        {
            Clauses.Add(new Clause.From());
            return this;
        }

        public QueryBuilder TableName(string name)
        {
            Clauses.Add(new Clause.TableName {Table = name});
            return this;
        }

        public QueryBuilder Where()
        {
            Clauses.Add(new Clause.Where());
            return this;
        }

        public QueryBuilder And()
        {
            Clauses.Add(new Clause.And());
            return this;
        }

        public QueryBuilder Not()
        {
            Clauses.Add(new Clause.Not());
            return this;
        }

        public QueryBuilder Or()
        {
            Clauses.Add(new Clause.Or());
            return this;
        }

        public QueryBuilder Equal()
        {
            Clauses.Add(new Clause.Equal());
            return this;
        }

        public QueryBuilder NotEqual()
        {
            Clauses.Add(new Clause.NotEqual());
            return this;
        }

        public QueryBuilder GreaterThan()
        {
            Clauses.Add(new Clause.GreaterThan());
            return this;
        }

        public QueryBuilder LessThan()
        {
            Clauses.Add(new Clause.LessThan());
            return this;
        }

        public QueryBuilder GreaterThanOrEqualTo()
        {
            Clauses.Add(new Clause.GreaterThan {OrEqualTo = true});
            return this;
        }

        public QueryBuilder LessThanOrEqualTo()
        {
            Clauses.Add(new Clause.LessThan {OrEqualTo = true});
            return this;
        }

        public QueryBuilder Between()
        {
            Clauses.Add(new Clause.Between());
            return this;
        }

        public QueryBuilder Like()
        {
            Clauses.Add(new Clause.Like());
            return this;
        }

        public QueryBuilder In()
        {
            Clauses.Add(new Clause.In());
            return this;
        }

        public QueryBuilder Insert()
        {
            Clauses.Add(new Clause.Insert());
            return this;
        }

        public QueryBuilder Into()
        {
            Clauses.Add(new Clause.Into());
            return this;
        }

        public QueryBuilder Delete()
        {
            Clauses.Add(new Clause.Delete());
            return this;
        }

        public QueryBuilder RowNumber()
        {
            Clauses.Add(new Clause.RowNumber());
            return this;
        }

        public QueryBuilder Value(object value)
        {
            Clauses.Add(new Clause.Value {Item = value});
            return this;
        }

        public QueryBuilder Values(params object[] values)
        {
            Clauses.Add(new Clause.Values {Items = values});
            return this;
        }

        public QueryBuilder Set(params (string, object)[] values)
        {
            Clauses.Add(new Clause.Set {Values = values.ToList()});
            return this;
        }

        public QueryBuilder Update()
        {
            Clauses.Add(new Clause.Update());
            return this;
        }
    }
}
