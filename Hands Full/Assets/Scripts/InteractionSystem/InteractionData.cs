using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDG
{
    [CreateAssetMenu(fileName = "InteractionData", menuName = "InteractionSystem/InteractionData")]
    public class InteractionData : ScriptableObject
    {
        private InteractableBase m_intectable;

        public InteractableBase Interactable
        {
            get => m_intectable;
            set => m_intectable = value;
        }

        public void Interact()
        {
            m_intectable.OnInteract();
            ResetData();  
        }

        public bool IsSameInteractable(InteractableBase _newInteractable) => m_intectable == _newInteractable;

        public bool IsEmpty() => m_intectable == null;
        public void ResetData() => m_intectable = null;
    }
}
