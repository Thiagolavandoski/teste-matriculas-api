using System.Collections.Generic;
using EscolaApi.Models;

namespace EscolaApi.Services
{
    public interface ITurmaService
    {
        IEnumerable<Turma> Listar();
        IEnumerable<AlunosPorTurmaItem> RelatorioAlunosPorTurma();
    }
}