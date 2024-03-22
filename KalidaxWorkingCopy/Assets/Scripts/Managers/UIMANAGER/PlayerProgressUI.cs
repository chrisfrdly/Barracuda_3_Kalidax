using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerProgressUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private SO_GameEvent gameEvents;

    private Dictionary<ProgressState, string> stateMessages = new Dictionary<ProgressState, string>();
    private static HashSet<ProgressState> shownMessages = new HashSet<ProgressState>(); 

    private void Awake()
    {
        InitializeStateMessages();
    }

    // Call this method to reset progress flags when starting a new game session
    public static void ResetProgressFlags()
    {
        shownMessages.Clear();
    }

    private void InitializeStateMessages()
    {
        stateMessages[ProgressState.None] = "Let's Collect a seed, go over to the grass on the left side of the map";
        stateMessages[ProgressState.SeedCollected] = "Good work on those fields! You should have one pod functional, Place the seed in the pod, wait for incubation.";
        stateMessages[ProgressState.SeedPlaced] = "Wait for incubation to complete. Should take 2 days, pass some time in the shelter pod";
        stateMessages[ProgressState.Incubating] = "It's still got some time in the oven, in the meantime; you can either farm or keep waiting. Don't forget to get some rest in the pod.";
        stateMessages[ProgressState.IncubationComplete] = "Take the seed out of the pod! Let's see what you've accomplished.";
    }

    private void OnEnable()
    {
        gameEvents.onProgressChanged.AddListener(HandleProgressChange);
    }

    private void OnDisable()
    {
        gameEvents.onProgressChanged.RemoveListener(HandleProgressChange);
    }

    private void HandleProgressChange(ProgressState state)
    {
        if (!shownMessages.Contains(state) && stateMessages.ContainsKey(state))
        {
            UpdateMessage(stateMessages[state]);
            shownMessages.Add(state); // Ensure this change is reflected across scenes by making 'shownMessages' static
        }
    }

    private void UpdateMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
        else
            Debug.LogWarning("Message Text UI component is not assigned in PlayerProgressUI.");
    }
}
