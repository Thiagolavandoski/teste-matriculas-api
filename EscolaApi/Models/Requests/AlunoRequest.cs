using System;

namespace EscolaApi.Models.Requests
{
    public class AlunoRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime? DataNascimento { get; set; }
    }
}