using System.Web.Http;
using EscolaApi.Infrastructure;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Application;

namespace EscolaApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new ApiExceptionFilter());

            config
                .EnableSwagger(c => c.SingleApiVersion("v1", "EscolaApi - Controle de Matrículas"))
                .EnableSwaggerUi();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
