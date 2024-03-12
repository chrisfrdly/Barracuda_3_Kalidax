using DS_Editor;

using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS_Node
{
    public class DS_EndNode : DS_BaseNode
    {
        private DS_EndNodeType endNodeType = DS_EndNodeType.End;
        public DS_EndNodeType EndNodeType { get => endNodeType; set => endNodeType = value; }

        //so we can actually make the EndNodeType visible
        private EnumField enumField;

        public DS_EndNode()
        {
            //Empty so we can search for the end Node
        }

        public DS_EndNode(Vector2 position, DS_DialogueEditorWindow dialogueEditorWindow, DS_DialogueGraphView dialogueGraphView)
        {
            editorWindow = dialogueEditorWindow;
            graphView = dialogueGraphView;

            //name the End Node "End"
            title = "End";

            //Set the position and size of the node
            SetPosition(new Rect(position, defaultNodeSize));

            //Give the node it's own unique ID
            nodeGUID = Guid.NewGuid().ToString();

            //Add an input port
            AddInputPort(inputPortName, Port.Capacity.Multi);

            //create a visible enum in the node
            enumField = new EnumField()
            {
                value = endNodeType
            };

            //have to instantiate the enum
            enumField.Init(endNodeType);

            //whenever we change the value we want the value we are putting into it to make the endNode Type to the inputted one
            //Save the change we made
            enumField.RegisterValueChangedCallback((value) =>
            {
                endNodeType = (DS_EndNodeType)value.newValue;
            });

            //Sets the current value the enumField has into the event.
            enumField.SetValueWithoutNotify(endNodeType);

            //Add the enumField to the end node's container
            mainContainer.Add(enumField);

            //Add a class to this node so we can refer to it in the style sheet
            AddToClassList("endNode");
        }

        public override void LoadValueIntoField()
        {
            enumField.SetValueWithoutNotify(endNodeType);
        }
    }

}
