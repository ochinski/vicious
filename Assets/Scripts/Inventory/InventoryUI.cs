using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsPartent;
    public Canvas inventoryDisplay;

    InventorySlot[] slots;

    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        slots = itemsPartent.GetComponentsInChildren<InventorySlot>();
    }

    void UpdateUI ()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void ToggleInventory()
    {
        inventoryDisplay.enabled = !inventoryDisplay.enabled;
    }
}
