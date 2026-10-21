using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door door;

    public bool locked = false;

    public bool lockedByScore = false;
    public int requiredScore = 100;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        FPSController player = other.GetComponent<FPSController>();

        if (locked)
        {
            if (KeyItem.hasKey)
            {
                locked = false;
                door.OpenDoor();
            }
        }
        else if (lockedByScore)
        {
            if (player != null && player.m_Score >= requiredScore)
            {
                lockedByScore = false;
                door.OpenDoor();
            }
        }
        else
        {
            door.OpenDoor();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !locked && !lockedByScore)
        {
            door.CloseDoor();
        }
    }
}