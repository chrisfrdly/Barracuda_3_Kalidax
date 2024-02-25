using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Game Events/New Game Event")]
public class SO_GameEvent : ScriptableObject
{
    [System.NonSerialized]
    public GameProgressEvent onGameStartEvent = new GameProgressEvent();
    [System.NonSerialized]
    public GameProgressEvent onSeedCollectedEvent = new GameProgressEvent();
    [System.NonSerialized]
    public GameProgressEvent onSeedPlacedEvent = new GameProgressEvent();
    [System.NonSerialized]
    public GameProgressEvent onIncubatingEvent = new GameProgressEvent();
    [System.NonSerialized]
    public GameProgressEvent onIncubationCompleteEvent = new GameProgressEvent();

    public void RaiseOnGameStart(ProgressState state)
    {
        onGameStartEvent.Invoke(state);
    }

    public void RaiseOnSeedCollected(ProgressState state)
    {
        onSeedCollectedEvent.Invoke(state);
    }

    public void RaiseOnSeedPlaced(ProgressState state)
    {
        onSeedPlacedEvent.Invoke(state);
    }

    public void RaiseOnSeedIncubating(ProgressState state)
    {
        onIncubatingEvent.Invoke(state);
    }

    public void RaiseOnIncubationComplete(ProgressState state)
    {
        onIncubationCompleteEvent.Invoke(state);
    }
}

[System.Serializable]
public class GameProgressEvent : UnityEvent<ProgressState> {}

public enum ProgressState
{
    None = 0,
    SeedCollected,
    SeedPlaced,
    Incubating,
    IncubationComplete
}
