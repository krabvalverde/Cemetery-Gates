# Segunda parte: casa do coveiro, catacumbas, esqueleto e final

Data: 18/11/2026 · Pedido por: Kaique

## Intenção

Fechar o jogo do começo ao fim. Hoje ele termina ao abrir a porta da casa do coveiro. A segunda parte continua assim:

1. O detetive entra na casa e acha a chave das catacumbas.
2. Leva a chave até a cripta, no norte do cemitério, e entra numa cena nova: as catacumbas.
3. Explora um labirinto curto de salas, enfrenta o esqueleto (o chefe, mais difícil que os zumbis) e pega o livro que desfaz a maldição.
4. O jogo termina.

## Objetivos

- Casa do coveiro com interior em que dá para entrar, com a chave das catacumbas em cima de uma mesa.
- Cripta (ao norte) como entrada das catacumbas: interagir com a chave carrega a cena `Catacumbas`, sem animação de abrir.
- Cena `Catacumbas` montada com o **Elementary Dungeon Pack Lite** (Gridness Studios):
  - um labirinto curto de salas simples;
  - itens de cura, munição e objetos sem utilidade espalhados;
  - um salão final com o esqueleto e o livro.
- Esqueleto (pacote **SazenGames/Skeleton**) como chefe forte.
- Livro (`Book_Holy_C`) que encerra o jogo com uma tela de fim e o botão "Voltar ao menu".

## Fora do escopo

- Save. Foi cancelado e continua sem existir.
- Animação de abrir a cripta.
- Ataque à distância do esqueleto (o pacote tem a animação de arremessar, mas não será usada).
- Ajuste de som novo, além de reaproveitar os sons que já existem.

## O que não pode acontecer

- Morrer no chefe e ter que refazer o cemitério inteiro.
- Material rosa (shader de outro pipeline) em qualquer objeto novo.
- Chegar ao chefe sem munição suficiente para vencê-lo.

## Restrições do projeto

- Unity 6000.5.8f1, pipeline Built-in, materiais Standard, Input Manager antigo (tank controls), UI com uGUI `Text`.
- Câmeras fixas por área (`CameraZone`), no estilo de survival horror clássico.
- O `Enemy.cs` gera o NavMesh em tempo de execução e guarda tudo num campo `static` que só é gerado uma vez. Ao trocar de cena, o NavMesh do cemitério continuaria valendo e o das catacumbas nunca seria gerado. É preciso descartar e gerar de novo por cena.
- O material do esqueleto usa um shader que não existe no projeto e precisa ser trocado para Standard com a textura dele. O do pacote de masmorra já é Standard.
- Paleta e fontes do GDD (UnifrakturMaguntia para títulos, Special Elite para textos).

## Abordagens

| Tema | Escolhida | Alternativa descartada |
| :-- | :-- | :-- |
| Levar o estado entre cenas | **Instantâneo na memória:** ao entrar na cripta, guarda vida, inventário, arma equipada e balas no tambor num estado estático. As catacumbas aplicam esse estado ao abrir, e morrer lá recarrega a cena com o mesmo instantâneo. Simples, sem arquivo. | Detetive persistente entre cenas (`DontDestroyOnLoad`): complica câmeras, menus e o recomeço. |
| Detetive e menus na cena nova | **Prefabs compartilhados:** Detetive, MenusUI e InventarioUI viram prefabs usados pelas duas cenas. | Copiar os objetos: mais rápido agora, mas cada ajuste teria que ser feito em dobro. |
| Esqueleto | **Estender o `Enemy.cs`** (perseguição, desvio, dano e morte já existem) com estado adormecido (surge e grita), sorteio entre ataques e valores próprios. | Script só do chefe: mais livre, mas duplica a perseguição. |

## Como fica

### Casa do coveiro (cena Cemiterio)

- O `BoxCollider` único da casa é trocado por colliders nas quatro paredes, com o vão da porta livre. A porta continua sendo a `HingedDoor`, aberta com a chave do coveiro.
- Interior com móveis do pacote de masmorra (mesa, cadeira, estante, barril) e uma luz âmbar, de lampião ou vela.
- Uma câmera fixa interna (`CameraZone` cobrindo o interior).
- **Chave das catacumbas:** item novo (tipo Objeto), em cima da mesa, coletado com E. Tem texto no Analisar.

### Cripta (cena Cemiterio)

