using System.Collections.Generic;
using System.Linq;
using Dapper;
using EscolaApi.Infrastructure;
using EscolaApi.Models;

namespace EscolaApi.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TurmaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<Turma> Listar()
        {
            const string sql = @"
SELECT Id, Nome, Periodo, VagasTotal, VagasDisponiveis
  FROM dbo.Turma
 ORDER BY Nome;";

            using (var conexao = _connectionFactory.CriarConexao())
            {
                return conexao.Query<Turma>(sql).ToList();
            }
        }

        public Turma ObterPorId(int id)
        {
            const string sql = @"
SELECT Id, Nome, Periodo, VagasTotal, VagasDisponiveis
  FROM dbo.Turma
 WHERE Id = @Id;";

            using (var conexao = _connectionFactory.CriarConexao())
            {
                return conexao.QueryFirstOrDefault<Turma>(sql, new { Id = id });
            }
        }

        public IEnumerable<AlunosPorTurmaItem> RelatorioAlunosPorTurma()
        {
            const string sql = @"
SELECT t.Nome              AS NomeTurma,
       COUNT(m.Id)         AS QuantidadeAlunos,
       t.VagasDisponiveis  AS VagasRestantes
  FROM dbo.Turma t
  LEFT JOIN dbo.Matricula m ON m.TurmaId = t.Id
 GROUP BY t.Id, t.Nome, t.VagasDisponiveis
 ORDER BY t.Nome;";

            using (var conexao = _connectionFactory.CriarConexao())
            {
                return conexao.Query<AlunosPorTurmaItem>(sql).ToList();
            }
        }
    }
}
