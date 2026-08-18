using System.Web.Http;

namespace EscolaApi.Controllers
{
    [RoutePrefix("api/health")]
    public class HealthController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(new { status = "ok" });
        }
    }
}
