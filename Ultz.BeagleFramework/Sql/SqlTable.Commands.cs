using System.Data.Common;
using System.Linq;
using System.Text;

namespace Ultz.BeagleFramework.Sql
{
    partial class SqlTable
    {
        internal void CreateCommands()
        {
            _adapter.InsertCommand = InsertCommand();
            _adapter.UpdateCommand = UpdateCommand();
            _adapter.DeleteCommand = DeleteCommand();
        }
/*
 * UPDATE table_name
SET column1 = value1, column2 = value2, ...
WHERE condition;
 */
        /*DELETE FROM table_name
WHERE condition;*/

        private DbCommand DeleteCommand()
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM `" + Name.ToUpper() + "` WHERE onultz_id = @onultz_id");
            var command = ((SqlEngine) _engine).Connector.CreateCommand(((SqlEngine) _engine).Connector.ProcessMessage(sb.ToString()),((SqlEngine) _engine).Connector.CreateConnection());
            command.Parameters.Add(((SqlEngine) _engine).Connector.CreateIntParameter("onultz_id"));
            return command;
        }

        private DbCommand UpdateCommand()
        {
            var sb = new StringBuilder();
            sb.Append("UPDATE `" + Name.ToUpper() + "` SET");
            var clms = Columns.ToList();
            sb.Append("onultz_id = @onultz_id, ");
            foreach (var cl in clms)
            {
                sb.Append("`"+cl + "` = @" + cl);
                if (!clms.Last().Equals(cl))
                {
                    sb.Append(",");
                }

                sb.Append(" ");
            }

            sb.Append("WHERE onultz_id = @onultz_id");
            var command = ((SqlEngine) _engine).Connector.CreateCommand(((SqlEngine) _engine).Connector.ProcessMessage(sb.ToString()),((SqlEngine) _engine).Connector.CreateConnection());
            command.Parameters.Add(((SqlEngine) _engine).Connector.CreateIntParameter("onultz_id"));
            foreach (var cl in clms)
            {
                command.Parameters.Add(((SqlEngine) _engine).Connector.CreateParameter(cl));
            }

            return command;
        }

        private DbCommand InsertCommand()
        {
            var sb = new StringBuilder();
            sb.Append("INSERT INTO `" + Name.ToUpper() + "` (");
            var clms = Columns.ToList();
            sb.Append("onultz_id, ");
            foreach (var cl in clms)
            {
                sb.Append("`"+cl+"`");
                if (!clms.Last().Equals(cl))
                {
                    sb.Append(", ");
                }
            }

            sb.Append(") VALUES (");
            sb.Append("@onultz_id, ");
            foreach (var cl in clms)
            {
                sb.Append("@" + cl);
                if (!clms.Last().Equals(cl))
                {
                    sb.Append(", ");
                }
            }

            sb.Append(")");
            var command = ((SqlEngine) _engine).Connector.CreateCommand(((SqlEngine) _engine).Connector.ProcessMessage(sb.ToString()),((SqlEngine) _engine).Connector.CreateConnection());
            command.Parameters.Add(((SqlEngine) _engine).Connector.CreateIntParameter("onultz_id"));
            foreach (var cl in clms)
            {
                command.Parameters.Add(((SqlEngine) _engine).Connector.CreateParameter(cl));
            }

            return command;
        }
        
        internal long GetId()
        {
            var available = false;
            var i = -1L;
            while (!available)
            {
                i++;
                available = !_table.Rows.Contains(i);
            }

            return i;
        }
    }
}