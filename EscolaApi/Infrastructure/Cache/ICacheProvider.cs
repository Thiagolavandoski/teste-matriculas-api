using System;

namespace EscolaApi.Infrastructure.Cache
{
    public interface ICacheProvider
    {
        T Obter<T>(string chave) where T : class;
        void Definir<T>(string chave, T valor, TimeSpan validade) where T : class;
        void Remover(string chave);
    }
}