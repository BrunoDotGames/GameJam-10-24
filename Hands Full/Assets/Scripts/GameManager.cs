using BDG;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private EnemyAI enemyAIRed;
    [SerializeField] private EnemyAI enemyAIPurple;
    [SerializeField] private EnemyAI enemyAIWhite;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private float totalDamageDuration = 5f;
    [SerializeField] private float doTDamage = 0.1f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider necklessSlider;
    [SerializeField] private Slider earringSlider;
    [SerializeField] private Slider ringSlider;
    [SerializeField] private PostProcessVolume postProcessVolume;

    public GameObject gameoverUI;

    public Action<InventoryItemData> inventoryHandler;
    public Action<bool, float> DamageHandlerPlayer;

    public float sliderSpeedValue = 0.05f;
    public float audioSpeedValue = 0.1f;
    public float blurSpeedValue = 0.1f;

    private ItemType itemType;
    private DepthOfField depthOfField;
    private bool isDamageOverTimeActive = false;
    public bool isDead = false;

    private float damageTimer = 0f;
    private float damageDurationTimer = 0f;

    private void Awake()
    {
        inputHandler.InventoryItemData += OnInventoryItemData;
        player.damageHandler += OnDamageHandler;
        postProcessVolume.profile.TryGetSettings(out depthOfField);
    }


    private void Update()
    {
        if (inputHandler is null)
        {
            Debug.Log($"There's no input handler attached.");
            return;
        }

        if (inputHandler.itemTypes.Contains(ItemType.Earings))
        {
            if (audioSource is null)
            {
                Debug.Log($"There's no audio attached.");
                return;
            }
            earringSlider.value += sliderSpeedValue * Time.deltaTime;
            DecreasePlayerSound();
            Debug.Log($"EnemyType : {enemyAIPurple.enemyType}");
            if (enemyAIPurple.enemyType == EnemyType.PurpleGhost)
            {
                enemyAIPurple.IgnorePlayer(true, ItemType.Earings);
            }
            if (earringSlider.value >= 1)
            {
                audioSource.volume = 0;
            }
        }
        else if (!inputHandler.itemTypes.Contains(ItemType.Earings))
        {
            if (earringSlider.value > 0)
            {
                earringSlider.value -= sliderSpeedValue * Time.deltaTime;
            }
            enemyAIPurple.IgnorePlayer(false);
            inputHandler.itemTypes.Remove(ItemType.Earings);
        }

        if (inputHandler.itemTypes.Contains(ItemType.Necklace))
        {
            damageTimer += Time.deltaTime;
            damageDurationTimer += Time.deltaTime;

            enemyAIRed.IgnorePlayer(true, ItemType.Necklace);
            necklessSlider.value += sliderSpeedValue * Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                ApplyDamage();
                damageTimer = 0f;
            }

            if (necklessSlider.value >= 1)
            {
                InstantDeath();
                necklessSlider.value = 0;
            }
        }
        else if (!inputHandler.itemTypes.Contains(ItemType.Necklace))
        {
            if (necklessSlider.value > 0)
            {
                necklessSlider.value -= sliderSpeedValue * Time.deltaTime;
            }
            isDamageOverTimeActive = false;
            damageTimer = 0f;
            damageDurationTimer = 0f;
            enemyAIRed.IgnorePlayer(false);
            DamageHandlerPlayer.Invoke(false, 0);
            inputHandler.itemTypes.Remove(ItemType.Necklace);
        }

        if (inputHandler.itemTypes.Contains(ItemType.Ring))
        {
            if (postProcessVolume is null || depthOfField is null)
            {
                Debug.Log($"There's no postProcessVolume / depthOfField attached.");
                return;
            }
            enemyAIWhite.IgnorePlayer(true, ItemType.Ring);
            ringSlider.value += sliderSpeedValue * Time.deltaTime;
            IncreaseBlurEffect();
            if (ringSlider.value >= 1)
            {
                depthOfField.aperture.value = 32;
                depthOfField.focalLength.value = 300;
            }
        }
        else if (!inputHandler.itemTypes.Contains(ItemType.Ring))
        {
            if (ringSlider.value > 0)
            {
                ringSlider.value -= sliderSpeedValue * Time.deltaTime;
            }
            enemyAIWhite.IgnorePlayer(false);
            depthOfField.aperture.value = 9.9f;
            depthOfField.focusDistance.value = 10f;
            depthOfField.focalLength.value = 1;
            inputHandler.itemTypes.Remove(ItemType.Ring);
        }
    }
    private void IncreaseBlurEffect()
    {
        depthOfField.aperture.value += blurSpeedValue;
        depthOfField.focusDistance.value = 1f;
        depthOfField.focalLength.value += blurSpeedValue;
    }

    private void DecreasePlayerSound()
    {
        audioSource.volume -= audioSpeedValue;
    }

    private void ApplyDamage()
    {
        DamageHandlerPlayer.Invoke(true, doTDamage);
    }

    private void InstantDeath()
    {
        DamageHandlerPlayer.Invoke(true, 1);
    }

    private void StartDamageOverTime()
    {
        damageDurationTimer = 0f;
        necklessSlider.value = 0;
    }

    private void StopDamageOverTime()
    {
        isDamageOverTimeActive = false;
        damageTimer = 0f;
        damageDurationTimer = 0f;
        itemType = ItemType.None;
        enemyAIRed.IgnorePlayer(false);
        DamageHandlerPlayer.Invoke(false, 0);
    }

    private void OnDamageHandler(bool value, float damage)
    {
        DamageHandlerPlayer.Invoke(value, damage);
    }

    private void OnInventoryItemData(InventoryItemData data)
    {
        inventoryHandler.Invoke(data);
    }

    public void gameOver()
    {
        gameoverUI.SetActive(true);
    }

    public void SetPlayerDied()
    {
        isDead = true;

    }
}
