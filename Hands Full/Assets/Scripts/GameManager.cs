using BDG;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private float totalDamageDuration = 5f;

    public Action<InventoryItemData> inventoryHandler;
    public Action<bool> DamageHandlerPlayer;

    private ItemType itemType;
    private bool isAttacking = false;
    public bool isDead = false;


    private float damageTimer = 0f; 
    private bool isDamageOverTimeActive = false;

    private void Awake()
    {
        inputHandler.InventoryItemData += OnInventoryItemData;
        inputHandler.NecklessDebuffHandler += OnNecklessDebuffHandler;
        enemyAI.damageHandler += OnDamageHandler;
    }

    private void OnNecklessDebuffHandler(ItemType type, bool value)
    {
        isDamageOverTimeActive = true;
        itemType = type;

    }

    private void Update()
    {
        if (isDamageOverTimeActive && itemType is ItemType.Necklace)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                ApplyDamage();
                damageTimer = 0f;

                // Check if the duration has elapsed
                if (damageTimer >= totalDamageDuration)
                {
                    StopDamageOverTime();
                }
            }
        }
    }

    private void StartDamageOverTime()
    {
        isDamageOverTimeActive = true;
        damageTimer = 0f; 
    }

    private void ApplyDamage()
    {
        DamageHandlerPlayer.Invoke(true);
    }

    private void StopDamageOverTime()
    {
        isDamageOverTimeActive = false;
        damageTimer = 0f;
        DamageHandlerPlayer = null;
        itemType = ItemType.None;
        inputHandler.NecklessDebuffHandler -= OnNecklessDebuffHandler;
    }


    private void DamageOverTime()
    {
        DamageHandlerPlayer.Invoke(true);
    }

    private void OnDamageHandler(bool value)
    {
        DamageHandlerPlayer.Invoke(value);
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
