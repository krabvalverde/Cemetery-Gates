using UnityEngine;

public enum ItemType { Objeto, Arma, Cura }

[CreateAssetMenu(menuName = "Cemetery Gates/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea(3, 8)] public string description;
    public ItemType type;
    public bool stackable;
    public int healAmount;
    public GameObject worldPrefab;
}
