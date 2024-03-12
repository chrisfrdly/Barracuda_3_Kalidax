using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DS_DialogueTalk : DS_GetData
{
    private DS_DialogueController dialogueController;
    private AudioSource audioSource;

    private DialogueNodeData currentDialogueNodeData; //see which dialogue node we're on right now
    private DialogueNodeData previousDialogueNodeData; //go back node
    private bool freezeMovement;

    private DS_LanguageType currentLanguageActive;

    private void Awake()
    {
        dialogueController = FindObjectOfType<DS_DialogueController>();
        audioSource = GetComponent<AudioSource>();

    }

    public void StartDialogue(DS_SO_ConversationTree _conversationTree, bool _freezePlayerMovement)
    {
        conversationTree = _conversationTree;
        freezeMovement = _freezePlayerMovement;

        //Returning a base node and getting the next node.
        //Returns the next node after the start node
        CheckNodeType(GetNextNode(conversationTree.StartNodeDatas[0]));

        //Show UI
        dialogueController.ShowDialogueUI(true);

        if(freezeMovement)
        {
            //freeze player movement
            PlayerInputHandler.Instance.SwitchActionMap(false);
        }

        //Cash the Language Type we have set to in the "Language Controller" game Object so we don't need to repeat it in this script

        currentLanguageActive = DS_LanguageController.Instance.LanguageType;

    }

    private void CheckNodeType(BaseNodeData _baseNodeData)
    {
        switch(_baseNodeData) 
        {
            case StartNodeData nodeData:
                RunNode(nodeData);
                break;

            case DialogueNodeData nodeData:
                RunNode(nodeData);
                break;

            case EventNodeData nodeData:
                RunNode(nodeData);
                break;

            case EndNodeData nodeData:
                RunNode(nodeData);
                break;

            default:
                break;

        
        }
    }

    private void RunNode(StartNodeData _nodeData)
    {
        CheckNodeType(GetNextNode(conversationTree.StartNodeDatas[0]));
    }

    private void RunNode(DialogueNodeData _nodeData)
    {
        audioSource.Stop();
        previousDialogueNodeData = currentDialogueNodeData;
        currentDialogueNodeData = _nodeData;

        //return the text of the node based on the current language we have set in the 
        dialogueController.SetText(_nodeData.Name, _nodeData.TextType.Find(text => text.LanguageType == currentLanguageActive).LanguageGenericType);
        dialogueController.SetImage(_nodeData.Sprite, _nodeData.DialogueSpriteImageType);

        MakeButtons(_nodeData.DialogueNodePorts);

        //play the audio that matches the language we have set in the scene
        audioSource.clip = _nodeData.AudioClips.Find(clip => clip.LanguageType == currentLanguageActive).LanguageGenericType;

        Invoke("PlayAudio", 0.1f);
    }

    private void PlayAudio()
    {
        audioSource.Play();
    }



    private void RunNode(EventNodeData _nodeData)
    {
        //plays event if we have one set
        if (_nodeData.DialogueEventSO != null)
        {
            _nodeData.DialogueEventSO.RunEvent();
        }

        CheckNodeType(GetNextNode(_nodeData));
    }
    private void RunNode(EndNodeData _nodeData)
    {
        switch (_nodeData.EndNodeType)
        {
            case DS_EndNodeType.End:
                dialogueController.ShowDialogueUI(false);

                if(freezeMovement)
                {
                    PlayerInputHandler.Instance.SwitchActionMap(true);
                }
                
                break;

            case DS_EndNodeType.Repeat:
                CheckNodeType(GetNodeByGUID(currentDialogueNodeData.NodeGUID));
                break;

            case DS_EndNodeType.Go_Back:
                CheckNodeType(GetNodeByGUID(previousDialogueNodeData.NodeGUID));
                break;

            case DS_EndNodeType.Return_To_Start:
                CheckNodeType(GetNextNode(conversationTree.StartNodeDatas[0]));
                break;
        }
    }

    private void MakeButtons(List<DialogueNodePort> _nodePorts)
    {
        List<string> texts = new List<string>();
        List<UnityAction> unityActions = new List<UnityAction>();

        foreach(DialogueNodePort nodePort in _nodePorts)
        {
            //add the text, look through the node port text field. List of all the different types of languages we have, check to see which language we-
            //are currently using and set the text to that language
            texts.Add(nodePort.TextLanguages.Find(text => text.LanguageType == currentLanguageActive).LanguageGenericType);

            //it can't be blank so we are setting it to null
            UnityAction tempAction = null;

            //way of adding a method to an action FOR DIALOGUE CHOICES
            tempAction += () =>
            {
                //CheckNodeType takes in a node. The GetNodeByGUID returns the node's data. checks which input the node is connected to and giving the button the- 
                //target connection's node's GUID
                CheckNodeType(GetNodeByGUID(nodePort.InputGUID));

                //stop all audio when we click next
                audioSource.Stop();
            };

            unityActions.Add(tempAction);
        }

        //Setting the text for the button and set up their action. AKA which node to go to next when we click the NEXT button.
        dialogueController.SetButtons(texts, unityActions);
    }
   
}