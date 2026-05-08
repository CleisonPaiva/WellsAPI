# WellsAPI

A geospatial REST API for managing and visualizing oil & gas well data from ANP (Brazilian National Petroleum Agency). Built with ASP.NET Core, Entity Framework, and PostgreSQL.

---

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core (.NET 10)
- **ORM:** Entity Framework Core
- **Database:** PostgreSQL
- **Bulk Operations:** EFCore.BulkExtensions
- **Documentation:** Swagger UI / OpenAPI

---

## ⚙️ Prerequisites

- .NET 10 SDK
- PostgreSQL (local or Docker container)
- Visual Studio 2022+

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/CleisonPaiva/WellsAPI.git
cd WellsAPI
```

### 2. Configure the database connection

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=wellsdb;Username=default;Password=secret"
  }
}
```

### 3. Run migrations

```bash
dotnet ef database update
```

### 4. Run the project

```bash
dotnet run
```

API will be available at: `https://localhost:7101`  
Swagger UI: `https://localhost:7101/swagger`

---

## 📡 API Endpoints

### Import Wells
**POST** `/api/WellsImport`

Imports well data from a CSV file (semicolon-separated, Latin-1 encoding).  
Supports upsert — duplicate wells are updated, not duplicated.

```
POST https://localhost:7101/api/WellsImport
Content-Type: multipart/form-data
Body: file = <your-csv-file>
```

---

### List Wells (Paginated)
**GET** `/api/Wells`

Returns a paginated list of wells with optional filters.

```
GET https://localhost:7101/api/Wells?Page=1&PageSize=20
```

**Query Parameters:**

| Parameter | Type | Description |
|---|---|---|
| Page | int | Page number (default: 1) |
| PageSize | int | Records per page (default: 20) |
| Name | string | Filter by well name |
| NameWellOperator | string | Filter by well operator name |
| OrganizationNumber | string | Filter by organization number |
| Operator | string | Filter by operator |
| State | string | Filter by state |
| Basin | string | Filter by basin |
| Status | string | Filter by operational status |
| Classification | string | Filter by technical classification |

**Response:**

```json
{
  "data": [
    {
      "name": "1-AA-1-RN",
      "nameWellOperator": "1AA  0001  RN",
      "organizationNumber": "721000712200",
      "operator": "",
      "state": "RN",
      "basin": "Potiguar",
      "status": "CEDIDO PARA A CAPTAÇÃO DE ÁGUA",
      "classification": "SECO SEM INDÍCIOS",
      "latitude": -4.9180855555,
      "longitude": -37.2246447222
    }
  ],
  "totalCount": 32000,
  "page": 1,
  "pageSize": 20
}
```

---

### Map Endpoint (Bounding Box)
**GET** `/api/Wells/map`

Returns well points within a geographic bounding box. Designed for map rendering with clustering. Bounding box parameters are required.

