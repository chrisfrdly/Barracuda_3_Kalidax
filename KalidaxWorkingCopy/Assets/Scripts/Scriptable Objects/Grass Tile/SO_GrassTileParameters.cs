using System.Collections;

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Grass Tile Parameters", menuName = "Scriptable Objects/Grass Tile Parameters")]
public class SO_GrassTileParameters : ScriptableObject
{
    //Variables
    [Range(0f, 100f)]
    public int chanceToRegrow_EndOfDay;

    [Range(0f, 100f)]
    public int chanceToGetSeed;

    /* UNITY EVENTS */

    //This method is called from the "PlayerMovement.cs" Script
    //The event is recieved from the "GrassTile.cs" Script
    [System.NonSerialized]
    public BreakGrassEvent breakGrassEvent = new BreakGrassEvent();
    public void BreakGrassEventSend(Transform transform)
    {
        breakGrassEvent.Invoke(transform);
    }
}
[System.Serializable]
public class BreakGrassEvent : UnityEvent<Transform> { }


