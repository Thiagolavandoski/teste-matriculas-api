using EscolaApi.Exceptions;
using EscolaApi.Models;
using EscolaApi.Models.Requests;
using EscolaApi.Services;
using EscolaApi.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EscolaApi.Tests
{
    [TestClass]
    public class MatriculaServiceTests
    {
        private AlunoRepositoryFake _alunoRepository;
        private TurmaRepositoryFake _turmaRepository;
        private MatriculaRepositoryFake _matriculaRepository;
        private CacheProviderFake _cache;
        private MatriculaService _service;

        [TestInitialize]
        public void Setup()
        {
            _alunoRepository = new AlunoRepositoryFake
            {
                AlunoParaRetornar = new Aluno { Id = 1, Nome = "Ana Souza", Ativo = true }
            };
            _turmaRepository = new TurmaRepositoryFake
            {
                TurmaParaRetornar = new Turma { Id = 2, Nome = "3B", VagasDisponiveis = 5 }
            };
            _matriculaRepository = new MatriculaRepositoryFake
            {
                MatriculaParaRetornar = new Matricula { Id = 10, AlunoId = 1, TurmaId = 2 }
            };
            _cache = new CacheProviderFake();

            _service = new MatriculaService(_alunoRepository, _turmaRepository, _matriculaRepository, _cache);
        }

        private static MatriculaRequest RequestValido() => new MatriculaRequest { AlunoId = 1, TurmaId = 2 };

        [TestMethod]
        public void Matricular_ComDadosValidos_CriaMatriculaEInvalidaCacheDeTurmas()
        {
            var matricula = _service.Matricular(RequestValido());

            Assert.AreEqual(10, matricula.Id);
            Assert.IsTrue(_matriculaRepository.MatricularFoiChamado);
            CollectionAssert.Contains(_cache.ChavesRemovidas, TurmaService.ChaveCacheListagem);
        }

        [TestMethod]
        public void Matricular_SemInformarIds_Lanca400ENaoChegaAoRepositorio()
        {
            Assert.ThrowsException<RequisicaoInvalidaException>(
                () => _service.Matricular(new MatriculaRequest()));

            Assert.IsFalse(_matriculaRepository.MatricularFoiChamado);
        }

        [TestMethod]
        public void Matricular_AlunoInexistente_Lanca404()
        {
            _alunoRepository.AlunoParaRetornar = null;

            Assert.ThrowsException<RecursoNaoEncontradoException>(() => _service.Matricular(RequestValido()));
        }

        [TestMethod]
        public void Matricular_AlunoInativo_Lanca409()
        {
            _alunoRepository.AlunoParaRetornar.Ativo = false;

            Assert.ThrowsException<RegraDeNegocioException>(() => _service.Matricular(RequestValido()));

            Assert.IsFalse(_matriculaRepository.MatricularFoiChamado);
        }

        [TestMethod]
        public void Matricular_TurmaInexistente_Lanca404()
        {
            _turmaRepository.TurmaParaRetornar = null;

            Assert.ThrowsException<RecursoNaoEncontradoException>(() => _service.Matricular(RequestValido()));
        }

        [TestMethod]
        public void Matricular_AlunoJaMatriculadoNaTurma_Lanca409()
        {
            _matriculaRepository.JaExisteMatricula = true;

            Assert.ThrowsException<RegraDeNegocioException>(() => _service.Matricular(RequestValido()));

            Assert.IsFalse(_matriculaRepository.MatricularFoiChamado);
        }

        [TestMethod]
        public void Matricular_TurmaSemVagas_Lanca409SemTentarGravar()
        {
            _turmaRepository.TurmaParaRetornar.VagasDisponiveis = 0;

            Assert.ThrowsException<RegraDeNegocioException>(() => _service.Matricular(RequestValido()));

            Assert.IsFalse(_matriculaRepository.MatricularFoiChamado);
        }

        [TestMethod]
        public void Matricular_UltimaVagaOcupadaEntreChecagemEGravacao_Lanca409()
        {
            _matriculaRepository.MatriculaParaRetornar = null;

            Assert.ThrowsException<RegraDeNegocioException>(() => _service.Matricular(RequestValido()));
        }

        [TestMethod]
        public void Matricular_QuandoFalha_NaoInvalidaOCache()
        {
            _turmaRepository.TurmaParaRetornar.VagasDisponiveis = 0;

            try { _service.Matricular(RequestValido()); }
            catch (RegraDeNegocioException) { }

            Assert.AreEqual(0, _cache.ChavesRemovidas.Count);
        }
    }
}