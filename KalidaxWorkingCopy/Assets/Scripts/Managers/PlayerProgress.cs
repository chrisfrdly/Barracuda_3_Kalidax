using UnityEngine;
using TMPro; // Add this namespace to use TextMeshPro components

public class PlayerProgress : MonoBehaviour
{
    // Enum to represent the player's progress states
    public enum ProgressState
    {
        SeedCollected,
        SeedPlaced,
        IncubationComplete
    }

    // Current state of the player's progress
    private ProgressState currentState = ProgressState.SeedCollected;

    // Reference to the TextMeshProUGUI component to display the message
    [SerializeField] private TextMeshProUGUI messageText;

    // The message string that will be updated based on the player's progress
    private string message = "Let's Collect a seed, go over to the grass on the left side of the map";

    private void Start()
    {
        // Update the UI text at the start
        UpdateMessageText();
    }

    // Method to call when the player collects a seed
    public void OnSeedCollected()
    {
        if (currentState == ProgressState.SeedCollected)
        {
            message = ", you should have one pod functional, Place the seed in the pod, wait for incubation.";
            currentState = ProgressState.SeedPlaced;
            UpdateMessageText();
        }
    }

    // Method to call when the player places the seed in the pod
    public void OnSeedPlaced()
    {
        if (currentState == ProgressState.SeedPlaced)
        {
            message = "Wait for incubation to complete. Should take 7 days, pass some time in the shelter pod";
            currentState = ProgressState.IncubationComplete;
            UpdateMessageText();
        }
    }

    // Method to call when incubation is complete and the player removes the seed
    public void OnIncubationComplete()
    {
        if (currentState == ProgressState.IncubationComplete)
        {
            message = "All steps complete!";
            UpdateMessageText();
        }
    }

    // Method to update the UI text with the current message
    private void UpdateMessageText()
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
        else
        {
            Debug.LogWarning("Message Text UI component is not assigned in PlayerProgress.");
        }
    }
}
