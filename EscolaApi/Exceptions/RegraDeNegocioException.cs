using System;

namespace EscolaApi.Exceptions
{
    /// <summary>Regra de negócio impediu a operação. Vira HTTP 409.</summary>
    public class RegraDeNegocioException : Exception
    {
        public RegraDeNegocioException(string mensagem) : base(mensagem) { }
    }
}
