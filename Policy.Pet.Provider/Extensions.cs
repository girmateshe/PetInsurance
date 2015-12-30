using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Policy.Pets.Provider
{
    public static class DataExtensions
    {
        public static String CommandAsSql(this SqlCommand sc)
        {
            var sql = new StringBuilder();
            Boolean FirstParam = true;

            sql.AppendLine("use " + sc.Connection.Database + ";");
            switch (sc.CommandType)
            {
                case CommandType.StoredProcedure:
                    sql.AppendLine("declare @return_value int;");

                    foreach (SqlParameter sp in sc.Parameters)
                    {
                        if ((sp.Direction == ParameterDirection.InputOutput) || (sp.Direction == ParameterDirection.Output))
                        {
                            sql.Append("declare " + sp.ParameterName + "\t" + sp.SqlDbType.ToString() + "\t= ");

                            sql.AppendLine(((sp.Direction == ParameterDirection.Output) ? "null" : sp.ParameterValueForSql()) + ";");

                        }
                    }

                    sql.AppendLine("exec [" + sc.CommandText + "]");

                    foreach (SqlParameter sp in sc.Parameters)
                    {
                        if (sp.Direction != ParameterDirection.ReturnValue)
                        {
                            sql.Append((FirstParam) ? "\t" : "\t, ");

                            if (FirstParam) FirstParam = false;

                            if (sp.Direction == ParameterDirection.Input)
                                sql.AppendLine(sp.ParameterName + " = " + sp.ParameterValueForSql());
                            else

                                sql.AppendLine(sp.ParameterName + " = " + sp.ParameterName + " output");
                        }
                    }
                    sql.AppendLine(";");

                    sql.AppendLine("select 'Return Value' = convert(varchar, @return_value);");

                    foreach (SqlParameter sp in sc.Parameters)
                    {
                        if ((sp.Direction == ParameterDirection.InputOutput) || (sp.Direction == ParameterDirection.Output))
                        {
                            sql.AppendLine("select '" + sp.ParameterName + "' = convert(varchar, " + sp.ParameterName + ");");
                        }
                    }
                    break;
                case CommandType.Text:
                    sql.AppendLine(sc.CommandText);
                    break;
            }

            return sql.ToString();
        }

        public static String ParameterValueForSql(this SqlParameter sp)
        {
            String retval = "";

            switch (sp.SqlDbType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.Time:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                    retval = "'" + sp.Value.ToEmptyStringIfNull().Replace("'", "''") + "'";
                    break;

                case SqlDbType.Bit:
                    retval = (sp.Value.ToBooleanOrDefault(false)) ? "1" : "0";
                    break;

                default:
                    retval = sp.Value.ToEmptyStringIfNull().Replace("'", "''");
                    break;
            }

            return retval;
        }

        public static object Get(this List<SqlParam> parameters, string name)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return null;
            }

            var param = parameters.SingleOrDefault(p => p.Name == name);
            if (param != null)
            {
                return param.Value;
            }
            return null;
        }
    }

    public static class ObjectExtensions
    {
        public static bool IsInt32(this object o)
        {
            if (o == null)
                return false;

            if (ObjectExtensions.IsString(o))
            {
                int r;
                return int.TryParse(o.ToString(), out r);
            }
            else
            {
                return o is Int32;
            }
        }

        public static bool IsFloat(this object o)
        {
            if (o == null)
                return false;

            if (ObjectExtensions.IsString(o))
            {
                float r;
                return float.TryParse(o.ToString(), out r);
            }
            else
            {
                return o is float;
            }
        }

        public static bool IsString(this object o)
        {
            if (o == null)
                return false;

            return o is String;
        }

        public static bool IsBoolean(this object o)
        {
            if (o == null)
                return false;

            if (ObjectExtensions.IsString(o))
            {
                bool r;
                return bool.TryParse(o.ToString(), out r);
            }
            else
            {
                return o is Boolean;
            }
        }

        public static int ToInt32OrZero(this object o)
        {
            return ObjectExtensions.ToInt32OrDefault(o, 0);
        }

        public static int ToInt32OrDefault(this object o, int def)
        {
            if (o == null)
                return def;

            if (o.IsInt32())
                return Convert.ToInt32(o);
            else
                return def;
        }

        public static float ToFloatOrZero(this object o)
        {
            return ObjectExtensions.ToFloatOrDefault(o, 0.0f);
        }

        public static float ToFloatOrDefault(this object o, float def)
        {
            if (o == null)
                return def;

            if (o.IsFloat())
                return (float)Convert.ToDouble(o);
            else
                return def;
        }

        public static bool ToBooleanOrDefault(this object o, bool def)
        {
            if (o == null)
                return def;

            if (o.IsBoolean())
                return Convert.ToBoolean(o);
            else
                return def;
        }

        public static string ToEmptyStringIfNull(this object o)
        {
            if (o == null)
                return String.Empty;
            else
                return o.ToString();
        }
    }
}
