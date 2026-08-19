using System;
using EscolaApi.Exceptions;
using EscolaApi.Models;
using EscolaApi.Models.Requests;
using EscolaApi.Repositories;

namespace EscolaApi.Services
{
    public class AlunoService : IAlunoService
    {
        private const int TamanhoPaginaPadrao = 10;
        private const int TamanhoPaginaMaximo = 100;

        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public PagedResult<Aluno> Listar(string nome, int pagina, int tamanhoPagina)
        {
            if (pagina < 1)
                throw new RequisicaoInvalidaException("A página deve ser maior ou igual a 1.");

            if (tamanhoPagina < 1)
                tamanhoPagina = TamanhoPaginaPadrao;

            if (tamanhoPagina > TamanhoPaginaMaximo)
                throw new RequisicaoInvalidaException($"O tamanho de página máximo é {TamanhoPaginaMaximo}.");

            return _alunoRepository.Listar(string.IsNullOrWhiteSpace(nome) ? null : nome.Trim(), pagina, tamanhoPagina);
        }

        public Aluno ObterPorId(int id)
        {
            var aluno = _alunoRepository.ObterPorId(id);
            if (aluno == null)
                throw new RecursoNaoEncontradoException($"Aluno {id} não encontrado.");

            return aluno;
        }

        public Aluno Criar(AlunoRequest request)
        {
            var aluno = MontarAlunoValidado(request);
            aluno.Id = _alunoRepository.Inserir(aluno);
            return _alunoRepository.ObterPorId(aluno.Id);
        }

        public Aluno Atualizar(int id, AlunoRequest request)
        {
            ObterPorId(id);

            var aluno = MontarAlunoValidado(request);
            aluno.Id = id;
            _alunoRepository.Atualizar(aluno);

            return _alunoRepository.ObterPorId(id);
        }

        public void Excluir(int id)
        {
            ObterPorId(id);
            _alunoRepository.Desativar(id);
        }

        private static Aluno MontarAlunoValidado(AlunoRequest request)
        {
            if (request == null)
                throw new RequisicaoInvalidaException("O corpo da requisição é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new RequisicaoInvalidaException("O nome do aluno é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
                throw new RequisicaoInvalidaException("Informe um e-mail válido.");

            if (!request.DataNascimento.HasValue)
                throw new RequisicaoInvalidaException("A data de nascimento é obrigatória.");

            if (request.DataNascimento.Value.Date >= DateTime.Today)
                throw new RequisicaoInvalidaException("A data de nascimento deve estar no passado.");

            return new Aluno
            {
                Nome = request.Nome.Trim(),
                Email = request.Email.Trim(),
                DataNascimento = request.DataNascimento.Value.Date
            };
        }
    }
}