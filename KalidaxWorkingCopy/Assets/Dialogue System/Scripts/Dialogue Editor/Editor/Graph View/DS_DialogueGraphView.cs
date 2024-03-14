using DS_Node;

using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS_Editor
{
    public class DS_DialogueGraphView : GraphView
    {
        //so we don't have to write the name of the style sheet over again
        static readonly string c_styleSheetsName = "DS_GraphViewStyleSheet";

        //Reference to the Editor Window
        private DS_DialogueEditorWindow editorWindow;

        private DS_NodeSearchWindow searchWindow;

        //Constructor. Called from the DS_DialogueEditorWindow Class
        public DS_DialogueGraphView(DS_DialogueEditorWindow _dialogueEditorWindow)
        {
            //get the editor window
            editorWindow = _dialogueEditorWindow;

            //Get the stylesheet for the graph view
            StyleSheet styleSheet = Resources.Load<StyleSheet>(c_styleSheetsName);

            //in graph view, we just add stylesheet by below. We don't need to do the rootVisualElement.Add
            styleSheets.Add(styleSheet);

            //Add the functionality tot he graph view
            GraphViewFunctions();
        }

        private void GraphViewFunctions()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());

            this.AddManipulator(new SelectionDragger());

            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(new FreehandSelector());

            //adding a grid background and inserting it into the editor window
            GridBackground grid = new GridBackground();
            Insert(0, grid);

            //no matter how big we make our editor window, the graph view will scale to be same size
            grid.StretchToParentSize();

            //Adding the search window
            AddSearchWindow();

        }

        private void AddSearchWindow()
        {
            //creating new version of search window
            searchWindow = ScriptableObject.CreateInstance<DS_NodeSearchWindow>();

            //setting up initial references for the search window
            searchWindow.Configure(editorWindow, this);

            //if the user presses space or right clicks and clicks "Create Node", a new search window will open
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }


        /// <summary>
        /// We are now going to set rules as to which ports the input an output edges can and cannot connect to
        /// Ports are found on every node and can be one of two types: Input and Output ports
        ///    
        /// Input Ports - Are the circles at the left of the node and allow for other nodes to connect to this node
        /// Output Ports - Are the circles on the right of the node and are used to connect this node to other nodes.
        /// Edge - Is the connection line that goes between ports
        /// </summary>
        public override List<Port> GetCompatiblePorts(Port _startPort, NodeAdapter _nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            Port startPortView = _startPort;

            //another way of doing a foreach loop with a list of ports in the graph view
            ports.ForEach((port) =>
            {
                Port portView = port;

                bool cantConnectToSelf = startPortView != portView;
                bool cantConnectToSameNodeInputOutput = startPortView.node != portView.node;
                bool cantConnectToSameDirection = startPortView.direction != port.direction;
                
                //if we want to connect a port to another port, make sure it matches these criteria
                if (cantConnectToSelf && cantConnectToSameNodeInputOutput && cantConnectToSameDirection)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        public void LanguageReload()
        {
            //go through each of the languages and dialogue nodes
            //first say we want all the nodes in the graph view as a list
            //then we go through the list and say we want ONLY the dialogue nodes. We have to cast them since they don't return as a dialogue node
            //then just get all the dialogue nodes and put them in the list
            List<DS_DialogueNode> dialogueNodes = nodes.ToList().Where(node => node is DS_DialogueNode).Cast<DS_DialogueNode>().ToList();

            foreach(DS_DialogueNode dialogueNode in dialogueNodes)
            {
                //go through all the dialogue nodes and reload the language
                dialogueNode.ReloadLanguage();
            }
        }

        //Creating the Nodes
        public DS_StartNode CreateStartNode(Vector2 _pos)
        {
            DS_StartNode tmp = new DS_StartNode(_pos, editorWindow, this);
            return tmp;
        }
        public DS_DialogueNode CreateDialogueNode(Vector2 _pos)
        {
            DS_DialogueNode tmp = new DS_DialogueNode(_pos, editorWindow, this);
            return tmp;
        }
        public DS_EventNode CreateEventNode(Vector2 _pos)
        {
            DS_EventNode tmp = new DS_EventNode(_pos, editorWindow, this);
            return tmp;
        }
        public DS_EndNode CreateEndNode(Vector2 _pos)
        {
            DS_EndNode tmp = new DS_EndNode(_pos, editorWindow, this);
            return tmp;
        }


    }

    
}
