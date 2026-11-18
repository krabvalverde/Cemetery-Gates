**GAME DESIGN DOCUMENT**

**Cemetery Gates**

Sobreviva ou morra

**Disciplina:** Desenvolvimento de Jogos Digitais 

**Curso:** Ciência da Computação

**Atividade:** Projeto Prático — P2

**Professor(a): Me. Luis Filipe Grael Tinos**

**Engine: Unity**

**Plataforma:** PC

**Entrega:** 19/11/2026

**Versão do documento: 3.1**

**Integrantes do grupo (até 5\)**

*1\. Filipe Pereira Ferraz* 

*2\. Isaque de Campos Miassaca*

*3\. Kaique Rabesco Valverde*

*4\. Rafaela Sanchez* 

*5\. Túlio Braga Alves*

*6\. Vinicius Vaes Massagli*

**Histórico de versões**

| Versão | Data | Autor(es) | Alterações |
| :---- | :---- | :---- | :---- |
| 1.0 | 28/10/2026 | Rafaela | Definição de Pontos Principais do Game |
| 1.1 | 29/10/2026 | Rafaela | Controles, mecânicas, estética e paleta definidas |
| 1.2 | 29/10/2026 | Rafaela | Menu inicial, mecânicas, dinâmicas, estética (MDA), item de cura e música do menu |
| 1.3 | 29/10/2026 | Rafaela | Tecla de interagir (E), créditos no menu, save manual no lampião |
| 1.4 | 29/10/2026 | Rafaela | Save com galhos especiais e extrator |
| 1.5 | 29/10/2026 | Rafaela | Galhos empilham no inventário; extratores ficam nas áreas seguras |
| 1.6 | 29/10/2026 | Rafaela | Modelo 3D do detetive (aparência) e crédito do modelo base |
| 1.7 | 05/11/2026 | Rafaela | Cenário inicial (cemitério com portão, casa do coveiro e cripta) |
| 1.8 | 05/11/2026 | Rafaela | Movimentação do detetive (tank controls, valores) e estados de animação |
| 1.9 | 05/11/2026 | Rafaela | Cenário: chão uniforme e sem caminho de pedras na frente do portão |
| 2.0 | 12/11/2026 | Rafaela | Sistema de inventário (coletar, usar, analisar, descartar), itens no cenário e tela do inventário |
| 2.1 | 15/11/2026 | Rafaela | Combate: soco e revólver (mira, munição, recarga), sem defesa nem esquiva; vida e IA básica dos inimigos |
| 2.2 | 15/11/2026 | Rafaela | Mira: o detetive sempre aponta o revólver ao segurar Mouse Dir., mesmo sem inimigos, e pode atirar em qualquer direção |
| 2.3 | 18/11/2026 | Kaique | Menu inicial (Iniciar, Créditos, Sair), tela de créditos, tela de Game Over, menu de Pausa (Esc), tela de Controles e fontes do jogo |
| 2.4 | 18/11/2026 | Kaique | Save com galhos e extrator cancelado (galhos removidos do jogo) |
| 2.5 | 18/11/2026 | Kaique | Cerca: removidos os pilares avulsos que ficavam sobrepostos aos pilares do próprio modelo da cerca e piscavam (z-fighting) |
| 2.6 | 18/11/2026 | Kaique | Mecânica de save cancelada: o jogo não tem save (sem Salvar na pausa, Continuar no menu ou Carregar no Game Over) |
| 2.7 | 18/11/2026 | Kaique | Segunda parte: interior da casa do coveiro com a chave das catacumbas, cripta como entrada, catacumbas (labirinto e salão), esqueleto chefe, livro da maldição e tela de fim |
| 2.8 | 18/11/2026 | Kaique | Catacumbas com uma câmera fixa por sala; trilha sonora (menu e fim, cemitério, catacumbas) |
| 2.9 | 18/11/2026 | Kaique | Chefe: golpe comprometido (ataca ao entrar no alcance, pode errar e fica parado 1,7 s), abrindo janela para o jogador atacar |
| 3.0 | 18/11/2026 | Kaique | Revisão geral contra o jogo implementado: pitch, diferencial, core loop, HUD, escopo, checklist, cronograma, playtesting, organização e autoavaliação preenchidos; removidos itens não implementados (Ghoul, ação contextual com Espaço, arremessar itens, Opções, filtro CRT); regras, vitória/derrota, valores de itens e créditos (modelo do zumbi, também na tela de créditos do jogo) corrigidos; README criado |
| 3.1 | 18/11/2026 | Kaique | Conferência com o enunciado da P2: tela de créditos passa a citar a fonte de cada asset (poly.pizza, Sketchfab, Asset Store, Google Fonts); malhas do Elementary Dungeon Pack com Read/Write para o NavMesh funcionar na build; comentários no código citados na autoavaliação |

**Sumário**

