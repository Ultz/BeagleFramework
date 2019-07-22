#region

using System;
using System.Linq;
using JetBrains.Annotations;

#endregion

namespace Ultz.BeagleFramework.Core
{
    public class BeagleContext : IDisposable
    {
        public BeagleContext(IStorageEngine engine)
        {
            Engine = engine;
        }

        public IStorageEngine Engine { get; }

        [NotNull]
        public Table GetSchema(SchemaTable table)
        {
            var qb = new QueryBuilder().Select().Wildcard().From();
            switch (table)
            {
                case SchemaTable.CheckConstraints:
                    qb.InformationSchema("CHECK_CONSTRAINTS");
                    break;
                case SchemaTable.ColumnDomainUsage:
                    qb.InformationSchema("COLUMN_DOMAIN_USAGE");
                    break;
                case SchemaTable.ColumnPrivileges:
                    qb.InformationSchema("COLUMN_PRIVILEGES");
                    break;
                case SchemaTable.Columns:
                    qb.InformationSchema("COLUMNS");
                    break;
                case SchemaTable.ConstraintColumnUsage:
                    qb.InformationSchema("CONSTRAINT_COLUMN_USAGE");
                    break;
                case SchemaTable.ConstraintTableUsage:
                    qb.InformationSchema("CONSTRAINT_TABLE_USAGE");
                    break;
                case SchemaTable.DomainConstraints:
                    qb.InformationSchema("DOMAIN_CONSTRAINTS");
                    break;
                case SchemaTable.Domains:
                    qb.InformationSchema("DOMAINS");
                    break;
                case SchemaTable.KeyColumnUsage:
                    qb.InformationSchema("KEY_COLUMN_USAGE");
                    break;
                case SchemaTable.Parameters:
                    qb.InformationSchema("PARAMETERS");
                    break;
                case SchemaTable.ReferentialConstraints:
                    qb.InformationSchema("REFERENTIAL_CONSTRAINTS");
                    break;
                case SchemaTable.RoutineColumns:
                    qb.InformationSchema("ROUTINE_COLUMNS");
                    break;
                case SchemaTable.Routines:
                    qb.InformationSchema("ROUTINES");
                    break;
                case SchemaTable.Schemata:
                    qb.InformationSchema("SCHEMATA");
                    break;
                case SchemaTable.TableConstraints:
                    qb.InformationSchema("TABLE_CONSTRAINTS");
                    break;
                case SchemaTable.TablePrivileges:
                    qb.InformationSchema("TABLE_PRIVILEGES");
                    break;
                case SchemaTable.Tables:
                    qb.InformationSchema("TABLES");
                    break;
                case SchemaTable.Views:
                    qb.InformationSchema("VIEWS");
                    break;
                case SchemaTable.ViewColumnUsage:
                    qb.InformationSchema("VIEW_COLUMN_USAGE");
                    break;
                case SchemaTable.ViewTableUsage:
                    qb.InformationSchema("VIEW_TABLE_USAGE");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(table), table, null);
            }

            return Engine.Execute(qb.Build()).AsTable();
        }

        [CanBeNull]
        public Table GetTable(string name)
        {
            if (Engine.Execute
            (
                new QueryBuilder()
                    .Select()
                    .Column("TABLE_NAME")
                    .From()
                    .InformationSchema("TABLES")
                    .Where()
                    .Column("TABLE_NAME")
                    .Equal()
                    .Value(name)
                    .Build()
            )
            .AsTable().Count == 0)
            {
                return null;
            }
            return Engine.Execute
            (
                new QueryBuilder()
                    .Select()
                    .Wildcard()
                    .From()
                    .TableName(name)
                    .Build()
            )
            .AsTable(false);
        }

        [NotNull]
        public Table CreateTable(string name, Column c1, params Column[] columns)
        {
            Engine.Execute
                (
                    new QueryBuilder()
                        .Create()
                        .Table()
                        .TableName(name)
                        .PseudoColumns
                        (
                            new[] {c1}.Concat(columns).Select(x => (x.Name, x.Type, x.Constraints.ToArray())).ToArray()
                        )
                        .Build()
                )
                .AsNonQuery();
            return GetTable(name);
        }

        public void DeleteTable(string name)
        {
            Engine.Execute(new QueryBuilder().Drop().Table().TableName(name).Build()).AsNonQuery();
        }

        public void Dispose()
        {
            Engine?.Dispose();
        }
    }
}
