using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDG
{

    [CreateAssetMenu(fileName = "InteractionInputData", menuName = "InteractionSystem/InputData")]
    public class InteractionInputData : ScriptableObject
    {
        private bool b_interactedClicked;
        private bool b_interactedRelease;

        public bool InteractedClicked
        {
            get => b_interactedClicked;
            set => b_interactedClicked = value;
        }

        public bool InteractedRelease
        {
            get => b_interactedRelease;
            set => b_interactedRelease = value;
        }

        public void ResetInput()
        {
            b_interactedClicked = false;
            b_interactedRelease = false;
        }
    }
}