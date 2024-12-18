using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public DoorData doorData;

    // Add Animation or Sound

    public void OpenDoor(ItemType itemType)
    {
        // Open the door
        // Temporary implementation
        // Once you already add the animation or sound, you can remove the Destroy(gameObject) line
        Debug.Log("Door Opened");
        if(doorData.LockId == itemType)
        {
            Destroy(gameObject);
        }
    }
}
