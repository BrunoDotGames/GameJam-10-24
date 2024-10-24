using BDG;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private float totalDamageDuration = 5f;
    [SerializeField] private float doTDamage = 0.1f;
    [SerializeField] private Slider necklessSlider;
    [SerializeField] private Slider earringSlider;
    [SerializeField] private Slider ringSlider;
    [SerializeField] private PostProcessVolume postProcessVolume;

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
        enemyAI.damageHandler += OnDamageHandler;
        postProcessVolume.profile.TryGetSettings(out depthOfField);
    }

    private void OnNecklessDebuffHandler(ItemType type, bool value)
    {
        isDamageOverTimeActive = value;
        if (value)
        {
            StartDamageOverTime();
            enemyAI.IgnorePlayer(true);
            itemType = type;
        }
        else
        {
            StopDamageOverTime();
        }
    }

    private void Update()
    {
        if(inputHandler is null)
        {
            Debug.Log($"There's no input handler attached.");
            return;
        }

        Debug.Log($"ItemType : {itemType}.");

        if (!inputHandler.Debuff().Item2)
        {
            itemType = ItemType.None;
            isDamageOverTimeActive = false;
            damageTimer = 0f;
            damageDurationTimer = 0f;
            enemyAI.IgnorePlayer(false);
            DamageHandlerPlayer.Invoke(false, 0);
            depthOfField.aperture.value = 9.9f;
            depthOfField.focusDistance.value = 10f;
            depthOfField.focalLength.value = 1;
        }

        if (inputHandler.Debuff().Item1 == ItemType.Earings)
        {
            if(audioSource is null)
            {
                Debug.Log($"There's no audio attached.");
                return;
            }
            earringSlider.value += sliderSpeedValue * Time.deltaTime;
            DecreasePlayerSound();
            enemyAI.IgnorePlayer(true, ItemType.Earings);
            if (earringSlider.value >= 1)
            {
                audioSource.volume = 0;
            }
        }

        if (inputHandler.Debuff().Item1 == ItemType.Necklace)
        {
            damageTimer += Time.deltaTime;
            damageDurationTimer += Time.deltaTime;
            enemyAI.IgnorePlayer(true, ItemType.Necklace);
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

        if (inputHandler.Debuff().Item1 == ItemType.Ring)
        {
            if (postProcessVolume is null || depthOfField is null)
            {
                Debug.Log($"There's no postProcessVolume / depthOfField attached.");
                return;
            }
            enemyAI.IgnorePlayer(true, ItemType.Ring);
            ringSlider.value += sliderSpeedValue * Time.deltaTime;
            IncreaseBlurEffect();
            if (ringSlider.value >= 1)
            {
                depthOfField.aperture.value = 32;
                depthOfField.focalLength.value = 300;
            }
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
        enemyAI.IgnorePlayer(false);
        DamageHandlerPlayer.Invoke(false, 0);
    }

    private void OnDamageHandler(bool value, float damage)
    {
        necklessSlider.value = 0;
        DamageHandlerPlayer.Invoke(value, damage);
    }

    private void OnInventoryItemData(InventoryItemData data)
    {
        inventoryHandler.Invoke(data);
    }

    public void SetPlayerDied()
    {
        isDead = true;
    }
}
