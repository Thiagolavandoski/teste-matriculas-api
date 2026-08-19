using System;
using System.Collections.Generic;
using System.Linq;
using EscolaApi.Infrastructure.Cache;
using EscolaApi.Models;
using EscolaApi.Repositories;

namespace EscolaApi.Services
{
    public class TurmaService : ITurmaService
    {
        public const string ChaveCacheListagem = "turmas:listagem";
        private static readonly TimeSpan ValidadeCache = TimeSpan.FromMinutes(5);

        private readonly ITurmaRepository _turmaRepository;
        private readonly ICacheProvider _cache;

        public TurmaService(ITurmaRepository turmaRepository, ICacheProvider cache)
        {
            _turmaRepository = turmaRepository;
            _cache = cache;
        }

        public IEnumerable<Turma> Listar()
        {
            var emCache = _cache.Obter<List<Turma>>(ChaveCacheListagem);
            if (emCache != null)
                return emCache;

            var turmas = _turmaRepository.Listar().ToList();
            _cache.Definir(ChaveCacheListagem, turmas, ValidadeCache);
            return turmas;
        }

        public IEnumerable<AlunosPorTurmaItem> RelatorioAlunosPorTurma()
        {
            return _turmaRepository.RelatorioAlunosPorTurma();
        }
    }
}