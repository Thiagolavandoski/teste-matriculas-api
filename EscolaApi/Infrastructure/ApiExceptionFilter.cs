using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using EscolaApi.Exceptions;

namespace EscolaApi.Infrastructure
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            HttpStatusCode status;

            switch (context.Exception)
            {
                case RequisicaoInvalidaException _:
                    status = HttpStatusCode.BadRequest;
                    break;
                case RecursoNaoEncontradoException _:
                    status = HttpStatusCode.NotFound;
                    break;
                case RegraDeNegocioException _:
                    status = HttpStatusCode.Conflict;
                    break;
                default:
                    return;
            }

            context.Response = context.Request.CreateResponse(
                status, new { mensagem = context.Exception.Message });
        }
    }
}