using UnityEngine;

public class LightCameraFollow : MonoBehaviour
{
    [Header("Main Camera Settings")]
    public Camera mainCamera; // Reference to the main camera (player's camera)

    private void Update()
    {
        // Ensure that the light camera follows the main camera's position and rotation
        if (mainCamera != null)
        {
            // Follow the main camera's position
            transform.position = mainCamera.transform.position;
            // Follow the main camera's rotation
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}
