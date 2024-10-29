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

        public ItemType itemType;
        public List<InventoryItemData> inventoryItemDatas = new List<InventoryItemData>();
        public List<ItemType> itemTypes = new List<ItemType>();

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
            if(door == null) return;
            if (door.doorData != null)
            {
                // Check is door locked
                if (door.doorData.IsLock)
                {
                    // Get key from inventory
                    if (inventorySystem.GetByItemId(ItemType.RedKey) == (int)ItemType.RedKey)
                    {
                        door.OpenDoor((ItemType)inventorySystem.GetByItemId(ItemType.RedKey));
                    }
                    if(inventorySystem.GetByItemId(ItemType.WhiteKey) == (int)ItemType.WhiteKey)
                    {
                        door.OpenDoor((ItemType)inventorySystem.GetByItemId(ItemType.WhiteKey));
                    }
                    if (inventorySystem.GetByItemId(ItemType.PurpleKey) == (int)ItemType.PurpleKey)
                    {
                        door.OpenDoor((ItemType)inventorySystem.GetByItemId(ItemType.PurpleKey));
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
                    playerMovement.SetCanPlayerMove(canMove);
                    canMove = !canMove;
                    itemType = ItemType.Necklace;
                    if (itemTypes.Contains(ItemType.Necklace))
                    {
                        itemTypes.Remove(ItemType.Necklace);
                        return;
                    }
                    itemTypes.Add(itemType);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (inventorySystem.GetByItemId(ItemType.Earings) == (int)ItemType.Earings)
                {
                    itemType = ItemType.Earings;
                    if (itemTypes.Contains(itemType)) 
                    {
                        itemTypes.Remove(ItemType.Earings);
                        return;
                    }
                    itemTypes.Add(itemType);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (inventorySystem.GetByItemId(ItemType.Ring) == (int)ItemType.Ring)
                {
                    if(itemTypes.Contains(ItemType.Ring))
                    {
                        itemTypes.Remove(ItemType.Ring);
                        return;
                    }
                    itemType = ItemType.Ring;
                    itemTypes.Add(itemType);
                }
            }
        }

        private void OnCanInteract(bool value, Item inventoryItem)
        {
            canInteract = value;
            item = inventoryItem;

        }

        public void Dispose()
        {
            InventoryItemData = null;
        }
    }
}
