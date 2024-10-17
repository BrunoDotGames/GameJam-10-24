using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDG
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        #region Variables

            [Header("Interactable Setting")]
            public float holdDuration;

            [Space]
            public bool holdInteract;
            public bool multipleUse;
            public bool isInteractable;
            [SerializeField] private string tooltipMessage = "Interact";

        #endregion

        #region Properties
            public float HoldDuration => holdDuration;
            public bool HoldInteract => holdInteract; 
            public bool MultipleUse => multipleUse;
            public bool IsInteractable => isInteractable;
            public string TooltipMessage => tooltipMessage;
        #endregion

        #region Methods
        public virtual void OnInteract()
            {
                Debug.Log("Interacted: " + gameObject.name);
            }
        #endregion
    }

}