using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BDG
{
    public class InputHandler : MonoBehaviour
    {
        public InteractionInputData interactionInputData;
        [SerializeField] private InteractionController InteractionController;

        private bool canInteract = false;

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

        void GetInteractionInputData()
        {
            interactionInputData.InteractedClicked = Input.GetKeyDown(KeyCode.E);
            interactionInputData.InteractedRelease = Input.GetKeyUp(KeyCode.E);

            if(interactionInputData.InteractedClicked)
            {
                Debug.Log($"INTERACT WITH OBJECT");
            }
        }

        private void OnCanInteract(bool value)
        {
            canInteract = value;
        }
    }
}
