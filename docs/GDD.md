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

**Versão do documento: 1.0**

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

*\[Em 2 a 4 frases: do que se trata o jogo, quem o jogador controla, o que ele faz e por que é divertido.\]*

## **1.2 Ficha técnica** {#1.2-ficha-técnica}

**Gênero: Survival horror**

**Público-alvo: \+12**

**Plataforma: PC (Windows)**

**Engine / versão: Unity 6.5**

**Perspectiva e câmera: Câmera Fixa**

O cemitério é dividido em 10 zonas, cada uma com a sua câmera fixa, que liga quando o detetive entra na zona. Cada câmera enquadra a zona inteira, inclusive as bordas por onde o detetive entra. Se ainda assim ele chegar perto da borda do quadro, a câmera gira no lugar (sem se mover) só o necessário para mantê-lo visível; ao trocar de zona, a câmera nova já entra com ele no quadro.

**Duração média do jogo: De 1 a 2 horas** 

## **1.3 Referências e inspirações** {#1.3-referências-e-inspirações}

* Alone in The Dark (1992) – História, cenários, câmera

* Silent Hill 2 – Controles, combate, atmosfera, inimigos

* Resident Evil 1 – Câmera fixa, mira, inventário

## **1.4 Diferencial** {#1.4-diferencial}

*\[O que torna este jogo único em relação às referências?\]*

# **2\. Objetivo e Regras** {#2.-objetivo-e-regras}

## **2.1 Objetivo do jogador** {#2.1-objetivo-do-jogador}

Resolver o mistério do desaparecimento de corpos do cemitério.

## **2.2 Condição de vitória**

Resolver o mistério e chegar ao final vivo (e são).

## **2.3 Condição de derrota** {#2.3-condição-de-derrota}

Morrer ao enfrentar inimigos, não resolver o mistério.

## **2.4 Regras do jogo** {#2.4-regras-do-jogo}

* Regra 1 – Inicia com 10 “hits” que pode tomar

* Regra 2- Inventário com 10 espaços, um item por espaço, sem sistema de peso

* Regra 3- Um inimigo causa 1 hit por ataque

* Regra 4- Inimigos Especiais causam 3 hits

* Regra 5 – Os itens encontrados podem ser arremessados no cenário

# **3\. Gameplay** {#3.-gameplay}

## **3.1 Loop principal (core loop)** {#3.1-loop-principal-(core-loop)}

*\[Descreva o ciclo que se repete durante o jogo. Ex.: explorar → enfrentar inimigos → coletar itens → avançar de fase.\]*

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
| Interagir | E |  |
| Ação contextual | Espaço |  |
| Correr | Shift |  |

## **3.3 Mecânicas principais** {#3.3-mecânicas-principais}

*\[Detalhe cada mecânica: como funciona, quais valores usa e como o jogador percebe.\]*

