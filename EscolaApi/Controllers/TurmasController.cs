using System.Web.Http;
using EscolaApi.Infrastructure;
using EscolaApi.Infrastructure.Cache;
using EscolaApi.Repositories;
using EscolaApi.Services;

namespace EscolaApi.Controllers
{
    [RoutePrefix("api/turmas")]
    public class TurmasController : ApiController
    {
        private readonly ITurmaService _turmaService;

        public TurmasController() : this(new TurmaService(new TurmaRepository(new DbConnectionFactory()), new MemoryCacheProvider()))
        {
        }

        public TurmasController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Listar()
        {
            return Ok(_turmaService.Listar());
        }
    }
}