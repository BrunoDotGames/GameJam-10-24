using BDG;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private float totalDamageDuration = 5f;
    [SerializeField] private float doTDamage = 0.1f;
    [SerializeField] private Slider necklessSlider;

    public Action<InventoryItemData> inventoryHandler;
    public Action<bool, float> DamageHandlerPlayer;

    public float sliderSpeedValue = 0.05f;
    private ItemType itemType;
    private bool isDamageOverTimeActive = false;
    public bool isDead = false;

    private float damageTimer = 0f;
    private float damageDurationTimer = 0f;

    private void Awake()
    {
        inputHandler.InventoryItemData += OnInventoryItemData;
        inputHandler.NecklessDebuffHandler += OnNecklessDebuffHandler;
        enemyAI.damageHandler += OnDamageHandler;
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
        if (isDamageOverTimeActive && itemType == ItemType.Necklace)
        {
            damageTimer += Time.deltaTime;
            damageDurationTimer += Time.deltaTime;

            necklessSlider.value += sliderSpeedValue * Time.deltaTime;

            if (necklessSlider.value >= 1)
            {
                ApplyDamage();
                necklessSlider.value = 0; 
            }
        }
    }

    private void ApplyDamage()
    {
        DamageHandlerPlayer.Invoke(true, doTDamage);
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
