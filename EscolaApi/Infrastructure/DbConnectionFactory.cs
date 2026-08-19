using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EscolaApi.Infrastructure
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["TesteEscola"].ConnectionString;
        }

        public IDbConnection CriarConexao()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
