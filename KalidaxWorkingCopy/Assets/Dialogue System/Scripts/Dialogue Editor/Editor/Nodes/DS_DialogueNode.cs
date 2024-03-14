using DS_Editor;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS_Node
{
    public class DS_DialogueNode : DS_BaseNode
    {
        //we need languages for this.
        //Can't use a dictionary because they are non-serializable
        //work around in the DS_ConversationTree script

        private List<LanguageGeneric<string>> texts = new List<LanguageGeneric<string>>();
        private List<LanguageGeneric<AudioClip>> audioClips = new List<LanguageGeneric<AudioClip>>();
        private Sprite spriteImage;
        private string characterName = ""; //so we know who is speaking
        private DS_DialogueSpriteImageType spriteImageType;

        private List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();

        public List<LanguageGeneric<string>> Texts { get => texts; set => texts = value; }
        public List<LanguageGeneric<AudioClip>> AudioClips { get => audioClips; set => audioClips = value; }
        public Sprite SpriteImage { get => spriteImage; set => spriteImage = value; }
        public string CharacterName { get => characterName; set => characterName = value; }
        public DS_DialogueSpriteImageType SpriteImageType { get => spriteImageType; set => spriteImageType = value; }
        public List<DialogueNodePort> DialogueNodePorts { get => dialogueNodePorts; set => dialogueNodePorts = value; }

        private TextField texts_Field;
        private ObjectField audioClips_Field;
        private ObjectField spriteImage_Field;
        private Image spriteImage_Preview;
        private TextField name_Field;
        private EnumField spriteImageType_Field;

        private DS_LanguageType languagesType;
        public DS_DialogueNode()
        {

        }

        public DS_DialogueNode(Vector2 _position, DS_DialogueEditorWindow _dialogueEditorWindow, DS_DialogueGraphView _dialogueGraphView)
        {
            base.editorWindow = _dialogueEditorWindow;
            base.graphView = _dialogueGraphView;

            //name of node
            title = "Dialogue";

            SetPosition(new Rect(_position, defaultNodeSize));

            //making a unique string ID for this node. Will be used for saving and loading reference
            nodeGUID = Guid.NewGuid().ToString();

            //Add an input port for the dialogue node
            AddInputPort(inputPortName, Port.Capacity.Multi);

            //we want to get all the values of the language type. Make sure that we want it to be cast as an array
            //we want the enum language and want it returned as an array

            languagesType = editorWindow.LanguageType;

            Array languages = (DS_LanguageType[])Enum.GetValues(typeof(DS_LanguageType));
            foreach (DS_LanguageType language in languages)
            {
                texts.Add(new LanguageGeneric<string>
                {
                    LanguageType = language,
                    LanguageGenericType = ""
                });

                //reason why null is because we need to specify it as something else errors.
                audioClips.Add(new LanguageGeneric<AudioClip>
                {
                    LanguageType = language,
                    LanguageGenericType = null
                });
            }

            //order of these methods determine how node will look hierarchically
            AddNameField();

            AddSpriteImageField();

            AddSpriteImageTypeField();

            AddTextField();

            AddAudioClipField();

            AddButtonField();

            //Add a class to this node so we can refer to it in the style sheet
            AddToClassList("dialogueNode");
        }



        #region Adding Fields to the main container



        private void AddSpriteImageField()
        {
            spriteImage_Field = new ObjectField
            {
                objectType = typeof(Sprite), //we're telling it what object we're working with. For this, a sprite
                allowSceneObjects = false, //if you have objects in the scene, you cannot link them to this. Has to be from project folder
                value = spriteImage //the variable we're affecting
            };

            spriteImage_Preview = new Image();
            spriteImage_Preview.AddToClassList("spriteImagePreview");
            spriteImage_Preview.transform.scale = new Vector3(-1, 1, 1);

            //when we change the image, make sure to save it in the spriteImage variable
            spriteImage_Field.RegisterValueChangedCallback(value =>
            {
                Sprite tmp = value.newValue as Sprite;
                spriteImage = tmp;

                if (tmp != null)
                    spriteImage_Preview.image = tmp.texture;
                else
                    spriteImage_Preview.image = null;

            });

            //adding the sprite field to the dialogue node
            mainContainer.Add(spriteImage_Field);
            mainContainer.Add(spriteImage_Preview);

            spriteImage_Preview.AddToClassList("spriteImage");
            spriteImage_Field.AddToClassList("spriteImage");
        }



        private void AddSpriteImageTypeField()
        {
            //for setting the direction of the sprite to face
            spriteImageType_Field = new EnumField()
            {
                value = spriteImageType
            };

            //we have to initialize an enum field at the beginning and give it the type spriteImageType
            spriteImageType_Field.Init(spriteImageType);

            //the number it's getting from the value we must convert to the enum (DS_DialogueSpriteImageType)
            //if Left, 1, if Right, 2 ... 
            spriteImageType_Field.RegisterValueChangedCallback(value =>
            {
                DS_DialogueSpriteImageType tmp = (DS_DialogueSpriteImageType)value.newValue;
                spriteImageType = tmp;

                if (tmp == DS_DialogueSpriteImageType.Right)
                    spriteImage_Preview.transform.scale = new Vector3(1, 1, 1);
                else
                    spriteImage_Preview.transform.scale = new Vector3(-1, 1, 1);

            });

            //adding the face type to the dialogue node
            mainContainer.Add(spriteImageType_Field);
            
            spriteImageType_Field.AddToClassList("spriteImage");
        }



        private void AddNameField()
        {
            //A label is like the [Header()] with variables. It says "Name" and then underneat it is an empt text box
            Label labelName = new Label("Name");
            labelName.AddToClassList("labelName");
            labelName.AddToClassList("label");

            mainContainer.Add(labelName);

            name_Field = new TextField("");

            //lunar expression so when we input a value, it sets the 'characterName' variable to the new value inputted
            name_Field.RegisterValueChangedCallback(value =>
            {
                characterName = value.newValue;
            });

            //now we're updating the name textfield with the value we inputted
            name_Field.SetValueWithoutNotify(characterName);

            //if we want to customize it's CSS properties, we give it a class name
            name_Field.AddToClassList("TextName");

            //Add the name field to the dialogue node
            mainContainer.Add(name_Field);

        }



        private void AddTextField()
        {
            //a label is like the [Header()] with variables. It says "Dialogue Text" and then underneat it is an empt text box
            Label labelName = new Label("Dialogue Text");
            labelName.AddToClassList("labelDialogueText");
            labelName.AddToClassList("label");
            mainContainer.Add(labelName);

            //now we need fields on the dialogue node for the text, image type, audio, etc.
            texts_Field = new TextField("");

            //when a new value is registered in the text field
            texts_Field.RegisterValueChangedCallback(value =>
            {
                //if we're working in english in the editor window, it will take the list (texts) from the foreach loop, find english, and 
                //plot the new text in
                //if we change the dialogue text and we're in French, then it will change the French text
                texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
            });

            //if the editor window's text is set to english, then we'll automatically load in the english dialogue text from the start
            texts_Field.SetValueWithoutNotify(texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType);

            //Can make multiple lines with ENTER. If it's false then we'll just have one long answer
            texts_Field.multiline = true;

            //adds a CSS class to this text field so we can apply some properties to make it look better
            texts_Field.AddToClassList("TextBox");

            //Add the text Field into the main dialogue Node
            mainContainer.Add(texts_Field);
        }



        private void AddAudioClipField()
        {
            audioClips_Field = new ObjectField()
            {
                objectType = typeof(AudioClip), //setting the object field to only allow audio Clips
                allowSceneObjects = false, //can only add Audio Clips from the project folders and not the scene objects
                value = audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType
                //for the value, it is the variable we are affecting in this objectField. In this case, it's the audio clip of the Language
                //we're affecting in the editor window
            };

            //when we change the value, make sure to save it in the variable
            audioClips_Field.RegisterValueChangedCallback(value =>
            {
                //checking through all the audio clips in audioClips List and if it matches the language we're affecting in the editor,
                //we\re changing that audio clip with the new audio clip inputted 
                audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip;
            });

            //when we load in the graph, we want to check the current language we're using and set the audioClip to that one.
            audioClips_Field.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType);

            //add the audio clip field to the dialogue node
            mainContainer.Add(audioClips_Field);
        }



        private void AddButtonField()
        {
            Button button = new Button()
            {
                text = "Add Choice"
            };

            button.clicked += () =>
            {
                AddChoicePort(this);
            };

            //make button appear in the titleButtonContainer
            titleButtonContainer.Add(button);
        }


        #endregion

        //Loading node
        public override void LoadValueIntoField()
        {
            //call all the fields and load the values in the fields
            //more of a refresh method. When we load in data, we call all this to see what we loaded in
            texts_Field.SetValueWithoutNotify(texts.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);

            audioClips_Field.SetValueWithoutNotify(audioClips.Find(language => language.LanguageType == editorWindow.LanguageType).LanguageGenericType);

            spriteImage_Field.SetValueWithoutNotify(spriteImage);

            spriteImageType_Field.SetValueWithoutNotify(spriteImageType);

            name_Field.SetValueWithoutNotify(characterName);

            if(spriteImage != null)
            {
                //First cast the object field as a sprite, and we get the value in the field
                //Then we want to get that sprite's texture.
                spriteImage_Preview.image = ((Sprite)spriteImage_Field.value).texture;
            }
        }


        //every time we are changing the language we want to change all the text
        public void ReloadLanguage()
        {
            /* RELOADING THE TEXT */
            

            //when a new value is registered in any of thetext field when we change the language in the editor window
            texts_Field.RegisterValueChangedCallback(value =>
            {
                //if we're working in english in the editor window, it will take the list (texts) from the foreach loop, find english, and 
                //if we change the dialogue text and we're in French, then it will change the French text
                texts.Find(text => text.LanguageType == languagesType).LanguageGenericType = value.newValue;
            });

            //if the editor window's text is set to english, then we'll automatically load in the english dialogue text from the start
            texts_Field.SetValueWithoutNotify(texts.Find(text => text.LanguageType == languagesType).LanguageGenericType);

            /* RELOADING THE AUDIO CLIP */

            //when we change the value, make sure to save it in the variable
            audioClips_Field.RegisterValueChangedCallback(value =>
            {
                //checking through all the audio clips in audioClips List and if it matches the language we're affecting in the editor,
                //we\re changing that audio clip with the new audio clip inputted 
                audioClips.Find(audioClip => audioClip.LanguageType == languagesType).LanguageGenericType = value.newValue as AudioClip;
            });

            //when we load in the graph, we want to check the current language we're using and set the audioClip to that one.
            audioClips_Field.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == languagesType).LanguageGenericType);

            foreach (DialogueNodePort nodePort in dialogueNodePorts)
            {
                nodePort.TextField.RegisterValueChangedCallback(value =>
                {
                    nodePort.TextLanguages.Find(language => language.LanguageType == languagesType).LanguageGenericType = value.newValue;
                });

                nodePort.TextField.SetValueWithoutNotify(nodePort.TextLanguages.Find(language => language.LanguageType == languagesType).LanguageGenericType);
            }
        }

        //for saving and loading, can use this method too
        //when we make a new one it will be empty, however if it's not a new one we will use the Dialogue Node port we give it instead
        public Port AddChoicePort(DS_BaseNode _baseNode, DialogueNodePort _dialogueNodePort = null)
        {
            //we're making an output port
            Port port = GetPortInstance(Direction.Output);

            //will go in the outputContainer and check how many output ports we have
            int outputPortCount = _baseNode.outputContainer.Query("connector").ToList().Count();

            //starts at 0 so we add 1
            string outputPortName = $"Choice {outputPortCount + 1}";

            DialogueNodePort newChoiceNodePort = new DialogueNodePort();

            //make all the language generics and add them into our newDialogueNodePort
            //this makes it support the multiple languages for each port
            foreach (DS_LanguageType language in (DS_LanguageType[])Enum.GetValues(typeof(DS_LanguageType)))
            {
                newChoiceNodePort.TextLanguages.Add(new LanguageGeneric<string>()
                {
                    LanguageType = language,

                    //in the beginning it's just choice 1. But now we can check to see if something's added
                    LanguageGenericType = outputPortName
                });
            }

            //if the node we're loading in isn't empty, it's going to put the correct language's text in.
            if (_dialogueNodePort != null)
            {
                //if the node Port isn't empty, we want to tell it which inputs and outputs it has
                newChoiceNodePort.InputGUID = _dialogueNodePort.InputGUID;
                newChoiceNodePort.OutputGUID = _dialogueNodePort.OutputGUID;

                foreach (LanguageGeneric<string> languageGeneric in _dialogueNodePort.TextLanguages)
                {
                    newChoiceNodePort.TextLanguages.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
                }
            }

            //now we need to make the textfield itself in the dialogue node field
            newChoiceNodePort.TextField = new TextField();

            //if the text field port is modified, save the changes using the current editor's language
            //saving it in the language list
            newChoiceNodePort.TextField.RegisterValueChangedCallback(value =>
            {
                //check what language it is before saving it. If the editor window is english, then we want to save it in the english List
                newChoiceNodePort.TextLanguages.Find(language => language.LanguageType == languagesType).LanguageGenericType = value.newValue;
            });

            //setting the text field in the graph editor to the new dialogue that we just saved above
            newChoiceNodePort.TextField.SetValueWithoutNotify(newChoiceNodePort.TextLanguages.Find(language => language.LanguageType == languagesType).LanguageGenericType);

            //adding the new choice to the current container
            port.contentContainer.Add(newChoiceNodePort.TextField);


            //Delete Button for new choices
            //add OnClick Event for the button (DeletePort)
            //telling it which node it belongs to and which port to delete
            Button deleteChoiceButton = new Button(() => DeletePort(_baseNode, port))
            {
                text = "X"
            };

            //adding the Delete button to the current container
            port.contentContainer.Add(deleteChoiceButton);

            //also saving the current Port into myPort
            //so we know which port to delete since each port has a unique connection
            newChoiceNodePort.MyPort = port;

            //giving it no name
            port.portName = "";

            //add it to the dialogue node's list of output ports
            dialogueNodePorts.Add(newChoiceNodePort);

            //add the new Choice Node to the Node's output container
            _baseNode.outputContainer.Add(port);

            //Refresh the base node so the changes show up
            _baseNode.RefreshPorts();
            _baseNode.RefreshExpandedState();

            return port;
        }

        private void DeletePort(DS_BaseNode _node, Port _port)
        {
            //find out which dialogue node port we're working with
            //will find correct dialogue node port so we can get all the information
            DialogueNodePort tmp = dialogueNodePorts.Find(port => port.MyPort == _port);

            dialogueNodePorts.Remove(tmp);

            //find all connecting edges to the output port so we can delete those too
            //going into the graph view, finding all the edges in the graph view and putting them in a list
            //telling it we only want the edges that are the same as the port's edge to remove
            IEnumerable<Edge> portEdge = graphView.edges.ToList().Where(edge => edge.output == _port);

            //check to see if there are any edges connected to this port
            if (portEdge.Any())
            {
                //find the matching edge
                Edge edge = portEdge.First();

                //disconnecting the edge
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);

                //removing the edge
                graphView.RemoveElement(edge);
            }

            //actually removing it visually from the node
            _node.outputContainer.Remove(_port);

            //Refresh the base node so the deleted port no longer shows up
            _node.RefreshPorts();
            _node.RefreshExpandedState();
        }
    }

}