| Mecânica | Descrição | Parâmetros (valores) |
| :---- | :---- | :---- |
| Vida | O detetive aguenta um número limitado de golpes; ao chegar a zero ele cai (animação Death), para de responder e os inimigos param de atacar; game over | 10 hits |
| Tank controls | W/S andam para frente/trás, A/D giram o personagem no lugar; Shift corre (só para frente) | Andar 1,6 m/s; correr 3,8 m/s; ré 1,1 m/s; giro 150°/s |
| Atirar com a arma (mira com auto-trava) | Com o revólver equipado, segurar Mouse Dir. deixa o personagem parado, com o braço estendido e o revólver apontado para frente, mesmo sem inimigos por perto. Sem alvo, A/D giram o detetive (150°/s) para mirar em qualquer direção; se houver inimigo à frente, a mira trava nele. Mouse Esq. atira a qualquer momento enquanto mira: o braço dá um coice, sai um clarão do cano e a bala voa visível com rastro, causando o dano ao chegar. Sem balas no tambor, aparece a mensagem para recarregar | Dano 5 (inimigo comum cai com 2 tiros); tambor de 6 balas; alcance 15 m; trava em inimigos até 60° à frente; 0,6 s entre tiros |
| Soco (corpo a corpo) | Sem mirar (ou sem o revólver equipado), Mouse Esq. dá um soco; o detetive fica parado durante o golpe. Não há defesa nem esquiva | Dano 2 (inimigo comum cai com 5 socos); alcance de \~1 m à frente; 0,9 s por soco |
| Recarregar | Com o revólver equipado, R completa o tambor usando balas do item Munição (empilhável) do inventário | Tambor de 6 balas; balas por caixa encontrada *\[a definir\]* |
| Coleta de itens | Itens encontrados no cenário vão para o inventário ao apertar E perto deles; com o inventário cheio o item fica no chão | Tecla E, até 1,5 m do item |
| Inventário | 10 espaços, um item por espaço, sem peso; itens empilháveis (ex.: munição) ocupam um espaço só; itens podem ser guardados, descartados ou arremessados no cenário. A tela do inventário pausa o jogo e mostra a condição (vida) e a arma equipada. Ações sobre o item escolhido: **Usar**, **Analisar** (mostra o texto explicativo do item) e **Descartar** (larga o item no chão, e ele pode ser pego de novo; itens sem modelo no mundo não podem ser largados) | 10 espaços |
| Usar item | Perto de um ponto do cenário que aceita aquele item (ex.: porta trancada), o item é usado nele e a história avança (a chave é consumida e a porta abre). Fora disso: arma equipa/guarda; item de cura restaura vida (não é gasto com a vida cheia) | Alcance de 2,2 m do ponto |
| Interagir com o cenário | Abrir/usar portas, portões e janelas; num ponto trancado, E mostra uma mensagem de dica | Tecla E |
| Save | Não há save. Morrer no cemitério ou sair para o menu recomeça do início; morrer nas catacumbas recomeça na entrada delas (ver Descer às catacumbas) | — |
| Descer às catacumbas | Usar a chave das catacumbas (inventário → Usar) diante da porta da cripta carrega as catacumbas. O detetive leva vida, inventário, arma equipada e balas no tambor; a chave é gasta. Morrer lá embaixo recomeça no pé da escada com exatamente esse estado | Sem a chave, E na porta mostra uma dica |
| Ação contextual | Espaço só funciona em pontos marcados do cenário (sair de buraco, atravessar vão); não há pulo livre | A definir |
| Lampião a óleo | O detetive carrega um lampião que ilumina em âmbar ao seu redor | A definir |

**Dinâmicas**

* Atirar em um inimigo causa dano.

* Socar um inimigo causa dano (menos que o tiro, mas não gasta munição).

* Gerenciar a munição: o tambor esvazia e só é recarregado com balas encontradas.

* Guardar ou descartar itens (gerenciamento de inventário).

* Levar um golpe reduz a vida.

* Consumir itens de cura restaura vida.

## **3.4 Colisão e física** {#3.4-colisão-e-física}

Detetive e inimigos usam CharacterController (cápsula com gravidade, sem pulo). O tiro do revólver é um raycast a partir do peito do detetive em direção ao inimigo travado (ou para frente, sem alvo): paredes e objetos do cenário bloqueiam a bala. O soco testa uma esfera pequena logo à frente do detetive no momento do golpe. Os inimigos causam dano ao chegar a 1,3 m do detetive.

# **4\. Personagem Principal** {#4.-personagem-principal}

**Nome:** *\[Nome do personagem\]*

**Descrição:** Detetive nos anos 1940; investiga a invasão e o desaparecimento de corpos no cemitério onde o pai está enterrado. Carrega lampião a óleo e revólver. Aparência: homem de meia-idade, cabelo e barba grisalhos, chapéu fedora escuro com fita preta, sobretudo cáqui na altura da canela (aberto na frente, com cinto), camisa clara, gravata vermelho-escura, calça e sapatos escuros. Modelo low poly (\~2.460 triângulos) com rig humanoide, baseado no "Man in Suit" (Seção 15). *\[Personalidade\]*

