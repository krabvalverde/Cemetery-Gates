using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Button[] slotButtons;
    [SerializeField] Text itemTitle;
    [SerializeField] Text itemText;
    [SerializeField] Text statusText;
    [SerializeField] Button useButton;
    [SerializeField] Button analyzeButton;
    [SerializeField] Button discardButton;
    [SerializeField] Text messageText;
    [SerializeField] Color slotColor = new Color32(0x3F, 0x25, 0x48, 0xFF);
    [SerializeField] Color selectedColor = new Color32(0x8A, 0x4B, 0x1F, 0xFF);
    [SerializeField] float messageDuration = 3f;

    public static bool IsOpen { get; private set; }

    static InventoryUI instance;

    Inventory inventory;
    DetectiveHealth health;
    DetectiveMovement movement;
    Coroutine messageRoutine;
    int selected;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);
        messageText.text = "";
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int index = i;
            slotButtons[i].onClick.AddListener(() => Select(index));
        }
        useButton.onClick.AddListener(() => Act(inventory.Use(selected)));
        analyzeButton.onClick.AddListener(() => itemText.text = inventory.Slots[selected].item.description);
        discardButton.onClick.AddListener(() => Act(inventory.Discard(selected)));
    }

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
        health = FindAnyObjectByType<DetectiveHealth>();
        movement = FindAnyObjectByType<DetectiveMovement>();
    }

    void OnDestroy()
    {
        if (instance != this) return;
        instance = null;
        IsOpen = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!inventory) return;
        if (Input.GetKeyDown(KeyCode.I)) SetOpen(!IsOpen);
        else if (IsOpen && Input.GetKeyDown(KeyCode.Escape)) SetOpen(false);
    }

    void SetOpen(bool open)
    {
        IsOpen = open;
        panel.SetActive(open);
        Time.timeScale = open ? 0f : 1f;
        if (movement) movement.enabled = !open;
        if (open) Select(0);
    }

    void Select(int index)
    {
        selected = index;
        itemTitle.text = HasItem ? inventory.Slots[index].item.itemName : "";
        itemText.text = "";
        Refresh();
        EventSystem.current.SetSelectedGameObject(slotButtons[index].gameObject);
    }

    void Act(string message)
    {
        Select(Mathf.Min(selected, Mathf.Max(0, inventory.Slots.Count - 1)));
        itemText.text = message;
    }

    bool HasItem => selected < inventory.Slots.Count;

    void Refresh()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            var label = slotButtons[i].GetComponentInChildren<Text>();
            if (i < inventory.Slots.Count)
            {
                var slot = inventory.Slots[i];
                label.text = slot.item.itemName + (slot.count > 1 ? $" x{slot.count}" : "") + (slot.item == inventory.EquippedWeapon ? " (equipado)" : "");
            }
            else label.text = "—";
            slotButtons[i].image.color = i == selected ? selectedColor : slotColor;
        }

        useButton.interactable = analyzeButton.interactable = discardButton.interactable = HasItem;
        string weapon = inventory.EquippedWeapon ? inventory.EquippedWeapon.itemName : "nenhuma";
        statusText.text = health ? $"Condição: {health.Current}/{health.Max}\nArma: {weapon}" : $"Arma: {weapon}";
    }

    public static void ShowMessage(string message)
    {
        if (!instance) return;
        if (IsOpen)
        {
            instance.itemText.text = message;
            return;
        }
        if (instance.messageRoutine != null) instance.StopCoroutine(instance.messageRoutine);
        instance.messageRoutine = instance.StartCoroutine(instance.Flash(message));
    }

    IEnumerator Flash(string message)
    {
        messageText.text = message;
        yield return new WaitForSecondsRealtime(messageDuration);
        messageText.text = "";
    }
}
