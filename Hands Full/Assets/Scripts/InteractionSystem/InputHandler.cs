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
        public Action<ItemType, bool> NecklessDebuffHandler;

        public bool haveDebuff = false;


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

        void GetInteractionInputData()        {
            //interactionInputData.InteractedClicked = Input.GetKeyDown(KeyCode.E);
            //interactionInputData.InteractedRelease = Input.GetKeyUp(KeyCode.E);

            if(Input.GetKeyDown(KeyCode.E))
            {
                if(item != null)
                {
                    inventorySystem.Add(item.data);
                    InventoryItemData.Invoke(item.data);
                    item?.OnPickupItem();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Pressed 1");
                if (Int32.Parse(inventorySystem.Get(item.data).data.ItemId) == (int)ItemType.Necklace) 
                {
                    // disable player movement
                    Debug.Log($"Can Player Move : {canMove}");
                    playerMovement.SetCanPlayerMove(canMove);
                    canMove = !canMove;
                    haveDebuff = !haveDebuff;
                    NecklessDebuffHandler.Invoke(ItemType.Necklace, haveDebuff);
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
