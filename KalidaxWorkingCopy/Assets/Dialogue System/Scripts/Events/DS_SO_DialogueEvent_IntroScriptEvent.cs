using DS_Events;
using UnityEngine;

[CreateAssetMenu(fileName = "Intro Script Event", menuName = "DialogueSystem/Events/Intro Script Event")]
[System.Serializable]

public class DS_SO_DialogueEvent_IntroScriptEvent : DS_SO_DialogueEvent
{
    //Can override the dialogueEvent Scriptable Object. We can customize it to do any Event we want it to do
    public override void RunEvent()
    {
        DS_GameEvents.Instance.CallIntroScriptAction();
        base.RunEvent();
    }
}
