using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class RoomColorRestorer : MonoBehaviour
{
    public PostProcessVolume roomPostProcessingVolume; // Local post-processing volume
    public GameObject[] roomObjects; // List of objects in the room
    public Material colorfulMaterial; // Material for colorful state
    public GameObject[] people; // List of "people" in the room
    private bool isRestored = false;

    public event Action OnRoomRestored;

    private void Start()
    {
        // Initially hide people
        SetPeopleVisibility(false);
    }

    public void RestoreRoom()
    {
        if (!isRestored)
        {
            StartCoroutine(TransitionToColor());
        }
    }

    private IEnumerator TransitionToColor()
    {
        float transitionTime = 2f;
        float elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = elapsedTime / transitionTime;

            // Gradually enable local post-processing volume
            if (roomPostProcessingVolume != null)
            {
                roomPostProcessingVolume.weight = Mathf.Lerp(0f, 1f, lerpValue);
            }

            yield return null;
        }

        // Change room objects' materials to colorful
        foreach (GameObject obj in roomObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = colorfulMaterial;
            }
        }

        // Mark as restored and invoke the event
        isRestored = true;
        OnRoomRestored?.Invoke();

        Debug.Log("Room restored to color!");
    }

    // Helper method to set people visibility
    private void SetPeopleVisibility(bool isVisible)
    {
        foreach (GameObject person in people)
        {
            person.SetActive(isVisible);
        }
    }
}