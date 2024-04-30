using System.Data;

namespace AfpCompanyApi.Data;

public class SqlTypesDictionary
{
    private readonly Dictionary<string, SqlDbType> _sqlTypes;
    internal SqlTypesDictionary()
    {
        _sqlTypes = new Dictionary<string, SqlDbType>();
    }

    internal Dictionary<string, SqlDbType> GetSqlTypes() => _sqlTypes;

    internal SqlTypesDictionary AddDefaultTypes()
    {
        _sqlTypes.Add(typeof(object).Name, SqlDbType.Variant);
        _sqlTypes.Add(typeof(string).Name, SqlDbType.Text);
        _sqlTypes.Add(typeof(char).Name, SqlDbType.NChar);
        _sqlTypes.Add(typeof(bool).Name, SqlDbType.Bit);
        _sqlTypes.Add(typeof(decimal).Name, SqlDbType.Decimal);
        _sqlTypes.Add(typeof(double).Name, SqlDbType.Float);
        _sqlTypes.Add(typeof(byte).Name, SqlDbType.TinyInt);
        _sqlTypes.Add(typeof(short).Name, SqlDbType.SmallInt);
        _sqlTypes.Add(typeof(int).Name, SqlDbType.Int);
        _sqlTypes.Add(typeof(long).Name, SqlDbType.BigInt);
        _sqlTypes.Add(typeof(DateTime).Name, SqlDbType.DateTime2);
        _sqlTypes.Add(typeof(Guid).Name, SqlDbType.UniqueIdentifier);
        return this;
    }

    internal SqlTypesDictionary Add(string key, SqlDbType value)
    {
        _sqlTypes.Add(key, value);
        return this;
    }
}