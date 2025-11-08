# 🏍️ MotoHub

O **MotoHub** é uma aplicação desenvolvida como parte do **Challenge FIAP + Mottu**, com o objetivo de facilitar o gerenciamento de motos para aluguel, venda e manutenção. O sistema foi desenvolvido com foco em escalabilidade, simplicidade, qualidade de código e integração com bancos de dados relacionais.

---

## 👨‍💻 Integrantes

- Caroline Assis Silva — RM 557596  
- Enzo de Moura Silva — RM 556532  
- Luis Henrique Gomes Cardoso — RM 558883  

---

## 🚀 Funcionalidades Principais

### ✅ Versão 1 (V1) — Sprint Anterior
- 📋 CRUD completo de motos  
- 🧍 CRUD de clientes  
- 💸 Registro de vendas e aluguéis  
- 🛠️ Indicação de manutenção  
- 🔗 HATEOAS implementado  
- 🌐 Documentação com Swagger  

### ✅ Versão 2 (V2) — Sprint Atual
- 🔑 **Segurança com API Key**  
- ✅ Middleware personalizado validando API Key  
- 🧪 Testes unitários com xUnit  
- ❤️ Health Check para verificação de disponibilidade  
- 🤖 Integração com Machine Learning (Modelo de Avaliação de Motos)

---

## 🛠️ Tecnologias Utilizadas

- .NET 8 (ASP.NET Core Web API)  
- C#  
- Entity Framework Core  
- Oracle Database  
- Swagger (Swashbuckle)  
- **API Key Authentication** (custom middleware)  
- xUnit (Testes Unitários)  
- Machine Learning Model  
- HATEOAS  

---

## 🧪 Como Executar o Projeto

### 1️⃣ Clone o repositório

```bash
git clone https://github.com/codecrazes/4Sprint.NET.git
cd 4Sprint.NET/MotoHub
```
- Restaure as dependências e execute o projeto:

```bash
dotnet restore

dotnet run
```

### 🧪 Como Rodar os Testes (xUnit)

```bash
cd MotoHub.Tests
```
```bash
dotnet test
```

#### Resultado esperado:

```bash
Resumo do teste: total: 8; falhou: 0; bem-sucedido: 8; ignorado: 0
```

## 🌐 Documentação da API

Swagger disponível em:

[http://localhost:5104/swagger](http://localhost:5104/swagger/index.html)

---

## 🔐 Autenticação — API Key

A API utiliza **segurança baseada em API Key**, enviada no header:

### ✅ API Key para Testes:

```bash
minha-chave-super-secreta
```

## 🔄 Exemplos de Requisições (JSON para Teste)

POST /api/moto

```json
{
  "modelo": "CB 650R",
  "marca": "Honda",
  "ano": 2023,
  "placa": "FTR9B21",
  "preco": 48990.00
}
```
POST /api/cliente

```json
{
  "nome": "Fernanda"
  "cpf": "56238920466",
  "telefone": "(11) 91503-1010",
  "email": "fernanda@email.com"
}
```

POST /api/aluguel

```json
{
  "motoId": 21,
  "clienteId": 21,
  "dataInicio": "2025-05-01T00:00:00",
  "dataFim": "2025-05-10T00:00:00"
}
```

### 🤖 Machine Learning — Modelo de Avaliação

Exemplo de Requisição:

```bash
{
  "ano": 2025,
  "km": 0,
  "numeroDeManutencoes": 0
}
```

```bash
{
  "ano": 2019,
  "km": 45000,
  "numeroDeManutencoes": 3
}
```

```bash
{
  "ano": 2014,
  "km": 90000,
  "numeroDeManutencoes": 6
}
```
