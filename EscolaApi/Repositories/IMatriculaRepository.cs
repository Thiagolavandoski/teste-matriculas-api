using EscolaApi.Models;

namespace EscolaApi.Repositories
{
    public interface IMatriculaRepository
    {
        bool Existe(int alunoId, int turmaId);

        /// <summary>
        /// Insere a matrícula e decrementa VagasDisponiveis da turma na MESMA transação.
        /// Retorna null se a turma não tinha vaga no momento do decremento (nada é gravado).
        /// </summary>
        Matricula MatricularComDecremento(int alunoId, int turmaId);
    }
}