**Atributos:** Vida: 10 hits. *\[Velocidade, dano, alcance…\]*

## **4.1 Estados de animação** {#4.1-estados-de-animação}

*\[Mínimo esperado: parado e andando. Inclua os demais estados que existirem.\]*

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

*\[Pelo menos 1 tipo de inimigo ou obstáculo com comportamento próprio.\]*

| Nome | Comportamento / IA | Dano / efeito | Como derrotar / evitar | Fases |
| :---- | :---- | :---- | :---- | :---- |
| Zumbi | Perambula devagar (0,5 m/s) a até 4 m de onde nasceu. Percebe o detetive se o vê (até 10 m, cone de 120° à frente, sem túmulo/parede no meio), se ele chega a 3 m ou se leva dano; então persegue (1,2 m/s) desviando de túmulos e cercas. A 1,3 m ataca uma vez a cada 1,5 s. Desiste com o detetive a mais de 18 m e volta para perto de onde nasceu. Ao morrer, toca Death (1,5 s: recua, os joelhos cedem e cai de bruços); o corpo fica no chão, sem colisão e sem ser alvo da mira. Animações: Idle; Walk (1,0 s, loop) mancando, arrastando a perna direita e com os braços esticados para frente, mais lento ao perambular; Attack (1,2 s) ergue os braços e golpeia para baixo, e o dano sai no golpe (0,45 s) se o detetive ainda estiver a até 1,6 m | 1 hit por ataque | 10 de vida: 2 tiros ou 5 socos | Cemitério (5: leste, oeste, nordeste, noroeste e frente da cripta) |
| Esqueleto (chefe) | Fica enterrado no centro do salão das catacumbas. Quando o detetive chega a 6 m, surge do chão (Spawn) e grita (Scream) — 2,4 s em que não leva dano nem pode ser alvo da mira — e passa a perseguir correndo (2,0 m/s) sem desistir dentro do salão. A 1,6 m ataca a cada 1,4 s, sorteando entre dois golpes de espada (Slash01, Slash02) e uma estocada (Stab); o dano sai no meio do golpe (0,5 s). Ao morrer, toca Death e o corpo fica no chão. Modelo e animações do pacote Skeleton (Seção 15) | 2 hits por ataque | 40 de vida: 8 tiros ou 20 socos | Catacumbas (salão final) |
| Ghoul | Igual ao zumbi | 1 hit por ataque | 10 de vida: 2 tiros ou 5 socos | Catacumbas |
|  |  |  |  |  |

## **5.1 Chefe (se houver)** {#5.1-chefe-(se-houver)}

**Esqueleto, guardião do livro.** 40 de vida (8 tiros); cada golpe tira 2 hits (o detetive aguenta 5). Fase única: surge e grita ao ser despertado, depois persegue e alterna três ataques (dois cortes e uma estocada). Ponto fraco: o tempo entre ataques (1,4 s) e a falta de ataque à distância — dá para atirar recuando. O livro da maldição, no altar atrás dele, só pode ser pego depois que ele cai.

# **6\. Itens, Coletáveis e Power-ups** {#6.-itens,-coletáveis-e-power-ups}

