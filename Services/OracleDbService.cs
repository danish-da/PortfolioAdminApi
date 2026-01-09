using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace PortfolioAdminApi.Services;

public class OracleDbService
{
    private readonly IConfiguration _config;

    public OracleDbService(IConfiguration config)
    {
        _config = config;
    }

    public OracleConnection GetConnection()
    {
        return new OracleConnection(_config.GetConnectionString("ConnectionStrings"));
    }
}
