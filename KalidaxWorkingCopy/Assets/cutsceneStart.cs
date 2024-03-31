using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutsceneStart : MonoBehaviour
{
     private DS_DialogueTalk dialogueTalk;
    [SerializeField] private DS_SO_ConversationTree conversation;

     void Awake()
    {
        dialogueTalk = GetComponent<DS_DialogueTalk>();

        if (dialogueTalk == null)
        {
            dialogueTalk = gameObject.AddComponent<DS_DialogueTalk>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //player will never walk so don't need to reference freeze player
        dialogueTalk.StartDialogue(conversation, false);
    }

}
