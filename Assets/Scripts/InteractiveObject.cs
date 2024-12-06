using UnityEngine;

// This script goes to a specific gameobject within the room to be interacable by the player.
public class InteractiveObject : MonoBehaviour
{
    public RoomColorRestorer roomRestorer; // Reference to the RoomColorRestorer script
    private bool playerInRange = false; // Tracks if the player is near the object

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the object detects the player
        {
            playerInRange = true;
            Debug.Log("Player near the object. Press 'E' to interact.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left the interaction range.");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // Check for E key press
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (roomRestorer != null)
        {
            roomRestorer.RestoreRoom(); // Trigger the room transition
        }

        Debug.Log("Object interacted with! Transitioning room to color.");
        Destroy(this); // Remove the script after interaction to prevent multiple triggers
    }
}
