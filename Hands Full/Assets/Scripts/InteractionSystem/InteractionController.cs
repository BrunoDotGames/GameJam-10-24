using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDG
{
    public class InteractionController : MonoBehaviour, IDisposable
    {
        #region Variables
        [Header("Data")]

        public InteractionInputData interactionInputData;
        public InteractionData interactionData;

        [Space, Header("UI")]
        [SerializeField] private InteractionUIPanel uIPanel;

        [Space]
        [Header("Ray Settings")]

        public float rayDistance;
        public float raySphereRadius;
        public LayerMask interactableLayer;
        private bool canInteract = false;
        public Action<bool> CanInteract;

        #region Private

        private Camera m_cam;
        private bool m_interacting;
        private float m_holdTimer = 0f;

        #endregion

        #endregion

        #region Built in Methods
        void Awake()
        {
            m_cam = FindObjectOfType<Camera>();
        }

        void Update()
        {
            CheckForInteractable();
            CheckForInteractableInput();
        }
        #endregion

        #region Custom Methods

        void CheckForInteractable()
        {
            Ray _ray = new Ray(m_cam.transform.position, m_cam.transform.forward);
            RaycastHit _hitInfo;

            Debug.Log($"Ray Origin: {_ray.origin}, Ray Direction: {_ray.direction}");

            bool _hitSomething = Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance,
            interactableLayer);

            if (_hitSomething)
            {
                InteractableBase _interactable = _hitInfo.transform.GetComponent<InteractableBase>();
                Debug.Log($"Interactable is not null : {_interactable != null}");
                Debug.Log($"Hit: {_hitInfo.transform.name}");
                uIPanel.SetTooltip(_hitInfo.transform.name);
                if(LayerMask.LayerToName(_hitInfo.transform.gameObject.layer) == "Interactable")
                {
                    canInteract = true;
                    CanInteract?.Invoke(canInteract);
                }
                else
                {
                    canInteract = false;
                    CanInteract?.Invoke(canInteract);
                }
                if (_interactable != null)
                {
                    // DEBT - Not sure if we really need this. so i wont remove it for now.
                    //if (interactionData.IsEmpty())
                    //{
                    //    interactionData.Interactable = _interactable;
                    //    uIPanel.SetTooltip(_interactable.TooltipMessage);
                    //}
                    //else
                    //{
                    //    Debug.Log($"IsNotSameInteractable : {!interactionData.IsSameInteractable(_interactable)}");
                    //    if (!interactionData.IsSameInteractable(_interactable)) 
                    //    { 
                    //        interactionData.Interactable = _interactable;
                    //        uIPanel.SetTooltip(_interactable.TooltipMessage);
                    //    }
                    //}
                }
            }
            else
            {
                interactionData.ResetData();
                uIPanel.ResetUI();
            }

            Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green :
            Color.red);
        }

        void CheckForInteractableInput()
        {
            if (interactionData.IsEmpty())
                return;

            if (interactionInputData.InteractedClicked)
            {
                m_interacting = true;
                m_holdTimer = 0f;
            }

            if (interactionInputData.InteractedRelease)
            {
                m_interacting = false;
                m_holdTimer = 0f;
                uIPanel.UpdateProgressBar(m_holdTimer);
            }

            if (m_interacting)
            {
                if (!interactionData.Interactable.IsInteractable)
                    return;

                if (interactionData.Interactable.HoldInteract)
                {
                    m_holdTimer += Time.deltaTime;

                    float heldPercent = m_holdTimer / interactionData.Interactable.HoldDuration;
                    uIPanel.UpdateProgressBar(heldPercent);


                    if (heldPercent > 1f)
                    {
                        interactionData.Interact();
                        m_interacting = false;
                    }
                }
                else
                {
                    interactionData.Interact();
                    m_interacting = false;
                }
            }
        }

        public void Dispose()
        {
            CanInteract = null;
        }
        #endregion
    }
}