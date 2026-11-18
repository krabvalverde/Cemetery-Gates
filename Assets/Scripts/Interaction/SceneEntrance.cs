using UnityEngine;

// Na cripta: ligado ao onUsed do ItemUseTarget (chave das catacumbas). Guarda o estado do detetive e desce para as catacumbas.
public class SceneEntrance : MonoBehaviour
{
    public void Enter() => GameState.EnterCatacombs(FindAnyObjectByType<Inventory>());
}
