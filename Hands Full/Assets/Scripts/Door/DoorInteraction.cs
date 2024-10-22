using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public DoorData doorData;

    public void OpenDoor()
    {
        // Open the door
        // Temporary implementation
        Debug.Log("Door Opened");
        Destroy(gameObject);
    }
}
