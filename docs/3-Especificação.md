
# 3. Especificações do Projeto

📌 **Pré-requisito:** Planejamento do Projeto (Cronograma e Sprints definidos).

Nesta seção serão detalhados:

- ✅ Requisitos Funcionais  
- ✅ Histórias de Usuário  
- ✅ Requisitos Não Funcionais  
- ✅ Restrições do Projeto  

O objetivo é organizar claramente as funcionalidades, qualidades e limites da solução.

---

# 3.1 Requisitos Funcionais

Os **Requisitos Funcionais (RF)** descrevem o que o sistema deve fazer.

📌 Cada requisito deve:
- Representar uma funcionalidade única
- Ser claro e objetivo
- Orientar diretamente o desenvolvimento

---

## Tabela de Requisitos Funcionais

| ID    | Descrição do Requisito | Prioridade |
|-------|------------------------|------------|
| RF-01 | O sistema deve permitir que o usuário realize cadastro informando dados básicos (ex: nome, e-mail e senha). | 🔴 ALTA |
| RF-02 | O sistema deve validar se todos os campos obrigatórios do cadastro foram preenchidos corretamente antes de enviar. | 🔴 ALTA  |
| RF-03 | O sistema deve validar se o e-mail informado possui um formato válido. | 🔴 ALTA |
| RF-04 | O sistema deve permitir que o usuário se cadastre/autentique utilizando uma conta Google. | 🔴 ALTA |
| RF-05 | O sistema deve armazenar os dados do usuário no banco de dados após cadastro bem-sucedido. | 🔴 ALTA |
| RF-06 | O sistema não deve permitir cadastro de usuários com e-mails já existentes. | 🔴 ALTA |
| RF-07 | (Descreva aqui o requisito funcional 7 do seu sistema) | 🟡 MÉDIA |
| RF-08 | (Descreva aqui o requisito funcional 8 do seu sistema) | (Alta/Média/Baixa) |
| RF-09 | (Descreva aqui o requisito funcional 9 do seu sistema) | (Alta/Média/Baixa) |
| RF-10 | (Descreva aqui o requisito funcional 10 do seu sistema) | (Alta/Média/Baixa) |

---

# 3.2 Histórias de Usuário

---

## Histórias do Projeto

---
## Cadastro

### História 1 (relacionada ao RF-01)

Como usuário novo,  
Eu quero me cadastrar no sistema informando meus dados básicos, 
Para que poder acessar a aplicação e gerenciar minhas compras.

---

### História 2 (relacionada ao RF-02 e RF-03)

Como usuário,  
Eu quero que o sistema valide os dados que eu informo no cadastro,  
Para que garantir que minhas informações estejam corretas e evitar erros no acesso.

---

### História 3 (relacionada ao RF-04)

Como usuário,  
Eu quero me cadastrar ou entrar utilizando minha conta Google,  
Para que acessar o sistema de forma mais rápida e prática, sem precisar criar uma nova senha.

---

> 💡 Dica: Agrupe as histórias por módulo (Cadastro, Relatórios, Pagamentos, etc.) para melhor organização.

---

# 3.3 Requisitos Não Funcionais

Os **Requisitos Não Funcionais (RNF)** definem características de qualidade do sistema, como:

- ⚡ Desempenho  
- 🔒 Segurança  
- 🎨 Usabilidade  
- 📈 Escalabilidade  
- 🌐 Compatibilidade  

Eles garantem a qualidade da solução.

---

## Tabela de Requisitos Não Funcionais

| ID     | Descrição do Requisito | Prioridade |
|--------|------------------------|------------|
| RNF-01 | O sistema deve responder às requisições de cadastro em até 3 segundos. | 🔴 ALTA |
| RNF-02 | As senhas dos usuários devem ser armazenadas de forma criptografada. | 🔴 ALTA |
| RNF-03 | O sistema deve estar disponível para cadastro 24/7, salvo manutenções programadas. | 🟡 MÉDIA |
| RNF-04 | O sistema deve funcionar em navegadores modernos (Chrome, Edge, etc.). | 🟡 MÉDIA |
| RNF-05 | O sistema deve suportar múltiplos usuários realizando cadastro simultaneamente. | 🟡 MÉDIA |
| RNF-06 | A autenticação via Google deve utilizar protocolos seguros | 🔴 ALTA |

---

# 3.4 Restrições do Projeto

📌 **Restrições** são limitações externas impostas ao projeto.

Elas podem envolver:
- 📅 Prazo
- 🖥️ Tecnologia obrigatória ou proibida
- 🌐 Ambiente de execução
- 📜 Normas legais
- 🏢 Políticas institucionais

⚠️ Diferente dos RNFs, as restrições impõem **limites fixos** ao projeto.

---

## Tabela de Restrições

| ID  | Restrição |
|-----|-----------|
| R-01 | O projeto deverá ser entregue até o final do semestre. |
| R-02 | O sistema deve funcionar apenas dentro da rede interna da empresa. |
| R-03 | O software deve ser compatível com Windows e Linux. |
| R-04 | (Descreva aqui a restrição 4 do seu projeto) |
| R-05 | (Descreva aqui a restrição 5 do seu projeto) |
| R-06 | (Descreva aqui a restrição 6 do seu projeto) |
| R-07 | (Descreva aqui a restrição 7 do seu projeto) |
| R-08 | (Descreva aqui a restrição 8 do seu projeto) |

---
## 3.5 Regras de Negócio

> Regras de Negócio definem as condições e políticas que o sistema deve seguir para garantir o correto funcionamento alinhado ao negócio.  
>  
> Elas indicam **quando** e **como** certas ações devem ocorrer, usando o padrão:  
>  
> **Se (condição) for verdadeira, então (ação) deve ser tomada.**  
>  
> Exemplo:  
> - "Um usuário só poderá finalizar um cadastro se todos os dados forem inseridos e validados com sucesso."  
>  
> Também pode ser escrito assim (if/then):  
> - "Se o usuário tem saldo acima de X, então a opção de empréstimo estará liberada."

---

 A tabela abaixo deve ser preenchida com as regras de negócio que **impactam seu projeto**. Os textos no quadro são apenas ilustrativos.

|ID    | Regra de Negócio                                                       |
|-------|-----------------------------------------------------------------------|
|RN-01 | Usuário só pode cadastrar até 10 tarefas por dia.                      |
|RN-02 | Apenas administradores podem alterar permissões de usuários.           |
|RN-03 | Tarefas vencidas devem ser destacadas em vermelho no sistema.          |
|RN-04 | *(Descreva aqui a restrição 4 do seu projeto)*                         |
|RN-05 | *(Descreva aqui a restrição 5 do seu projeto)*                         |

💡 **Dica:** Explique sempre o motivo ou impacto da regra no sistema.

---
> **Links Úteis**:
> - [O que são Requisitos Funcionais e Requisitos Não Funcionais?](https://codificar.com.br/requisitos-funcionais-nao-funcionais/)
> - [O que são requisitos funcionais e requisitos não funcionais?](https://analisederequisitos.com.br/requisitos-funcionais-e-requisitos-nao-funcionais-o-que-sao/)
