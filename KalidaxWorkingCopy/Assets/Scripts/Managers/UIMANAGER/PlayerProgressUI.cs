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
        stateMessages[ProgressState.None] = "Proceed to the designated area on the left for seed collection. Compliance is expected.";
        stateMessages[ProgressState.SeedCollected] = "Seed acquisition confirmed. Utilize the allocated pod for seed incubation immediately. Further instructions will follow.";
        stateMessages[ProgressState.SeedPlaced] = "Seed placement acknowledged. Incubation period is two days. Utilize shelter pod facilities as necessary during this interval.";
        stateMessages[ProgressState.Incubating] = "Incubation in progress. Allocate your time between auxiliary tasks or mandatory rest periods in the shelter pod. Further disturbance is not advised.";
        stateMessages[ProgressState.IncubationComplete] = "Incubation phase concluded. Extract the seed from the pod for assessment. Your compliance will be noted.";
        stateMessages[ProgressState.PostIncubation] = "Initiate further splicing operations to ensure continuous profit generation. \n Failure to contribute to profit margins will result in termination of your position.";
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
