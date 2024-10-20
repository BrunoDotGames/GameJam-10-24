using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image[] images;
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        gameManager.inventoryHandler += OnInventoryItemData;
        gameManager.DamageHandlerPlayer += OnDamageHandler;
    }

    private void Update()
    {
        if(healthSlider.value <= 0)
        {
            gameManager.SetPlayerDied();
            DeathUI();
        }
    }

    private void OnDamageHandler(bool value)
    {
        if (value)
        {
            healthSlider.value -= 0.1f;
        }
    }

    private void DeathUI()
    {
        // Show death UI
        Debug.Log("Player is dead");
        // Back to main menu
    }

    private void OnDestroy()
    {
        gameManager.inventoryHandler -= OnInventoryItemData;
        gameManager.DamageHandlerPlayer -= OnDamageHandler;
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
