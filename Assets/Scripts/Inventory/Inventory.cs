using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Serializable]
    public class Slot
    {
        public ItemData item;
        public int count = 1;
    }

    public const int Capacity = 10;

    [SerializeField] List<Slot> slots = new List<Slot>();

    public IReadOnlyList<Slot> Slots => slots;
    public ItemData EquippedWeapon { get; private set; }

    public bool Add(ItemData item, int count = 1)
    {
        var stack = item.stackable ? slots.Find(s => s.item == item) : null;
        if (stack != null) stack.count += count;
        else if (slots.Count >= Capacity) return false;
        else slots.Add(new Slot { item = item, count = item.stackable ? count : 1 });
        return true;
    }

    public bool Has(ItemData item) => slots.Exists(s => s.item == item);

    public void RemoveOne(ItemData item)
    {
        var slot = slots.Find(s => s.item == item);
        if (slot == null) return;
        if (--slot.count <= 0) slots.Remove(slot);
        if (EquippedWeapon == item && !Has(item)) EquippedWeapon = null;
    }

    public string Use(int index)
    {
        var item = slots[index].item;

        foreach (var target in FindObjectsByType<ItemUseTarget>(FindObjectsInactive.Exclude))
            if (target.CanReceive(item, transform.position)) return target.Receive(this);

        switch (item.type)
        {
            case ItemType.Arma:
                EquippedWeapon = EquippedWeapon == item ? null : item;
                return EquippedWeapon ? $"{item.itemName} equipado." : $"{item.itemName} guardado.";
            case ItemType.Cura:
                var health = GetComponent<DetectiveHealth>();
                if (health.IsFull) return "Você não precisa disso agora.";
                health.Heal(item.healAmount);
                RemoveOne(item);
                return $"Você usou {item.itemName}.";
            default:
                return "Não há onde usar isso aqui.";
        }
    }

    public string Discard(int index)
    {
        var slot = slots[index];
        if (!slot.item.worldPrefab) return "Não dá para largar isso.";

        var drop = Instantiate(slot.item.worldPrefab, transform.position + transform.forward * 0.8f, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
        var pickup = drop.GetComponent<ItemPickup>();
        if (!pickup) pickup = drop.AddComponent<ItemPickup>();
        pickup.Setup(slot.item, slot.count);

        slots.RemoveAt(index);
        if (EquippedWeapon == slot.item) EquippedWeapon = null;
        return $"Você largou {slot.item.itemName}.";
    }
}
