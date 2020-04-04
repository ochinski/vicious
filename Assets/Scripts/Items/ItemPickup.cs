using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public override void Interact()
    {
        base.Interact();
    }

    public void Pickup()
    {
        Debug.Log("Picking up Item");
        if (Inventory.instance.Add(item))
        {
            Destroy(gameObject);
        }
    }
}
