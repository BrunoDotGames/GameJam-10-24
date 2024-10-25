using System;
using System.Collections.Generic;
using UnityEngine;

namespace BDG
{
    public class InputHandler : MonoBehaviour, IDisposable
    {
        [SerializeField] private InteractionController InteractionController;
        [SerializeField] private InventoryController inventorySystem;
        [SerializeField] private PlayerMovement playerMovement;

        public InteractionInputData interactionInputData;
        public Action<InventoryItemData> InventoryItemData;

        public bool haveDebuff = false;
        public ItemType itemType;
        public List<InventoryItemData> inventoryItemDatas = new List<InventoryItemData>();

        private bool canInteract = false;
        private bool canMove = false;
        private Item item;

        private void Awake()
        {
            InteractionController.CanInteract += OnCanInteract;
        }

        void Start()
        {
            interactionInputData.ResetInput();
        }
        void Update()
        {
            if (canInteract)
            {
                GetInteractionInputData();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var door = other.GetComponent<DoorInteraction>();
            if (door.doorData != null)
            {
                // Check is door locked
                if (door.doorData.IsLock)
                {
                    // Get key from inventory
                    if (inventorySystem.GetByItemId(ItemType.RedKey) != 0)
                    {
                        // Open the door
                        door.OpenDoor();
                    }
                }
            }
        }

        void GetInteractionInputData()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (item != null)
                {
                    inventorySystem.Add(item.data);

                    if (item.data != null)
                    {
                        InventoryItemData.Invoke(item.data);
                    }
                    else
                    {
                        Debug.LogWarning("Item data is null.");
                    }
                    
                    item.EnemySpawn((ItemType)Int32.Parse(item.data.ItemId));
                    item.OnPickupItem();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (inventorySystem.GetByItemId(ItemType.Necklace) == (int)ItemType.Necklace)
                {
                    // disable player movement
                    playerMovement.SetCanPlayerMove(canMove);
                    canMove = !canMove;
                    haveDebuff = !haveDebuff;
                    itemType = haveDebuff ? ItemType.Necklace : ItemType.None;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (inventorySystem.GetByItemId(ItemType.Earings) == (int)ItemType.Earings)
                {
                    // disable player movement
                    haveDebuff = !haveDebuff;
                    itemType = haveDebuff ? ItemType.Earings : ItemType.None;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (inventorySystem.GetByItemId(ItemType.Ring) == (int)ItemType.Ring)
                {
                    // disable player movement
                    haveDebuff = !haveDebuff;
                    itemType = haveDebuff ? ItemType.Ring : ItemType.None;
                }
            }
        }

        private void OnCanInteract(bool value, Item inventoryItem)
        {
            canInteract = value;
            item = inventoryItem;

        }

        public (ItemType, bool) Debuff()
        {
            return (itemType, haveDebuff);
        }

        public void Dispose()
        {
            InventoryItemData = null;
        }
    }
}