| Item | Efeito | Duração | Onde aparece |
| :---- | :---- | :---- | :---- |
| Item de cura (Tônico medicinal) | Restaura vida | *\[Quantidade a definir\]* | Cemitério (1) e catacumbas (2: sala central e sala leste) |
| Revólver | Arma do detetive; equipada pelo inventário | Permanente | Inventário inicial |
| Chave da casa do coveiro | Destranca a porta da casa do coveiro | Consumida ao usar | Cemitério, ao lado da cova aberta perto da casa do coveiro |
| Munição | Balas do revólver; empilha no inventário; R recarrega o tambor de 6 | Consumida ao recarregar | Catacumbas: 3 caixas de 6 balas (sala de entrada, beco norte e sala leste) |
| Chave das catacumbas | Abre a cripta e leva às catacumbas | Consumida ao usar | Casa do coveiro, em cima da mesa |
| Moedas de ouro | Sem utilidade; ocupa um espaço, pode ser analisada e descartada | — | Catacumbas, sala oeste (ao lado do baú) |
| Relógio de bolso | Sem utilidade; ocupa um espaço, pode ser analisado e descartado | — | Catacumbas, beco sul (em cima da mesa) |
| Cálice | Sem utilidade; ocupa um espaço, pode ser analisado e descartado | — | Catacumbas, sala nordeste (em cima da mesa) |
| Livro da maldição | Pegá-lo (E) com o esqueleto morto desfaz a maldição e encerra o jogo | — | Catacumbas, altar do salão |
|  |  |  |  |
|  |  |  |  |
|  |  |  |  |

# **7\. Fases e Progressão** {#7.-fases-e-progressão}

*\[2 ou 3 fases, ou dificuldade que aumenta com o tempo/ondas. Descreva o que muda em cada etapa.\]*

| Fase / onda | Objetivo | Novidade introduzida | Dificuldade |
| :---- | :---- | :---- | :---- |
| 1. Cemitério | Achar a chave do coveiro (guardada pelos zumbis), abrir a casa do coveiro e pegar a chave das catacumbas | Zumbis, inventário, revólver e soco | Fácil |
| 2. Catacumbas | Explorar o labirinto (munição, cura e objetos sem utilidade), vencer o esqueleto e pegar o livro | Chefe, gestão de munição e espaço no inventário | Difícil |
|  |  |  |  |
|  |  |  |  |
|  |  |  |  |

## **7.1 Curva de dificuldade** {#7.1-curva-de-dificuldade}

*\[Como a dificuldade cresce: mais inimigos, maior velocidade, menos tempo, novos obstáculos. Mencione checkpoints, se houver.\]*

## **7.2 Pontuação e recompensas** {#7.2-pontuação-e-recompensas}

*\[Como os pontos são calculados, se há ranking, estrelas por fase, combo etc.\]*

# **8\. Interface (Telas e HUD)** {#8.-interface-(telas-e-hud)}

## **8.1 Fluxo de telas** {#8.1-fluxo-de-telas}

*\[Descreva a navegação entre telas. Estrutura completa esperada: menu inicial, jogo, vitória e/ou game over, reiniciar/voltar ao menu e créditos.\]*

*Menu Inicial  →  Jogo  →  Vitória / Game Over  →  Reiniciar ou Menu       \|       Menu Inicial  →  Créditos*

| Tela | Elementos / botões | Leva para |
| :---- | :---- | :---- |
| **Menu inicial** | Sobre a própria cena, que fica congelada e escurecida ao fundo (câmera do portão). Título "Cemetery Gates" em letra gótica, subtítulo "sobreviva ou morra" e os botões Iniciar, Controles, Créditos e Sair, em fonte de máquina de escrever (âmbar quando selecionados; mouse ou setas + Enter). Música: Silent Pog. Planejado: Opções | Jogo (Iniciar), Controles, Créditos, fecha o jogo (Sair) |
| **Controles** (pelo menu inicial ou pela pausa) | Cada tecla em âmbar ao lado do que ela faz (W, S, A/D, Shift + W, E, Mouse Esq., Mouse Dir., R, I, Esc), uma dica sobre os tank controls e Voltar (ou Esc) | A tela que abriu (menu inicial ou pausa) |
| **Opções** | *\[A definir\]* | Menu inicial |
| **Inventário** (tecla I, pausa o jogo) | 10 espaços, detalhes do item, condição e arma equipada, botões Usar / Analisar / Descartar | Jogo (I ou Esc) |
| **Pausa** (Esc, pausa o jogo e o som) | Título "Pausa" em letra gótica, a frase "Os mortos também sabem esperar." e os botões Voltar ao jogo, Controles e Sair, no estilo do menu inicial | Jogo (Voltar ao jogo ou Esc), Controles, Menu inicial (Sair; o progresso se perde) |
| **Vitória** | *\[Pontuação final, Jogar novamente, Menu\]* | *\[Jogo, Menu\]* |
| **Game over** | Aparece com fade 2,3 s depois da morte (fim da animação Death): "Game Over" em vermelho-sangue, a frase "O detetive tombou entre os túmulos." e um botão: Recomeçar do início (cemitério) ou Recomeçar nas catacumbas | Jogo do início, ou entrada das catacumbas com o estado que o detetive tinha ao descer; sem passar pelo menu |
| **Fim** | Ao pegar o livro da maldição: fade de 2 s para "Fim" em letra gótica (cor osso), a frase "A maldição foi desfeita. Os mortos voltam a descansar." e Voltar ao menu; o jogo congela por trás | Menu inicial |
| **Créditos** | Integrantes, orientação, modelos 3D, fontes e sons; Voltar (ou Esc) | Menu inicial |

