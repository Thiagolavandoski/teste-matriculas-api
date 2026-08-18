# matriculas-back

Teste prático back-end (.NET). API de controle de matrículas de uma escola
(solução `EscolaApi`).

## Stack

- .NET Framework 4.8 + ASP.NET Web API 2
- Dapper com SQL escrito à mão (sem ORM)
- SQL Server

## Como rodar

1. **Banco**: execute o `script-banco.sql` (raiz do repositório) em um SQL Server local.
   Ele cria o banco `TesteEscola`, as tabelas `Aluno`, `Turma` e `Matricula` e os dados de
   exemplo.

   ```powershell
   sqlcmd -S localhost -E -i script-banco.sql
   ```

2. **Connection string**: em `EscolaApi/Web.config`, ajuste a connection string
   `TesteEscola` se o seu servidor não for `localhost` com autenticação integrada
   (para LocalDB use `Server=(localdb)\MSSQLLocalDB;...`).

3. **Build e execução**: abra `EscolaApi.sln` no Visual Studio (com o workload de
   desenvolvimento web) e rode com IIS Express (F5), ou pela linha de comando:

   ```powershell
   msbuild EscolaApi.sln -t:Restore,Build -p:Configuration=Debug
   & "C:\Program Files\IIS Express\iisexpress.exe" /path:$PWD\EscolaApi /port:53000
   ```

4. Teste: `GET http://localhost:53000/api/health` deve responder `{"status":"ok"}`.

## Status

Casca do projeto: solução, Web API configurada (rotas por atributo, JSON camelCase) e
endpoint de health. Próximos passos: camada de dados com Dapper, CRUD de alunos, turmas,
matrícula transacional e relatório.
