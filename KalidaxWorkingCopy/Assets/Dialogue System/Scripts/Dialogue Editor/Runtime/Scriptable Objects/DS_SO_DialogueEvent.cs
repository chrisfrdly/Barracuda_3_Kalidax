using UnityEngine;

[CreateAssetMenu(fileName = "DialogueContainer", menuName = "DialogueSystem/New Dialogue Event")]
[System.Serializable]

public class DS_SO_DialogueEvent : ScriptableObject
{
    //Can override the dialogueEvent Scriptable Object. We can customize it to do any Event we want it to do
    public virtual void RunEvent()
    {
        Debug.Log("Event was Called");
    }
}

