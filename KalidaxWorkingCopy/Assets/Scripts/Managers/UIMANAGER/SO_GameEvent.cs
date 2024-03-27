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
    public ProgressChangedEvent onProgressChanged = new ProgressChangedEvent();

    //Event to reset all data variables when we game over
    [System.NonSerialized]
    public UnityEvent resetDataEent = new UnityEvent();

    public void RaiseProgressChanged(ProgressState state)
    {
        onProgressChanged.Invoke(state);
    }

    
}

// Ensure this enum is accessible by including it in a relevant namespace or making it public
public enum ProgressState
{
    None = 0,
    SeedCollected,
    SeedPlaced,
    Incubating,
    IncubationComplete
}
