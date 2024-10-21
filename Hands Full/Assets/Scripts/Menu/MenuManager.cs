using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image[] images;

    private void Awake()
    {
        gameManager.inventoryHandler += OnInventoryItemData;
    }

    private void OnInventoryItemData(InventoryItemData data)
    {
        inventoryController.Get(data);
        foreach (var item in images) 
        {
            if (item.sprite == null)
            {
                item.sprite = data.sprite;
                break;
            }
        }
    }
}
