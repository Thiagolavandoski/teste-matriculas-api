using System.Web.Http;
using EscolaApi.Infrastructure;
using EscolaApi.Infrastructure.Cache;
using EscolaApi.Models.Requests;
using EscolaApi.Repositories;
using EscolaApi.Services;

namespace EscolaApi.Controllers
{
    [RoutePrefix("api/matriculas")]
    public class MatriculasController : ApiController
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculasController() : this(CriarServicePadrao())
        {
        }

        public MatriculasController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Matricular([FromBody] MatriculaRequest request)
        {
            var matricula = _matriculaService.Matricular(request);
            return Created($"api/matriculas/{matricula.Id}", matricula);
        }

        private static IMatriculaService CriarServicePadrao()
        {
            var connectionFactory = new DbConnectionFactory();
            return new MatriculaService(
                new AlunoRepository(connectionFactory),
                new TurmaRepository(connectionFactory),
                new MatriculaRepository(connectionFactory),
                new MemoryCacheProvider());
        }
    }
}