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
| Navegar no inventário | Mouse, ou setas + Enter |  |
| Mirar (segurar) | Mouse Dir. |  |
| Ataque/ Tiro | Mouse Esq. |  |
| Interagir | E |  |
| Ação contextual | Espaço |  |
| Correr | Shift |  |

## **3.3 Mecânicas principais** {#3.3-mecânicas-principais}

*\[Detalhe cada mecânica: como funciona, quais valores usa e como o jogador percebe.\]*

| Mecânica | Descrição | Parâmetros (valores) |
| :---- | :---- | :---- |
| Vida | O detetive aguenta um número limitado de golpes; ao chegar a zero, game over | 10 hits |
| Tank controls | W/S andam para frente/trás, A/D giram o personagem no lugar; Shift corre (só para frente) | Andar 1,6 m/s; correr 3,8 m/s; ré 1,1 m/s; giro 150°/s |
| Atirar com a arma (mira com auto-trava) | Segurar Mouse Dir. deixa o personagem parado e trava a mira no inimigo mais próximo à frente; Mouse Esq. atira com o revólver | A definir |
| Bater com objetos (corpo a corpo) | Ataque sem mirar contra inimigos próximos, usando objetos | Objetos e valores a definir |
| Coleta de itens | Itens encontrados no cenário vão para o inventário ao apertar E perto deles; com o inventário cheio o item fica no chão | Tecla E, até 1,5 m do item |
| Inventário | 10 espaços, um item por espaço, sem peso; galhos especiais empilham no mesmo espaço; itens podem ser guardados, descartados ou arremessados no cenário. A tela do inventário pausa o jogo e mostra a condição (vida) e a arma equipada. Ações sobre o item escolhido: **Usar**, **Analisar** (mostra o texto explicativo do item) e **Descartar** (larga o item no chão, e ele pode ser pego de novo; itens sem modelo no mundo não podem ser largados) | 10 espaços |
| Usar item | Perto de um ponto do cenário que aceita aquele item (ex.: porta trancada), o item é usado nele e a história avança (a chave é consumida e a porta abre). Fora disso: arma equipa/guarda; item de cura restaura vida (não é gasto com a vida cheia) | Alcance de 2,2 m do ponto |
| Interagir com o cenário | Abrir/usar portas, portões e janelas; num ponto trancado, E mostra uma mensagem de dica | Tecla E |
| Save manual (extrator) | O jogador encontra galhos especiais, que ficam no inventário. Só é possível salvar em um extrator (espécie de moedor), que fica nas áreas seguras: o galho é inserido, vira óleo e o detetive reabastece o lampião. Cada save consome 1 galho | 1 galho por save; áreas seguras a definir |
| Ação contextual | Espaço só funciona em pontos marcados do cenário (sair de buraco, atravessar vão); não há pulo livre | A definir |
| Lampião a óleo | O detetive carrega um lampião que ilumina em âmbar ao seu redor | A definir |

**Dinâmicas**

* Atirar em um inimigo causa dano.

* Guardar ou descartar itens (gerenciamento de inventário).

* Levar um golpe reduz a vida.

* Consumir itens de cura restaura vida.

## **3.4 Colisão e física** {#3.4-colisão-e-física}

*\[Quais interações físicas existem (pulo, gravidade, projéteis, empurrar objetos, dano por contato) e como foram implementadas (colliders, rigidbody, triggers, camadas). Se o jogo não usar física — ex.: puzzle —, explique quais mecânicas ocupam esse papel.\]*

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

*\[Opcional: insira aqui a imagem/concept do personagem ou a máquina de estados do Animator.\]*

# **5\. Inimigos e Obstáculos** {#5.-inimigos-e-obstáculos}

*\[Pelo menos 1 tipo de inimigo ou obstáculo com comportamento próprio.\]*

| Nome | Comportamento / IA | Dano / efeito | Como derrotar / evitar | Fases |
| :---- | :---- | :---- | :---- | :---- |
| *\[Slime\]* | *\[Patrulha entre dois pontos; vira ao bater na parede\]* | *\[1 de vida\]* | *\[Pular em cima\]* | *\[1, 2\]* |
|  |  |  |  |  |
|  |  |  |  |  |
|  |  |  |  |  |

## **5.1 Chefe (se houver)** {#5.1-chefe-(se-houver)}

