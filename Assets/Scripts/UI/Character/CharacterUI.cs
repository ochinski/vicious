using UnityEngine;

public class CharacterUI : MonoBehaviour {
    public Transform characterSlotsParent;
    public Canvas characterPanelDisplay;
    public GameObject characterPanelUI;


    InventorySlot[] slots;

    CharacterPanel characterPanel;


    private void Start() {
        characterPanel = CharacterPanel.instance;
        characterPanel.onItemChangedCallback += UpdateUI;
        /*slots = itemsPartent.GetComponentsInChildren<InventorySlot>();*/
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            characterPanelUI.SetActive(!characterPanelUI.activeSelf);
        }
    }

    void UpdateUI() {
/*        for (int i = 0; i < slots.Length; i++) {
            if (i < characterPanel.items.Count) {
                slots[i].AddItem(characterPanel.items[i]);
            } else {
                slots[i].ClearSlot();
            }
        }*/
    }

    public void ToggleInventory() {
        characterPanelDisplay.enabled = !characterPanelDisplay.enabled;
    }
}
