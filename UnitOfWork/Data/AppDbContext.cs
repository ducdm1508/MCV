using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace UnitOfWork.Data
{
    public class AppDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Default");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
