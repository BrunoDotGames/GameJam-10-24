using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
            Debug.Log($"ENTER DOOR");
            Debug.Log($"Name : {other.transform.name}");
            var door = other.GetComponent<DoorInteraction>();
            if (door.doorData != null)
            {
                // Check is door locked
                if (door.doorData.IsLock)
                {
                    // Get key from inventory
                    if(inventorySystem.GetByItemId(ItemType.RedKey) != 0)
                    {
                        // Open the door
                        door.OpenDoor();
                    }   
                }
            }
        }

        void GetInteractionInputData()        {
            //interactionInputData.InteractedClicked = Input.GetKeyDown(KeyCode.E);
            //interactionInputData.InteractedRelease = Input.GetKeyUp(KeyCode.E);

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

                    item.OnPickupItem();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Pressed 1");
                Debug.Log($"Item Get From Inventory : {Int32.Parse(inventorySystem.Get(item.data).data.ItemId)}");
                Debug.Log($"Item Get From Inventory System : {inventorySystem.Get(item.data).data.ItemId}");
                Debug.Log($"Is Same With Value : {Int32.Parse(inventorySystem.Get(item.data).data.ItemId) == (int)ItemType.Necklace}");
                if (inventorySystem.GetByItemId(ItemType.Necklace) == (int)ItemType.Necklace) 
                {
                    // disable player movement
                    Debug.Log($"Can Player Move : {canMove}");
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
                    Debug.Log($"JEWELS EARRINGS");
                    haveDebuff = !haveDebuff;
                    itemType = haveDebuff ?  ItemType.Earings : ItemType.None;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (inventorySystem.GetByItemId(ItemType.Ring) == (int)ItemType.Ring)
                {
                    // disable player movement
                    Debug.Log($"JEWELS RINGS");
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
            Debug.Log($"Debuff item type : {itemType}");
            Debug.Log($"Debuff haveDebuff : {haveDebuff  }");
            return (itemType, haveDebuff);
        }

        public void Dispose()
        {
            InventoryItemData = null;
        }
    }
}