## **8.2 HUD** {#8.2-hud}

*\[Informações exibidas durante o jogo: vida, pontos, tempo, munição, fase atual…\]*

| Informação | Representação | Posição na tela |
| :---- | :---- | :---- |
| *\[Vida\]* | *\[3 ícones de coração\]* | *\[Canto superior esquerdo\]* |
|  |  |  |
|  |  |  |
|  |  |  |

*\[Opcional: insira aqui um esboço/print das telas e do HUD.\]*

# **9\. Arte e Estilo Visual** {#9.-arte-e-estilo-visual}

**Estilo: Low Poly** — modelos simples, mas bem definidos.

**Resolução / tamanho dos sprites:** Render interno em baixa resolução (\~320×240, filtro point) ampliado para a tela, com filtro CRT de televisão antiga (scanlines, estática, ruído, vinheta). A estática é apenas visual.

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

**Origem dos assets:** Ambos — próprios e de terceiros (listados na Seção 15).

*\[Opcional: moodboard ou capturas de referência.\]*

# **10\. Áudio** {#10.-áudio}

## **10.1 Trilha sonora** {#10.1-trilha-sonora}

| Música | Onde toca | Fonte / autor |
| :---- | :---- | :---- |
| Silent Pog (silent_pog.wav) | Menu inicial e tela de fim | Vinicius Vaes Massagli (equipe) |
| Alta Tensão (alta_tensao.wav) | Cemitério, durante o jogo | Vinicius Vaes Massagli (equipe) |
| CHASE (chase.wav) | Catacumbas, incluindo a luta com o esqueleto | Vinicius Vaes Massagli (equipe) |

As faixas ficam em Assets/Audio/Musica e tocam em loop. A troca entre elas tem fade de 1,5 s. No Game Over a música some e fica só o som da morte; no menu de pausa ela pausa junto com o jogo.

## **10.2 Efeitos sonoros** {#10.2-efeitos-sonoros}

*\[Efeitos ligados a ações: pulo, dano, coleta, vitória, derrota, clique em botão…\]*

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
| Porta da casa do coveiro abrindo | porta_abrindo.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Porta trancada / destrancada com a chave | trancado.wav / destrancar.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |
| Pegar item | item_pegar.wav | Sintetizado por código (Assets/Audio/Source~/gerar_sons.py) |

Os sons tocam com pequena variação de pitch a cada vez, para não soarem repetidos.

# **11\. Escopo do Projeto** {#11.-escopo-do-projeto}

*\[Separe o que é obrigatório para o jogo funcionar do que será feito se sobrar tempo.\]*

| Escopo mínimo (obrigatório) | Extras (se houver tempo) |
| :---- | :---- |
| *\[Ex.: 2 fases com pulo e 2 inimigos\]* | *\[Ex.: chefe final e pulo duplo\]* |
|  |  |
|  |  |
|  |  |
|  |  |
|  |  |

