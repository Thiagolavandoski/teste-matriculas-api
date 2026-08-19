using EscolaApi.Models;

namespace EscolaApi.Repositories
{
    public interface IAlunoRepository
    {
        PagedResult<Aluno> Listar(string nome, int pagina, int tamanhoPagina);
        Aluno ObterPorId(int id);
        int Inserir(Aluno aluno);
        bool Atualizar(Aluno aluno);
        bool Desativar(int id);
    }
}