```
GET https://localhost:7101/api/Wells/map?MinLatitude=-6&MaxLatitude=-4&MinLongitude=-38&MaxLongitude=-36
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|---|---|---|---|
| MinLatitude | decimal | ✅ | Minimum latitude of the visible area |
| MaxLatitude | decimal | ✅ | Maximum latitude of the visible area |
| MinLongitude | decimal | ✅ | Minimum longitude of the visible area |
| MaxLongitude | decimal | ✅ | Maximum longitude of the visible area |
| Name | string | ❌ | Filter by well name |
| Operator | string | ❌ | Filter by operator |
| State | string | ❌ | Filter by state |
| Basin | string | ❌ | Filter by basin |
| Status | string | ❌ | Filter by status |

**Response:**

```json
[
  {
    "name": "1-AA-1-RN",
    "status": "CEDIDO PARA A CAPTAÇÃO DE ÁGUA",
    "latitude": -4.9180855555,
    "longitude": -37.2246447222
  }
]
```

---

### Well Detail
**GET** `/api/Wells/{name}`

Returns full details of a specific well by name.

```
GET https://localhost:7101/api/wells/1-AA-1-RN
```

**Response:**

```json
{
  "name": "1-AA-1-RN",
  "nameWellOperator": "1AA  0001  RN",
  "organizationNumber": "721000712200",
  "operator": "",
  "state": "RN",
  "basin": "Potiguar",
  "status": "CEDIDO PARA A CAPTAÇÃO DE ÁGUA",
  "classification": "SECO SEM INDÍCIOS",
  "latitude": -4.9180855555,
  "longitude": -37.2246447222
}
```

---

## 🏗️ Project Structure

```
WellsAPI/
├── Controllers/
│   ├── WellsController.cs        # GET endpoints
│   └── WellsImportController.cs  # POST import endpoint
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── WellFilterDto.cs          # Query filters + pagination
│   ├── WellResponseDto.cs        # Full response DTO
│   ├── WellMapDto.cs             # Lightweight map DTO
│   └── PaginatedResponseDto.cs   # Generic paginated wrapper
├── Entities/
│   └── Well.cs
├── Migrations/
├── Services/
│   ├── WellService.cs            # Query logic
│   └── WellImportService.cs      # CSV import logic
└── Program.cs
```

---

## 📋 CSV Format

The import endpoint expects a semicolon-separated CSV file with the following column mapping:

| Column Index | CSV Column | Entity Field |
|---|---|---|
| 1 | POCO | Name |
| 2 | CADASTRO | OrganizationNumber |
| 3 | OPERADOR | Operator |
| 4 | POCO_OPERADOR | NameWellOperator |
| 5 | ESTADO | State |
| 6 | BACIA | Basin |
| 14 | RECLASSIFICACAO | Classification |
| 15 | SITUACAO | Status |
| 22 | LATITUDE_BASE_DD | Latitude |
| 23 | LONGITUDE_BASE_DD | Longitude |

> File encoding: **Latin-1 (Windows-1252)**

---

## 📦 Data Source

Well data sourced from **ANP (Agência Nacional do Petróleo, Gás Natural e Biocombustíveis)** — Brazil's National Petroleum Agency.

- **Portal:** [ANP Public Data — Well Search](https://cdp.anp.gov.br/ords/r/cdp_apex/consulta-dados-publicos-cdp/consulta-de-po%C3%A7os)
- **Dataset:** Consulta de Poços
- **Format:** CSV (semicolon-separated, Latin-1 encoding)
- **Coverage:** All oil & gas wells registered with ANP across Brazil

---

---

# WellsAPI (Português)

API REST geoespacial para gerenciamento e visualização de dados de poços de petróleo e gás da ANP. Desenvolvida com ASP.NET Core, Entity Framework e PostgreSQL.

---

## 🛠️ Stack Tecnológica

- **Backend:** ASP.NET Core (.NET 10)
- **ORM:** Entity Framework Core
- **Banco de Dados:** PostgreSQL
- **Operações em Massa:** EFCore.BulkExtensions
- **Documentação:** Swagger UI / OpenAPI

---

## ⚙️ Pré-requisitos

- .NET 10 SDK
- PostgreSQL (local ou container Docker)
- Visual Studio 2022+

---

## 🚀 Como Executar

### 1. Clonar o repositório

```bash
git clone https://github.com/CleisonPaiva/WellsAPI.git
cd WellsAPI
```

### 2. Configurar a conexão com o banco

Edite o `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=wellsdb;Username=default;Password=secret"
  }
}
```

### 3. Executar as migrations

```bash
dotnet ef database update
```

### 4. Executar o projeto

```bash
dotnet run
```

A API estará disponível em: `https://localhost:7101`  
Swagger UI: `https://localhost:7101/swagger`

---

## 📡 Endpoints

### Importar Poços
**POST** `/api/WellsImport`

Importa dados de poços a partir de um arquivo CSV separado por ponto e vírgula, codificado em Latin-1. Suporta upsert — registros duplicados são atualizados, não duplicados.

```
POST https://localhost:7101/api/WellsImport
Content-Type: multipart/form-data
Body: file = <seu-arquivo-csv>
```

---

### Listar Poços (Paginado)
**GET** `/api/Wells`

Retorna uma lista paginada de poços com filtros opcionais.

