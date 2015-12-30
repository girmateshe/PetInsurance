using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Reflection;
using Policy.Pets.Models;
using Policy.Pets.Provider.Interfaces;

namespace Policy.Pets.Provider
{
    public abstract class BasePetProvider<T> : IProvider<T> where T : Model
    {
        protected readonly string ConnectionString;
        private SqlConnection Connection { get; set; }
        private SqlCommand Command { get; set; }
        public List<SqlParam> OutParameters { get; private set; }
        public IDebugContext DebugContext { get; set; }

        protected BasePetProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public abstract Task<IEnumerable<T>> GetAll();

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> Create(T pet)
        {
            throw new NotImplementedException();
        }

        public Task<T> Delete(int id)
        {
            throw new NotImplementedException();
        }

        private async Task Open()
        {
            try
            {
                var cn = (new SqlConnectionStringBuilder(ConnectionString) { AsynchronousProcessing = true }).ToString();
                Connection = new SqlConnection(cn);
                Connection.Open();
            }
            catch (Exception ex)
            {
                Close();
            }
        }

        private void Close()
        {
            if (Connection != null)
            {
                Connection.Close();
            }
        }

        // executes stored procedure with DB parameteres if they are passed
        protected async Task<object> Execute(string procedureName, ExecuteType executeType, List<SqlParam> parameters)
        {
            object returnObject = null;

            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Command = new SqlCommand
                    {
                        CommandText = procedureName,
                        Connection = Connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    // pass stored procedure parameters to command
                    if (parameters != null)
                    {
                        Command.Parameters.Clear();

                        foreach (var param in parameters)
                        {
                            var parameter = new SqlParameter
                            {
                                ParameterName = "@" + param.Name,
                                Direction = param.Direction,
                                Value = param.Value,
                                SqlDbType = param.Type
                            };
                            if (param.Size != 0)
                            {
                                parameter.Size = param.Size;
                            }

                            Command.Parameters.Add(parameter);
                        }
                    }

                    using (new Profiler(DebugContext, LogLevel.Debug, Command.CommandText))
                    {
                        switch (executeType)
                        {
                            case ExecuteType.ExecuteReader:
                                returnObject = await Command.ExecuteReaderAsync();
                                break;
                            case ExecuteType.ExecuteNonQuery:
                                returnObject = await Command.ExecuteNonQueryAsync();
                                break;
                            case ExecuteType.ExecuteScalar:
                                returnObject = await Command.ExecuteScalarAsync();
                                break;
                            default:
                                break;
                        }
                        DebugContext.Log(LogLevel.Debug, new { sql = Command.CommandAsSql() });
                    }

                }
            }

            return returnObject;
        }

        // updates output parameters from stored procedure
        protected void UpdateOutParameters()
        {
            if (Command.Parameters.Count > 0)
            {
                OutParameters = new List<SqlParam>();
                OutParameters.Clear();

                for (var i = 0; i < Command.Parameters.Count; i++)
                {
                    if (Command.Parameters[i].Direction == ParameterDirection.Output)
                    {
                        OutParameters.Add(new SqlParam(
                                                Command.Parameters[i].ParameterName,
                                                Command.Parameters[i].Value,
                                                ParameterDirection.Output,
                                                Command.Parameters[i].SqlDbType
                                         )
                       );
                    }
                }
            }
        }

        // executes scalar query stored procedure without parameters
        protected async Task<T> ExecuteSingle<T>(string procedureName) where T : new()
        {
            return await ExecuteSingle<T>(procedureName, null);
        }

        // executes scalar query stored procedure and maps result to single object
        protected async Task<T> ExecuteSingle<T>(string procedureName, List<SqlParam> parameters) where T : new()
        {

            await Open();

            var r = await Execute(procedureName, ExecuteType.ExecuteReader, parameters);

            var reader = (IDataReader)r;

            var tempObject = default(T);

            using (new Profiler(DebugContext, LogLevel.Debug, "Read SqlReader for " + Command.CommandText))
            {
                if (reader.Read())
                {
                    tempObject = new T();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetValue(i) == DBNull.Value) continue;

                        var propertyInfo = typeof(T).GetProperty(reader.GetName(i));
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(tempObject, reader.GetValue(i), null);
                        }
                        else
                        {
                            var prop = GetPropertyInfo<T>(reader.GetName(i));
                            if (prop != null)
                            {
                                prop.SetValue(tempObject, reader.GetValue(i), null);
                            }
                        }
                    }
                    DebugContext.Log(LogLevel.Debug, tempObject);
                }
            }
            reader.Close();
            UpdateOutParameters();
            Close();

            return tempObject;
        }

        // executes list query stored procedure without parameters
        protected async Task<IEnumerable<T>> ExecuteList<T>(string procedureName) where T : new()
        {
            return await ExecuteList<T>(procedureName, null);
        }

        // executes list query stored procedure and maps result generic list of objects
        protected async Task<IEnumerable<T>> ExecuteList<T>(string procedureName, List<SqlParam> parameters)
            where T : new()
        {
            var objects = new List<T>();

            await Open();

            var r = await Execute(procedureName, ExecuteType.ExecuteReader, parameters);

            var reader = (IDataReader)r;

            using (new Profiler(DebugContext, LogLevel.Debug, "Read SqlReader for " + Command.CommandText))
            {
                while (reader.Read())
                {
                    var tempObject = new T();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetValue(i) == DBNull.Value) continue;

                        var propertyInfo = typeof(T).GetProperty(reader.GetName(i));
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(tempObject, reader.GetValue(i), null);
                        }
                        else
                        {
                            var prop = GetPropertyInfo<T>(reader.GetName(i));
                            if (prop != null)
                            {
                                prop.SetValue(tempObject, reader.GetValue(i), null);
                            }
                        }
                    }
                    objects.Add(tempObject);
                }
                DebugContext.Log(LogLevel.Debug, objects);
            }

            reader.Close();
            UpdateOutParameters();
            Close();

            return objects;
        }

        private PropertyInfo GetPropertyInfo<T>(string name)
        {

            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);

                if (attrs.Length == 0) { }

                foreach (object attr in attrs)
                {
                    var columnAttr = attr as ColumnAttribute;
                    if (columnAttr != null)
                    {
                        if (columnAttr.Name == name)
                        {
                            return prop;
                        }
                    }
                }
            }

            return null;
        }

        // executes non query stored procedure with parameters
        protected async Task<T> ExecuteNonQuery<T>(string procedureName, List<SqlParam> parameters) where T : new()
        {
            await Open();
            await Execute(procedureName, ExecuteType.ExecuteNonQuery, parameters);
            UpdateOutParameters();

            var tempObject = new T();

            foreach (var sqlParam in OutParameters)
            {
                var name = sqlParam.Name.Substring(1);
                var propertyInfo = typeof(T).GetProperty(name);
                if (propertyInfo != null && sqlParam.Value != DBNull.Value)
                {
                    propertyInfo.SetValue(tempObject, sqlParam.Value, null);
                }
                else
                {
                    var prop = GetPropertyInfo<T>(name);
                    if (prop != null && sqlParam.Value != DBNull.Value)
                    {
                        prop.SetValue(tempObject, sqlParam.Value, null);
                    }
                }
            }

            Close();
            return tempObject;
        }

        public enum ExecuteType
        {
            ExecuteReader,
            ExecuteNonQuery,
            ExecuteScalar
        }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : System.Attribute
    {
        public string Name;

        public ColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}
