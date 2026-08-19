using EscolaApi.Models;
using EscolaApi.Models.Requests;

namespace EscolaApi.Services
{
    public interface IMatriculaService
    {
        Matricula Matricular(MatriculaRequest request);
    }
}