using UnityEngine;

// No detetive: fora do cemitério, aplica o estado trazido de lá (vida, itens, arma, tambor).
// Roda depois do Awake de Inventory/DetectiveHealth/DetectiveCombat, que preenchem os valores iniciais.
[DefaultExecutionOrder(50)]
public class ApplyGameStateOnStart : MonoBehaviour
{
    void Start()
    {
        if (gameObject.scene.name != GameState.CemeteryScene) GameState.Apply(GetComponent<Inventory>());
    }
}
