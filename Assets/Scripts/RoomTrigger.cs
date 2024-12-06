using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public RoomColorRestorer roomColorRestorer;  // Reference to the RoomColorRestorer script
    public string playerTag = "Player";          // Tag to identify the player
    public GameObject[] people;                  // List of people (static objects) to activate/deactivate in the room
    private bool playerInRoom = false;           // Tracks whether the player is inside the room
    private bool roomRestored = false;           // Tracks whether the room has been restored

    private void Start()
    {
        // Subscribe to the room restoration event
        if (roomColorRestorer != null)
        {
            roomColorRestorer.OnRoomRestored += HandleRoomRestored;
        }

        // Initially hide people
        TogglePeopleVisibility(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag(playerTag))
        {
            playerInRoom = true;

            // Only toggle people visibility if the room is restored
            if (roomRestored)
            {
                TogglePeopleVisibility(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is the player
        if (other.CompareTag(playerTag))
        {
            playerInRoom = false;

            // Only toggle people visibility if the room is restored
            if (roomRestored)
            {
                TogglePeopleVisibility(false);
            }
        }
    }

    // This method will be called by the interactable object when the player interacts with it
    public void ActivateRoomRestoration()
    {
        if (playerInRoom)
        {
            roomColorRestorer.RestoreRoom();
        }
    }

    // Method to handle room restoration
    private void HandleRoomRestored()
    {
        roomRestored = true;

        // If player is currently in the room, show people
        if (playerInRoom)
        {
            TogglePeopleVisibility(true);
        }
    }

    private void TogglePeopleVisibility(bool isActive)
    {
        // Toggle the active state of each "people" object in the room
        foreach (GameObject person in people)
        {
            person.SetActive(isActive);
        }
    }

    // Cleanup to prevent memory leaks
    private void OnDestroy()
    {
        if (roomColorRestorer != null)
        {
            roomColorRestorer.OnRoomRestored -= HandleRoomRestored;
        }
    }
}