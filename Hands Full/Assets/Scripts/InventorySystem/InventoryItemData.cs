using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemData",menuName = "InventorySystem/InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    public string ItemId;
    public string ItemName;
    public string Description;
    public Sprite sprite; 
}
