using System;

namespace EscolaApi.Exceptions
{
    /// <summary>Erro de validação de entrada. Vira HTTP 400.</summary>
    public class RequisicaoInvalidaException : Exception
    {
        public RequisicaoInvalidaException(string mensagem) : base(mensagem) { }
    }
}
