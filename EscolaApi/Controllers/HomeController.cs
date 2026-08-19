using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace EscolaApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Index()
        {
            return Redirect(new Uri(Request.RequestUri, "/swagger"));
        }
    }
}
