using DS_Editor;

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS_Node
{
    public class DS_BaseNode : Node
    {
        //Id made with string so we can tell the different nodes appart 
        protected string nodeGUID;
        public string NodeGUID { get => nodeGUID; set => nodeGUID = value; }

        protected DS_DialogueGraphView graphView;
        protected DS_DialogueEditorWindow editorWindow;
        protected Vector2 defaultNodeSize;

        protected static readonly string inputPortName = "";
        protected static readonly string outputPortName = "";
        //Constructor
        public DS_BaseNode()
        {
            //call the node styleSheet for formatting the Node
            StyleSheet styleSheet = Resources.Load<StyleSheet>("DS_NodeStyleSheet");
            styleSheets.Add(styleSheet);
        }

        #region Port Functions

        public Port GetPortInstance(Direction _nodeDirection, Port.Capacity _capacity = Port.Capacity.Single)
        {
            //not really using typeof float. Can transfer different data to different nodes.
            return InstantiatePort(Orientation.Horizontal, _nodeDirection, _capacity, typeof(float));
        }

        //output port can only have 1 output. Can't drag to multiple
        public void AddOutputPort(string _name, Port.Capacity _capacity = Port.Capacity.Single)
        {
            //Make the port, tell what type it is. Direction Output means it will be on the right side of the node
            //Capacity means it can only have 1 connection
            Port outputPort = GetPortInstance(Direction.Output, _capacity);

            //Give the output port a name
            outputPort.portName = _name;

            //Add it to the outputContainer
            outputContainer.Add(outputPort);

            //Adding the name so we can edit it in the css properties
            outputContainer.name = "output";
        }

        public void AddInputPort(string _name, Port.Capacity _capacity = Port.Capacity.Multi) 
        {
            //Make the port, tell what type it is. Direction Input means it will be on the Left side of the node
            //Capacity multi means that the input port will be able to take in multiple different node ports.
            Port inputPort = GetPortInstance(Direction.Input, _capacity);

            //Give the input port a name
            inputPort.portName = _name;

            //Add it to the inputContainer
            inputContainer.Add(inputPort);

            //Adding the name so we can edit it in the css properties (I want it to share the same properties as the output port)
            inputContainer.name = "output";
        }

        #endregion

        public virtual void LoadValueIntoField()
        {
            //load all the data into the field
        }

    }

}
