using System.Data;

namespace EscolaApi.Infrastructure
{
    public interface IDbConnectionFactory
    {
        IDbConnection CriarConexao();
    }
}
