using EscolaApi.Exceptions;
using EscolaApi.Infrastructure.Cache;
using EscolaApi.Models;
using EscolaApi.Models.Requests;
using EscolaApi.Repositories;

namespace EscolaApi.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly ITurmaRepository _turmaRepository;
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly ICacheProvider _cache;

        public MatriculaService(
            IAlunoRepository alunoRepository,
            ITurmaRepository turmaRepository,
            IMatriculaRepository matriculaRepository,
            ICacheProvider cache)
        {
            _alunoRepository = alunoRepository;
            _turmaRepository = turmaRepository;
            _matriculaRepository = matriculaRepository;
            _cache = cache;
        }

        public Matricula Matricular(MatriculaRequest request)
        {
            if (request == null || !request.AlunoId.HasValue || !request.TurmaId.HasValue)
                throw new RequisicaoInvalidaException("Informe o id do aluno e o id da turma.");

            var alunoId = request.AlunoId.Value;
            var turmaId = request.TurmaId.Value;

            var aluno = _alunoRepository.ObterPorId(alunoId);
            if (aluno == null)
                throw new RecursoNaoEncontradoException($"Aluno {alunoId} não encontrado.");

            if (!aluno.Ativo)
                throw new RegraDeNegocioException($"O aluno {aluno.Nome} está inativo e não pode ser matriculado.");

            var turma = _turmaRepository.ObterPorId(turmaId);
            if (turma == null)
                throw new RecursoNaoEncontradoException($"Turma {turmaId} não encontrada.");

            if (_matriculaRepository.Existe(alunoId, turmaId))
                throw new RegraDeNegocioException($"O aluno {aluno.Nome} já está matriculado na turma {turma.Nome}.");

            if (turma.VagasDisponiveis <= 0)
                throw new RegraDeNegocioException($"A turma {turma.Nome} não tem vagas disponíveis.");

            var matricula = _matriculaRepository.MatricularComDecremento(alunoId, turmaId);
            if (matricula == null)
                throw new RegraDeNegocioException($"A turma {turma.Nome} não tem vagas disponíveis.");

            _cache.Remover(TurmaService.ChaveCacheListagem);

            return matricula;
        }
    }
}