# Cemetery Gates

*Sobreviva ou morra.*

Survival horror em 3D low poly com câmera fixa, nos anos 1940. O detetive Phil investiga o sumiço de corpos do cemitério onde o pai está enterrado: atravessa o cemitério tomado por zumbis, desce às catacumbas e enfrenta o esqueleto que guarda o livro da maldição.

Projeto Prático (P2) da disciplina **Desenvolvimento de Jogos Digitais** — Ciência da Computação.
Professor: Me. Luis Filipe Grael Tinos.

## Integrantes

- Filipe Pereira Ferraz
- Isaque de Campos Miassaca
- Kaique Rabesco Valverde
- Rafaela Sanchez
- Túlio Braga Alves
- Vinicius Vaes Massagli

## Como jogar

### Objetivo

Encontre a chave da casa do coveiro no cemitério, pegue lá dentro a chave das catacumbas e use-a na cripta. Nas catacumbas, explore o labirinto, vença o esqueleto guardião e pegue o livro da maldição no altar.

Você perde se a vida do detetive chegar a zero. Não há save: morrer no cemitério recomeça do início; morrer nas catacumbas recomeça na entrada delas.

### Controles

| Ação | Tecla |
| :--- | :--- |
| Andar para frente / para trás | W / S |
| Girar para a esquerda / direita | A / D |
| Correr | Shift Esq. + W |
| Interagir e pegar itens | E |
| Mirar o revólver (segurar) | Mouse Dir. |
| Atirar (mirando) / soco (sem mirar) | Mouse Esq. |
| Recarregar | R |
| Inventário (abrir/fechar) | I |
| Pausa | Esc |
| Navegar nos menus | Mouse, ou setas + Enter |

Os controles são *tank controls*: o detetive anda para onde está virado, não para onde a câmera aponta.

### Dicas

- O revólver começa guardado: abra o inventário (I), escolha o Revólver e clique em **Usar** para equipá-lo.
- Chaves também se usam pelo inventário, de frente para a porta trancada.
- O tambor tem 6 balas e a munição é escassa. Zumbis são lentos: às vezes é melhor fugir ou usar o soco.
- O esqueleto se compromete com cada golpe e fica parado por um instante depois dele, mesmo quando erra. Provoque o golpe, saia do alcance e atire.
- Sua vida (Condição) aparece na tela do inventário. O Tônico medicinal recupera 3 pontos.

## Executando

- **Build (Windows 64 bits):** extraia o `Cemetery Gates.zip` e abra `Cemetery Gates.exe` dentro da pasta `Cemetery Gates`. Mantenha o executável junto com os outros arquivos e pastas da build (`Cemetery Gates_Data`, `MonoBleedingEdge`, DLLs). Uma partida leva cerca de 20 a 40 minutos.
- **Na Unity:** abra o projeto com a Unity 6.5, abra a cena `Assets/Scenes/Cemiterio.unity` e aperte Play. O menu inicial aparece sobre o cemitério.
- **Gerar a build:** File → Build Profiles → Windows, com as cenas `Cemiterio` e `Catacumbas` (nessa ordem) na lista, e Build.

Repositório: https://github.com/krabvalverde/Cemetery-Gates. Ele usa Git LFS para cenas, modelos, texturas e áudio: rode `git lfs install` antes de clonar, senão esses arquivos vêm vazios.

## Documentação

- [Game Design Document](docs/GDD.md)

## Créditos de terceiros

| Asset | Autor | Licença / fonte |
| :--- | :--- | :--- |
| Man in Suit (base do detetive) | Quaternius | CC0 — poly.pizza |
| Halloween Bits | Kay Lousberg | poly.pizza |
| Revolver | austincford | poly.pizza |
| Lowpoly_zombie | churliaevyaroslav | CC BY 4.0 — Sketchfab |
| Elementary Dungeon Pack Lite | Gridness Studios | Unity Asset Store |
| Skeleton | SazenGames | Unity Asset Store |
| UnifrakturMaguntia | j. "mach" wust | SIL OFL — Google Fonts |
| Special Elite | Astigmatic | Apache 2.0 — Google Fonts |

Trilha sonora original de Vinicius Vaes Massagli. Efeitos sonoros sintetizados por código pela equipe (`Assets/Audio/Source~/gerar_sons.py`). A lista completa, com links, está na Seção 15 do GDD.
