# API de Reservas de Salas – Backend (.NET)

Este repositório contém a API desenvolvida em .NET para um sistema de **reserva de salas**.

A API foi construída utilizando:
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQLite como banco de dados**
- **Swagger/OpenAPI**
- **Padrão REST**
- **Validações de domínio**
- **Idempotência e regras de negócio**

---

## Funcionalidades

### (Local)
- Criar locais
- Listar locais
- Editar
- Deletar

### (Sala)
- Criar salas vinculadas a um Place
- Listar salas
- Editar
- Deletar
- Validação de Place existente

### (Reserva)
- Criar reserva com validação de conflito de horário
- Listar reservas
- Editar e excluir
- Opção de solicitar café
- Validação de disponibilidade da sala

##  Tecnologias Utilizadas

| Tecnologia | Descrição |
|-----------|-----------|
| **.NET 7 / 8** | Framework para API |
| **ASP.NET Core Web API** | Estrutura REST |
| **SQLite** | Banco de dados local |
| **Entity Framework Core** | ORM |
| **Swagger / Swashbuckle** | Documentação da API |
| **C#** | Linguagem principal |
| **Migrations** | Controle de estrutura do banco |


