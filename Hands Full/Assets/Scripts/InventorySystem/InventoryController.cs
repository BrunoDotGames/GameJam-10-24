using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> inventoryData;
    public List<InventoryItem> inventoryItems;

    private void Awake()
    {
        inventoryData = new Dictionary<InventoryItemData, InventoryItem>();
        inventoryItems = new List<InventoryItem>();
    }

    public InventoryItem Get(InventoryItemData itemData)
    {
        if (inventoryData.TryGetValue(itemData, out InventoryItem data))
        {
            return data;
        }
        return null;
    }

    public int GetByItemId(ItemType id)
    {
        foreach(var item in inventoryItems)
        {
            if (Int32.Parse(item.data.ItemId) == (int)id)
            {
                return Int32.Parse(item.data.ItemId);
            }
        }
        return 0;
    }

    public void Add(InventoryItemData itemData)
    {
        if (inventoryData.TryGetValue(itemData, out InventoryItem data))
        {
            data.AddToStack();
        }
        else
        {
            InventoryItem item = new InventoryItem(itemData);
            inventoryItems.Add(item);
            inventoryData.Add(itemData, item);
        }
    }

    public void Remove(InventoryItemData itemData)
    {
        if (inventoryData.TryGetValue(itemData, out InventoryItem data))
        {
            data.RemoveFromStack();
            if (data.stackSize == 0)
            {
                inventoryItems.Remove(data);
                inventoryData.Remove(itemData);
            }
        }
    }
}
