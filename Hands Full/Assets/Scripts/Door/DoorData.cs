using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoorData", menuName = "Door/DoorData")]
public class DoorData : ScriptableObject
{
    public ItemType LockId;
    public bool IsLock;
    public string Description;
}