[1\. Visão Geral	1](#1.-visão-geral)

[1.1 Conceito (pitch)	1](#1.1-conceito-\(pitch\))

[1.2 Ficha técnica	1](#1.2-ficha-técnica)

[1.3 Referências e inspirações	1](#1.3-referências-e-inspirações)

[1.4 Diferencial	1](#1.4-diferencial)

[2\. Objetivo e Regras	1](#2.-objetivo-e-regras)

[2.1 Objetivo do jogador	1](#2.1-objetivo-do-jogador)

[2.2 Condição de vitória	1](#2.1-objetivo-do-jogador)

[2.3 Condição de derrota	1](#2.3-condição-de-derrota)

[2.4 Regras do jogo	1](#2.4-regras-do-jogo)

[3\. Gameplay	1](#3.-gameplay)

[3.1 Loop principal (core loop)	1](#3.1-loop-principal-\(core-loop\))

[3.2 Controles	1](#3.2-controles)

[3.3 Mecânicas principais	1](#3.3-mecânicas-principais)

[3.4 Colisão e física	1](#3.4-colisão-e-física)

[4\. Personagem Principal	1](#4.-personagem-principal)

[4.1 Estados de animação	1](#4.1-estados-de-animação)

[5\. Inimigos e Obstáculos	1](#5.-inimigos-e-obstáculos)

[5.1 Chefe (se houver)	1](#5.1-chefe-\(se-houver\))

[6\. Itens, Coletáveis e Power-ups	1](#6.-itens,-coletáveis-e-power-ups)

[7\. Fases e Progressão	1](#7.-fases-e-progressão)

[7.1 Curva de dificuldade	1](#7.1-curva-de-dificuldade)

[7.2 Pontuação e recompensas	1](#7.2-pontuação-e-recompensas)

[8\. Interface (Telas e HUD)	1](#8.-interface-\(telas-e-hud\))

[8.1 Fluxo de telas	1](#8.1-fluxo-de-telas)

[8.2 HUD	1](#8.2-hud)

[9\. Arte e Estilo Visual	1](#9.-arte-e-estilo-visual)

[10\. Áudio	1](#10.-áudio)

[10.1 Trilha sonora	1](#10.1-trilha-sonora)

[10.2 Efeitos sonoros	1](#10.2-efeitos-sonoros)

[11\. Escopo do Projeto	1](#11.-escopo-do-projeto)

[11.1 Checklist de elementos sugeridos	1](#11.1-checklist-de-elementos-sugeridos)

[12\. Equipe e Divisão de Tarefas	1](#12.-equipe-e-divisão-de-tarefas)

[13\. Cronograma	1](#13.-cronograma)

[14\. Playtesting e Balanceamento	1](#14.-playtesting-e-balanceamento)

[15\. Créditos e Assets de Terceiros	1](#15.-créditos-e-assets-de-terceiros)

[16\. Organização do Projeto	1](#16.-organização-do-projeto)

[16.1 Estrutura de pastas	1](#16.1-estrutura-de-pastas)

[16.2 Convenções de código e nomenclatura	1](#16.2-convenções-de-código-e-nomenclatura)

[16.3 Versionamento (opcional)	1](#16.3-versionamento-\(opcional\))

[17\. Entrega e Apresentação	1](#17.-entrega-e-apresentação)

[17.1 Itens da entrega (19/11/2026 — CONNECT+)	1](#17.1-itens-da-entrega-\(19/11/2026-—-connect+\))

[17.2 Roteiro da apresentação (máx. 15 min)	1](#17.2-roteiro-da-apresentação-\(máx.-15-min\))

[17.3 Autoavaliação pelos critérios	1](#17.3-autoavaliação-pelos-critérios)

# **1\. Visão Geral** {#1.-visão-geral}

## **1.1 Conceito (pitch)** {#1.1-conceito-(pitch)}

Survival horror em 3D low poly com câmera fixa, nos anos 1940. O jogador controla um detetive que investiga o sumiço de corpos do cemitério onde o pai está enterrado: explora o cemitério tomado por zumbis, encontra chaves, desce às catacumbas e enfrenta o esqueleto que guarda o livro da maldição. A graça está na tensão de poucos recursos (6 balas no tambor, munição contada, 10 espaços no inventário) e de uma câmera que nunca mostra tudo o que está à espreita.

## **1.2 Ficha técnica** {#1.2-ficha-técnica}

**Gênero: Survival horror**

**Público-alvo: \+12**

**Plataforma: PC (Windows)**

**Engine / versão: Unity 6.5**

**Perspectiva e câmera: Câmera Fixa**

O cemitério é dividido em 10 zonas externas, cada uma com a sua câmera fixa, que liga quando o detetive entra na zona, mais uma câmera para o interior da casa do coveiro (11 no total). As catacumbas têm 14 câmeras, uma por sala. Cada câmera enquadra a zona inteira, inclusive as bordas por onde o detetive entra. Se ainda assim ele chegar perto da borda do quadro, a câmera gira no lugar (sem se mover) só o necessário para mantê-lo visível; ao trocar de zona, a câmera nova já entra com ele no quadro.

**Duração média do jogo: cerca de 20 a 40 minutos** (duas fases: cemitério e catacumbas)

## **1.3 Referências e inspirações** {#1.3-referências-e-inspirações}

* Alone in The Dark (1992) – História, cenários, câmera

* Silent Hill 2 – Controles, combate, atmosfera, inimigos

* Resident Evil 1 – Câmera fixa, mira, inventário

## **1.4 Diferencial** {#1.4-diferencial}

* Estética de survival horror clássico (câmera fixa, tank controls, inventário limitado) com visual low poly próprio e paleta restrita: cenário frio e arroxeado em que o âmbar só aparece onde há fogo e luz humana, guiando o olhar do jogador.
* Detetive nos anos 1940 com motivação pessoal (o pai enterrado no cemitério), em vez de um policial ou soldado.
* Escopo enxuto e fechado: uma investigação curta, jogável do início ao fim em uma sessão, com chefe final de padrão legível (o golpe dele abre uma janela de ataque).
* Trilha sonora original da equipe e todos os efeitos sonoros sintetizados por código.

# **2\. Objetivo e Regras** {#2.-objetivo-e-regras}

## **2.1 Objetivo do jogador** {#2.1-objetivo-do-jogador}

Resolver o mistério do desaparecimento de corpos do cemitério.

## **2.2 Condição de vitória**

Vencer o esqueleto guardião no salão das catacumbas e pegar o livro da maldição no altar, o que desfaz a maldição e mostra a tela de Fim.

## **2.3 Condição de derrota** {#2.3-condição-de-derrota}

A vida do detetive chegar a zero (Game Over). Não há limite de tempo.

## **2.4 Regras do jogo** {#2.4-regras-do-jogo}

* Regra 1 – O detetive aguenta 10 “hits”; itens de cura restauram parte deles

* Regra 2 – Inventário com 10 espaços, um item por espaço, sem sistema de peso (itens empilháveis, como a munição, ocupam um espaço só)

* Regra 3 – Um zumbi causa 1 hit por ataque

* Regra 4 – O chefe (esqueleto) causa 2 hits por ataque

* Regra 5 – Os itens podem ser descartados no chão (e pegos de novo); itens sem utilidade só ocupam espaço

* Regra 6 – O detetive começa só com o revólver (tambor cheio, 6 balas, sem munição extra); balas só vêm de caixas encontradas

* Regra 7 – Não há save: morrer no cemitério recomeça do início; morrer nas catacumbas recomeça na entrada delas

# **3\. Gameplay** {#3.-gameplay}

## **3.1 Loop principal (core loop)** {#3.1-loop-principal-(core-loop)}

**Explorar** a área (câmera fixa por zona) → **enfrentar ou evitar** inimigos (tiro, soco ou fuga) → **coletar** itens (chaves, munição, cura) e gerenciar o inventário → **usar** a chave certa na porta certa → **avançar** para a próxima área.

Cemitério: pegar a chave da casa do coveiro → abrir a casa e pegar a chave das catacumbas → abrir a cripta e descer. Catacumbas: explorar o labirinto → vencer o esqueleto → pegar o livro (fim).

## **3.2 Controles** {#3.2-controles}

| Ação | Teclado / Mouse | Controle (opcional) |
| :---- | :---- | :---- |
| Movimentação (tank controls) | W, A, S, D |  |
| Inventário (abrir/fechar) | I (Esc também fecha) |  |
| Pausa (abrir/fechar) | Esc |  |
| Navegar no inventário | Mouse, ou setas + Enter |  |
| Mirar (segurar, com o revólver equipado) | Mouse Dir. |  |
| Ataque/ Tiro | Mouse Esq. (soco; tiro enquanto mira) |  |
| Recarregar o revólver | R |  |
| Interagir / pegar item | E |  |
| Correr (segurar, junto com W) | Shift Esq. |  |
| Navegar nos menus | Mouse, ou setas + Enter |  |

## **3.3 Mecânicas principais** {#3.3-mecânicas-principais}

| Mecânica | Descrição | Parâmetros (valores) |
| :---- | :---- | :---- |
| Vida | O detetive aguenta um número limitado de golpes; ao chegar a zero ele cai (animação Death), para de responder e os inimigos param de atacar; game over | 10 hits |
| Tank controls | W/S andam para frente/trás, A/D giram o personagem no lugar; Shift corre (só para frente) | Andar 1,6 m/s; correr 3,8 m/s; ré 1,1 m/s; giro 150°/s |
| Atirar com a arma (mira com auto-trava) | Com o revólver equipado, segurar Mouse Dir. deixa o personagem parado, com o braço estendido e o revólver apontado para frente, mesmo sem inimigos por perto. Sem alvo, A/D giram o detetive (150°/s) para mirar em qualquer direção; se houver inimigo à frente, a mira trava nele. Mouse Esq. atira a qualquer momento enquanto mira: o braço dá um coice, sai um clarão do cano e a bala voa visível com rastro, causando o dano ao chegar. Sem balas no tambor, aparece a mensagem para recarregar | Dano 5 (inimigo comum cai com 2 tiros); tambor de 6 balas; alcance 15 m; trava em inimigos até 60° à frente; 0,6 s entre tiros |
| Soco (corpo a corpo) | Sem mirar (ou sem o revólver equipado), Mouse Esq. dá um soco; o detetive fica parado durante o golpe. Não há defesa nem esquiva | Dano 2 (inimigo comum cai com 5 socos); alcance de \~1 m à frente; 0,9 s por soco |
| Recarregar | Com o revólver equipado, R completa o tambor usando balas do item Munição (empilhável) do inventário | Tambor de 6 balas; 6 balas por caixa encontrada (3 caixas, 18 balas no jogo todo) |
| Coleta de itens | Itens encontrados no cenário vão para o inventário ao apertar E perto deles; com o inventário cheio o item fica no chão | Tecla E, até 1,5 m do item |
| Inventário | 10 espaços, um item por espaço, sem peso; itens empilháveis (ex.: munição) ocupam um espaço só; itens podem ser guardados ou descartados no chão. A tela do inventário pausa o jogo e mostra a condição (vida) e a arma equipada. Ações sobre o item escolhido: **Usar**, **Analisar** (mostra o texto explicativo do item) e **Descartar** (larga o item no chão, e ele pode ser pego de novo; itens sem modelo no mundo não podem ser largados) | 10 espaços |
| Usar item | Perto de um ponto do cenário que aceita aquele item (ex.: porta trancada), o item é usado nele e a história avança (a chave é consumida e a porta abre). Fora disso: arma equipa/guarda (o revólver começa guardado e precisa ser equipado pelo inventário); item de cura restaura vida (não é gasto com a vida cheia) | Alcance de 2,2 m do ponto; tônico cura 3 hits |
| Interagir com o cenário | E abre o portão de entrada do cemitério; diante de uma porta trancada (casa do coveiro, cripta), E mostra uma dica e toca o som de trancado — ela só abre usando a chave pelo inventário | Tecla E; portão a até 2,5 m |
| Save | Não há save. Morrer no cemitério ou sair para o menu recomeça do início; morrer nas catacumbas recomeça na entrada delas (ver Descer às catacumbas) | — |
| Descer às catacumbas | Usar a chave das catacumbas (inventário → Usar) diante da porta da cripta carrega as catacumbas. O detetive leva vida, inventário, arma equipada e balas no tambor; a chave é gasta. Morrer lá embaixo recomeça no pé da escada com exatamente esse estado | Sem a chave, E na porta mostra uma dica |
| Lampião a óleo | O detetive carrega um lampião sempre aceso que ilumina em âmbar ao seu redor (não consome óleo) | Luz pontual âmbar, alcance 5 m |
| Câmera fixa por zona | Ao entrar numa zona, a câmera dela assume; gira no lugar só para manter o detetive no quadro (ver 1.2) | 11 câmeras no cemitério, 14 nas catacumbas |

**Dinâmicas**

* Atirar em um inimigo causa dano.

* Socar um inimigo causa dano (menos que o tiro, mas não gasta munição).

* Gerenciar a munição: o tambor esvazia e só é recarregado com balas encontradas.

* Guardar ou descartar itens (gerenciamento de inventário).

* Levar um golpe reduz a vida.

* Consumir itens de cura restaura vida.

* Fugir ou contornar zumbis (são lentos e desistem a 18 m) em vez de gastar balas.

* Provocar o golpe do chefe, sair do alcance e atacar durante a recuperação dele.

## **3.4 Colisão e física** {#3.4-colisão-e-física}

Detetive e inimigos usam CharacterController (cápsula com gravidade, sem pulo). O tiro do revólver é um raycast a partir do peito do detetive em direção ao inimigo travado (ou para frente, sem alvo): paredes e objetos do cenário bloqueiam a bala. O soco testa uma esfera pequena logo à frente do detetive no momento do golpe. Os zumbis atacam ao chegar a 1,3 m do detetive (o chefe, a 1,6 m). Para perseguir desviando de túmulos, cercas e paredes, os inimigos usam um NavMesh gerado em tempo de execução ao abrir cada cena. Portas e o portão têm um collider que bloqueia a passagem até serem abertos. Inimigos mortos perdem a colisão.

# **4\. Personagem Principal** {#4.-personagem-principal}

**Nome:** *\[Nome do personagem\]*

**Descrição:** Detetive nos anos 1940; investiga a invasão e o desaparecimento de corpos no cemitério onde o pai está enterrado. Carrega lampião a óleo e revólver. Aparência: homem de meia-idade, cabelo e barba grisalhos, chapéu fedora escuro com fita preta, sobretudo cáqui na altura da canela (aberto na frente, com cinto), camisa clara, gravata vermelho-escura, calça e sapatos escuros. Modelo low poly (\~2.460 triângulos) com rig humanoide, baseado no "Man in Suit" (Seção 15). *\[Personalidade\]*

**Atributos:** Vida: 10 hits. Velocidade: andar 1,6 m/s, correr 3,8 m/s, ré 1,1 m/s, giro 150°/s. Dano: tiro 5 (alcance 15 m), soco 2 (alcance ~1 m). Inventário: 10 espaços.

## **4.1 Estados de animação** {#4.1-estados-de-animação}

| Estado | Quando ocorre | Nº de frames / clipe | Transição para |
| :---- | :---- | :---- | :---- |
| Idle (parado) | Sem input de movimento | 100 frames (4,2 s, loop) | Walk, Run, WalkBack |
| Walk (andando) | W pressionado, ou girando no lugar com A/D | 25 frames (1,0 s, loop) | Idle, Run, WalkBack |
| Run (correndo) | W + Shift | 21 frames (0,9 s, loop) | Idle, Walk, WalkBack |
| WalkBack (andando para trás) | S pressionado (clipe Walk tocado ao contrário) | 25 frames (1,0 s, loop) | Idle, Walk, Run |
| Punch (soco) | Mouse Esq. sem mirar | 22 frames (0,9 s) | Idle, Walk, Run, WalkBack |
| Mira | Mouse Dir. com o revólver equipado: Idle com o braço direito estendido e o revólver na mão (pose feita por código) | — | Idle, Walk, Run, WalkBack |
| Tiro | Mouse Esq. mirando: o braço dá um coice de 25° para cima (mais na mão que no ombro) e volta em ~0,2 s; clarão laranja com luz no cano por 0,05 s; a bala sai do cano com rastro (feito por código) | — | Mira |
| Death (morte) | Vida chega a zero | 50 frames (2,1 s) | — |

*\[Opcional: insira aqui a imagem/concept do personagem ou a máquina de estados do Animator.\]*

# **5\. Inimigos e Obstáculos** {#5.-inimigos-e-obstáculos}

| Nome | Comportamento / IA | Dano / efeito | Como derrotar / evitar | Fases |
| :---- | :---- | :---- | :---- | :---- |
| Zumbi | Perambula devagar (0,5 m/s) a até 4 m de onde nasceu. Percebe o detetive se o vê (até 10 m, cone de 120° à frente, sem túmulo/parede no meio), se ele chega a 3 m ou se leva dano; então persegue (1,2 m/s) desviando de túmulos e cercas. A 1,3 m ataca uma vez a cada 1,5 s. Desiste com o detetive a mais de 18 m e volta para perto de onde nasceu. Ao morrer, toca Death (1,5 s: recua, os joelhos cedem e cai de bruços); o corpo fica no chão, sem colisão e sem ser alvo da mira. Animações: Idle; Walk (1,0 s, loop) mancando, arrastando a perna direita e com os braços esticados para frente, mais lento ao perambular; Attack (1,2 s) ergue os braços e golpeia para baixo, e o dano sai no golpe (0,45 s) se o detetive ainda estiver a até 1,6 m. Modelo "Lowpoly_zombie" (Seção 15), que só trazia Idle; Walk, Attack e Death foram geradas por código no editor (ZombieAnimationBaker) | 1 hit por ataque | 10 de vida: 2 tiros ou 5 socos | Cemitério (5: leste, oeste, nordeste, noroeste e frente da cripta) |
| Esqueleto (chefe) | Fica enterrado no centro do salão das catacumbas. Quando o detetive chega a 6 m, surge do chão (Spawn) e grita (Scream) — 2,4 s em que não leva dano nem pode ser alvo da mira — e passa a perseguir correndo (2,0 m/s) sem desistir dentro do salão. Ao chegar a 1,6 m ataca na hora, sorteando entre dois golpes de espada (Slash01, Slash02) e uma estocada (Stab), e se compromete com o golpe: fica 1,7 s parado e sem virar (golpe + recuperação). O dano sai a 0,5 s, só se o detetive ainda estiver a até 1,9 m e num cone de 120° à frente dele — recuar, correr ou passar para o lado faz o golpe errar, e a recuperação é a janela para atirar. Ao morrer, toca Death e o corpo fica no chão. Modelo e animações do pacote Skeleton (Seção 15) | 2 hits por ataque | 40 de vida: 8 tiros ou 20 socos | Catacumbas (salão final) |

## **5.1 Chefe (se houver)** {#5.1-chefe-(se-houver)}

**Esqueleto, guardião do livro.** 40 de vida (8 tiros); cada golpe tira 2 hits (o detetive aguenta 5). Fase única: surge e grita ao ser despertado, depois persegue e alterna três ataques (dois cortes e uma estocada). Ponto fraco: cada golpe o prende no lugar por 1,7 s, mesmo quando erra, e ele não tem ataque à distância — dá para provocar o golpe, sair do alcance e atirar enquanto ele se recupera. O livro da maldição, no altar atrás dele, só pode ser pego depois que ele cai.

# **6\. Itens, Coletáveis e Power-ups** {#6.-itens,-coletáveis-e-power-ups}

| Item | Efeito | Duração | Onde aparece |
| :---- | :---- | :---- | :---- |
| Item de cura (Tônico medicinal) | Restaura 3 hits de vida (não é gasto com a vida cheia) | Consumido ao usar | Cemitério (1) e catacumbas (2: sala central e sala leste) |
| Revólver | Arma do detetive; começa guardado e é equipado/guardado pelo inventário (Usar) | Permanente | Inventário inicial (tambor cheio) |
| Chave da casa do coveiro | Destranca a porta da casa do coveiro | Consumida ao usar | Cemitério, ao lado da cova aberta perto da casa do coveiro |
| Munição | Balas do revólver; empilha no inventário; R recarrega o tambor de 6 | Consumida ao recarregar | Catacumbas: 3 caixas de 6 balas (sala de entrada, beco norte e sala leste) |
| Chave das catacumbas | Abre a cripta e leva às catacumbas | Consumida ao usar | Casa do coveiro, em cima da mesa |
| Moedas de ouro | Sem utilidade; ocupa um espaço, pode ser analisada e descartada | — | Catacumbas, sala oeste (ao lado do baú) |
| Relógio de bolso | Sem utilidade; ocupa um espaço, pode ser analisado e descartado | — | Catacumbas, beco sul (em cima da mesa) |
| Cálice | Sem utilidade; ocupa um espaço, pode ser analisado e descartado | — | Catacumbas, sala nordeste (em cima da mesa) |
| Livro da maldição | Pegá-lo (E) com o esqueleto morto desfaz a maldição e encerra o jogo (não vai para o inventário) | — | Catacumbas, altar do salão |

Os itens são ScriptableObjects (Assets/Items) com nome, texto de análise, tipo (Objeto, Arma ou Cura), se empilha, quanto cura e o modelo usado ao largar no chão.

# **7\. Fases e Progressão** {#7.-fases-e-progressão}

| Fase / onda | Objetivo | Novidade introduzida | Dificuldade |
| :---- | :---- | :---- | :---- |
| 1. Cemitério | Achar a chave do coveiro (guardada pelos zumbis), abrir a casa do coveiro e pegar a chave das catacumbas | Zumbis, inventário, revólver e soco | Fácil |
| 2. Catacumbas | Explorar o labirinto (munição, cura e objetos sem utilidade), vencer o esqueleto e pegar o livro | Chefe, gestão de munição e espaço no inventário | Difícil |

## **7.1 Curva de dificuldade** {#7.1-curva-de-dificuldade}

* **Cemitério (fácil):** zumbis lentos (1,2 m/s), 10 de vida e 1 hit por ataque, espalhados em cinco pontos; dá para fugir deles. O jogador aprende tank controls, mira, soco e inventário. Só 6 balas (as do tambor) e 1 tônico, então já é preciso escolher quando atirar.
* **Catacumbas (difícil):** espaço apertado em labirinto, munição e cura contadas (18 balas, 2 tônicos) e itens sem utilidade que tomam espaço no inventário. No fim, o esqueleto: 4× a vida do zumbi, corre (2,0 m/s), não desiste e tira 2 hits por golpe.
* **Checkpoint:** a entrada das catacumbas. Morrer lá embaixo recomeça no pé da escada com o estado que o detetive tinha ao descer, sem refazer o cemitério.

## **7.2 Pontuação e recompensas** {#7.2-pontuação-e-recompensas}

O jogo não tem pontuação nem ranking, como é comum no survival horror clássico. A recompensa é a progressão na história: chaves que abrem novas áreas, munição e cura que dão fôlego para continuar e, no fim, o livro que desfaz a maldição.

# **8\. Interface (Telas e HUD)** {#8.-interface-(telas-e-hud)}

## **8.1 Fluxo de telas** {#8.1-fluxo-de-telas}

*Menu Inicial  →  Jogo (Cemitério → Catacumbas)  →  Fim (vitória) → Menu   \|   Game Over → Recomeçar   \|   Pausa → Menu   \|   Menu Inicial  →  Controles / Créditos*

| Tela | Elementos / botões | Leva para |
| :---- | :---- | :---- |
| **Menu inicial** | Sobre a própria cena, que fica congelada e escurecida ao fundo (câmera do portão). Título "Cemetery Gates" em letra gótica, subtítulo "sobreviva ou morra" e os botões Iniciar, Controles, Créditos e Sair, em fonte de máquina de escrever (âmbar quando selecionados; mouse ou setas + Enter). Música: Silent Pog | Jogo (Iniciar), Controles, Créditos, fecha o jogo (Sair) |
| **Controles** (pelo menu inicial ou pela pausa) | Cada tecla em âmbar ao lado do que ela faz (W, S, A/D, Shift + W, E, Mouse Esq., Mouse Dir., R, I, Esc), uma dica sobre os tank controls e Voltar (ou Esc) | A tela que abriu (menu inicial ou pausa) |
| **Inventário** (tecla I, pausa o jogo) | 10 espaços, detalhes do item, condição e arma equipada, botões Usar / Analisar / Descartar | Jogo (I ou Esc) |
| **Pausa** (Esc, pausa o jogo e o som) | Título "Pausa" em letra gótica, a frase "Os mortos também sabem esperar." e os botões Voltar ao jogo, Controles e Sair, no estilo do menu inicial | Jogo (Voltar ao jogo ou Esc), Controles, Menu inicial (Sair; o progresso se perde) |
| **Game over** | Aparece com fade 2,3 s depois da morte (fim da animação Death): "Game Over" em vermelho-sangue, a frase "O detetive tombou entre os túmulos." e um botão: Recomeçar do início (cemitério) ou Recomeçar nas catacumbas | Jogo do início, ou entrada das catacumbas com o estado que o detetive tinha ao descer; sem passar pelo menu |
| **Fim (vitória)** | Ao pegar o livro da maldição: fade de 2 s para "Fim" em letra gótica (cor osso), a frase "A maldição foi desfeita. Os mortos voltam a descansar." e Voltar ao menu; o jogo congela por trás | Menu inicial |
| **Créditos** | Integrantes, orientação, modelos 3D, fontes e sons; Voltar (ou Esc) | Menu inicial |

## **8.2 HUD** {#8.2-hud}

Como nas referências (Resident Evil, Alone in the Dark), a tela de jogo fica limpa: não há barras fixas. As informações aparecem quando importam.

| Informação | Representação | Posição na tela |
| :---- | :---- | :---- |
| Mensagens de jogo | Texto em amarelo pálido por 3 s: item pego, inventário cheio, porta trancada, dicas | Centro inferior |
| Munição | Mensagem ao recarregar ("Revólver recarregado (4/6)."), ao tentar atirar sem balas e quando não há munição; a quantidade de balas aparece no espaço do inventário (Munição x12) | Centro inferior / inventário |
| Vida | "Condição: 7/10" na tela do inventário | Inventário (I) |
| Arma equipada | "Arma: Revólver" na tela do inventário e "(equipado)" no espaço dele | Inventário (I) |

# **9\. Arte e Estilo Visual** {#9.-arte-e-estilo-visual}

**Estilo: Low Poly** — modelos simples, mas bem definidos.

**Resolução / renderização:** Jogo 3D na resolução nativa da tela, no pipeline padrão (Built-in) da Unity. A atmosfera vem da iluminação (lua, tochas e lampiões), da névoa colorida e da paleta abaixo; as telas de menu, pausa e Game Over têm uma vinheta escura (imagem própria). Não há sprites: a interface usa texto (fontes da Seção 15) e painéis simples.

**Paleta de cores:**

| Cor | Hex | Uso no jogo |
| :---- | :---- | :---- |
| Preto quente | \#24201A | Sombras, fundo da câmera, fundo da UI/menus |
| Roxo profundo | \#3F2548 | Luz ambiente/tom das sombras, névoa distante |
| Índigo | \#35369C | Luz da lua (luz direcional), áreas frias |
| Lavanda | \#7C679E | Névoa próxima, pedra, lápides, contornos |
| Amarelo pálido | \#C8C793 | Texto da UI, documentos/notas, ossos, destaque de interagível |
| Âmbar (luz) | \#E09A3E | Lampião, velas, janelas acesas, postes |
| Âmbar escuro | \#8A4B1F | Brasas, madeira, ferrugem, falloff das luzes quentes |

Clima: o cenário é frio e arroxeado; o âmbar aparece só onde há fogo e luz humana, guiando o olhar do jogador. Texto em amarelo pálido sobre preto quente mantém a leitura mesmo com o filtro CRT.

**Estética (sensações):**

* Jogo de câmera — a câmera fixa esconde partes do cenário e traz a sensação de algo escondido, de mistério.

* Contraste de luz e sombra — dualidade entre bondade e maldade.

* Tons frios e pouca interação com o ambiente — sensação de solidão, clima sombrio.

**Cenário inicial — Cemitério:** área cercada de \~44 × 36 m, com cerca de ferro e pilares duplos nas emendas, que já fazem parte de cada trecho da cerca (alguns trechos quebrados) e um portão em arco ao sul, por onde o detetive chega. O chão de terra é uniforme. Do portão sai um caminho de pedra até a cripta, ao norte, cruzado por um caminho leste–oeste que termina num oratório com velas. Lápides em quatro quadras irregulares. A casa do coveiro (casebre de madeira, janela acesa, lampião na porta) fica no canto sudoeste, ao lado de uma cova aberta com um lampião no chão; um portão lateral de serviço fica na cerca oeste. Na quadra nordeste há uma cova saqueada (lápide quebrada, caixão fora da cova e ossos espalhados). Pinheiros e árvores secas cercam o cemitério e somem na névoa. Luz: lua índigo, ambiente roxo profundo e névoa; o âmbar aparece só nos lampiões, velas e na janela do casebre.

**Casa do coveiro — interior:** aberta a chave, a porta dá para um cômodo de \~3,3 × 4,3 m com chaminé de pedra, mesa com a chave das catacumbas, banco, estante, barril e pote, iluminado por um lampião âmbar. Câmera fixa no canto sudeste, sob o telhado; ao sair, volta a câmera de fora.

**Catacumbas:** cena própria, montada com o Elementary Dungeon Pack Lite (pedra acinzentada, tochas com fogo). O detetive chega pelo pé de uma escada. Labirinto curto em grade de \~4,8 m: sala de entrada, sala oeste com beco ao norte, sala central com beco ao sul, sala leste e sala nordeste, ligadas por arcos; antecâmara com corredor até o salão final (3 × 3 módulos, quatro colunas, altar com o livro ao fundo e o esqueleto enterrado no centro). Uma câmera fixa por sala (cada módulo separado por arco tem a sua, no canto do lado por onde se entra; o salão tem uma só). Luz: só tochas âmbar, ambiente quase preto, névoa escura e um brilho lavanda no centro do salão e sobre o livro.

**Origem dos assets:** Ambos — próprios e de terceiros (listados na Seção 15). Próprios: modelo do detetive (adaptado no Blender a partir do "Man in Suit"), casa do coveiro, materiais, vinheta, trilha sonora e efeitos sonoros. Os arquivos-fonte ficam em Assets/Models/Source~ (.blend) e Assets/Audio/Source~ (gerador dos sons).

# **10\. Áudio** {#10.-áudio}

## **10.1 Trilha sonora** {#10.1-trilha-sonora}

| Música | Onde toca | Fonte / autor |
| :---- | :---- | :---- |
| Silent Pog (silent_pog.wav) | Menu inicial e tela de fim | Vinicius Vaes Massagli (equipe) |
| Alta Tensão (alta_tensao.wav) | Cemitério, durante o jogo | Vinicius Vaes Massagli (equipe) |
| CHASE (chase.wav) | Catacumbas, incluindo a luta com o esqueleto | Vinicius Vaes Massagli (equipe) |

As faixas ficam em Assets/Audio/Musica e tocam em loop. A troca entre elas tem fade de 1,5 s. No Game Over a música some e fica só o som da morte; no menu de pausa ela pausa junto com o jogo.

## **10.2 Efeitos sonoros** {#10.2-efeitos-sonoros}

| Ação / evento | Efeito sonoro | Fonte / autor |
| :---- | :---- | :---- |
| Passos do detetive (andando a cada 0,5 s, correndo a cada 0,45 s): calcanhar, sola e ponta do pé em terra com cascalho | passo_1 a passo_6 (sorteado) | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Tiro do revólver | tiro.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Gatilho sem balas | tiro_vazio.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Recarregar (R) | recarga.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Soco (movimento / acerto) | soco_ar.wav / soco_acerto.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Detetive leva dano / morre | detetive_dano.wav / detetive_morte.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Zumbi gemendo (a cada 5–12 s) | zumbi_gemido_1 a 3 (sorteado) | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Zumbi avista o detetive | zumbi_rosnado.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Zumbi ataca (golpe em 0,45 s, junto com o dano) | zumbi_ataque.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Zumbi leva dano / morre (corpo bate no chão em 1,25 s) | zumbi_dor.wav / zumbi_morte.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Esqueleto (chefe): grito ao surgir, golpe, dano e morte | zumbi_rosnado / zumbi_ataque / zumbi_dor / zumbi_morte (os mesmos do zumbi) | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Portão de entrada e porta da casa do coveiro abrindo | porta_abrindo.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Porta trancada / destrancada com a chave | trancado.wav / destrancar.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Pegar item / pegar o livro da maldição | item_pegar.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |

Os sons tocam com pequena variação de pitch a cada vez, para não soarem repetidos.

# **11\. Escopo do Projeto** {#11.-escopo-do-projeto}

| Escopo mínimo (obrigatório) | Extras (se houver tempo) |
| :---- | :---- |
| Detetive com tank controls e animações — **feito** | Chefe final (esqueleto) — **feito** |
| Cemitério com câmeras fixas e zumbis com IA — **feito** | Segunda fase (catacumbas) com checkpoint — **feito** |
| Combate: revólver (mira, munição, recarga) e soco — **feito** | Trilha sonora original (3 faixas) — **feito** |
| Inventário com itens, chaves e cura — **feito** | Tela de Controles e menu de Pausa — **feito** |
| Menu inicial, Game Over, Fim e Créditos — **feito** | Tela de Opções (volume) — não feito |
| Efeitos sonoros ligados às ações — **feito** | Filtro CRT / render em baixa resolução — não feito |
|  | Segundo inimigo comum (Ghoul) — não feito (modelo importado, sem uso) |
|  | Save no lampião com galhos e extrator — cancelado (v2.4 / 2.6) |

## **11.1 Checklist de elementos sugeridos** {#11.1-checklist-de-elementos-sugeridos}

☑  Estrutura completa: menu inicial, vitória/game over, reiniciar/voltar ao menu e créditos — Menu inicial, tela de Fim, Game Over com Recomeçar, Pausa → Sair para o menu, Créditos (8.1)

☑  Objetivo claro: condição de vitória e de derrota definidas (2.2 e 2.3)

☑  Controle do jogador com resposta imediata ao input — tank controls, tiro e soco (3.2 e 3.3)

☑  Colisão e física: CharacterController com gravidade, raycast do tiro bloqueado pelo cenário, esfera do soco, portas com collider e NavMesh (3.4)

☑  Animação: detetive com Idle, Walk, Run, WalkBack, Punch, Death e mira/tiro (4.1); zumbis e chefe também animados

☑  Desafio: zumbis com percepção, perseguição e ataque; chefe com padrão próprio (5)

☑  HUD: munição e mensagens em tela; vida e arma na tela do inventário (8.2)

☑  Áudio: trilha de fundo por cena e efeitos sonoros ligados às ações (10)

☑  Progressão: 2 fases (cemitério e catacumbas) com dificuldade crescente (7)

# **12\. Equipe e Divisão de Tarefas** {#12.-equipe-e-divisão-de-tarefas}

*\[A distribuição equilibrada de tarefas é critério de avaliação. Esta tabela também serve de base para a parte da apresentação sobre as contribuições de cada integrante.\]*

| Integrante | Função principal | Tarefas / contribuições |
| :---- | :---- | :---- |
| Filipe Pereira Ferraz | *\[preencher\]* | *\[preencher\]* |
| Isaque de Campos Miassaca | *\[preencher\]* | *\[preencher\]* |
| Kaique Rabesco Valverde | Programação e integração | Repositório, cenário, movimentação, inventário, combate, inimigos e IA, câmeras, menus, segunda parte (casa do coveiro, catacumbas, chefe) e revisão do GDD *\[conferir\]* |
| Rafaela Sanchez | Game design e documentação | GDD (versões 1.0 a 2.2): conceito, controles, mecânicas, MDA, paleta, aparência do detetive, cenário inicial, inventário e combate *\[conferir\]* |
| Túlio Braga Alves | *\[preencher\]* | *\[preencher\]* |
| Vinicius Vaes Massagli | Áudio | Trilha sonora original (Silent Pog, Alta Tensão, CHASE) *\[conferir\]* |

# **13\. Cronograma** {#13.-cronograma}

Datas tiradas do histórico de versões deste documento e dos commits do repositório.

| Período | Entrega / marco | Responsável(is) | Status |
| :---- | :---- | :---- | :---- |
| 28/10 a 05/11 | GDD e escopo definidos (conceito, mecânicas, estética, cenário) | Rafaela *\[+ nomes\]* | Concluído |
| 09/11 a 12/11 | Protótipo jogável: cenário, detetive e movimentação, portão, câmeras fixas, inventário | Kaique *\[+ nomes\]* | Concluído |
| 15/11 a 17/11 | Combate (soco e revólver), zumbis com IA, animações e sons | Kaique *\[+ nomes\]* | Concluído |
| 18/11 | Menus, Game Over, segunda fase (catacumbas e chefe), trilha sonora | Kaique, Vinicius *\[+ nomes\]* | Concluído |
| 18/11 | Playtesting, balanceamento e correção de bugs (Seção 14) | *\[Nomes\]* | Em andamento |
| até 18/11 | README e créditos completos | Kaique | Concluído |
| até 18/11 | Build final e ensaio da apresentação | *\[Nomes\]* | A fazer |
| 19/11/2026 | Entrega via CONNECT+ e sorteio da ordem de apresentação | Todos | A fazer |

# **14\. Playtesting e Balanceamento** {#14.-playtesting-e-balanceamento}

| Data | Testador | Problema / observação | Ajuste realizado |
| :---- | :---- | :---- | :---- |
| 15/11 | Equipe | Sem inimigo por perto, segurar Mouse Dir. não fazia nada: não dava para mirar à vontade | O detetive sempre aponta o revólver ao mirar; sem alvo, A/D giram a mira (v2.2) |
| 17/11 | Equipe | Testando por acesso remoto, o detetive andava aos trancos com a tecla segurada | Leitura do input segura o último valor por 0,1 s (HeldInput) |
| 17/11 | Equipe | O detetive saía do quadro perto das bordas de algumas zonas | Câmeras reposicionadas e giro no lugar para manter o detetive visível (1.2) |
| 18/11 | Equipe | Pilares da cerca piscavam (z-fighting) | Removidos os pilares avulsos sobrepostos aos do modelo da cerca (v2.5) |
| 18/11 | Equipe | O save com galhos e extrator complicava o jogo curto sem acrescentar tensão | Save cancelado; checkpoint só na entrada das catacumbas (v2.4 / 2.6) |
| 18/11 | Equipe | O chefe virava e acertava o detetive o tempo todo: não havia momento seguro para atacar | Golpe comprometido: ataca ao entrar no alcance, pode errar e fica 1,7 s parado (v2.9) |
| 18/11 | Equipe | Console da Unity: as malhas das catacumbas não tinham Read/Write, e o NavMesh gerado em runtime falharia na build (o esqueleto ficaria sem caminho) | Read/Write ligado no modelo do Elementary Dungeon Pack |
| *\[dd/mm\]* | *\[Nome\]* | *\[Registrar os testes de jogo completo antes da entrega\]* | *\[…\]* |

# **15\. Créditos e Assets de Terceiros** {#15.-créditos-e-assets-de-terceiros}

| Asset | Tipo | Autor | Fonte (link) |
| :---- | :---- | :---- | :---- |
| Revolver | Modelo | [austincford](https://poly.pizza/u/austincford) | https\://poly.pizza/m/m6GY8hBgWJ |
| Halloween Bits | Modelo | Kay Lousberg | https\://poly.pizza/bundle/Halloween-Bits-5J1jHdy21M |
| Man in Suit (base do detetive, CC0) | Modelo + animações | [Quaternius](https://poly.pizza/u/Quaternius) | https\://poly.pizza/m/mQnGoME1ez |
| UnifrakturMaguntia (título, SIL OFL) | Fonte | j. "mach" wust | https\://fonts.google.com/specimen/UnifrakturMaguntia |
| Special Elite (botões e textos, Apache 2.0) | Fonte | Astigmatic | https\://fonts.google.com/specimen/Special+Elite |
| Elementary Dungeon Pack Lite (catacumbas, móveis da casa do coveiro, livro) | Modelos | Gridness Studios | Unity Asset Store |
| Skeleton (esqueleto chefe e animações) | Modelo + animações | SazenGames | Unity Asset Store |
| Lowpoly_zombie (zumbi, CC BY 4.0) | Modelo | churliaevyaroslav | https\://sketchfab.com/3d-models/e550de538cf042fdb15bc71e78800863 |
| Trilha sonora (3 faixas) | Música | Vinicius Vaes Massagli (equipe) | Própria |
| Efeitos sonoros | Sons | Equipe (sintetizados por código) | Própria |

Também estão no projeto, mas não aparecem no jogo final: "Ghoul" (Northcliffe, CC BY 4.0, https\://sketchfab.com/3d-models/32a93de8d7454b19bca6866746a8e3ee), "Skeleton Warrior" (Arsyuta777, CC BY 4.0, https\://sketchfab.com/3d-models/dfeb342871204ecba0ae25f48c487738) e Polygon Materials LITE (Poly Etereo, Unity Asset Store).

# **16\. Organização do Projeto** {#16.-organização-do-projeto}

## **16.1 Estrutura de pastas** {#16.1-estrutura-de-pastas}

Assets/Scenes — Cemiterio (menu inicial, cemitério e casa do coveiro) e Catacumbas; as duas estão no Build Settings (SampleScene é sobra do template e não entra no build)

Assets/Scripts — Player (movimento, vida), Combat (combate do detetive, inimigos), Inventory (inventário, itens, coleta, uso), Interaction (portão, porta, cripta, livro), Camera (zonas de câmera), UI (menu, pausa, controles, Game Over, fim), Audio (música, efeitos), Core (estado levado às catacumbas), Editor (gerador das animações do zumbi)

Assets/Items — dados dos itens (ScriptableObjects)

Assets/Prefabs — detetive, zumbi, esqueleto chefe, bala; Items (modelos dos itens no chão); UI (inventário e menus)

Assets/Models — detetive, casa do coveiro, revólver, inimigos; Source~ guarda os .blend (ignorado pela Unity)

Assets/Animations — Animator Controllers e clipes (zumbi, portas)

Assets/Material — materiais por cena (Cemiterio, Catacumbas)

Assets/Audio — Musica, SFX; Source~ guarda o gerador dos sons

Assets/Art/UI, Assets/Fonts — vinheta e fontes

Pacotes de terceiros nas pastas originais: Assets/Assets/Halloween Bits, Gridness Studios, SazenGames, Poly Etereo

## **16.2 Convenções de código e nomenclatura** {#16.2-convenções-de-código-e-nomenclatura}

* Classes e métodos em PascalCase (DetectiveMovement, TakeDamage); campos e variáveis em camelCase (walkSpeed, shotRange).
* Um script por responsabilidade, agrupado por sistema em pastas; valores de ajuste expostos no Inspector com [SerializeField].
* Comentários e textos do jogo em português.
* Assets: materiais com prefixo M\_ (M\_Pedra), texturas com T\_ (T\_Zombie\_BaseColor); sons e músicas em minúsculas com \_ (zumbi\_gemido\_1, alta\_tensao); itens e cenas pelo nome em português (ChaveCatacumbas, Cemiterio).

## **16.3 Versionamento (opcional)** {#16.3-versionamento-(opcional)}

**Repositório:** https\://github.com/krabvalverde/Cemetery-Gates

Git com Git LFS para cenas, modelos, texturas e áudio. Branch main para versões estáveis e develop para o desenvolvimento; commits por funcionalidade, com mensagens em português.

# **17\. Entrega e Apresentação** {#17.-entrega-e-apresentação}

## **17.1 Itens da entrega (19/11/2026 — CONNECT+)** {#17.1-itens-da-entrega-(19/11/2026-—-connect+)}

☐  Executável / build jogável para PC — *a gerar (File → Build Profiles, Windows, cenas Cemiterio e Catacumbas)*

☐  Projeto completo da engine (.zip) ou link do repositório no GitHub — https\://github.com/krabvalverde/Cemetery-Gates *(a branch main ainda só tem o primeiro commit: juntar a develop na main e enviar ao GitHub antes de entregar, e conferir se o repositório é público)*

☑  Game Design Document (este documento)

☑  README com os integrantes do grupo e as instruções do jogo — README.md na raiz do repositório

☑  Tela de créditos no jogo (obrigatória se houver assets de terceiros) — Menu inicial → Créditos: equipe, orientação, modelos 3D com autor e fonte (incluindo o zumbi), fontes tipográficas, música e sons

## **17.2 Roteiro da apresentação (máx. 15 min)** {#17.2-roteiro-da-apresentação-(máx.-15-min)}

| Tempo | Conteúdo | Quem apresenta |
| :---- | :---- | :---- |
| *\[\~5 min\]* | O jogo desenvolvido — demonstração jogável | *\[Nomes\]* |
| *\[\~5 min\]* | O código-fonte — estrutura do projeto e principais scripts | *\[Nomes\]* |
| *\[\~5 min\]* | Principais contribuições de cada integrante | *\[Nomes\]* |

## **17.3 Autoavaliação pelos critérios** {#17.3-autoavaliação-pelos-critérios}

| Critério | O que é observado | Como o jogo atende |
| :---- | :---- | :---- |
| **Funcionamento do jogo** | Executa sem erros críticos, jogável do início ao fim e cumpre a proposta | Fluxo completo: menu → cemitério → casa do coveiro → cripta → catacumbas → chefe → livro → Fim; Game Over com recomeço e checkpoint nas catacumbas *\[confirmar com um teste completo na build\]* |
| **Conceitos da disciplina** | Uso correto de estruturas e conceitos trabalhados em aula | MonoBehaviour e ciclo de vida (Awake/Start/Update/LateUpdate), CharacterController, raycast e OverlapSphere, triggers (zonas de câmera), Animator e CrossFade, coroutines, ScriptableObjects (itens), UnityEvents (portas), NavMesh, troca de cenas (SceneManager) e Time.timeScale para pausa |
| **Design e experiência do jogador** | Clareza das regras, curva de dificuldade, feedback visual/sonoro e diversão | Regras simples (2.4), duas fases com dificuldade crescente (7.1), mensagens de dica em portas trancadas, som para cada ação (10.2), coice e clarão do tiro, chefe com janela de ataque legível |
| **Trabalho em equipe** | Distribuição equilibrada de tarefas entre os integrantes | Ver Seção 12 *\[completar as contribuições de cada integrante\]* |
| **Organização do projeto e do código** | Nomenclatura significativa, organização de cenas/scripts | Pastas por sistema e por tipo de asset (16.1), convenções de nomes (16.2), comentários em português explicando o porquê de cada script e dos trechos não óbvios, Git com LFS (16.3) |
| **Apresentação** | Apresentação clara e coerente, com domínio e segurança | Roteiro em 17.2 *\[definir quem apresenta cada parte\]* |

