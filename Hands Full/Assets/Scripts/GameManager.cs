using BDG;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;

    public Action<InventoryItemData> inventoryHandler;

    private void Awake()
    {
        inputHandler.InventoryItemData += OnInventoryItemData;
    }

    private void OnInventoryItemData(InventoryItemData data)
    {
        inventoryHandler.Invoke(data);
    }
}
