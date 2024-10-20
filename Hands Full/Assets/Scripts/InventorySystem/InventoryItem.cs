
using System;

[Serializable]
public class InventoryItem
{
    public InventoryItemData data;
    public int stackSize;

    public InventoryItem(InventoryItemData value)
    {
        data = value;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}
