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
Para isso, ele pode utilizar este endpoint Responder Convite na qual ele informa o Id da Partida (pode ser obtido utilizando a feature convites pendentes) e o status de aceitação (2 para aceitar e 3 para recusar).

**Rota**: Api/Partidas/Convites/Responder\
**Método HTTP**: POST\
**Autenticação**: Necessita token jwt gerado em Api/Identity/GerarToken, do contrário retorna status 401 (Unauthorized)\
**Autorização**: Precisa obrigatoriamente ser o **adversário** da partida, do contrário retorna status 403 (Forbidden)\
**Observações**: Se necessário utilize a seção [Valores de domínio](#valores_dominio) para encontrar os códigos para **Categoria**, **ModeloPartida**, **StatusConvite** e **StatusPlacar**



