# AgendaTenis.Partidas


## Sobre<a name = "sobre"></a>
AgendaTenis.Partidas é um microsserviço da aplicação AgendaTenis cujo objetivo é registrar as partidas entre os jogadores.
Este serviço é constituído por uma Web Api escrita em .NET 8 e utiliza o MongoDb para persistência de dados (mais detalhes na seção *Descrição Técnica*)

## Endpoints<a name = "endpoints"></a>

### Histórico de partidas
Este endpoint busca as partidas do usuário autenticado (inclusive partidas canceladas e as que ainda não aconteceram).\
Para não onerar o banco de dados e a performance da aplicação, a consulta do histórico de partidas é paginada.\
Dessa forma o usuário precisa informar o número da página e os items por página.

**Rota**: Api/Partidas/Historico\
**Método HTTP**: GET\
**Autenticação**: Necessita token jwt gerado em Api/Identity/GerarToken, do contrário retorna status 401 (Unauthorized)\
**Autorização**: Não tem políticas de autorização, somente autenticação é suficiente\
**Observações**: Se necessário utilize a seção [Valores de domínio](#valores_dominio) para encontrar os códigos para **Categoria**, **ModeloPartida**, **StatusConvite** e **StatusPlacar**


### Convidar para jogar
Este endpoint é utilizado para convidar um adversário para uma partida.\
Para isso, basta informar as seguintes informações:
- DesafianteNome (este é um parâmetro desnormalizado)
- AdversarioId
- AdversarioNome (este é um parâmetro desnormalizado)
- DataDaPartida
- DescricaoLocal
- ModeloDaPartida

Observa-se que o Id do desafiante não é informado no payload, pois a Api obtém esta informação a partir do token JWT necessário para autorização.

**Rota**: Api/Partidas/Convites/Convidar\
**Método HTTP**: POST\
**Autenticação**: Necessita token jwt gerado em Api/Identity/GerarToken, do contrário retorna status 401 (Unauthorized)\
**Autorização**: Não tem políticas de autorização, somente autenticação é suficiente\
**Observações**: Conforme expliquei na seção [Considerações sobre o projeto](#consideracoes), o sistema ainda não valida se o valor AdversarioId pertence a um Usuário cadastrado no sistema. Por isso, é muito importante informar um UsuarioId que existe.
UsuarioId do seu adversario pode ser obtido em BuscarAdversarios (é o campo **usuarioId** do response).


### Convites para jogar pendentes
Este endpoiont permite ao usuário obter a lista de convites pendentes.\
Por exemplo, se o jogador A convidar o jogador B para uma partida, então quando o jogador B fizer login no sistema e acessar este endpoint,\ 
ele poderá ver o convite do jogador A.

**Rota**: Api/Partidas/Convites/Pendentes\
**Método HTTP**: GET\
**Autenticação**: Necessita token jwt gerado em Api/Identity/GerarToken, do contrário retorna status 401 (Unauthorized)\
**Autorização**: Não tem políticas de autorização, somente autenticação é suficiente
**Observações**: Contém cache com 2 minutos de expiração.


### Responder convite
Após verificar seus convites pendentes, o jogador poderá aceitar ou recusar os convites.\
Para isso, ele pode utilizar este endpoint Responder Convite na qual ele informando:
  - Id da Partida
  - Aceitar (true ou false)

**Rota**: Api/Partidas/Convites/Responder\
**Método HTTP**: POST\
**Autenticação**: Necessita token jwt gerado em Api/Identity/GerarToken, do contrário retorna status 401 (Unauthorized)\
**Autorização**: Precisa obrigatoriamente ser o **adversário** da partida, do contrário retorna status 403 (Forbidden)\
**Observações**: Se necessário utilize a seção [Valores de domínio](#valores_dominio) para encontrar os códigos para **Categoria**, **ModeloPartida**, **StatusConvite** e **StatusPlacar**


### Registrar placar
Depois do jogo, o desafiante da partida poderá registrar o resultado na partida.
Segue os metadados do payload:
- Id (da partida)
- VencedorId
- Sets (array):
  - NumeroSet
  - GamesDesafiante
  - GamesAdversario
  - TiebreakDesafiante
  - TiebreaAdversario 

**Rota**: Api/Partidas/Placar/Registrar\
**Método HTTP**: POST\
**Autenticação**: Necessita token jwt gerado em Api/Identity/GerarToken, do contrário retorna status 401 (Unauthorized)\
**Autorização**: Precisa obrigatoriamente ser o **desafiante** da partida, do contrário retorna status 403 (Forbidden)

### Responder Placar
Essa feature conclui o ciclo de vida de uma partida.\
Ela deverá ser usada pelo adversário da partida para confirmar ou contestar o placar registrado pelo desafiante.\
Se o placar for confirmado, então as seguintes ações irão acontecer:

1. Vencedor é registrado na partida
2. Evento "Placar Confirmado" é emitido
3. Evento "Placar Confirmado" é consumido
    1. Vencedor ganha 10 pontos
    2. Perdedor perde 10 pontos

Segue os metadados do payload:
- Id (da partida)
- ConfirmarPlacar (true ou false)

**Rota**: Api/Partidas/Placar/Responder\
**Método HTTP**: GET\
**Autenticação**: Necessita token jwt gerado em Api/Identity/GerarToken, do contrário retorna status 401 (Unauthorized)\
**Autorização**: Precisa obrigatoriamente ser o **adversário** da partida, do contrário retorna status 403 (Forbidden)


## Valores de domínio <a name = "valores_dominio">
Valores numéricos de domínio (enums) são utilizado em diversos locais da aplicação, tais como parâmetros de query (ie., feature Buscar Jogadores), em requests http (ie., feature Responder Convite) e responses da api (ie., feature obter resumo do tenista).\
Segue abaixo a lista de valores de domínio:
- Jogadores:
    - Desafiante = 1
    - Adversario = 2
    
- ModeloPartida:
    - SetUnico = 1
    - MelhorDeTresSets = 2
    - MelhorDeCincoSets = 3
    
- StatusConvite:
    - Pendente = 1
    - Aceito = 2
    - Recusado = 3
    
- StatusConvite:
    - AguardandoConfirmacao = 1
    - Aceito = 2
    - Contestado = 3
 
## Descrição técnica<a name = "descricao_tecnica"></a>
Segue a descrição técnica do AgendaTenis.Partidas.

- Projetos:
  - AgendaTenis.Partidas.Core (biblioteca de classes .NET 8)
  - AgendaTenis.Partidas.WebApi (Asp Net Web Api .NET 8)
- Modelo de dados:
    - Partida
        - Id: string
        - DesafianteId: int
        - DesafianteNome: string
        - AdversarioId: int
        - AdversarioNome: string
        - DataDaPartida: DateTime
        - DescricaoLocal: string
        - ModeloDaPartida: ModeloPartidaEnum
        - StatusConvite: StatusConviteEnum
        - StatusPlacar: StatusPlacarEnum
        - VencedorId: string
        - JogadorWO: string
            - Sets []: 
                - NumeroSet
                - GamesDesafiante
                - GamesAdversario
                - TiebreakDesafiante
                - TiebreakAdversario
- Banco de Dados: MongoDb
- Cache: Redis
- Mensageria: RabbitMQ
- Acesso a dados: O acesso a dados foi abstraído com uso do MongoDB.Driver
- Observações:
    - Utilizei o "Repository Pattern" para abstrair o uso do MongoDB.Driver
- Dependências:
    - AgendaTenis.Notificacoes.Core (pacote nuget) versão 1.0.1
    - AgendaTenis.Cache.Core (pacote nuget) versão 1.0.1
    - AgendaTenis.Eventos.Core (pacote nuget) versão 1.0.0
    - MongoDB.Driver versão
    - Microsoft.Extensions.Http versão
    - Microsoft.VisualStudio.Azure.Containers.Tools.Targets versão 1.20.1
    - Swashbuckle.AspNetCore versão 6.4.0
    - Microsoft.AspNetCore.Authentication.JwtBearer versão 8.0.6
 
### Docker
- Criei um arquivo Dockerfile na raiz do repositório
- Utilize as instruções presentes na seção *Como executar* do repositório [Agte](https://github.com/AgendaTenisOrg/AgendaTenis.WebApp) para executar a stack inteira da aplicação
