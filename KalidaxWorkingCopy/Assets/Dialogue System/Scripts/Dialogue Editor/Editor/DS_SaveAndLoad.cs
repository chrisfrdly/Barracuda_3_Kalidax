using DS_Editor;
using DS_Node;

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS_SaveAndLoad
{
    public class DS_SaveAndLoad
    {
        //whenever we call this list, we are telling it to go into the graph view and get all the edges.
        //Whenever we update the graph view, this variable also gets updated
        private List<Edge> edges => graphView.edges.ToList();

        //Same with the Nodes. Whenever we call this variable we want to call the graphview.Nodes and convert it to a list
        //Go through that list and tell it we only want the nodes that are the base nodes and then we want it to return as a Base Node List
        private List<DS_BaseNode> nodes => graphView.nodes.ToList().Where(node => node is DS_BaseNode).Cast<DS_BaseNode>().ToList();

        private DS_DialogueGraphView graphView;

        //constructor to access the graph view
        public DS_SaveAndLoad(DS_DialogueGraphView _graphView)
        {
            graphView = _graphView;
        }


        public void Save(DS_SO_ConversationTree _conversationTree)
        {
            SaveEdges(_conversationTree);
            SaveNodes(_conversationTree);

            //Ehen saving in runtime, we need to tell editor that we changed it, that it's dirty now
            //If you want to save a Scriptable Object in runtime, you need to mark it as dirty (Unity's way of doing things)
            EditorUtility.SetDirty(_conversationTree);
            AssetDatabase.SaveAssets();
        }

        #region Load
        public void Load(DS_SO_ConversationTree _conversationTree)
        {
            ClearGraph();
            GenerateNodes(_conversationTree);
            GenerateNodeConnections(_conversationTree);

        }

        private void ClearGraph()
        {
            //Clearing the current graph contents so we can load the new ones in
            foreach (DS_BaseNode node in nodes)
            {
                graphView.RemoveElement(node);
            }

            foreach(Edge edge in edges)
            {
                graphView.RemoveElement(edge);
            }
        }

        private void GenerateNodes(DS_SO_ConversationTree _conversationTree)
        {
            /* START NODE */
            foreach (StartNodeData node in _conversationTree.StartNodeDatas)
            {
                //we already created a method in the DS_DialogueGraphView class that creates a new Node
                DS_StartNode tempNode = graphView.CreateStartNode(node.Pos);

                //set the node's GUID to the GUID we have saved in the StartNodeDatas List
                tempNode.NodeGUID = node.NodeGUID;

                //Add the node to the graph
                graphView.AddElement(tempNode);
            }

            /* END NODE */
            foreach (EndNodeData node in _conversationTree.EndNodeDatas)
            {
                //we already created a method in the DS_DialogueGraphView class that creates a new Node
                DS_EndNode tempNode = graphView.CreateEndNode(node.Pos);

                //set the node's GUID to the GUID we have saved in the EndNodeDatas List
                tempNode.NodeGUID = node.NodeGUID;

                //load in the end type node
                tempNode.EndNodeType = node.EndNodeType;

                //Add the node to the graph
                graphView.AddElement(tempNode);
            }

            /* EVENT NODE */
            foreach (EventNodeData node in _conversationTree.EventNodeDatas)
            {
                //we already created a method in the DS_DialogueGraphView class that creates a new Node
                DS_EventNode tempNode = graphView.CreateEventNode(node.Pos);

                //set the node's GUID to the GUID we have saved in the EventNodeDatas List
                tempNode.NodeGUID = node.NodeGUID;

                //load in the event SO
                tempNode.DialogueEvent = node.DialogueEventSO;

                //Add the node to the graph
                graphView.AddElement(tempNode);
            }

            /* DIALOGUE NODE */
            foreach (DialogueNodeData node in _conversationTree.DialogueNodeDatas)
            {
                //we already created a method in the DS_DialogueGraphView class that creates a new Node
                DS_DialogueNode tempNode = graphView.CreateDialogueNode(node.Pos);

                //set the node's GUID to the GUID we have saved in the DialogueNodeDatas List
                tempNode.NodeGUID = node.NodeGUID;

                //Dialogue Node specific loading
                tempNode.CharacterName = node.Name;
                tempNode.Texts = node.TextType;
                tempNode.SpriteImage = node.Sprite;
                tempNode.SpriteImageType = node.DialogueSpriteImageType;
                tempNode.AudioClips = node.AudioClips;

                //Add the choice ports as well
                //reason we don't override the old one instead of creating a new one is because if you delete a language we will get errors
                foreach(DialogueNodePort nodePort in node.DialogueNodePorts)
                {
                    //we already made this function in the DS_DialogueNode class
                    tempNode.AddChoicePort(tempNode, nodePort);
                }

                //Function we made in the DS_DialogueNodes. Reload new values into the fields
                //We are setting new data but it won't be visible unless we SetValueWithoutNotify (changes it visually)
                tempNode.LoadValueIntoField();

                //Add the node to the graph
                graphView.AddElement(tempNode);
            }
        }

        private void GenerateNodeConnections(DS_SO_ConversationTree _conversationTree)
        {
            //nodes are all the nodes found in our graph editor
            for(int i = 0; i < nodes.Count; i++)
            {
                //looking at the node and tell it how many edges are connected to this current node
                //Get all the edges connected to this current node
                List<NodeLinkData> connections = _conversationTree.NodeLinkDatas.Where(edge => edge.BaseNodeGUID == nodes[i].NodeGUID).ToList();

                for(int j = 0; j < connections.Count; j++)
                {
                    string targetNodeGUID = connections[j].TargetNodeGUID;

                    //Go through list of connections and find the first node that matches the target node GUID
                    //reason why we do first is because we know that there's only 1 of each node GUID, so there's no need to continue-
                    //searching if we find the first one
                    DS_BaseNode targetNode = nodes.First(node => node.NodeGUID == targetNodeGUID);

                    //With Dialogue Node Choices, it will get the first connection you made and always assign that to-
                    //the first choice. We don't want that for dialogue nodes so we have to exclude them here
                    if ((nodes[i] is DS_DialogueNode) == false)
                    {
                        //giving it the input and output, tell its connections and connect. Idk what the Q does
                        //Input container has a 0 because it only has 1 input port, so put it in the first slot
                        LinkNodesTogether(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                    }
                }

            }
            //Go into the list of all the Base Nodes and Find all the dialogueNodes, then we want to return them (Cast) as a DS_DialogueNode-
            //and make the converted nodes into a list
            List<DS_DialogueNode> dialogueNodes = nodes.FindAll(node => node is DS_DialogueNode).Cast<DS_DialogueNode>().ToList();

            foreach(DS_DialogueNode dialogueNode in dialogueNodes)
            {
                foreach(DialogueNodePort nodePort in dialogueNode.DialogueNodePorts)
                {
                    //only if the dialogue choice port has a connection we want it to connect
                    if(nodePort.InputGUID != string.Empty)
                    {
                        //find our base node
                        DS_BaseNode targetNode = nodes.First(node => node.NodeGUID == nodePort.InputGUID);

                        LinkNodesTogether(nodePort.MyPort, (Port)targetNode.inputContainer[0]);
                    }
                    
                }
            }
        }

        private void LinkNodesTogether(Port _outputPort, Port _inputPort)
        {
            Edge tempEdge = new Edge()
            {
                output = _outputPort,
                input = _inputPort,
            };

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            graphView.Add(tempEdge);

        }
        #endregion

        #region Save
        private void SaveEdges(DS_SO_ConversationTree _conversationTree)
        {
            //when we save, we want to delete all the old saves
            _conversationTree.NodeLinkDatas.Clear();

            //we only want the connected edges so we get no errors
            //looking through all the edges and if they have an input node then we add them to the array
            Edge[] connectedEdges = edges.Where(edge => edge.input.node != null).ToArray();

            for (int i = 0; i < connectedEdges.Count(); i++)
            {
                DS_BaseNode outputNode = (DS_BaseNode)connectedEdges[i].output.node;
                DS_BaseNode inputNode = (DS_BaseNode)connectedEdges[i].input.node;

                //create new node link
                _conversationTree.NodeLinkDatas.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.NodeGUID, //base node is output node it comes from
                    TargetNodeGUID = inputNode.NodeGUID //target node is the node it goes into
                });
            }
        }

        private void SaveNodes(DS_SO_ConversationTree _conversationTree)
        {
            //when we save, we want to delete all the old saves
            _conversationTree.DialogueNodeDatas.Clear();
            _conversationTree.EventNodeDatas.Clear();
            _conversationTree.StartNodeDatas.Clear();
            _conversationTree.EndNodeDatas.Clear();

            //take ALL nodes in the graph view and save the data using it's specific methods
            foreach (DS_BaseNode node in nodes)
            {
                switch (node)
                {
                    case DS_DialogueNode dialogueNode:
                        //Save the data we get
                        //go into the conversation tree, and add the node data to the list of Dialogue Nodes
                        _conversationTree.DialogueNodeDatas.Add(SaveNodeData(dialogueNode));
                        break;
                    case DS_StartNode startNode:
                        //Save the data we get
                        //go into the conversation tree, and add the node data to the list of Start Nodes
                        _conversationTree.StartNodeDatas.Add(SaveNodeData(startNode));
                        break;
                    case DS_EndNode endNode:
                        //Save the data we get
                        //go into the conversation tree, and add the node data to the list of End Nodes
                        _conversationTree.EndNodeDatas.Add(SaveNodeData(endNode));
                        break;
                    case DS_EventNode eventNode:
                        //Save the data we get
                        //go into the conversation tree, and add the node data to the list of Event Nodes
                        _conversationTree.EventNodeDatas.Add(SaveNodeData(eventNode));
                        break;
                    default:
                        break;
                }
            }

        }


        private DialogueNodeData SaveNodeData(DS_DialogueNode _dialogueNode)
        {
            DialogueNodeData dialogueNodeData = new DialogueNodeData
            {
                NodeGUID = _dialogueNode.NodeGUID,
                Pos = _dialogueNode.GetPosition().position,
                TextType = _dialogueNode.Texts,
                Name = _dialogueNode.CharacterName,
                AudioClips = _dialogueNode.AudioClips,
                DialogueSpriteImageType = _dialogueNode.SpriteImageType,
                Sprite = _dialogueNode.SpriteImage,
                DialogueNodePorts = _dialogueNode.DialogueNodePorts
            };

            //Go through each of the dialogue choice ports
            foreach (DialogueNodePort nodePort in dialogueNodeData.DialogueNodePorts)
            {
                //make sure that we set it to empty as well in the beginning. So if we remove a connection it will also be empty
                nodePort.OutputGUID = string.Empty;
                nodePort.InputGUID = string.Empty;

                //Check each edge and check which edges are actually connected to this dialogue node
                foreach (Edge edge in edges)
                {
                    if (edge.output == nodePort.MyPort)
                    {
                        //tell it which node it's connected to 
                        nodePort.OutputGUID = (edge.output.node as DS_BaseNode).NodeGUID;
                        nodePort.InputGUID = (edge.input.node as DS_BaseNode).NodeGUID;
                    }
                }
            }

            return dialogueNodeData;
        }
        private StartNodeData SaveNodeData(DS_StartNode _startNode)
        {

            StartNodeData nodeData = new StartNodeData()
            {
                NodeGUID = _startNode.NodeGUID,
                Pos = _startNode.GetPosition().position,
            };

            return nodeData;
        }

        private EndNodeData SaveNodeData(DS_EndNode _endNode)
        {
            EndNodeData nodeData = new EndNodeData()
            {
                NodeGUID = _endNode.NodeGUID,
                Pos = _endNode.GetPosition().position,
                EndNodeType = _endNode.EndNodeType
            };

            return nodeData;
        }

        private EventNodeData SaveNodeData(DS_EventNode _eventNode)
        {
            EventNodeData nodeData = new EventNodeData()
            {
                NodeGUID = _eventNode.NodeGUID,
                Pos = _eventNode.GetPosition().position,
                DialogueEventSO = _eventNode.DialogueEvent,
            };

            return nodeData;
        }
        #endregion

    }

}
