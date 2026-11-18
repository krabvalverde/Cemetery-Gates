# Segunda parte (casa, catacumbas, esqueleto, final): plano de implementação

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fechar o jogo, do interior da casa do coveiro até o livro que encerra a maldição nas catacumbas.

**Architecture:**
- Um estado estático (`GameState`) leva vida, inventário, arma e tambor do Cemiterio para a cena nova `Catacumbas`.
- Detetive, MenusUI e InventarioUI viram prefabs compartilhados pelas duas cenas.
- O esqueleto é um `Enemy` com estado adormecido e ataques sorteados.

**Tech Stack:** Unity 6000.5.8f1, pipeline Built-in (materiais Standard), Input Manager antigo, uGUI `Text`. A edição da cena é feita por `Unity_RunCommand` (MCP).

**Spec:** `docs/superpowers/specs/2026-11-18-segunda-parte-design.md`

## Global Constraints

- **Materiais:** todo material novo ou importado usa o Standard. Nada rosa.
- **Fontes:** títulos em UnifrakturMaguntia, textos e botões em Special Elite.
- **Paleta:** Bone #C8C793, Amber #E09A3E, Rust #8A4B1F, Lavender #7C679E, WarmBlack #24201A, vermelho do Game Over #9B2A1C.
- **Botões:** só texto, com ColorTint (normal Bone, selecionado Amber, pressionado Rust), como os menus atuais.
- **Câmeras:** fixas por área, com `CameraZone`.
- **Esqueleto:**
  - vida 40, dano 2 por golpe;
  - persegue a ~2,0 m/s;
  - alcance ~1,6 m, intervalo entre ataques ~1,4 s;
  - acorda a ~6 m;
  - ataques `slash01`, `slash02` e `stab`.
- **Itens nas catacumbas:** 3 caixas de munição (6 balas cada), 2 tônicos, Moedas de ouro, Relógio de bolso e Cálice (não empilham).
- **Textos:**
  - tela de fim: "Fim" / "A maldição foi desfeita. Os mortos voltam a descansar." / "Voltar ao menu";
  - Game Over nas catacumbas: "Recomeçar nas catacumbas".
- **Commits:** só quando o usuário pedir. O projeto não tem framework de testes; cada task é verificada no Play por `Unity_RunCommand` e com renders da câmera.

## Review Focus

1. **NavMesh na troca de cena.** O esqueleto precisa achar caminho nas catacumbas. Hoje o NavMesh fica num campo `static` gerado uma vez só (Task 1 testa).
2. **Morrer nas catacumbas e recomeçar.** O detetive precisa voltar com exatamente o estado da entrada, e não com o estado de quando morreu (Task 2 testa).
3. **Voltar ao menu a partir das catacumbas.** Pela pausa ou pela tela de fim, o menu inicial tem que abrir no Cemiterio e o estado guardado tem que ser limpo, para "Iniciar" não começar com o inventário das catacumbas (Task 2 e Task 7 testam).
4. **Inventário cheio diante de um item útil.** Os objetos sem utilidade podem lotar os 10 espaços. A mensagem "O inventário está cheio." já existe e "Descartar" resolve; só confirmar que a munição continua empilhando no espaço que já existe (Task 5 testa).
5. **Livro antes do chefe morrer.** E não encerra o jogo e mostra a frase do guardião (Task 7 testa).

---

### Task 1: Estado entre cenas e NavMesh por cena

**Files:**
- Create: `Assets/Scripts/Core/GameState.cs`
- Modify: `Assets/Scripts/Combat/Enemy.cs` (`BakeNavMeshOnce`)
- Modify: `Assets/Scripts/Inventory/Inventory.cs`, `Assets/Scripts/Player/DetectiveHealth.cs`, `Assets/Scripts/Combat/DetectiveCombat.cs` (ganchos de captura e restauração)

