using UnityEngine;
using UnityEngine.Events;

// Define a UnityEvent type that can take a ProgressState as a parameter
[System.Serializable]
public class ProgressEvent : UnityEvent<ProgressState> { }
[System.Serializable]
public class ProgressChangedEvent : UnityEvent<ProgressState> { }

[CreateAssetMenu(fileName = "GameEvent", menuName = "Game Events/New Game Event")]
public class SO_GameEvent : ScriptableObject
{
    // Define events for each game progress state
    [System.NonSerialized]
    public ProgressEvent onGameStart = new ProgressEvent();
    [System.NonSerialized]
    public ProgressEvent onSeedCollected = new ProgressEvent();
    [System.NonSerialized]
    public ProgressEvent onSeedPlaced = new ProgressEvent();
    [System.NonSerialized]
    public ProgressEvent onIncubating = new ProgressEvent();
    [System.NonSerialized]
    public ProgressEvent onIncubationComplete = new ProgressEvent();
    [System.NonSerialized]
    public ProgressEvent postComplete = new ProgressEvent();
    [System.NonSerialized]
    public ProgressChangedEvent onProgressChanged = new ProgressChangedEvent();


    public void RaiseProgressChanged(ProgressState state)
    {
        onProgressChanged.Invoke(state);
    }

    // Generic method to raise an event based on the provided ProgressState
    public void RaiseEvent(ProgressState state)
    {
        switch (state)
        {

            case ProgressState.None:
                break;
            case ProgressState.SeedCollected:
                onSeedCollected.Invoke(state);
                break;
            case ProgressState.SeedPlaced:
                onSeedPlaced.Invoke(state);
                break;
            case ProgressState.Incubating:
                onIncubating.Invoke(state);
                break;
            case ProgressState.IncubationComplete:
                onIncubationComplete.Invoke(state);
                break;
            case ProgressState.PostIncubation:
                postComplete.Invoke(state);
                break;
            default:
                Debug.LogWarning("Unhandled ProgressState: " + state);
                break;
        }
    }
}

// Ensure this enum is accessible by including it in a relevant namespace or making it public
public enum ProgressState
{
    None = 0,
    SeedCollected,
    SeedPlaced,
    Incubating,
    IncubationComplete,
    PostIncubation
}
