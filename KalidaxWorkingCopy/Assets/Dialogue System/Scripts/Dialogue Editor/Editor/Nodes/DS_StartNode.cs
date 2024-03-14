using DS_Editor;

using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS_Node
{
    public class DS_StartNode : DS_BaseNode
    {
        public DS_StartNode()
        {
            //Empty. We need to search for this node which is why we have one that's empty
        }

        public DS_StartNode(Vector2 position, DS_DialogueEditorWindow dialogueEditorWindow, DS_DialogueGraphView dialogueGraphView)
        {
            editorWindow = dialogueEditorWindow;
            graphView = dialogueGraphView;

            //Chage title of the node
            title = "Start";
            SetPosition(new Rect(position, defaultNodeSize));

            //Create it's custom ID
            //Always try to reference the field (nodeGUID) instead of the property (NodeGUID) when working in the same class
            nodeGUID = Guid.NewGuid().ToString();

            //Adding the output port for the Start Node
            AddOutputPort(outputPortName, Port.Capacity.Single);

            //Need to tell the system that we changed the Start Node and to refresh it with the updates
            RefreshExpandedState();

            //Tell the graphview to refresh the layout of the ports
            RefreshPorts();

            //Add a class to this node so we can refer to it in the style sheet
            AddToClassList("startNode");
        }
    }

}
