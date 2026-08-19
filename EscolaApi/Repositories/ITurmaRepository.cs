using System.Collections.Generic;
using EscolaApi.Models;

namespace EscolaApi.Repositories
{
    public interface ITurmaRepository
    {
        IEnumerable<Turma> Listar();
        Turma ObterPorId(int id);
        IEnumerable<AlunosPorTurmaItem> RelatorioAlunosPorTurma();
    }
}
