# 🟣 MeuErp - Sistema de Controle de Estoque & Auditoria

O **MeuErp** é uma plataforma moderna e segura de planejamento de recursos empresariais focada na gestão e auditoria dinâmica de estoques em tempo real. Desenvolvido sob a arquitetura ASP.NET Core MVC, o sistema substitui controles manuais descentralizados por uma trilha digital auditável de movimentações de entrada e saída de insumos.

O projeto apresenta uma experiência visual contemporânea baseada em **Premium Dark Mode** e **Glassmorphism**, combinando uma estética visual refinada com alto desempenho e legibilidade de dados.

🌐 **Acesse o Sistema de Testes Online**: [meu-erp-production-ec3a.up.railway.app](https://meu-erp-production-ec3a.up.railway.app)

---

## 🚀 Funcionalidades Principais

* **Segurança e Controle de Acesso**: Módulo de login isolado protegido por Cookie Authentication. As senhas dos usuários são criptografadas em hash seguro utilizando o algoritmo **PBKDF2** (`PasswordHasher<User>`).
* **Cadastro de Produtos (CRUD)**: Gestão de catálogo contendo Nome, Descrição, Preço unitário, Quantidade atual e parâmetros preventivos de Quantidade Mínima e Máxima para controle de abastecimento.
* **Trilha de Auditoria (Movimentações)**: Registro completo de movimentações de Entrada (`Entry`) e Saída (`Exit`), vinculados a produtos específicos. O sistema impede saídas se o saldo de estoque for insuficiente e recalcula a quantidade física do produto de forma automática se uma movimentação for editada ou removida.
* **Painel Dinâmico (Dashboard)**: Painel estatístico em tempo real exibindo a quantidade de produtos cadastrados, quantidade de itens em nível crítico (abaixo do mínimo ou acima do máximo), seção de atenção para reposição de "Produtos Críticos" e o log de auditoria rápida das últimas 5 movimentações realizadas.

---

## 🎨 Design & Experiência do Usuário (UX/UI)

* **Estética Glassmorphism**: Cartões e formulários transparentes com desfoque de fundo de vidro fosco (`backdrop-filter: blur(12px)`) e bordas com gradientes sutis em HSL.
* **Acessibilidade e Alto Contraste**: Sobrescrita das classes nativas do Bootstrap 5 para cores claras e vibrantes (Violeta, Azul, Verde e Vermelho transparentes), garantindo legibilidade perfeita de textos de tabelas e listas sobre fundos escuros.
* **Alinhamento Fluido**: Layout flexível baseado em Flexbox com rodapé dinâmico posicionado de forma estática (`position: static`), adaptando-se confortavelmente ao final das páginas sem sobrepor tabelas ou cards.
* **Aceleração Gráfica por Hardware**: Otimização de renderização no navegador utilizando propriedades como `transform: translate3d(0,0,0)` e `will-change: transform` nos elementos de interface para estabilizar animações constantes a 60fps suaves.

---

## 💻 Tecnologias Utilizadas

* **Backend**: .NET 8.0, ASP.NET Core MVC (C#).
* **Persistência de Dados**: Entity Framework Core 8.0 (EF Core Migrations) operando sobre banco de dados relacional leve **SQLite** (`meuerp.db`).
* **Frontend**: Razor Pages/Views (`.cshtml`), HTML5, CSS3 Customizado, jQuery, Bootstrap 5 e Google Fonts (Família de Fontes *Outfit*).
* **Hospedagem & Infraestrutura**: Dockerfile otimizado para deploy contêiner Linux implantado na plataforma **Railway**.

---

## 🛠️ Como Executar o Projeto Localmente

### Pré-requisitos
* Ter o **.NET 8.0 SDK** instalado em sua máquina.

### Passos para Execução
1. Clone o repositório em sua máquina local.
2. Abra o terminal na pasta raiz do projeto.
3. Restaure as dependências do projeto e do Entity Framework Core:
   ```bash
   dotnet restore
   ```
4. Execute a aplicação (o banco de dados SQLite será criado e as migrações aplicadas automaticamente no primeiro startup):
   ```bash
   dotnet run
   ```
5. Abra o seu navegador e acesse a URL local fornecida pelo console (geralmente `http://localhost:5171`).

### 🔐 Credenciais de Teste Iniciais (Seed)
Na inicialização automática do banco de dados, o sistema cria um usuário administrador padrão pronto para login:
* **Usuário (Username)**: `admin`
* **Senha (Password)**: `admin123`

---

## 📦 Estrutura de Deploy (Dockerfile)

O projeto contém um `Dockerfile` na raiz estruturado em múltiplos estágios (*multi-stage build*), isolando o ambiente de build das SDKs do contêiner de execução final em produção:

```dockerfile
# Estágio de Compilação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

# Estágio de Execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "MeuErp.dll"]
```
*(Nota: O arquivo `Program.cs` está configurado para ler dinamicamente a porta através da variável global `PORT` injetada pela Railway).*
