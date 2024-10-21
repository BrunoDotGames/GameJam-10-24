using UnityEngine;

public class Item : MonoBehaviour
{
    public InventoryItemData data;

    public void OnPickupItem()
    {
        Destroy(gameObject);
    }
}
