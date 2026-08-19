using System;
using System.Runtime.Caching;

namespace EscolaApi.Infrastructure.Cache
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;

        public T Obter<T>(string chave) where T : class
        {
            return Cache.Get(chave) as T;
        }

        public void Definir<T>(string chave, T valor, TimeSpan validade) where T : class
        {
            if (valor == null) return;
            Cache.Set(chave, valor, DateTimeOffset.Now.Add(validade));
        }

        public void Remover(string chave)
        {
            Cache.Remove(chave);
        }
    }
}