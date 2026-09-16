# FIAP Cloud Games - Notifications Lambda

AWS Lambda responsável pelo processamento de notificações do **FIAP Cloud Games**.

A aplicação consome eventos através do **Amazon MQ for RabbitMQ** e armazena as notificações no **MongoDB Atlas**.

---

## 🏗️ Arquitetura

```text
Users API ────────┐
                  │
Catalog API ──────┼──► Amazon MQ ──► AWS Lambda
                  │                    │
Payments API ─────┘                    ▼
                                MongoDB Atlas
```

A Lambda substitui a antiga Notifications API no ambiente AWS, utilizando uma arquitetura serverless.

---

## ⚙️ Tecnologias

* .NET 9
* C#
* AWS Lambda
* Amazon MQ for RabbitMQ
* Amazon ECR
* MongoDB Atlas
* Docker
* Git Submodule
* FCG.Contracts

| Tecnologia               | Utilização                                                                               |
| ------------------------ | ---------------------------------------------------------------------------------------- |
| **AWS Lambda**           | Execução serverless da aplicação, processando mensagens de forma automática e escalável. |
| **Amazon MQ (RabbitMQ)** | Gerenciamento das filas utilizadas para o processamento assíncrono das notificações.     |
| **Amazon ECR**           | Armazenamento da imagem Docker utilizada pelo AWS Lambda.                                |
| **AWS Secrets Manager**  | Armazenamento seguro de credenciais e informações sensíveis da aplicação.                |
| **Docker**               | Containerização e padronização do ambiente da aplicação.                                 |
| **MongoDB Atlas**        | Banco NoSQL em nuvem para armazenamento das notificações e histórico de processamento.   |


---

## 📂 Estrutura

```text
Lambda/
├── src/
│   └── NotificationsLambda/
│       ├── Function.cs
│       └── NotificationsLambda.csproj
│
└── libs/
    └── FCG.Contracts/
```

O projeto utiliza o `FCG.Contracts` como Git Submodule para compartilhamento dos contratos de eventos.

---

## 📨 Eventos

### UserCreatedEvent

Publicado pela Users API.

```text
IdUsuario
Nome
Email
CreatedAt
```

### PaymentProcessedEvent

Publicado pela Payments API.

```text
IdOrdemCompra
IdUsuario
IdJogo
Preco
Status
CreatedAt
```

---

## ☁️ AWS

A aplicação é executada como uma função **AWS Lambda** utilizando uma imagem Docker armazenada no **Amazon ECR**.

```text
Docker
   │
   ▼
Amazon ECR
   │
   ▼
AWS Lambda
   │
   ▼
Amazon MQ
   │
   ▼
MongoDB Atlas
```

O Amazon MQ utiliza **RabbitMQ** como broker de mensageria.

---

## 🐳 Docker

Build da imagem:

```bash
docker build -t fiap-cloud-games-notifications-lambda .
```

Após o build, a imagem é enviada para o Amazon ECR e utilizada pela função Lambda.

---

## 🔗 Git Submodule

Inicializar o submodule:

```bash
git submodule update --init --recursive
```

---

## 🎯 FIAP Cloud Games

Este projeto faz parte do **FIAP Cloud Games**, desenvolvido durante a Pós-graduação em Arquitetura de Sistemas .NET da FIAP.

Principais tecnologias da solução:

* .NET
* Docker
* Kubernetes
* RabbitMQ / Amazon MQ
* Redis
* MongoDB
* SQL Server
* Kong
* AWS Lambda

---
