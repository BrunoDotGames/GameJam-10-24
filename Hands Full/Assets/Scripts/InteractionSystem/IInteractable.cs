using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDG
{
    public interface IInteractable
    {
        float HoldDuration {get;}

        bool HoldInteract {get;}
        bool MultipleUse {get;}
        bool IsInteractable {get;}

        string TooltipMessage { get; }

    }
    
}
