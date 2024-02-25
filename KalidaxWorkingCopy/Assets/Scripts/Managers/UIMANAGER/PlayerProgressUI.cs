using UnityEngine;
using TMPro;

public class PlayerProgressUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private SO_GameEvent gameEvents;

    // Flags to check if the message has been shown
    private bool gameStartMessageShown = false;
    private bool seedCollectedMessageShown = false;
    private bool seedPlacedMessageShown = false;
    private bool incubationCompleteMessageShown = false;
    private bool seedIncubatingMessageShown = false;

    private void OnEnable()
    {
        // Subscribe to the events
        gameEvents.onGameStartEvent.AddListener(HandleGameStart);
        gameEvents.onSeedCollectedEvent.AddListener(HandleSeedCollected);
        gameEvents.onSeedPlacedEvent.AddListener(HandleSeedPlaced);
        gameEvents.onIncubatingEvent.AddListener(HandleSeedIncubating);
        gameEvents.onIncubationCompleteEvent.AddListener(HandleIncubationComplete);
    }

    private void OnDisable()
    {
        // Unsubscribe from the events
        gameEvents.onGameStartEvent.RemoveListener(HandleGameStart);
        gameEvents.onSeedCollectedEvent.RemoveListener(HandleSeedCollected);
        gameEvents.onSeedPlacedEvent.RemoveListener(HandleSeedPlaced);
        gameEvents.onIncubatingEvent.RemoveListener(HandleSeedIncubating);
        gameEvents.onIncubationCompleteEvent.RemoveListener(HandleIncubationComplete);
    }

    // Event Handlers
    private void HandleGameStart(ProgressState state)
    {
        if (!gameStartMessageShown)
        {
            UpdateMessage("Let's Collect a seed, go over to the grass on the left side of the map");
            gameStartMessageShown = true;
        }
    }

    private void HandleSeedCollected(ProgressState state)
    {
        if (!seedCollectedMessageShown)
        {
            UpdateMessage("Good work on those fields! You should have one pod functional, Place the seed in the pod, wait for incubation.");
            seedCollectedMessageShown = true;
        }
    }

    private void HandleSeedPlaced(ProgressState state)
    {
        if (!seedPlacedMessageShown)
        {
            UpdateMessage("Wait for incubation to complete. Should take 7 days, pass some time in the shelter pod");
            seedPlacedMessageShown = true;
        }
    }
    private void HandleSeedIncubating(ProgressState state)
    {
        if (!seedIncubatingMessageShown)
        {
            UpdateMessage("It's still got some time in the oven, in the meantime; you can either farm or keep waiting. Don't forget to get some rest in the pod.");
            seedIncubatingMessageShown = true;
        }
    }

    private void HandleIncubationComplete(ProgressState state)
    {
        if (!incubationCompleteMessageShown)
        {
            UpdateMessage("Take the seed out of the pod! Let's see what you've accomplished.");
            incubationCompleteMessageShown = true;
        }
    }

    // Utility method to update message text
    private void UpdateMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
        else
            Debug.LogWarning("Message Text UI component is not assigned in PlayerProgressUI.");
    }
}
