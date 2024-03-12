using DS_Editor;

using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS_Node
{
    public class DS_EventNode : DS_BaseNode
    {
        private DS_SO_DialogueEvent dialogueEvent;
        public DS_SO_DialogueEvent DialogueEvent { get => dialogueEvent; set => dialogueEvent = value; }

        //for when you don't have that specific field in the system. There's no DialogueEvent SO field in Unity, so we're creating our own
        private ObjectField objectField;

        public DS_EventNode()
        {

        }
        public DS_EventNode(Vector2 position, DS_DialogueEditorWindow dialogueEditorWindow, DS_DialogueGraphView dialogueGraphView)
        {
            editorWindow = dialogueEditorWindow;
            graphView = dialogueGraphView;

            title = "Event";

            SetPosition(new Rect(position, defaultNodeSize));

            nodeGUID = Guid.NewGuid().ToString();

            //needs both input and output since not start or end
            AddInputPort(inputPortName, Port.Capacity.Multi);
            AddOutputPort(outputPortName, Port.Capacity.Single);

            //Creating that new Object Field to store DialogueEvents
            objectField = new ObjectField()
            {
                objectType = typeof(DS_SO_DialogueEvent),
                allowSceneObjects = false,
                value = dialogueEvent
            };

            //every time we change the value we want to save the object 
            objectField.RegisterValueChangedCallback(value =>
            {
                dialogueEvent = objectField.value as DS_SO_DialogueEvent;
            });

            //Sets the current value the objectField has into the event.
            objectField.SetValueWithoutNotify(dialogueEvent);

            //adding the object field to the event node
            mainContainer.Add(objectField);

            //Add a class to this node so we can refer to it in the style sheet
            AddToClassList("eventNode");
        }

        public override void LoadValueIntoField()
        {
            objectField.SetValueWithoutNotify(dialogueEvent);
        }
    }

}