- Ganha um `ItemUseTarget` que pede a chave das catacumbas.
- Sem a chave, E mostra uma dica (por exemplo, "Uma fechadura pesada de ferro. Deve haver uma chave em algum lugar.").
- Com a chave (pelo Usar do inventário, como na porta da casa), a chave é gasta, o estado do detetive é guardado e a cena `Catacumbas` é carregada.

### Catacumbas (cena nova)

- **Montagem:** peças modulares do pacote (piso, paredes, teto, coluna, porta, escada), com tochas acesas (`Torch` e `Fire_Effect`), ambiente escuro e névoa.
- **Planta:**
  - sala de entrada com a escada de onde o detetive chega;
  - 3 ou 4 salas pequenas ligadas por corredores, com um ou dois becos sem saída;
  - um salão final grande.
- **Itens no labirinto:**
  - 3 caixas de munição de 6 balas (18 no total; o esqueleto cai com 8 tiros, então sobra margem para errar);
  - 2 tônicos;
  - objetos sem utilidade: Moedas de ouro, Relógio de bolso e Cálice. Cada um ocupa um espaço do inventário, não empilha, tem texto no Analisar e pode ser descartado. São modelos low poly simples feitos no projeto, porque o pacote Lite não tem.
- **Decoração:** barris, baús, estantes, potes, correntes e caldeirão do pacote.
- **Salão final:** o esqueleto e, no fundo, o livro sobre um altar ou mesa.
- **Câmeras:** fixas por sala (`CameraZone`), como no cemitério.

### Esqueleto

| Atributo | Valor |
| :-- | :-- |
| Vida | 40 (8 tiros ou 20 socos) |
| Dano | 2 por golpe |
| Velocidade de perseguição | ~2,0 m/s (zumbi: 1,2) |
| Alcance do ataque | ~1,6 m |
| Intervalo entre ataques | ~1,4 s |

- **Começo:** fica enterrado no salão (pose `underground`). Quando o detetive chega a ~6 m, toca `spawn` e depois `scream`, e só então persegue. Não leva dano antes de surgir.
- **Ataques:** sorteia entre `slash01`, `slash02` e `stab`. O dano sai no meio do golpe, como no zumbi.
- **Morte:** toca `death` e o corpo fica no chão.
- **Animator próprio,** com os estados que o `Enemy.cs` usa: Idle, Walk e Attack (com variações), Death, e os estados de surgir.
- **Material:** troca para Standard com a textura do pacote. A espada (Falchion) vai presa na mão.

### Livro e final

- O livro fica no altar desde o começo, mas só pode ser pego depois que o esqueleto morre. Antes disso, E mostra uma frase de que o guardião não deixa.
- Pegar o livro dispara um fade para a tela "Fim", no estilo do Game Over:
  - título "Fim" em letra gótica;
  - a frase "A maldição foi desfeita. Os mortos voltam a descansar.";
  - o botão "Voltar ao menu".
- "Voltar ao menu" carrega a cena Cemiterio com o menu inicial aberto e limpa o estado guardado.

### Menus e morte nas catacumbas

- Pausa (Esc), Controles e inventário funcionam igual.
- O menu inicial não abre nas catacumbas. "Sair" na pausa volta ao menu inicial, na cena Cemiterio.
- No Game Over das catacumbas, o botão recarrega as catacumbas com o estado guardado na entrada.

### Build e documentação

- A cena `Catacumbas` entra no Build Settings, depois de `Cemiterio`.
- GDD: casa, cripta, catacumbas, esqueleto, itens novos, tela de fim e histórico de versões.

## Decisões minhas para você conferir

1. **Nenhum outro inimigo nas catacumbas além do esqueleto.** Você não citou outros; dá para pôr zumbis no labirinto se quiser.
2. **Munição:** 18 balas (3 caixas) e 2 tônicos nas catacumbas.
3. **Esqueleto adormecido:** surge quando o detetive chega a ~6 m e não leva dano antes disso.
4. **Livro:** visível desde o começo, mas só pode ser pego com o esqueleto morto.
5. **Objetos sem utilidade:** Moedas de ouro, Relógio de bolso e Cálice, com modelos simples feitos no projeto.
6. **Texto do botão do Game Over nas catacumbas:** "Recomeçar nas catacumbas" (no cemitério continua "Recomeçar do início").
7. **Interior da casa:** mesa, cadeira, estante e barril do pacote de masmorra, além de uma luz âmbar.
