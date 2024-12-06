using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    public RoomColorRestorer[] rooms; // Array of all rooms in the house
    public PostProcessVolume globalPostProcessing; // Post-processing volume for black-and-white effect
    public Light globalLight; // Main light for the environment
    public Color finalEnvironmentColor = Color.white; // Final environment light color

    // Array to collect all unique "people" objects from all rooms
    public GameObject[] allPeopleObjects;

    private int restoredRooms = 0;

    private void Start()
    {
        // Collect all unique people objects from rooms
        CollectAllPeopleObjects();

        // Initialize restored rooms counter
        restoredRooms = 0;

        // Subscribe to the OnRoomRestored event for each room
        foreach (RoomColorRestorer room in rooms)
        {
            room.OnRoomRestored += HandleRoomRestored;
        }

        // Ensure post-processing starts in black and white
        if (globalPostProcessing != null)
        {
            var colorGrading = globalPostProcessing.profile.GetSetting<ColorGrading>();
            colorGrading.saturation.value = -100; // Fully desaturated
        }

        // Dim global light at the start
        if (globalLight != null)
        {
            globalLight.color = Color.gray;
            globalLight.intensity = 0.5f; // Start with dim light
        }

        // Initially hide all people objects
        SetAllPeopleVisibility(false);
    }

    private void CollectAllPeopleObjects()
    {
        // Create a list to collect unique people objects
        var peopleList = new System.Collections.Generic.List<GameObject>();

        // Collect people objects from each room
        foreach (RoomColorRestorer room in rooms)
        {
            if (room.people != null)
            {
                foreach (GameObject person in room.people)
                {
                    if (!peopleList.Contains(person))
                    {
                        peopleList.Add(person);
                    }
                }
            }
        }

        // Convert list to array
        allPeopleObjects = peopleList.ToArray();
    }

    private void HandleRoomRestored()
    {
        restoredRooms++;

        // Check if all rooms are restored
        if (restoredRooms == rooms.Length)
        {
            CompleteTransformation();
        }
    }

    private void CompleteTransformation()
    {
        Debug.Log("All rooms restored! Completing transformation...");

        // Start the transition to full color
        StartCoroutine(RestoreGlobalColor());
      
        // Make all people objects permanently visible
        SetAllPeopleVisibility(true);
    }

    private void SetAllPeopleVisibility(bool isVisible)
    {
        // Activate or deactivate all collected people objects
        if (allPeopleObjects != null)
        {
            foreach (GameObject person in allPeopleObjects)
            {
                if (person != null)
                {
                    person.SetActive(isVisible);
                }
            }
        }
    }

    private IEnumerator RestoreGlobalColor()
    {
        float transitionTime = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = elapsedTime / transitionTime;

            // Restore global post-processing saturation
            if (globalPostProcessing != null)
            {
                var colorGrading = globalPostProcessing.profile.GetSetting<ColorGrading>();
                colorGrading.saturation.value = Mathf.Lerp(-100, 0, lerpValue);
            }

            // Brighten global light and transition its color
            if (globalLight != null)
            {
                globalLight.color = Color.Lerp(globalLight.color, finalEnvironmentColor, lerpValue);
                globalLight.intensity = Mathf.Lerp(globalLight.intensity, 2f, lerpValue); // Adjust final intensity as needed
            }

            yield return null;
        }

        Debug.Log("Environment fully restored to color!");
    }

    // Cleanup to prevent memory leaks
    private void OnDestroy()
    {
        // Unsubscribe from events
        foreach (RoomColorRestorer room in rooms)
        {
            room.OnRoomRestored -= HandleRoomRestored;
        }
    }
}