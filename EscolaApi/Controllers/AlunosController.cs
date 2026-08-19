using System.Web.Http;
using EscolaApi.Infrastructure;
using EscolaApi.Models.Requests;
using EscolaApi.Repositories;
using EscolaApi.Services;

namespace EscolaApi.Controllers
{
    [RoutePrefix("api/alunos")]
    public class AlunosController : ApiController
    {
        private readonly IAlunoService _alunoService;

        public AlunosController() : this(new AlunoService(new AlunoRepository(new DbConnectionFactory())))
        {
        }

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Listar(string nome = null, int pagina = 1, int tamanhoPagina = 10)
        {
            return Ok(_alunoService.Listar(nome, pagina, tamanhoPagina));
        }

        [HttpGet]
        [Route("{id:int}", Name = "ObterAlunoPorId")]
        public IHttpActionResult ObterPorId(int id)
        {
            return Ok(_alunoService.ObterPorId(id));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Criar([FromBody] AlunoRequest request)
        {
            var aluno = _alunoService.Criar(request);
            return CreatedAtRoute("ObterAlunoPorId", new { id = aluno.Id }, aluno);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Atualizar(int id, [FromBody] AlunoRequest request)
        {
            return Ok(_alunoService.Atualizar(id, request));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Excluir(int id)
        {
            _alunoService.Excluir(id);
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}