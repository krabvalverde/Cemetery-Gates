using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] ItemData item;
    [SerializeField] int count = 1;
    [SerializeField] float pickupDistance = 1.5f;

    Inventory inventory;

    public void Setup(ItemData data, int amount)
    {
        item = data;
        count = amount;
    }

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    void Update()
    {
        if (InventoryUI.IsOpen || !inventory || !item || !Input.GetKeyDown(KeyCode.E)) return;

        Vector3 offset = inventory.transform.position - transform.position;
        offset.y = 0f;
        if (offset.sqrMagnitude > pickupDistance * pickupDistance) return;

        if (!inventory.Add(item, count))
        {
            InventoryUI.ShowMessage("O inventário está cheio.");
            return;
        }

        InventoryUI.ShowMessage(count > 1 ? $"Você pegou {item.itemName} ({count})." : $"Você pegou {item.itemName}.");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}
