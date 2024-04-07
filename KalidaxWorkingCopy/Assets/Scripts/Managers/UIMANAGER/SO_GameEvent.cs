using UnityEngine;
using UnityEngine.Events;

// Define a UnityEvent type that can take a ProgressState as a parameter

[System.Serializable]
public class ProgressChangedEvent : UnityEvent<ProgressState> { }

[CreateAssetMenu(fileName = "GameEvent", menuName = "Game Events/New Game Event")]
public class SO_GameEvent : ScriptableObject
{
    // Define events for each game progress state


    public ProgressChangedEvent onProgressChanged = new ProgressChangedEvent();
    public ProgressState currentState;

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
    IncubationComplete,
    PostIncubation,
    PostSplice
}