**Interfaces:**
- **Produces:**
  - `static class GameState`:
    - `static bool HasSnapshot`
    - `static void Capture(Inventory inv)`: guarda vida, slots, arma e `Loaded`
    - `static void Apply(Inventory inv)`
    - `static void Clear()`
    - `static void EnterCatacombs(Inventory inv)`: chama `Capture` e carrega a cena `"Catacumbas"`
  - `Inventory`:
    - `List<(ItemData item, int count)> Snapshot()`
    - `void Restore(List<(ItemData, int)> slots, ItemData equipped)`
  - `DetectiveHealth`: `void Restore(int value)`
  - `DetectiveCombat`: `int Loaded { get; set; }`
- O NavMesh do `Enemy` é descartado quando a cena que o gerou é descarregada (`SceneManager.sceneUnloaded`) e gerado de novo na próxima.

- [ ] **Step 1:** Implementar o `GameState` e os ganchos acima.
- [ ] **Step 2:** No `Enemy`, guardar o handle da cena junto com `navMesh`. Se `navMesh.valid` mas a cena ativa for outra, chamar `NavMesh.RemoveNavMeshData` e gerar de novo.
- [ ] **Step 3: Verificar no Play.**
  - Pegar um item e levar 3 golpes, depois rodar `GameState.Capture(inv)`, recarregar a cena com `GameState.Apply` no `Start` e conferir que vida = 7 e que os slots são os mesmos.
  - Conferir que um `Enemy` acha caminho (`hasPath`) depois de um `LoadScene`.
  - Console sem erros.

### Task 2: Prefabs compartilhados e comportamento por cena

**Files:**
- Create: `Assets/Prefabs/Detective.prefab`, `Assets/Prefabs/UI/MenusUI.prefab`, `Assets/Prefabs/UI/InventarioUI.prefab` (gerados a partir dos objetos da cena Cemiterio)
- Modify: `Assets/Scripts/UI/MainMenu.cs`, `Assets/Scripts/UI/PauseMenu.cs`, `Assets/Scripts/UI/GameOverUI.cs`

**Interfaces:**
- **Consumes:** `GameState` (Task 1).
- **Produces:**
  - `MainMenu`: `[SerializeField] bool openOnStart = true` (no Cemiterio é `true`, nas catacumbas é sobrescrito para `false`)
  - `PauseMenu.QuitToMenu()`: `GameState.Clear()` e `LoadScene("Cemiterio")`
  - `GameOverUI.RestartFromBeginning()`:
    - com `GameState.HasSnapshot`: recarrega a cena atual (as catacumbas aplicam o instantâneo);
    - sem instantâneo: comportamento atual;
    - rótulo do botão vem de `[SerializeField] string restartLabel`.
  - O detetive aplica o `GameState` no `Start`, quando há instantâneo e a cena não é Cemiterio, via um componente `ApplyGameStateOnStart` no prefab do detetive, com ordem de execução depois de `Inventory`.

- [ ] **Step 1:** Criar os prefabs com `PrefabUtility.SaveAsPrefabAssetAndConnect`, mantendo as referências da cena.
- [ ] **Step 2:** Fazer as mudanças nos três scripts acima.
- [ ] **Step 3: Verificar no Play (Cemiterio).**
  - O menu abre.
  - A pausa e "Sair" voltam ao menu.
  - O Game Over recomeça do início.
  - Console sem erros.

### Task 3: Interior da casa do coveiro e chave das catacumbas

**Files:**
- Create: `Assets/Items/ChaveCatacumbas.asset` (ItemData, tipo Objeto, com descrição) e `Assets/Prefabs/Items/ChaveCatacumbas.prefab` (modelo de chave como o `Chave.prefab`, com `ItemPickup` e `M_ItemDestaque`)
- Modify: `Assets/Scenes/Cemiterio.unity`

