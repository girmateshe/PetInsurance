using System.Data;

namespace Policy.Pets.Provider
{
    public class SqlParam
    {
        public string Name { get; set; }
        public ParameterDirection Direction { get; set; }
        public object Value { get; set; }
        public SqlDbType Type { get; set; }
        public int Size { get; set; }

        public SqlParam() { }
        public SqlParam(string paramName, object paramValue) : this(paramName, paramValue, ParameterDirection.Input, SqlDbType.VarChar) { }
        public SqlParam(string paramName, object paramValue, ParameterDirection paramDirection, SqlDbType type)
        {
            Name = paramName;
            Direction = paramDirection;
            Value = paramValue;
            Type = type;
        }
    }
}
