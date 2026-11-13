using UnityEngine;
using UnityEngine.Events;

public class ItemUseTarget : MonoBehaviour
{
    [SerializeField] ItemData requiredItem;
    [SerializeField] bool consumeItem = true;
    [SerializeField] float useDistance = 2.2f;
    [TextArea] [SerializeField] string lockedMessage = "Está trancado.";
    [TextArea] [SerializeField] string usedMessage = "Funcionou.";
    [SerializeField] UnityEvent onUsed;

    Transform player;
    bool done;

    void Start()
    {
        var inventory = FindAnyObjectByType<Inventory>();
        if (inventory) player = inventory.transform;
    }

    void Update()
    {
        if (done || InventoryUI.IsOpen || !player || !Input.GetKeyDown(KeyCode.E) || !InRange(player.position)) return;
        InventoryUI.ShowMessage(lockedMessage);
    }

    public bool CanReceive(ItemData item, Vector3 from) => !done && item == requiredItem && InRange(from);

    public string Receive(Inventory inventory)
    {
        done = true;
        if (consumeItem) inventory.RemoveOne(requiredItem);
        onUsed.Invoke();
        return usedMessage;
    }

    bool InRange(Vector3 position)
    {
        Vector3 offset = position - transform.position;
        offset.y = 0f;
        return offset.sqrMagnitude <= useDistance * useDistance;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, useDistance);
    }
}
