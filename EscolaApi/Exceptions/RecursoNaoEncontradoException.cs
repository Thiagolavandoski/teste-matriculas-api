using System;

namespace EscolaApi.Exceptions
{
    /// <summary>Registro não encontrado. Vira HTTP 404.</summary>
    public class RecursoNaoEncontradoException : Exception
    {
        public RecursoNaoEncontradoException(string mensagem) : base(mensagem) { }
    }
}
