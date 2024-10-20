using BDG;
using System;
using Unity.VisualScripting;
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
    private bool isStillHaveDebuff = false;
    public bool isDead = false;

    private float damageTimer = 0f;
    private float damageDurationTimer = 0f; // Timer to track total duration
    private bool isDamageOverTimeActive = false;

    private void Awake()
    {
        inputHandler.InventoryItemData += OnInventoryItemData;
        inputHandler.NecklessDebuffHandler += OnNecklessDebuffHandler;
        enemyAI.damageHandler += OnDamageHandler;
    }

    private void OnNecklessDebuffHandler(ItemType type, bool value)
    {
        Debug.Log($"Is Player Take DoT : {value}");
        isDamageOverTimeActive = value;
        if (value) // Only start DoT if the debuff is active
        {
            StartDamageOverTime();
            itemType = type;
        }
        else // If debuff is removed, stop DoT
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

            if (damageTimer >= damageInterval)
            {
                ApplyDamage();
                damageTimer = 0f; // Reset the damage timer
            }

            // Check if the duration has elapsed
            if (damageDurationTimer >= totalDamageDuration)
            {
                StopDamageOverTime();
            }
        }
    }

    private void ApplyDamage()
    {
        DamageHandlerPlayer.Invoke(true); // Apply damage to the player
    }

    private void StartDamageOverTime()
    {
        damageDurationTimer = 0f; // Reset duration timer when the debuff is activated
    }

    private void StopDamageOverTime()
    {
        isDamageOverTimeActive = false;
        damageTimer = 0f;
        damageDurationTimer = 0f; // Reset duration timer
        itemType = ItemType.None;
        DamageHandlerPlayer.Invoke(false); // Notify that the debuff is no longer active
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
