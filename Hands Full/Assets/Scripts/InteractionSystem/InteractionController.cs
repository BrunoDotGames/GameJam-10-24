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
        public Action<bool, Item> CanInteract;

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

            //Debug.Log($"Ray Origin: {_ray.origin}, Ray Direction: {_ray.direction}");

            bool _hitSomething = Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance,
            interactableLayer);

            if (_hitSomething)
            {
                Item _interactable = _hitInfo.transform.GetComponent<Item>();
                if(LayerMask.LayerToName(_hitInfo.transform.gameObject.layer) == "Interactable" && _interactable != null)
                {
                    uIPanel.SetTooltip(_interactable.data.ItemName);
                    canInteract = true;
                    // This can be a problem for us, since we invoke at the update. We should invoke at the end of the frame.
                    // But this work. so dont change it for now.
                    CanInteract?.Invoke(canInteract,_interactable);
                    
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