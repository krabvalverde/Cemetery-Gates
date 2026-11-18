using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Leva o detetive (vida, itens, arma equipada, balas no tambor) do cemitério para as catacumbas.
// Fica só na memória: o jogo não tem save. Morrer nas catacumbas recarrega a cena com este mesmo estado.
public static class GameState
{
    public const string CemeteryScene = "Cemiterio";
    public const string CatacombsScene = "Catacumbas";

    static List<(ItemData item, int count)> slots;
    static ItemData equipped;
    static int health, loaded;

    public static bool HasSnapshot => slots != null;

    public static void Capture(Inventory inventory)
    {
        slots = inventory.Snapshot();
        equipped = inventory.EquippedWeapon;
        health = inventory.GetComponent<DetectiveHealth>().Current;
        loaded = inventory.GetComponent<DetectiveCombat>().Loaded;
    }

    public static void Apply(Inventory inventory)
    {
        if (!HasSnapshot) return;
        inventory.Restore(slots, equipped);
        inventory.GetComponent<DetectiveHealth>().Restore(health);
        inventory.GetComponent<DetectiveCombat>().Loaded = loaded;
    }

    public static void Clear() => slots = null;

    public static void EnterCatacombs(Inventory inventory)
    {
        Capture(inventory);
        SceneManager.LoadScene(CatacombsScene);
    }
}
