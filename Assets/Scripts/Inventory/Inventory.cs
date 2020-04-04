using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More that one instance of inventory found");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChange();
    public OnItemChange onItemChangedCallback;

    public delegate void OnInventoryDisplay();
    public OnInventoryDisplay onInventoryDisplay;

    public int space = 20;
    public List<Item> items = new List<Item>();

    public Transform playerLocation;
    public bool Add(Item item)
    {
        bool flag = false;
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("inventory is full");
                flag = false;
            }
            items.Add(item);
            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
            flag = true;
        }
        return flag;
    }

    public void Remove(Item item)
    {
        Instantiate(item.gameItem, playerLocation.transform.position, Quaternion.identity);
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void Open()
    {
        if (onInventoryDisplay != null)
            onInventoryDisplay.Invoke();
    }
}
