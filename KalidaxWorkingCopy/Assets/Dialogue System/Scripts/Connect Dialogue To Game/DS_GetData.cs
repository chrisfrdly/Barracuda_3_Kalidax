using UnityEngine;

public class DS_GetData : MonoBehaviour
{
    //Load in different dialogues that this one is going to use
    protected DS_SO_ConversationTree conversationTree;

    //will find the specific node in the conversation tree with the passed in GUID
    protected BaseNodeData GetNodeByGUID(string _targetNodeGUID)
    {
        //go search all the nodes in the conversation tree and find the node that matches our target node GUID
        return conversationTree.AllNodes.Find(node => node.NodeGUID == _targetNodeGUID);
    }

    protected BaseNodeData GetNodeByNodePort(DialogueNodePort _nodePort)
    {
        //look at the node port where the line is going and return the ndoe that the line is connecting to
        return conversationTree.AllNodes.Find(node => node.NodeGUID == _nodePort.InputGUID);
    }

    protected BaseNodeData GetNextNode(BaseNodeData _baseNodeData)
    {
        //finding the edge that is connected to the specific node 
        NodeLinkData nodeLinkData = conversationTree.NodeLinkDatas.Find(edge => edge.BaseNodeGUID == _baseNodeData.NodeGUID);

        //find link and give it target
        return GetNodeByGUID(nodeLinkData.TargetNodeGUID);
    }

}
