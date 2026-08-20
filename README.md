# EscolaApi — Controle de Matrículas

API REST para a secretaria de uma escola administrar alunos, turmas e matrículas.
Cadastra e consulta alunos, mostra as vagas restantes de cada turma, matricula um aluno
em uma turma aplicando as regras do negócio (turma precisa ter vaga, aluno precisa estar
ativo e não pode repetir matrícula na mesma turma) e emite o relatório de alunos por
turma. A matrícula grava o registro e desconta a vaga dentro de uma única transação: ou
as duas coisas acontecem, ou nenhuma.

## Tecnologias

- **.NET Framework 4.8** e **ASP.NET Web API 2** — a API e o roteamento
- **Dapper 2.1** — acesso a dados com SQL escrito à mão (sem ORM gerando consulta)
- **SQL Server** — banco `TesteEscola`
- **Swashbuckle 5.6** — documentação interativa em `/swagger`
- **MSTest** — testes unitários das regras de matrícula
- **jQuery** — tela simples de alunos servida pela própria API
- Cache em memória (`System.Runtime.Caching`) na listagem de turmas, atrás da interface
  `ICacheProvider`

## Pré-requisitos e Instalação

Antes de começar você precisa ter:

- **Visual Studio 2019 ou mais novo**, com a carga de trabalho "Desenvolvimento ASP.NET e
  Web" (ela já traz o .NET Framework 4.8 e o IIS Express)
- **SQL Server** — qualquer edição serve (Express e LocalDB inclusive)

Os pacotes NuGet (Dapper, Web API, Swashbuckle) são restaurados sozinhos no primeiro build.

### 1. Criar o banco

O `script-banco.sql`, na raiz do repositório, cria o banco `TesteEscola`, as três tabelas
e os dados de exemplo. Pelo terminal:

```powershell
sqlcmd -S localhost -E -i script-banco.sql
```

Ou abra o arquivo no SSMS e execute com F5. Rodar de novo recria tudo do zero.

### 2. Conferir a connection string

Em `EscolaApi/Web.config`:

```xml
<add name="TesteEscola"
     connectionString="Server=localhost;Database=TesteEscola;Integrated Security=true;" />
```

Ajuste o `Server=` se o seu SQL Server não for a instância padrão local:
LocalDB → `Server=(localdb)\MSSQLLocalDB;` · instância nomeada → `Server=.\SQLEXPRESS;`

### 3. Subir a API

Abra `EscolaApi.sln` no Visual Studio e aperte **F5**. O IIS Express sobe em
`http://localhost:53000` e a raiz já abre o Swagger.

Para conferir que está no ar: `http://localhost:53000/api/health` responde
`{"status":"ok"}`.

## Como Usar

### Pelo Swagger

`http://localhost:53000/swagger` lista todos os endpoints e executa qualquer um deles pelo
botão **Try it out!**. O spec fica em `/swagger/docs/v1` e pode ser importado no Postman
(Import → colar a URL).

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/alunos?nome=&pagina=1&tamanhoPagina=10` | Lista paginada (só ativos), filtro opcional por nome, retorna o total |
| GET | `/api/alunos/{id}` | Aluno por id |
| POST | `/api/alunos` | Cria — body: `{ "nome", "email", "dataNascimento" }` |
| PUT | `/api/alunos/{id}` | Atualiza |
| DELETE | `/api/alunos/{id}` | Exclusão lógica (marca `Ativo = 0`) |
| GET | `/api/turmas` | Turmas com vagas restantes |
| POST | `/api/matriculas` | Matricula — body: `{ "alunoId", "turmaId" }` |
| GET | `/api/relatorios/alunos-por-turma` | Alunos por turma, via SQL (JOIN + GROUP BY) |
| GET | `/api/health` | Sanidade |

Status devolvidos: `200/201/204` sucesso · `400` requisição inválida · `404` não
encontrado · `409` regra de negócio impediu (turma sem vaga, aluno inativo, matrícula
duplicada).

### Exemplo: matricular um aluno

```http
POST http://localhost:53000/api/matriculas
Content-Type: application/json

{ "alunoId": 8, "turmaId": 2 }
```

Resposta `201` com a matrícula criada. Consultando `/api/turmas` em seguida, a turma 2
aparece com uma vaga a menos.

Os dados de exemplo já trazem os casos de erro prontos para testar: o aluno 4 está
inativo (`409`), a turma 4 está lotada (`409`) e a turma 3 tem uma vaga só — matricule
dois alunos nela para ver a segunda tentativa ser recusada.

### Pela tela de alunos

Com a API rodando, `http://localhost:53000/Content/alunos.html` abre uma tela em HTML +
jQuery que consome `GET /api/alunos`, com busca por nome e paginação.

### Rodando os testes

Os testes unitários ficam no projeto `EscolaApi.Tests` e usam **MSTest** (`MSTest.TestFramework`
e `MSTest.TestAdapter` 3.6) — o framework de testes da própria Microsoft, com os atributos
`[TestClass]` / `[TestMethod]` e a classe `Assert`. Não é xUnit nem NUnit: o MSTest roda no
Test Explorer do Visual Studio sem instalar mais nada.

As dependências dos serviços são substituídas por **fakes escritos à mão** (classes que
implementam `IAlunoRepository`, `ITurmaRepository`, `IMatriculaRepository` e
`ICacheProvider`), sem biblioteca de mock. Por isso os 9 cenários da regra de matrícula
rodam sem banco e sem a API no ar.

No Visual Studio, use o menu **Test → Run All Tests**. Pela linha de comando:

```powershell
vstest.console.exe EscolaApi.Tests\bin\Debug\net48\EscolaApi.Tests.dll
```

## Contribuir

1. Crie uma branch a partir da `main` (`git checkout -b minha-melhoria`).
2. Mantenha o desenho em camadas: regra de negócio nos *services*, SQL nos *repositories*,
   controllers só com rota e status HTTP.
3. Cubra com teste unitário toda regra de negócio nova.
4. Rode **Test → Run All Tests** e garanta a suíte verde antes de abrir o Pull Request.
5. Descreva no PR o que muda e como testar.

## Observações

- O `script-banco.sql` fornecido no teste não foi alterado.
- O aluno excluído sai da listagem, mas `GET /api/alunos/{id}` ainda o retorna com
  `ativo: false` — a exclusão lógica preserva o histórico de matrículas.
