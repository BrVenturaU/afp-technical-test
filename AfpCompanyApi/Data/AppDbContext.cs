using Dapper;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;
using System.Data;

namespace AfpCompanyApi.Data;

public class DapperContextOptions
{
    public string ConnectionString { get; set; }
    public Dictionary<string, SqlDbType> AllowedSqlDbTypes { get; set; }

}

public class AppDbContext : IDisposable
{
    private readonly DapperContextOptions _dapperContextOptions = new();
    private IDbConnection _connection;
    private IDbTransaction _dbTransaction;
    private bool _disposed;

    /// <summary>
    /// Obtiene la conexión subyacente a este contexto.
    /// </summary>
    public IDbConnection Connection => _connection;

    /// <summary>
    /// Obtiene la transacción activa de la conexión del contexto.
    /// </summary>
    public IDbTransaction Transaction => _dbTransaction;

    /// <summary>
    /// Obtiene la cadena de conexión actual del contexto.
    /// </summary>
    public string ContextConnectionString => _dapperContextOptions.ConnectionString;

    public AppDbContext(Action<DapperContextOptions> options)
    {
        options(_dapperContextOptions);
        _connection = new SqlConnection(_dapperContextOptions.ConnectionString);
        _connection.Open();
        _dbTransaction = _connection.BeginTransaction();
    }

    /// <summary>
    /// Permite crear una conexión SQL distinta a la de este contexto utilizando las mismas opciones de configuración.
    /// </summary>
    /// <returns>Retorna una conexion abierta hacia la instancia referida.</returns>
    public IDbConnection CreateIsolatedConnection() => new SqlConnection(_dapperContextOptions.ConnectionString);

    /// <summary>
    /// Confirma la transacción activa de la conexión del contexto, la revierte si falla y crea una nueva para operaciones subsiguientes.
    /// </summary>
    public void CommitTransaction()
    {
        try
        {
            _dbTransaction.Commit();
        }
        catch (Exception)
        {
            _dbTransaction.Rollback();
            throw;
        }
        finally
        {
            _dbTransaction.Dispose();
            _dbTransaction = _connection.BeginTransaction();

        }
    }

    /// <summary>
    /// Permite obtener una consulta personalizada para TVP mediante entidades o clases.
    /// </summary>
    /// <typeparam name="T">La entidad o clase que contiene los campos del TVP.</typeparam>
    /// <param name="elements">El listado de entidades que representara una lista dentro del TVP referido.</param>
    /// <returns>Una consulta personalizada de Dapper.</returns>
    public ICustomQueryParameter GetTableValuedParameterByClassType<T>(IEnumerable<T> elements)
        where T : class
    {
        if (elements.AsList().Count == 0)
        {
            elements = new List<T>();
        }

        return CreateSqlDataRecord(elements).AsTableValuedParameter();
    }

    /// <summary>
    /// Permite obtener una consulta personalizada para TVP que solo contienen un campo con el nombre <paramref name="fieldName"/>.
    /// </summary>
    /// <typeparam name="T">El tipo de valor de la columna única en el TVP</typeparam>
    /// <param name="elements">El listado de valores que representara una lista dentro del TVP referido.</param>
    /// <param name="fieldName">El nombre del campo unico del TVP que se construirá.</param>
    /// <returns>Una consulta personalizada de Dapper.</returns>
    public ICustomQueryParameter GetTableValuedParameterByValueType<T>(IEnumerable<T> elements, string fieldName)
    {
        if (elements.AsList().Count == 0)
        {
            elements = new List<T>();
        }

        return CreateSqlDataRecord(elements, fieldName).AsTableValuedParameter();
    }

    private IEnumerable<SqlDataRecord> CreateSqlDataRecord<T>(IEnumerable<T> elements)
       where T : class
    {
        var sqlDataRecordClosure = SqlDataRecordClosure<T>();
        foreach (var element in elements)
            yield return sqlDataRecordClosure(element);

    }

    private IEnumerable<SqlDataRecord> CreateSqlDataRecord<T>(IEnumerable<T> elements, string fieldName)
    {
        var sqlDataRecordClosure = SqlDataRecordClosure<T>(fieldName);
        foreach (var element in elements)
            yield return sqlDataRecordClosure(element);
    }

    private Func<T, SqlDataRecord> SqlDataRecordClosure<T>()
        where T : class
    {
        var type = typeof(T);
        var properties = type.GetProperties();
        var parameters = properties.Select(property => new { PropertyName = property.Name, Property = property }).ToArray();

        var metadata = parameters.Select(parameter => new SqlMetaData(parameter.PropertyName, GetDbTypeFromType(Nullable.GetUnderlyingType(parameter.Property.PropertyType) ?? parameter.Property.PropertyType))).ToArray();
        var record = new SqlDataRecord(metadata);

        return (T element) =>
        {
            for (int i = 0; i < parameters.Length; i++)
                record.SetValue(i, parameters[i].Property.GetValue(element));

            return record;
        };
    }

    private Func<T, SqlDataRecord> SqlDataRecordClosure<T>(string fieldName)
    {
        var metadata = new SqlMetaData(fieldName, GetDbTypeFromType(typeof(T)));
        var record = new SqlDataRecord(metadata);
        return (T element) =>
        {
            record.SetValue(0, element);
            return record;
        };
    }

    private SqlDbType GetDbTypeFromType(Type type) => _dapperContextOptions.AllowedSqlDbTypes[type.Name];

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (!disposing)
        {
            _disposed = true;
            return;
        }

        if (_dbTransaction != null)
        {
            _dbTransaction.Dispose();
            _dbTransaction = null;
        }
        if (_connection != null)
        {
            _connection.Dispose();
            _connection = null;
        }

        _disposed = true;
    }

    ~AppDbContext()
    {
        Dispose(false);
    }
}