- [ ] **Step 1: Colliders da casa.** Trocar o `BoxCollider` de `CasaDoCoveiro` por 4 `BoxCollider` de parede, deixando livre o vão da porta (`VaoDaPorta`, x≈-14,8, z≈-10,4). Medir as paredes pelo `Renderer.bounds` da casa.
- [ ] **Step 2: Interior.** Mobiliar com `Table.003`, `Chair_Small.001`, `Shelf_Framed` e `Barrel_C` do pacote, mais uma Point Light âmbar (#E09A3E, alcance ~5). A chave vai em cima da mesa.
- [ ] **Step 3: Câmera.** Adicionar uma `CameraZone` cobrindo o interior, com uma câmera fixa num canto alto.
- [ ] **Step 4: Verificar no Play.**
  - Abrir a porta com a chave, levar o detetive para dentro e conferir que a câmera troca.
  - Pegar a chave com E e conferir que ela entra no inventário.
  - Fazer um render do interior pela câmera nova e conferir que não há material rosa.

### Task 4: Cripta como entrada

**Files:**
- Create: `Assets/Scripts/Interaction/SceneEntrance.cs` (`public void Enter()` chama `GameState.EnterCatacombs(FindAnyObjectByType<Inventory>())`)
- Modify: `Assets/Scenes/Cemiterio.unity` (na `Cripta`: um filho com `ItemUseTarget` à frente da porta, `requiredItem = ChaveCatacumbas`, `lockedMessage` = "Uma fechadura pesada de ferro. Deve haver uma chave em algum lugar.", `onUsed`, chamando `SceneEntrance.Enter`)

- [ ] **Step 1:** Implementar e ligar na cena.
- [ ] **Step 2: Verificar no Play.**
  - Sem a chave, E mostra a mensagem.
  - Com a chave, "Usar" carrega `Catacumbas`.
  - Antes de existir a cena, a Task 4 termina com a cena vazia registrada no Build Settings.

### Task 5: Cena das catacumbas (labirinto, luz, câmeras, itens)

**Files:**
- Create: `Assets/Scenes/Catacumbas.unity`
- Create: `Assets/Items/MoedasOuro.asset`, `Assets/Items/RelogioBolso.asset`, `Assets/Items/Calice.asset`, com prefabs em `Assets/Prefabs/Items/` (modelos low poly simples de primitivos, material Standard dourado ou metálico)
- Modify: `ProjectSettings/EditorBuildSettings.asset` (`Catacumbas` depois de `Cemiterio`)

- [ ] **Step 1: Medir o pacote.** Medir o módulo de `Floor_Flat`, `Wall_Chiseled` e `Ceiling_SquareLarge` (bounds) para definir a grade.
- [ ] **Step 2: Montar a planta.** A montagem é feita por script de editor, e todas as peças ganham collider:
  - sala de entrada com `Stair`;
  - 3 ou 4 salas pequenas, corredores e 1 ou 2 becos;
  - salão final de ~3×3 módulos.
- [ ] **Step 3: Iluminação.**
  - `Torch.001` com `Fire_Effect` e Point Light âmbar a cada sala;
  - luz ambiente escura;
  - névoa preta.
- [ ] **Step 4: Câmeras e prefabs.**
  - Uma `CameraZone` com câmera fixa por sala e corredor.
  - Instanciar os prefabs `Detective` (no pé da escada), `MenusUI` (`openOnStart = false`, `restartLabel` = "Recomeçar nas catacumbas") e `InventarioUI`, mais um `EventSystem`.
- [ ] **Step 5: Itens e decoração.**
  - Espalhar as 3 caixas de munição, os 2 tônicos e os 3 objetos sem utilidade.
  - Decorar com barris, baús, potes, correntes e caldeirão.
- [ ] **Step 6: Verificar no Play, abrindo direto em Catacumbas.**
  - Andar por todas as salas e conferir que a câmera troca e que não há buracos.
  - Pegar a munição: ela empilha no espaço já existente e conta 18 balas.
  - Com o inventário cheio, aparece "O inventário está cheio.".
  - Fazer renders de cada sala e conferir que nada fica rosa.

### Task 6: Esqueleto

**Files:**
- Modify: `Assets/Scripts/Combat/Enemy.cs`:
  - `[SerializeField] float wakeRange = 0` (0 = já acordado, como os zumbis)
  - `[SerializeField] string[] attackStates = { "Attack" }` (sorteia um a cada golpe)
  - acordar: toca `"Spawn"`, depois `"Scream"`; persegue só depois de `[SerializeField] float wakeDuration`
  - antes de acordar, `TakeDamage` não tem efeito
- Create: `Assets/Animations/Skeleton.controller`, com os estados `Underground`, `Spawn`, `Scream`, `Idle`, `Walk` (`walk_forward`, sem root motion), `Slash01`, `Slash02`, `Stab` e `Death`, usando os clipes do pacote
- Create: `Assets/Material/Skeleton_Standard.mat`, com a textura do pacote
- Modify: `Assets/Scenes/Catacumbas.unity`, com o esqueleto (`Skeleton_110` + `Falchion_01` presa na mão) no salão

- [ ] **Step 1:** Fazer as extensões no `Enemy` sem mudar o comportamento dos zumbis, já que os valores padrão mantêm tudo igual.
- [ ] **Step 2:** Criar o controller e o material, montar o esqueleto (`CharacterController` + `Enemy`) e definir:
  - vida 40, dano 2;
  - speed 2,0, `attackRange` 1,6, `attackCooldown` 1,4;
  - `wakeRange` 6;
  - `attackStates` = Slash01, Slash02 e Stab;
  - sons dos zumbis por enquanto.
- [ ] **Step 3: Verificar no Play.**
  - Longe, o esqueleto fica enterrado e não leva dano.
  - A 6 m, surge e grita, depois persegue.
  - Os golpes tiram 2 de vida e os ataques variam.
  - 8 tiros o matam.
  - No cemitério, os zumbis continuam iguais.

### Task 7: Livro e tela de fim

**Files:**
- Create: `Assets/Scripts/Interaction/CurseBook.cs`:
  - `[SerializeField] Enemy guardian`
  - E a ≤1,5 m:
    - com o guardião vivo, `InventoryUI.ShowMessage("O guardião não deixa você se aproximar do livro.")`;
    - com o guardião morto, chama `EndingUI.Show()`.
- Create: `Assets/Scripts/UI/EndingUI.cs`: fade como no `GameOverUI`, `static bool IsShowing`, `BackToMenu()` (`GameState.Clear()` e `LoadScene("Cemiterio")`)
- Modify: `Assets/Prefabs/UI/MenusUI.prefab` (painel `Fim` copiando o estilo do `GameOver`, mas com título Bone, nunca vermelho), `PauseMenu`/`InventoryUI` (bloqueio por `EndingUI.IsShowing`), `Assets/Scenes/Catacumbas.unity` (`Book_Holy_C` num altar no fundo do salão)

- [ ] **Step 1:** Implementar e montar.
- [ ] **Step 2: Verificar no Play.**
  - E no livro com o esqueleto vivo mostra a frase.
  - Depois de matar o esqueleto, E faz o fade para "Fim".
  - "Voltar ao menu" abre o menu no Cemiterio, e "Iniciar" começa com só o revólver.
  - Fazer um render da tela de fim.

### Task 8: Jogo completo e GDD

**Files:**
- Modify: `docs/GDD.md` (casa, cripta, catacumbas, esqueleto, itens novos, tela de fim e histórico 2.7)

- [ ] **Step 1:** Jogar do início ao fim por `RunCommand`: menu, chave do coveiro, porta, chave das catacumbas, cripta, catacumbas, morrer, recomeçar nas catacumbas, matar o esqueleto, livro, fim e menu. Conferir o console em cada etapa.
- [ ] **Step 2:** Atualizar o GDD.
