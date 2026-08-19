using Dapper;
using EscolaApi.Infrastructure;
using EscolaApi.Models;

namespace EscolaApi.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MatriculaRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool Existe(int alunoId, int turmaId)
        {
            const string sql = @"
SELECT COUNT(1)
  FROM dbo.Matricula
 WHERE AlunoId = @AlunoId
   AND TurmaId = @TurmaId;";

            using (var conexao = _connectionFactory.CriarConexao())
            {
                return conexao.ExecuteScalar<int>(sql, new { AlunoId = alunoId, TurmaId = turmaId }) > 0;
            }
        }

        public Matricula MatricularComDecremento(int alunoId, int turmaId)
        {
            // O UPDATE com o guard "VagasDisponiveis > 0" é quem garante que duas matrículas
            // simultâneas não estouram a última vaga: só uma delas afeta a linha.
            const string sqlDecrementarVaga = @"
UPDATE dbo.Turma
   SET VagasDisponiveis = VagasDisponiveis - 1
 WHERE Id = @TurmaId
   AND VagasDisponiveis > 0;";

            const string sqlInserirMatricula = @"
INSERT INTO dbo.Matricula (AlunoId, TurmaId)
VALUES (@AlunoId, @TurmaId);

SELECT Id, AlunoId, TurmaId, DataMatricula
  FROM dbo.Matricula
 WHERE Id = SCOPE_IDENTITY();";

            using (var conexao = _connectionFactory.CriarConexao())
            {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction())
                {
                    var vagasAfetadas = conexao.Execute(
                        sqlDecrementarVaga, new { TurmaId = turmaId }, transacao);

                    if (vagasAfetadas == 0)
                    {
                        transacao.Rollback();
                        return null;
                    }

                    var matricula = conexao.QueryFirst<Matricula>(
                        sqlInserirMatricula, new { AlunoId = alunoId, TurmaId = turmaId }, transacao);

                    transacao.Commit();
                    return matricula;
                }
            }
        }
    }
}
