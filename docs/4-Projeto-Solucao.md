
# 4. Projeto da Solução

> ⚠️ **Aviso aos Squads (Software House)**
>
> Esta seção **não deve ser preenchida integralmente antes da codificação**.
> Trata-se de um **Documento Vivo**, que deverá ser atualizado **incrementalmente a cada Sprint**, refletindo fielmente o código real implementado.

---

## 4.1 Arquitetura da Solução (Sprint 1 e 2)

Apresente um **diagrama macro** demonstrando como os componentes do sistema se comunicam.

A arquitetura deve refletir o modelo de **fatias verticais**, evidenciando o fluxo:

**Front-end → API (Back-end) → Banco de Dados**

Semelhante à imagem abaixo:

![Exemplo de Arquitetura](https://uds.com.br/blog/wp-content/uploads/2024/09/Imagem-1-Comparativo-ilustrativo-das-diferencas-entre-front-end-e-back-end.jpg)


 **Fonte:** [Guia Completo de Desenvolvimento de Software - UDS](https://uds.com.br/blog/desenvolvimento-de-software-guia-completo/) <br><br>
 
 ### 📎 Inserir o Diagrama de Arquitetura do Projeto do Grupo
🚨 O grupo deverá inserir aqui a imagem

![Exemplo de Arquitetura](https://drive.google.com/uc?export=view&id=1UGYk4cfXBTrfvGgDlrDyKn3siiIXQxJm)

---

## 4.2 Tecnologias Utilizadas (Sprint 1)

Descreva as tecnologias, linguagens, frameworks, bibliotecas e serviços escolhidos pelo Squad.

| Dimensão | Tecnologia Escolhida |
|----------|----------------------|
| Banco de Dados (SGBD) | PostgreSQL |
| Back-end (API) | C# (.NET Core) |
| Front-end / Mobile | React, Typescript, Tailwind|
| Hospedagem / Deploy | Em Análise (Observando opções) |
| Gestão e Versionamento | GitHub e GitHub Projects (Kanban) |

---

##  4.3 Wireframes ou Mockups (A partir da Sprint 2)

Apresente os protótipos das telas (Wireframes/Mockups) apenas das funcionalidades que estão sendo implementadas na Sprint atual.

Cada Wireframe ou Mockups devem estar associados a pelo menos:

- Um Requisito Funcional (RF-XX)
- Uma História de Usuário


## 📌 Exemplo Ilustrativo – Tela de Cadastro (RF-01)

**História associada:** Como usuário, quero criar uma conta para acessar o sistema.

Representação simplificada do Wireframe:

<img src="images/TelaCadastro.png" width="80%">

**Descrição:** A interface contempla todos os campos exigidos pelo RF-01 e permite persistência no banco após validação no backend.

---
🔧 **Ferramentas sugeridas:**
- Figma  
- MarvelApp  
- Balsamiq  
---

### 📎 Inserir AQUI Wireframes/ Mockups do Projeto de Software

🚨 O grupo deverá inserir aqui a imagem

---

## 4.4 Modelagem de Dados (Sprint 2 e 3)

O sistema exige persistência de dados.

A documentação do banco seguirá a abordagem de **entrega contínua**, sendo expandida conforme evolução do projeto.

---

### 4.4.1 Script Físico (Entrega na Sprint 2 - MVP)

```sql
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "UnitsOfMeasure" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Abbreviation" text NOT NULL,
    CONSTRAINT "PK_UnitsOfMeasure" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    "GoogleId" text NOT NULL,
    "Provider" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "CustomProducts" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Description" text,
    "UnitOfMeasureId" uuid,
    CONSTRAINT "PK_CustomProducts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CustomProducts_UnitsOfMeasure_UnitOfMeasureId" FOREIGN KEY ("UnitOfMeasureId") REFERENCES "UnitsOfMeasure" ("Id")
);

CREATE TABLE "OfficialProducts" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Description" text,
    "UnitOfMeasureId" uuid,
    "ImageUrl" text,
    "Barcode" text,
    CONSTRAINT "PK_OfficialProducts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OfficialProducts_UnitsOfMeasure_UnitOfMeasureId" FOREIGN KEY ("UnitOfMeasureId") REFERENCES "UnitsOfMeasure" ("Id")
);

CREATE TABLE "ShoppingCarts" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "CreatedByUserId" uuid NOT NULL,
    CONSTRAINT "PK_ShoppingCarts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ShoppingCarts_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "CartItems" (
    "Id" uuid NOT NULL,
    "ShoppingCartId" uuid NOT NULL,
    "OfficialProductId" uuid,
    "CustomProductId" uuid,
    "Quantity" numeric NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_CartItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CartItems_CustomProducts_CustomProductId" FOREIGN KEY ("CustomProductId") REFERENCES "CustomProducts" ("Id"),
    CONSTRAINT "FK_CartItems_OfficialProducts_OfficialProductId" FOREIGN KEY ("OfficialProductId") REFERENCES "OfficialProducts" ("Id"),
    CONSTRAINT "FK_CartItems_ShoppingCarts_ShoppingCartId" FOREIGN KEY ("ShoppingCartId") REFERENCES "ShoppingCarts" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserShoppingCarts" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "ShoppingCartId" uuid NOT NULL,
    CONSTRAINT "PK_UserShoppingCarts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserShoppingCarts_ShoppingCarts_ShoppingCartId" FOREIGN KEY ("ShoppingCartId") REFERENCES "ShoppingCarts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserShoppingCarts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_CartItems_CustomProductId" ON "CartItems" ("CustomProductId");

CREATE INDEX "IX_CartItems_OfficialProductId" ON "CartItems" ("OfficialProductId");

CREATE INDEX "IX_CartItems_ShoppingCartId" ON "CartItems" ("ShoppingCartId");

CREATE INDEX "IX_CustomProducts_UnitOfMeasureId" ON "CustomProducts" ("UnitOfMeasureId");

CREATE INDEX "IX_OfficialProducts_UnitOfMeasureId" ON "OfficialProducts" ("UnitOfMeasureId");

CREATE INDEX "IX_ShoppingCarts_CreatedByUserId" ON "ShoppingCarts" ("CreatedByUserId");

CREATE INDEX "IX_UserShoppingCarts_ShoppingCartId" ON "UserShoppingCarts" ("ShoppingCartId");

CREATE INDEX "IX_UserShoppingCarts_UserId" ON "UserShoppingCarts" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260121211914_InitialCreate', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "UserShoppingCarts" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "UserShoppingCarts" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "Users" ALTER COLUMN "Name" TYPE character varying(100);

ALTER TABLE "Users" ALTER COLUMN "Email" TYPE character varying(150);

ALTER TABLE "UnitsOfMeasure" ALTER COLUMN "Name" TYPE character varying(50);

ALTER TABLE "UnitsOfMeasure" ALTER COLUMN "Abbreviation" TYPE character varying(25);

ALTER TABLE "UnitsOfMeasure" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "UnitsOfMeasure" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "ShoppingCarts" ALTER COLUMN "Name" TYPE character varying(100);

ALTER TABLE "ShoppingCarts" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "OfficialProducts" ALTER COLUMN "Name" TYPE character varying(100);

ALTER TABLE "OfficialProducts" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "OfficialProducts" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "CustomProducts" ALTER COLUMN "Name" TYPE character varying(100);

ALTER TABLE "CustomProducts" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "CustomProducts" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260122172326_AddBaseEntityAndEntityValidations', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "CartItems" DROP CONSTRAINT "FK_CartItems_CustomProducts_CustomProductId";

ALTER TABLE "CartItems" DROP CONSTRAINT "FK_CartItems_OfficialProducts_OfficialProductId";

ALTER TABLE "CustomProducts" DROP CONSTRAINT "FK_CustomProducts_UnitsOfMeasure_UnitOfMeasureId";

ALTER TABLE "OfficialProducts" DROP CONSTRAINT "FK_OfficialProducts_UnitsOfMeasure_UnitOfMeasureId";

ALTER TABLE "ShoppingCarts" DROP CONSTRAINT "FK_ShoppingCarts_Users_CreatedByUserId";

ALTER TABLE "Users" DROP COLUMN "GoogleId";

ALTER TABLE "Users" DROP COLUMN "Provider";

CREATE TABLE "UserProviders" (
    "Id" uuid NOT NULL,
    "Provider" character varying(50) NOT NULL,
    "ProviderUserId" character varying(200) NOT NULL,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_UserProviders" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserProviders_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_UserProviders_UserId" ON "UserProviders" ("UserId");

ALTER TABLE "CartItems" ADD CONSTRAINT "FK_CartItems_CustomProducts_CustomProductId" FOREIGN KEY ("CustomProductId") REFERENCES "CustomProducts" ("Id") ON DELETE SET NULL;

ALTER TABLE "CartItems" ADD CONSTRAINT "FK_CartItems_OfficialProducts_OfficialProductId" FOREIGN KEY ("OfficialProductId") REFERENCES "OfficialProducts" ("Id") ON DELETE SET NULL;

ALTER TABLE "CustomProducts" ADD CONSTRAINT "FK_CustomProducts_UnitsOfMeasure_UnitOfMeasureId" FOREIGN KEY ("UnitOfMeasureId") REFERENCES "UnitsOfMeasure" ("Id") ON DELETE SET NULL;

ALTER TABLE "OfficialProducts" ADD CONSTRAINT "FK_OfficialProducts_UnitsOfMeasure_UnitOfMeasureId" FOREIGN KEY ("UnitOfMeasureId") REFERENCES "UnitsOfMeasure" ("Id") ON DELETE SET NULL;      

ALTER TABLE "ShoppingCarts" ADD CONSTRAINT "FK_ShoppingCarts_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260131202129_AddUserAuthentication', '8.0.0');

COMMIT;

START TRANSACTION;

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE UNIQUE INDEX "IX_UserProviders_Provider_ProviderUserId" ON "UserProviders" ("Provider", "ProviderUserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260205213524_AddUserUniquenessConstraints', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "CustomProducts" ADD "CreatedByUserId" uuid;

CREATE INDEX "IX_CustomProducts_CreatedByUserId" ON "CustomProducts" ("CreatedByUserId");

ALTER TABLE "CustomProducts" ADD CONSTRAINT "FK_CustomProducts_Users_CreatedByUserId" FOREIGN KEY ("CreatedByUserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260320223445_AddCustomProductUserOwnership', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "OfficialProducts" ADD "UnitQuantity" numeric;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260327224614_AddOfficialProductUnitQuantity', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE "Users" ADD "IsAdmin" boolean NOT NULL DEFAULT FALSE;

CREATE TABLE "OfficialProductRequests" (
    "Id" uuid NOT NULL,
    "OwnerId" uuid NOT NULL,
    "CustomProductId" uuid NOT NULL,
    "Status" integer NOT NULL,
    "Name" text NOT NULL,
    "Description" text,
    "UnitOfMeasureId" uuid,
    "UnitQuantity" numeric,
    "ImageUrl" text,
    "Barcode" text,
    "RejectedComment" text,
    "ReviewedByUserId" uuid,
    "ReviewedAt" timestamp with time zone,
    "OfficialProductId" uuid,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_OfficialProductRequests" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OfficialProductRequests_CustomProducts_CustomProductId" FOREIGN KEY ("CustomProductId") REFERENCES "CustomProducts" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_OfficialProductRequests_OfficialProducts_OfficialProductId" FOREIGN KEY ("OfficialProductId") REFERENCES "OfficialProducts" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_OfficialProductRequests_UnitsOfMeasure_UnitOfMeasureId" FOREIGN KEY ("UnitOfMeasureId") REFERENCES "UnitsOfMeasure" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_OfficialProductRequests_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_OfficialProductRequests_Users_ReviewedByUserId" FOREIGN KEY ("ReviewedByUserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX "IX_OfficialProductRequests_CustomProductId" ON "OfficialProductRequests" ("CustomProductId") WHERE "Status" IN (1, 2);

CREATE INDEX "IX_OfficialProductRequests_OfficialProductId" ON "OfficialProductRequests" ("OfficialProductId");

CREATE INDEX "IX_OfficialProductRequests_OwnerId" ON "OfficialProductRequests" ("OwnerId");

CREATE INDEX "IX_OfficialProductRequests_ReviewedByUserId" ON "OfficialProductRequests" ("ReviewedByUserId");

CREATE INDEX "IX_OfficialProductRequests_UnitOfMeasureId" ON "OfficialProductRequests" ("UnitOfMeasureId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260331223455_AddOfficialProductRequestWorkflow', '8.0.0');

COMMIT;
``` 
---
### 4.4.2 Representação do Modelo Físico de Dados (Entrega na Sprint 3 - Core)


> **Fundamentação:** Os modelos de dados físicos fornecem detalhes minuciosos que auxiliam administradores e desenvolvedores na implementação da lógica de negócios em um banco de dados real.
> Eles incluem elementos não especificados no modelo lógico, como:
> - Tipos de dados específicos da plataforma
> - Restrições
> - Índices
> - Triggers (quando aplicável)
> - Procedimentos armazenados (quando aplicável)
>
>Por representarem um banco real, devem respeitar:
> - Convenções de nomenclatura
> - Restrições da plataforma
> - Uso adequado de palavras reservadas <br>


**Exemplo:**

<img src="https://d2908q01vomqb2.cloudfront.net/b6692ea5df920cad691c20319a6fffd7a4a766b8/2021/11/09/BDB-1321-image005.png" width="85%">

**FONTE:** <https://aws.amazon.com/pt/compare/the-difference-between-logical-and-physical-data-model/>

<br>O grupo deverá gerar um diagrama físico do banco de dados (estrutura real das tabelas), evidenciando PKs, FKs e relacionamentos, conforme implementado no código.

Este modelo deve exibir:
- Tabelas ou coleções existentes
- Atributos com seus respectivos tipos de dados
- Chaves Primárias (PK)
- Chaves Estrangeiras (FK)
- Relacionamentos entre tabelas
- Restrições implementadas (quando aplicável)

---

### 📌 Requisitos Obrigatórios

- O diagrama deve representar fielmente o banco já implementado.
- Deve refletir exatamente o que foi criado nas Sprints 2 e 3.
- Não incluir tabelas que não existam no código.
- Deve contemplar o controle de acesso de usuários, quando implementado.
- Deve respeitar as convenções e restrições da plataforma utilizada.

---

### 📎 Representação do Modelo Físico de Dados
🚨 O grupo deverá inserir aqui a imagem do diagrama físico de dados.

---
🔧**Ferramentas Sugeridas**
- MySQL Workbench (engenharia reversa automática)
- DbDesigner
- Lucidchart
