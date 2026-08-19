using System;
using System.Collections.Generic;
using EscolaApi.Infrastructure.Cache;
using EscolaApi.Models;
using EscolaApi.Repositories;

namespace EscolaApi.Tests.Fakes
{
    public class AlunoRepositoryFake : IAlunoRepository
    {
        public Aluno AlunoParaRetornar { get; set; }

        public Aluno ObterPorId(int id) => AlunoParaRetornar;

        public PagedResult<Aluno> Listar(string nome, int pagina, int tamanhoPagina) => throw new NotImplementedException();

        public int Inserir(Aluno aluno) => throw new NotImplementedException();
        public bool Atualizar(Aluno aluno) => throw new NotImplementedException();
        public bool Desativar(int id) => throw new NotImplementedException();
    }

    public class TurmaRepositoryFake : ITurmaRepository
    {
        public Turma TurmaParaRetornar { get; set; }

        public Turma ObterPorId(int id) => TurmaParaRetornar;

        public IEnumerable<Turma> Listar() => throw new NotImplementedException();

        public IEnumerable<AlunosPorTurmaItem> RelatorioAlunosPorTurma() =>
            throw new NotImplementedException();
    }

    public class MatriculaRepositoryFake : IMatriculaRepository
    {
        public bool JaExisteMatricula { get; set; }
        public Matricula MatriculaParaRetornar { get; set; }
        public bool MatricularFoiChamado { get; private set; }

        public bool Existe(int alunoId, int turmaId) => JaExisteMatricula;

        public Matricula MatricularComDecremento(int alunoId, int turmaId)
        {
            MatricularFoiChamado = true;
            return MatriculaParaRetornar;
        }
    }

    public class CacheProviderFake : ICacheProvider
    {
        public List<string> ChavesRemovidas { get; } = new List<string>();

        public T Obter<T>(string chave) where T : class => null;

        public void Definir<T>(string chave, T valor, TimeSpan validade) where T : class { }

        public void Remover(string chave) => ChavesRemovidas.Add(chave);
    }
}