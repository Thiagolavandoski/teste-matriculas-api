using EscolaApi.Models;
using EscolaApi.Models.Requests;

namespace EscolaApi.Services
{
    public interface IAlunoService
    {
        PagedResult<Aluno> Listar(string nome, int pagina, int tamanhoPagina);
        Aluno ObterPorId(int id);
        Aluno Criar(AlunoRequest request);
        Aluno Atualizar(int id, AlunoRequest request);
        void Excluir(int id);
    }
}