## **11.1 Checklist de elementos sugeridos** {#11.1-checklist-de-elementos-sugeridos}

*\[Marque o que o jogo contempla.\]*

☐  Estrutura completa: menu inicial, vitória/game over, reiniciar/voltar ao menu e créditos

☐  Objetivo claro: condição de vitória e de derrota definidas

☐  Controle do jogador com resposta imediata ao input

☐  Colisão e física: ao menos uma interação física relevante (ou mecânica equivalente)

☐  Animação: personagem principal com estados animados (ex.: parado e andando)

☐  Desafio: ao menos 1 inimigo ou obstáculo com comportamento próprio

☐  HUD: vida, pontos, tempo ou munição em tela

☐  Áudio: trilha de fundo e efeitos sonoros ligados a ações

☐  Progressão: 2–3 fases ou dificuldade crescente

# **12\. Equipe e Divisão de Tarefas** {#12.-equipe-e-divisão-de-tarefas}

*\[A distribuição equilibrada de tarefas é critério de avaliação. Esta tabela também serve de base para a parte da apresentação sobre as contribuições de cada integrante.\]*

| Integrante | Função principal | Tarefas / contribuições |
| :---- | :---- | :---- |
| *\[Nome\]* | *\[Programação do jogador\]* | *\[Movimento, pulo, animações, colisões\]* |
|  |  |  |
|  |  |  |
|  |  |  |
|  |  |  |
|  |  |  |

# **13\. Cronograma** {#13.-cronograma}

*\[Planejamento até a entrega em 19/11/2026. Ajuste as datas conforme a realidade do grupo.\]*

| Período | Entrega / marco | Responsável(is) | Status |
| :---- | :---- | :---- | :---- |
| *\[até \_\_/\_\_\]* | GDD e escopo definidos e validados com o professor | *\[Nomes\]* | *\[A fazer\]* |
| *\[até \_\_/\_\_\]* | Protótipo jogável (movimento, colisão, 1 inimigo) | *\[Nomes\]* | *\[A fazer\]* |
| *\[até \_\_/\_\_\]* | Fases, HUD, menus e áudio integrados | *\[Nomes\]* | *\[A fazer\]* |
| *\[até \_\_/\_\_\]* | Playtesting, balanceamento e correção de bugs | *\[Nomes\]* | *\[A fazer\]* |
| *\[até 18/11\]* | Build final, README, créditos e ensaio da apresentação | *\[Nomes\]* | *\[A fazer\]* |
| 19/11/2026 | Entrega via CONNECT+ e sorteio da ordem de apresentação | *\[Nomes\]* | *\[A fazer\]* |

# **14\. Playtesting e Balanceamento** {#14.-playtesting-e-balanceamento}

*\[Registre os testes de jogabilidade e os ajustes feitos a partir deles.\]*

| Data | Testador | Problema / observação | Ajuste realizado |
| :---- | :---- | :---- | :---- |
| *\[dd/mm\]* | *\[Nome\]* | *\[Fase 2 muito difícil no salto final\]* | *\[Plataforma aproximada em 1 unidade\]* |
|  |  |  |  |
|  |  |  |  |
|  |  |  |  |

# **15\. Créditos e Assets de Terceiros** {#15.-créditos-e-assets-de-terceiros}

*\[Obrigatório se houver assets de terceiros: devem ser gratuitos, de uso livre ou licenciados, e também aparecer na tela de créditos do jogo.\]*

