using System.Linq;
using Dapper;
using EscolaApi.Infrastructure;
using EscolaApi.Models;

namespace EscolaApi.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AlunoRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public PagedResult<Aluno> Listar(string nome, int pagina, int tamanhoPagina)
        {
            const string sql = @"
SELECT COUNT(*)
  FROM dbo.Aluno
 WHERE Ativo = 1
   AND (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%');

SELECT Id, Nome, Email, DataNascimento, Ativo, DataCadastro
  FROM dbo.Aluno
 WHERE Ativo = 1
   AND (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%')
 ORDER BY Nome
OFFSET (@Pagina - 1) * @TamanhoPagina ROWS
 FETCH NEXT @TamanhoPagina ROWS ONLY;";

            using (var conexao = _connectionFactory.CriarConexao())
            using (var multi = conexao.QueryMultiple(sql, new { Nome = nome, Pagina = pagina, TamanhoPagina = tamanhoPagina }))
            {
                var total = multi.ReadFirst<int>();
                var itens = multi.Read<Aluno>().ToList();

                return new PagedResult<Aluno>
                {
                    Itens = itens,
                    Total = total,
                    Pagina = pagina,
                    TamanhoPagina = tamanhoPagina
                };
            }
        }

        public Aluno ObterPorId(int id)
        {
            const string sql = @"
SELECT Id, Nome, Email, DataNascimento, Ativo, DataCadastro
  FROM dbo.Aluno
 WHERE Id = @Id;";

            using (var conexao = _connectionFactory.CriarConexao())
            {
                return conexao.QueryFirstOrDefault<Aluno>(sql, new { Id = id });
            }
        }

        public int Inserir(Aluno aluno)
        {
            const string sql = @"
INSERT INTO dbo.Aluno (Nome, Email, DataNascimento, Ativo)
VALUES (@Nome, @Email, @DataNascimento, 1);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var conexao = _connectionFactory.CriarConexao())
            {
                return conexao.ExecuteScalar<int>(sql, new { aluno.Nome, aluno.Email, aluno.DataNascimento });
            }
        }

        public bool Atualizar(Aluno aluno)
        {
            const string sql = @"
UPDATE dbo.Aluno
   SET Nome = @Nome,
       Email = @Email,
       DataNascimento = @DataNascimento
 WHERE Id = @Id;";

            using (var conexao = _connectionFactory.CriarConexao())
            {
                return conexao.Execute(sql, new { aluno.Id, aluno.Nome, aluno.Email, aluno.DataNascimento }) > 0;
            }
        }

        public bool Desativar(int id)
        {
            const string sql = "UPDATE dbo.Aluno SET Ativo = 0 WHERE Id = @Id;";

            using (var conexao = _connectionFactory.CriarConexao())
            {
                return conexao.Execute(sql, new { Id = id }) > 0;
            }
        }
    }
}
