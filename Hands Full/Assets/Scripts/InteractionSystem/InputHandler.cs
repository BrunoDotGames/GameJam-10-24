using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDG
{
    public class InputHandler : MonoBehaviour
    {
        public InteractionInputData interactionInputData;

        void Start()
        {
            interactionInputData.ResetInput();
        }
        void Update()
        {
            GetInteractionInputData();
        }

        void GetInteractionInputData()
        {
            interactionInputData.InteractedClicked = Input.GetKeyDown(KeyCode.E);
            interactionInputData.InteractedRelease = Input.GetKeyUp(KeyCode.E);
        }

    }
}
