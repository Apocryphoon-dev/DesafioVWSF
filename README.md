# DesafioVWFS

Desafio técnico para vaga de Analista Sênior (C#/.NET) — Instituição Financeira (Volkswagen Financial Services).

API para gestão de **contratos de financiamento de veículos**, permitindo cadastro de contratos, registro de pagamentos de parcelas e geração de resumo consolidado por cliente.

## Stack

- **.NET 10 / ASP.NET Core** (Web API)
- **PostgreSQL** (via Entity Framework Core + Npgsql)
- **Swagger / OpenAPI** para documentação
- **Serilog** para logs estruturados (console + arquivo) com Correlation ID
- **xUnit + Moq + EF Core InMemory** para testes automatizados
- **Docker + docker-compose** para execução local

## Arquitetura

```
DesafioVWFS/
├── Controllers/           # Endpoints HTTP (Contratos, Pagamentos, Clientes)
├── Application/
│   ├── DTOs/               # Requests/Responses expostos pela API
│   ├── Features/           # Casos de uso, organizados por domínio (Contracts, Payments, Clients)
│   │   └── <Domínio>/
│   │       ├── <CasoDeUso>/
│   │       │   ├── Models/     # Input/Output do caso de uso
│   │       │   └── Handlers/   # UseCase (regra de negócio)
│   │       └── <Domínio>ApplicationService.cs  # Fachada usada pelo Controller
│   ├── Validators/         # Validação de CPF/CNPJ e FluentValidation dos Inputs
│   └── Shared/              # Código compartilhado entre todas as Features
│       ├── Core/             # Infraestrutura do padrão UseCase (IRequest, IUseCase, Output, UseCaseHandlerBase)
│       ├── Domain/           # Modelo de domínio puro: Entities e Enums (sem dependências)
│       ├── Repository/       # Interfaces + implementações de acesso a dados (EF Core)
│       └── Services/         # Interfaces + implementações de serviços de domínio (cálculo de parcela)
├── Data/                  # DbContext e mapeamento das tabelas
├── Migrations/            # Migrations do EF Core
└── DependencyInjection/   # Configuração de Swagger, token fixo, logging, middlewares
```

Padrão em camadas orientado a casos de uso: **Controller → ApplicationService (fachada) → UseCase/Handler → Repository → EF Core/PostgreSQL**, com DTOs isolando o contrato público da API do modelo de domínio.

Cada caso de uso segue a mesma estrutura: um `Input` (implementa `IRequest<TOutput>`), um `Output` com o resultado, um `Handler` que herda de `UseCaseHandlerBase` (responsável por validar via FluentValidation, executar a regra de negócio e tratar exceções) e, opcionalmente, um `Validator`. As `ApplicationService` (`ContractsApplicationService`, `PaymentsApplicationService`, `ClientsApplicationService`) atuam como fachada entre os Controllers e os UseCases, traduzindo DTOs da API para os Inputs/Outputs dos casos de uso.

## Como executar

### Pré-requisitos

- [Docker](https://www.docker.com/) e Docker Compose

### Subindo com Docker

```bash
docker-compose up --build
```

Isso sobe:

- `db`: PostgreSQL na porta `5432`
- `api`: a aplicação na porta `8080`
- `migrator`: aplica as migrations automaticamente no banco

Após subir, acesse o Swagger em:

```
http://localhost:8080/
```

### Executando localmente (sem Docker)

```bash
cd DesafioVWFS
dotnet restore
dotnet ef database update
dotnet run
```

> Ajuste a connection string em `appsettings.Development.json` para apontar para o seu PostgreSQL local.

## Autenticação

Rotas protegidas exigem um token fixo enviado no header `Authorization`:

```
Authorization: Bearer desafio-token-fixo
```

O token pode ser configurado em `appsettings.json` (`Authentication:FixedToken`). No Swagger, use o botão **Authorize** e informe `Bearer desafio-token-fixo`.

## Endpoints

### Contratos

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/contratos` | Cria um novo contrato de financiamento |
| `GET` | `/api/contratos/{id}` | Obtém um contrato pelo ID |
| `GET` | `/api/contratos?page=1&pageSize=10&ordenarDescendente=false` | Lista contratos com paginação/ordenação |
| `DELETE` | `/api/contratos/{id}` | Inativa (soft delete) um contrato |

Exemplo de payload (`POST /api/contratos`):

```json
{
  "clienteCpfCnpj": "123.456.789-09",
  "valorTotal": 50000,
  "taxaMensal": 1.99,
  "prazoMeses": 24,
  "dataVencimentoPrimeiraParcela": "2026-08-15",
  "tipoVeiculo": "AUTOMOVEL",
  "condicaoVeiculo": "USADO"
}
```

Valores aceitos:
- `tipoVeiculo`: `AUTOMOVEL`, `MOTO`, `CAMINHAO`, `ONIBUS`
- `condicaoVeiculo`: `NOVO`, `USADO`, `SEMINOVO`

### Pagamentos

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/contratos/{contratoId}/pagamentos` | Registra o pagamento de uma parcela |
| `GET` | `/api/contratos/{contratoId}/pagamentos` | Lista os pagamentos de um contrato |
| `GET` | `/api/contratos/{contratoId}/pagamentos/{pagamentoId}` | Obtém um pagamento específico |

Regras de negócio aplicadas:
- O valor da parcela, juros e amortização são calculados pela tabela Price a partir do saldo devedor atual e do prazo restante.
- O **saldo devedor** é atualizado a cada novo pagamento registrado.
- O status do pagamento é calculado automaticamente comparando `dataPagamento` com `dataVencimento`:
  - Sem `dataPagamento` e vencimento no futuro → `EMDIA`
  - Sem `dataPagamento` e vencimento no passado → `ATRASADO`
  - `dataPagamento` após o vencimento → `ATRASADO`
  - `dataPagamento` antes do vencimento → `ANTECIPADO`
  - `dataPagamento` na mesma data do vencimento → `EMDIA`

### Resumo do cliente

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/clientes/{cpfCnpj}/resumo` | Resumo consolidado do cliente |

Retorna: quantidade de contratos ativos, total de parcelas, parcelas pagas/em atraso/a vencer, percentual de parcelas pagas em dia e saldo devedor consolidado.

## Validações

- `clienteCpfCnpj` é validado com o algoritmo oficial de dígitos verificadores de CPF (11 dígitos) e CNPJ (14 dígitos).

## Logs e observabilidade

- Logs estruturados via Serilog, gravados em console e em `logs/app-{data}.txt`.
- Cada requisição recebe um **Correlation ID** (`X-Correlation-ID`), gerado automaticamente ou propagado se enviado no header, e incluído em todos os logs da requisição (incluindo exceções não tratadas).

## Testes

O projeto `DesafioVWFS.Tests` contém testes unitários com xUnit, Moq e EF Core InMemory, cobrindo:

- Cálculo de parcela e saldo devedor (`PagamentoCalculoServiceTests`)
- Regras de resumo do cliente (`GetSummaryUseCaseTests`)
- Regras de repositório de contratos (`ContratoRepositoryTests`)

Para rodar os testes:

```bash
dotnet test
```
