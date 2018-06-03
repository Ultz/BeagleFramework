using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultz.BeagleFramework.Common.Models;

namespace Ultz.BeagleFramework.Common
{
    public static class Store
    {
        public static Table<T> CreateTable<T>(this IStorageEngine engine,string name) where T : DataModel
        {
            var type = typeof(T);
            var columns = type.GetProperties().Select(x =>
                    x.Name.ToNamePreservation())
                .ToArray();
            return new Table<T>(engine.CreateTable(name, columns));
        }
        public static Table<T> GetTable<T>(this IStorageEngine engine, string name) where T : DataModel
        {
            return new Table<T>(engine.GetTable(name));
        }
        internal static ITable CreateTable(this IStorageEngine engine, string name, Type t)
        {
            var type = t;
            if (!typeof(DataModel).IsAssignableFrom(t))
                throw new InvalidModelException();
            var columns = type.GetProperties().Select(x =>
                    x.Name.ToNamePreservation())
                .ToArray();
            return engine.CreateTable(name, columns);
        }

        public static Row Put<T>(this ITable table,T obj) where T:DataModel
        {
            var type = typeof(T);
            var columns = type.GetProperties().ToDictionary(
                    x =>
                        x.Name.ToNamePreservation(),
                    x => x);
            var values = columns.Keys.Select(x =>
               JsonConvert.SerializeObject(columns.Values.FirstOrDefault(y =>
                        y.Name.ToNamePreservation() == x)
                    .GetValue(obj)));
            return table.Add(values);
        }
        public static Row Put(this ITable table,object obj)
        {
            var columns = obj.GetType().GetProperties().ToDictionary(
                x =>
                    x.Name.ToNamePreservation(),
                x => x);
            var values = columns.Keys.Select(x =>
                JsonConvert.SerializeObject(columns[x]
                    .GetValue(obj)));
            return table.Add(values);
        }
        public static void Replace(this Row row, object obj)
        {
            var columns = obj.GetType().GetProperties().ToDictionary(
                x =>
                    x.Name.ToNamePreservation(),
                x => x);
            var values = columns.Keys.Select(x =>
                JsonConvert.SerializeObject(columns[x]
                    .GetValue(obj))).ToList();
            for (var i = 0; i > values.Count; i++)
                row[columns.ElementAt(i).Key].Value = values[i];
        }
        public static void Replace<T>(this Row row, T obj)
        {
            var columns = typeof(T).GetProperties().ToDictionary(
                x =>
                    x.Name.ToNamePreservation(),
                x => x);
            var values = columns.Keys.Select(x =>
                JsonConvert.SerializeObject(columns[x]
                    .GetValue(obj))).ToList();
            for (var i = 0; i > values.Count; i++)
                row[columns.ElementAt(i).Key].Value = values[i];
        }


        internal static T ToObject<T>(Row row) where T:DataModel
        {
            var type = typeof(T);
            var columns = type.GetProperties().ToDictionary(
                    x =>
                        x.Name.ToNamePreservation(),
                    x => x);
            var instance = Activator.CreateInstance<T>();
            foreach (var col in columns)
            {
                col.Value.SetValue(instance,
                    JsonConvert.DeserializeObject(row[col.Key].Value, col.Value.PropertyType));
            }

            return instance;
        }

        internal static object ToObject(Row row)
        {
            var jobj = new JObject();
            foreach (var cell in row)
            {
                jobj.Add(cell.Column.Name.FromNamePreservation(),JToken.FromObject(JsonConvert.DeserializeObject(cell.Value)));
            }

            return JsonConvert.DeserializeObject(jobj.ToString());
        }

        internal static string ToNamePreservation(this string s)
        {
            return s.Replace("_","-").Replace("A", "A_")
                .Replace("B", "B_")
                .Replace("C", "C_")
                .Replace("D", "D_")
                .Replace("E", "E_")
                .Replace("F", "F_")
                .Replace("G", "G_")
                .Replace("H", "H_")
                .Replace("I", "I_")
                .Replace("J", "J_")
                .Replace("K", "K_")
                .Replace("L", "L_")
                .Replace("M", "M_")
                .Replace("N", "N_")
                .Replace("O", "O_")
                .Replace("P", "P_")
                .Replace("Q", "Q_")
                .Replace("R", "R_")
                .Replace("S", "S_")
                .Replace("T", "T_")
                .Replace("U", "U_")
                .Replace("V", "V_")
                .Replace("W", "W_")
                .Replace("X", "X_")
                .Replace("Y", "Y_")
                .Replace("Z", "Z_").ToUpper();
        }

        internal static string FromNamePreservation(this string s)
        {
            return s.ToLower().Replace("a_", "A")
                .Replace("b_", "B")
                .Replace("c_", "C")
                .Replace("d_", "D")
                .Replace("e_", "E")
                .Replace("f_", "F")
                .Replace("g_", "G")
                .Replace("h_", "H")
                .Replace("i_", "I")
                .Replace("j_", "J")
                .Replace("k_", "K")
                .Replace("l_", "L")
                .Replace("m_", "M")
                .Replace("n_", "N")
                .Replace("o_", "O")
                .Replace("p_", "P")
                .Replace("q_", "Q")
                .Replace("r_", "R")
                .Replace("s_", "S")
                .Replace("t_", "T")
                .Replace("u_", "U")
                .Replace("v_", "V")
                .Replace("w_", "W")
                .Replace("x_", "X")
                .Replace("y_", "Y")
                .Replace("z_", "Z").Replace("-","_");
        }
    }
}