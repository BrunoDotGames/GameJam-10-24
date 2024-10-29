using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer[] images;
    [SerializeField] private Slider healthSlider;

    public GameObject gameoverUI;
    GameManager gamemanager;

    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    private void Awake()
    {
        gameManager.inventoryHandler += OnInventoryItemData;
        gameManager.DamageHandlerPlayer += OnDamageHandler;
    }

    private void Update()
    {
        if(healthSlider.value <= 0 )
        {
            gameManager.SetPlayerDied();
            DeathUI();
        }

         if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
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
        if(healthSlider.value <= 0)
        {
            gameoverUI.SetActive(true);
            Time.timeScale = 0f;
        }
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
