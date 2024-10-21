using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer[] images;
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

    private void OnDamageHandler(bool value, float damage)
    {
        if (value)
        {
            healthSlider.value -= damage;
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
            if (item == null)
            {
                item.sprite = data.sprite;
                break;
            }
        }
    }
}
