using DS_Node;

using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS_Editor
{
    public class DS_NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        //reference to editor window
        private DS_DialogueEditorWindow dialogueEditorWindow;
        private DS_DialogueGraphView dialogueGraphView;

        //for creating a space before the Node Names
        private Texture2D pic;

        //variables
        static readonly string c_searchTreeTitle = "Dialogue System";
        static readonly string c_startNodeName = "Start node";
        static readonly string c_dialogueNodeName = "Dialogue node";
        static readonly string c_eventNodeName = "Event node";
        static readonly string c_endNodeName = "End node";


        //initial setup of references
        public void Configure(DS_DialogueEditorWindow _editorWindow, DS_DialogueGraphView _dialogueGraphView)
        {
            dialogueEditorWindow = _editorWindow;
            dialogueGraphView = _dialogueGraphView;

            //Creating a small square
            pic = new Texture2D(1, 1);
            pic.SetPixel(0, 0, new Color(0, 0, 0, 0));
            pic.Apply();
        }


        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>()
            {
                //header of the search window
                new SearchTreeGroupEntry (new GUIContent(c_searchTreeTitle),1),

                //Adding the different nodes to the list when you Right Click. If you click on one it opens corresponding script
                AddNodeSearch(c_startNodeName, new DS_StartNode()),

                AddNodeSearch(c_dialogueNodeName, new DS_DialogueNode()),

                AddNodeSearch(c_eventNodeName, new DS_EventNode()),

                AddNodeSearch(c_endNodeName, new DS_EndNode()),

            };

            return tree;
        }

        private SearchTreeEntry AddNodeSearch(string _name, DS_BaseNode _baseNode)
        {
            //Add the picture before we put the name so it appears as intented
            SearchTreeEntry tmp = new SearchTreeEntry(new GUIContent(_name, pic))
            {
                level = 2,
                userData = _baseNode
            };

            return tmp;
        }

        public bool OnSelectEntry(SearchTreeEntry _SearchTreeEntry, SearchWindowContext _context)
        {
            //when we select a node we want to return the thing we actually selected. Going into the editor window
            Vector2 mousePosition = dialogueEditorWindow.rootVisualElement.ChangeCoordinatesTo
            (
                //we want the position of the editor window AND the position of the mouse - the editor window's position
                //where the mouse is on the screen
                dialogueEditorWindow.rootVisualElement.parent, _context.screenMousePosition - dialogueEditorWindow.position.position
            );

            Vector2 graphMousePosition = dialogueGraphView.contentViewContainer.WorldToLocal(mousePosition);

            return CheckForNodeType(_SearchTreeEntry, graphMousePosition);
        }

        //now we actually need to add it to graph view
        private bool CheckForNodeType(SearchTreeEntry _searchTreeEntry, Vector2 _pos)
        {
            switch(_searchTreeEntry.userData)
            {
                case DS_StartNode node:
                    dialogueGraphView.AddElement(dialogueGraphView.CreateStartNode(_pos));
                    return true;

                case DS_DialogueNode node:
                    dialogueGraphView.AddElement(dialogueGraphView.CreateDialogueNode(_pos));
                    return true;

                case DS_EventNode node:
                    dialogueGraphView.AddElement(dialogueGraphView.CreateEventNode(_pos));
                    return true;

                case DS_EndNode node:
                    dialogueGraphView.AddElement(dialogueGraphView.CreateEndNode(_pos));
                    return true;

                default:
                    break;

            }

            return false;
        }
    }

}
