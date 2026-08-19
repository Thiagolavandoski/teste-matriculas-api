using System.Web.Http;
using EscolaApi.Infrastructure;
using EscolaApi.Infrastructure.Cache;
using EscolaApi.Repositories;
using EscolaApi.Services;

namespace EscolaApi.Controllers
{
    [RoutePrefix("api/relatorios")]
    public class RelatoriosController : ApiController
    {
        private readonly ITurmaService _turmaService;

        public RelatoriosController() : this(new TurmaService(new TurmaRepository(new DbConnectionFactory()), new MemoryCacheProvider()))
        {
        }

        public RelatoriosController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        [HttpGet]
        [Route("alunos-por-turma")]
        public IHttpActionResult AlunosPorTurma()
        {
            return Ok(_turmaService.RelatorioAlunosPorTurma());
        }
    }
}