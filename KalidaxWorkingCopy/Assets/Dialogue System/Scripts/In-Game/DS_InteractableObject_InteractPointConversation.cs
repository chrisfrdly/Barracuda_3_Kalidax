
using UnityEngine;

public class DS_InteractableObject_InteractPointConversation : InteractableObject
{

    [Header("Dialogue")]
    [SerializeField] private DS_SO_ConversationTree conversation;

    private DS_DialogueTalk dialogueTalk;


    protected override void Awake()
    {
        base.Awake();
        dialogueTalk = GetComponent<DS_DialogueTalk>();

        if(dialogueTalk == null )
        {
            dialogueTalk = gameObject.AddComponent<DS_DialogueTalk>();
        }
    }


    public override void OnInteract(GameObject _interactedActor)
    {
        if(conversation == null)
        {
            Debug.LogError("This Interaction Point doesn't have a conversation attached to it! Please attach one for the interaction to work");
            return;
        }

        if (!CheckIsInteractable()) return;

        dialogueTalk.StartDialogue(conversation,freezePlayerMovement);
    }

    public override bool CheckIsInteractable() { return isInteractable; }
    public override bool IsTargetPointVisible() { return isInteractPointVisible; }
    public override bool FreezePlayerMovement() { return freezePlayerMovement; }
    public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }

   
}