```
GET https://localhost:7101/api/Wells?Page=1&PageSize=20
```

**Parâmetros:**

| Parâmetro | Tipo | Descrição |
|---|---|---|
| Page | int | Número da página (padrão: 1) |
| PageSize | int | Registros por página (padrão: 20) |
| Name | string | Filtrar por nome do poço |
| NameWellOperator | string | Filtrar por nome do poço/operador |
| OrganizationNumber | string | Filtrar por número de cadastro |
| Operator | string | Filtrar por operador |
| State | string | Filtrar por estado |
| Basin | string | Filtrar por bacia |
| Status | string | Filtrar por situação |
| Classification | string | Filtrar por reclassificação |

---

### Endpoint do Mapa (Bounding Box)
**GET** `/api/Wells/map`

Retorna os poços dentro de uma área geográfica visível no mapa. Os parâmetros de bounding box são obrigatórios. Projetado para renderização com clustering.

```
GET https://localhost:7101/api/Wells/map?MinLatitude=-6&MaxLatitude=-4&MinLongitude=-38&MaxLongitude=-36
```

**Parâmetros:**

| Parâmetro | Tipo | Obrigatório | Descrição |
|---|---|---|---|
| MinLatitude | decimal | ✅ | Latitude mínima da área visível |
| MaxLatitude | decimal | ✅ | Latitude máxima da área visível |
| MinLongitude | decimal | ✅ | Longitude mínima da área visível |
| MaxLongitude | decimal | ✅ | Longitude máxima da área visível |
| Name | string | ❌ | Filtrar por nome do poço |
| Operator | string | ❌ | Filtrar por operador |
| State | string | ❌ | Filtrar por estado |
| Basin | string | ❌ | Filtrar por bacia |
| Status | string | ❌ | Filtrar por situação |

---

### Detalhe do Poço
**GET** `/api/Wells/{name}`

Retorna os dados completos de um poço específico pelo nome.

```
GET https://localhost:7101/api/wells/1-AA-1-RN
```

---

## 🏗️ Estrutura do Projeto

```
WellsAPI/
├── Controllers/
│   ├── WellsController.cs        # Endpoints GET
│   └── WellsImportController.cs  # Endpoint de importação
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── WellFilterDto.cs          # Filtros e paginação
│   ├── WellResponseDto.cs        # DTO de resposta completa
│   ├── WellMapDto.cs             # DTO leve para o mapa
│   └── PaginatedResponseDto.cs   # Wrapper genérico paginado
├── Entities/
│   └── Well.cs
├── Migrations/
├── Services/
│   ├── WellService.cs            # Lógica de consulta
│   └── WellImportService.cs      # Lógica de importação CSV
└── Program.cs
```

---

## 📋 Formato do CSV

O endpoint de importação espera um arquivo CSV separado por ponto e vírgula com o seguinte mapeamento de colunas:

| Índice | Coluna CSV | Campo |
|---|---|---|
| 1 | POCO | Name |
| 2 | CADASTRO | OrganizationNumber |
| 3 | OPERADOR | Operator |
| 4 | POCO_OPERADOR | NameWellOperator |
| 5 | ESTADO | State |
| 6 | BACIA | Basin |
| 14 | RECLASSIFICACAO | Classification |
| 15 | SITUACAO | Status |
| 22 | LATITUDE_BASE_DD | Latitude |
| 23 | LONGITUDE_BASE_DD | Longitude |

> Encoding do arquivo: **Latin-1 (Windows-1252)**

---

## 📦 Fonte de Dados

Dados de poços fornecidos pela **ANP (Agência Nacional do Petróleo, Gás Natural e Biocombustíveis)**.

- **Portal:** [Consulta de Poços — ANP](https://cdp.anp.gov.br/ords/r/cdp_apex/consulta-dados-publicos-cdp/consulta-de-po%C3%A7os)
- **Dataset:** Consulta de Poços
- **Formato:** CSV (separado por ponto e vírgula, codificado em Latin-1)
- **Cobertura:** Todos os poços de petróleo e gás registrados na ANP em território brasileiro
