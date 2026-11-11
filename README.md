# Gestão de Usuários e Pacientes (MVC)

Sistema desenvolvido para gerenciamento e controle de cadastros de usuários e pacientes, estruturado utilizando o padrão de arquitetura **MVC (Model-View-Controller)** em C# .NET com persistência de dados em arquivos **JSON**.

---

## 🚀 Funcionalidades

- **Autenticação:** Tela de login com controle de acesso.
- **Gerenciamento de Usuários:** Cadastro, consulta, edição e remoção de usuários.
- **Gerenciamento de Pacientes:** Registro e acompanhamento de dados dos pacientes.
- **Persistência em JSON:** Armazenamento local dos dados nos arquivos `usuarios.json` e `Pacientes.json`, sem necessidade de banco de dados externo.
- **Arquitetura MVC:** Separação clara entre regra de negócio, interface gráfica e controladores de fluxo.

---

## 🔐 Acesso para Testes

Para acessar o sistema em ambiente de desenvolvimento, utilize as credenciais padrão:

- **Usuário:** `desenvolvedor`
- **Senha:** `carambola`

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem:** C# (.NET)
- **Armazenamento:** Arquivos JSON (`usuarios.json` e `Pacientes.json`)
- **Arquitetura:** MVC (Model-View-Controller)

---

## 📂 Estrutura do Projeto

```text
├── Controllers/    # Lógica de controle e fluxo das requisições
├── Models/         # Regras de negócio e representação dos dados
├── Views/          # Interfaces visuais do sistema
├── Pacientes.json  # Armazenamento dos dados de pacientes
├── usuarios.json   # Armazenamento dos dados de usuários
└── README.md       # Documentação do projeto