| Asset | Tipo | Autor | Fonte (link) |
| :---- | :---- | :---- | :---- |
| Revolver | Modelo | [austincford](https://poly.pizza/u/austincford) | https\://poly.pizza/m/m6GY8hBgWJ |
| Halloween Bits | Modelo | Kay Lousberg | https\://poly.pizza/bundle/Halloween-Bits-5J1jHdy21M |
| Man in Suit (base do detetive, CC0) | Modelo + animações | [Quaternius](https://poly.pizza/u/Quaternius) | https\://poly.pizza/m/mQnGoME1ez |
| UnifrakturMaguntia (título, SIL OFL) | Fonte | j. "mach" wust | https\://fonts.google.com/specimen/UnifrakturMaguntia |
| Special Elite (botões e textos, Apache 2.0) | Fonte | Astigmatic | https\://fonts.google.com/specimen/Special+Elite |
| Elementary Dungeon Pack Lite (catacumbas, móveis da casa do coveiro, livro) | Modelos | Gridness Studios | Unity Asset Store |
| Skeleton (esqueleto chefe e animações) | Modelo + animações | SazenGames | Unity Asset Store |

# **16\. Organização do Projeto** {#16.-organização-do-projeto}

## **16.1 Estrutura de pastas** {#16.1-estrutura-de-pastas}

*\[Descreva a organização de cenas, scripts e assets. Exemplo:\]*

Assets/Scenes — MainMenu, Level01, Level02, Credits

Assets/Scripts — Player, Enemies, UI, Managers

Assets/Art — Sprites, Animations, UI

Assets/Audio — Music, SFX

Assets/Prefabs

## **16.2 Convenções de código e nomenclatura** {#16.2-convenções-de-código-e-nomenclatura}

*\[Ex.: classes em PascalCase (PlayerController), variáveis em camelCase, um script por responsabilidade, prefixos de assets (spr\_, sfx\_, mus\_).\]*

## **16.3 Versionamento (opcional)** {#16.3-versionamento-(opcional)}

**Repositório:** *\[Link do GitHub\]*

*\[Fluxo de trabalho do grupo: branches, frequência de commits, quem faz o merge.\]*

# **17\. Entrega e Apresentação** {#17.-entrega-e-apresentação}

## **17.1 Itens da entrega (19/11/2026 — CONNECT+)** {#17.1-itens-da-entrega-(19/11/2026-—-connect+)}

☐  Executável / build jogável para PC

☐  Projeto completo da engine (.zip) ou link do repositório no GitHub

☐  Game Design Document (este documento)

☐  README com os integrantes do grupo e as instruções do jogo

☐  Tela de créditos no jogo (obrigatória se houver assets de terceiros)

## **17.2 Roteiro da apresentação (máx. 15 min)** {#17.2-roteiro-da-apresentação-(máx.-15-min)}

| Tempo | Conteúdo | Quem apresenta |
| :---- | :---- | :---- |
| *\[\~5 min\]* | O jogo desenvolvido — demonstração jogável | *\[Nomes\]* |
| *\[\~5 min\]* | O código-fonte — estrutura do projeto e principais scripts | *\[Nomes\]* |
| *\[\~5 min\]* | Principais contribuições de cada integrante | *\[Nomes\]* |

## **17.3 Autoavaliação pelos critérios** {#17.3-autoavaliação-pelos-critérios}

*\[Antes de entregar, confira como o jogo atende a cada critério de avaliação.\]*

| Critério | O que é observado | Como o jogo atende |
| :---- | :---- | :---- |
| **Funcionamento do jogo** | Executa sem erros críticos, jogável do início ao fim e cumpre a proposta | *\[Evidência\]* |
| **Conceitos da disciplina** | Uso correto de estruturas e conceitos trabalhados em aula | *\[Evidência\]* |
| **Design e experiência do jogador** | Clareza das regras, curva de dificuldade, feedback visual/sonoro e diversão | *\[Evidência\]* |
| **Trabalho em equipe** | Distribuição equilibrada de tarefas entre os integrantes | *\[Evidência\]* |
| **Organização do projeto e do código** | Nomenclatura significativa, organização de cenas/scripts | *\[Evidência\]* |
| **Apresentação** | Apresentação clara e coerente, com domínio e segurança | *\[Evidência\]* |

