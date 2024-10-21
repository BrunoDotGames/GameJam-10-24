using System;
using UnityEngine;

namespace BDG
{
    public class InputHandler : MonoBehaviour, IDisposable
    {
        public InteractionInputData interactionInputData;
        [SerializeField] private InteractionController InteractionController;
        [SerializeField] private InventoryController inventorySystem;

        public Action<InventoryItemData> InventoryItemData;

        private bool canInteract = false;
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
                Debug.Log($"INTERACT WITH OBJECT");
                if(item != null)
                {
                    inventorySystem.Add(item.data);
                    item?.OnPickupItem();
                    InventoryItemData.Invoke(item.data);
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