*\[Padrões de ataque, fases do chefe, pontos fracos e quantidade de vida.\]*

# **6\. Itens, Coletáveis e Power-ups** {#6.-itens,-coletáveis-e-power-ups}

| Item | Efeito | Duração | Onde aparece |
| :---- | :---- | :---- | :---- |
| Item de cura | Restaura vida | *\[Quantidade a definir\]* | *\[A definir\]* |
| Galho especial | Inserido no extrator (áreas seguras), vira óleo para o lampião e permite salvar o jogo; empilha no inventário | Consumido ao salvar (1 por save) | *\[A definir\]* |
| Revólver | Arma do detetive; equipada pelo inventário | Permanente | Inventário inicial |
| Chave da casa do coveiro | Destranca a porta da casa do coveiro | Consumida ao usar | Cemitério, ao lado da cova aberta perto da casa do coveiro |
|  |  |  |  |
|  |  |  |  |
|  |  |  |  |

# **7\. Fases e Progressão** {#7.-fases-e-progressão}

*\[2 ou 3 fases, ou dificuldade que aumenta com o tempo/ondas. Descreva o que muda em cada etapa.\]*

| Fase / onda | Objetivo | Novidade introduzida | Dificuldade |
| :---- | :---- | :---- | :---- |
| *\[Fase 1\]* | *\[Chegar à bandeira\]* | *\[Pulo e primeiro inimigo\]* | *\[Fácil\]* |
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
| **Menu inicial** | Título do jogo, Novo jogo, Continuar, Opções, Créditos, Sair (com música de fundo) | Jogo (novo), Jogo (último save), Opções, Créditos, fecha o jogo |
| **Opções** | *\[A definir\]* | Menu inicial |
| **Inventário** (tecla I, pausa o jogo) | 10 espaços, detalhes do item, condição e arma equipada, botões Usar / Analisar / Descartar | Jogo (I ou Esc) |
| **Pausa (opcional)** | *\[Continuar, Reiniciar, Menu\]* | *\[Jogo, Menu\]* |
| **Vitória** | *\[Pontuação final, Jogar novamente, Menu\]* | *\[Jogo, Menu\]* |
| **Game over** | *\[Mensagem, Reiniciar, Menu\]* | *\[Jogo, Menu\]* |
| **Créditos** | *\[Integrantes, assets de terceiros, Voltar\]* | *\[Menu\]* |

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

**Cenário inicial — Cemitério:** área cercada de \~44 × 36 m, com cerca de ferro e pilares de pedra (alguns trechos quebrados) e um portão em arco ao sul, por onde o detetive chega. O chão de terra é uniforme. Do portão sai um caminho de pedra até a cripta, ao norte, cruzado por um caminho leste–oeste que termina num oratório com velas. Lápides em quatro quadras irregulares. A casa do coveiro (casebre de madeira, janela acesa, lampião na porta; interior a fazer) fica no canto sudoeste, ao lado de uma cova aberta com um lampião no chão; um portão lateral de serviço fica na cerca oeste. Na quadra nordeste há uma cova saqueada (lápide quebrada, caixão fora da cova e ossos espalhados). Pinheiros e árvores secas cercam o cemitério e somem na névoa. Luz: lua índigo, ambiente roxo profundo e névoa; o âmbar aparece só nos lampiões, velas e na janela do casebre.

**Origem dos assets:** Ambos — próprios e de terceiros (listados na Seção 15).

*\[Opcional: moodboard ou capturas de referência.\]*

# **10\. Áudio** {#10.-áudio}

## **10.1 Trilha sonora** {#10.1-trilha-sonora}

| Música | Onde toca | Fonte / autor |
| :---- | :---- | :---- |
| Tema do menu | Menu inicial | Fornecida por Rafaela — *\[autor/fonte a definir\]* |
|  |  |  |
|  |  |  |

## **10.2 Efeitos sonoros** {#10.2-efeitos-sonoros}

*\[Efeitos ligados a ações: pulo, dano, coleta, vitória, derrota, clique em botão…\]*

| Ação / evento | Efeito sonoro | Fonte / autor |
| :---- | :---- | :---- |
| *\[Pulo\]* | *\[jump.wav\]* | *\[Autor — site\]* |
|  |  |  |
|  |  |  |
|  |  |  |
|  |  |  |

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
|  |  |  |  |
|  |  |  |  |

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

