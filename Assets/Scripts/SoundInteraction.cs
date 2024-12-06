using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundInteraction : MonoBehaviour
{
    public Transform player;         // Reference to the player's transform
    public float maxDistance = 5f;  // Maximum distance from the object's center to hear full volume
    public float minDistance = 1f;  // Distance for maximum volume
    private AudioSource audioSource; // The object's AudioSource
    private bool isPlayerInRange = false; // Tracks if the player is in range

    void Start()
    {
        // Get the AudioSource attached to this object
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f; // Start with sound muted
    }

    void Update()
    {
        // Adjust volume only if the player is within range
        if (isPlayerInRange)
        {
            AdjustSoundVolume();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            audioSource.volume = 0f; // Mute the sound when the player leaves
        }
    }

    void AdjustSoundVolume()
    {
        // Calculate the distance between the player and the object
        float distance = Vector3.Distance(player.position, transform.position);

        // Adjust the volume based on the distance
        if (distance <= minDistance)
        {
            audioSource.volume = 1f; // Full volume if very close
        }
        else if (distance >= maxDistance)
        {
            audioSource.volume = 0f; // No sound if far away
        }
        else
        {
            // Calculate interpolated volume based on distance
            float volume = 1 - (distance - minDistance) / (maxDistance - minDistance);
            audioSource.volume = Mathf.Clamp(volume, 0f, 1f);
        }
    }
}