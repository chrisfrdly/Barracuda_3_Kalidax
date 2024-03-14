using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

[CreateAssetMenu(fileName = "Conversation Tree", menuName = "DialogueSystem/New Conversation Tree")]
[System.Serializable] //so we can create it in runtime without any problems

public class DS_SO_ConversationTree : ScriptableObject
{
    public List<NodeLinkData> NodeLinkDatas = new List<NodeLinkData>();

    //creating new lists of the node types
    public List<DialogueNodeData> DialogueNodeDatas = new List<DialogueNodeData>();
    public List<EndNodeData> EndNodeDatas = new List<EndNodeData>();
    public List<StartNodeData> StartNodeDatas = new List<StartNodeData>();
    public List<EventNodeData> EventNodeDatas = new List<EventNodeData>();

    //property to get ALL of these node types at the same time without changing the type they are
    //when this is called you return all the nodes which are returned as "BaseNodeData"
    public List<BaseNodeData> AllNodes
    {
        get
        {
            List<BaseNodeData> tmp = new List<BaseNodeData>();
            tmp.AddRange(DialogueNodeDatas);
            tmp.AddRange(StartNodeDatas);
            tmp.AddRange(EndNodeDatas);
            tmp.AddRange(EventNodeDatas);

            return tmp;
        }
    }
}

#region Saving and Loading
/// <summary>
/// If you ever want to create a new node, you'd have to add a new class for saving it's contents
/// Need it to be serializable since we're going to save it in runtime
/// This is where we are storing all of the data
/// </summary>

[System.Serializable]
public class NodeLinkData
{
    public string BaseNodeGUID;
    public string TargetNodeGUID;
}

[System.Serializable]
public class BaseNodeData
{
    //All other nodes inherit from the base node data
    public string NodeGUID;
    public Vector2 Pos; //position on the graph
}

[System.Serializable]
public class DialogueNodeData : BaseNodeData
{
    //Text Box
    public List<LanguageGeneric<string>> TextType;
    public List<DialogueNodePort> DialogueNodePorts; //for the choices ports
    public Sprite Sprite;
    public DS_DialogueSpriteImageType DialogueSpriteImageType;
    public List<LanguageGeneric<AudioClip>> AudioClips;
    public string Name;

}
[System.Serializable]
public class EndNodeData : BaseNodeData
{
    public DS_EndNodeType EndNodeType;

}

[System.Serializable]
public class StartNodeData : BaseNodeData
{

}

[System.Serializable]
public class EventNodeData : BaseNodeData
{
    public DS_SO_DialogueEvent DialogueEventSO;

}
#endregion


//use a generic. 
//Whatever we put in the <T> it becomes that. Eg a string will be a string, Audio clip will be audio clip
[System.Serializable]
public class LanguageGeneric<T>
{
    public DS_LanguageType LanguageType;
    public T LanguageGenericType;

}

//for saving how many ports we have for each node
[System.Serializable]
public class DialogueNodePort
{
    //know what node the node is connected to. 
    //This is for the dialogue node only and for the choices
    public string InputGUID;
    public string OutputGUID;

    //only unityEditor supports this type <Port> so need to put it in this If statement or else can't build project
#if UNITY_EDITOR
    public Port MyPort;
#endif

    public TextField TextField; //for the choices, they each have a text field 
    public List<LanguageGeneric<string>> TextLanguages = new List<LanguageGeneric<string>>();
}
