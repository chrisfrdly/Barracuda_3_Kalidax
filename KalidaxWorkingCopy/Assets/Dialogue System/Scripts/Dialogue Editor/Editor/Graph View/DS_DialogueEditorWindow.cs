using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace DS_Editor
{
    using DS_SaveAndLoad;
   

    public class DS_DialogueEditorWindow : EditorWindow
    {
        /* REFERENCES */

        //Knows which dialogue for saving and loading
        private DS_SO_ConversationTree currentConversationTree;
        private DS_DialogueGraphView graphView;
        private DS_SaveAndLoad saveAndLoad;

        //Store a reference of the current Language at all times
        private DS_LanguageType languageType = DS_LanguageType.English;
        public DS_LanguageType LanguageType { get => languageType; set => languageType = value; }

        /* COMPONENTS */

        //Toolbar. Will be referenced later on
        private ToolbarMenu toolbarMenu;
        private Label nameOfConversationTree;

        //Variables
        static readonly string c_graphStyleName = "DS_GraphViewStyleSheet";
        static readonly string c_containerNameClass = "DialogueContainerName";

        //Docked Window Variables
        private readonly float dockedPanelWidthStart = 300f;
        private float toolbarHeight;
        private Rect panelRect;
        private float panelWidth;
        private GUIStyle panelStyle;
        private GUIStyle panelTitleStyle;
        private Rect panelResizerRect;
         


        [OnOpenAsset(1)]
        public static bool ShowWindow(int instanceID, int line)
        {
            UnityEngine.Object item = EditorUtility.InstanceIDToObject(instanceID);

            //if the item we have clicked in the project is a Conversation Tree SO, we want to load that into the dialogue editor
            if(item is DS_SO_ConversationTree)
            {
                DS_DialogueEditorWindow window = (DS_DialogueEditorWindow)GetWindow(typeof(DS_DialogueEditorWindow));

                //name of the dialogue window
                window.titleContent = new GUIContent("Dialogue Editor");

                //want to read the window as a Conversation Tree
                window.currentConversationTree = item as DS_SO_ConversationTree;

                //set a minimum window size
                window.minSize = new Vector2(500, 250);

                //Loading in the data from it
                window.Load();

            }

            //if click something that's not a Conversation Tree in the Project, we shouldn't open any window
            return false;
        }

        //when the window opens up
        private void OnEnable()
        {
            ConstructGraphView();

            GenerateToolbar();

            panelWidth = dockedPanelWidthStart;

            InitializeStyles();

        }

        //when the window closes down
        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }

        #region Editor Tools
        private void ConstructGraphView()
        {
            //Add graph view
            graphView = new DS_DialogueGraphView(this);

            //stretch the graph to the editor window
            graphView.StretchToParentSize();

            //Add the graph view to the editor window
            rootVisualElement.Add(graphView);

            saveAndLoad = new DS_SaveAndLoad(graphView);

        }
        private void GenerateToolbar()
        {
            //Add a toolbar
            Toolbar toolbar = new Toolbar();

            //Add the style sheet for the toolbar (text is not in the middle, but hugging the top
            StyleSheet styleSheet = Resources.Load<StyleSheet>(c_graphStyleName);
            rootVisualElement.styleSheets.Add(styleSheet);


            /* SAVE BUTTON */
            Button saveBtn = new Button()
            {
                text = "Save"
            };
            saveBtn.clicked += () =>
            {
                Save();
            };
            toolbar.Add(saveBtn);

            /* LOAD BUTTON */
            Button loadBtn = new Button()
            {
                text = "Load"
            };
            loadBtn.clicked += () =>
            {
                Load();
            };
            toolbar.Add(loadBtn);

            /* LANGUAGE DROPDOWN */
            //Dropdown Menu for Language. Not a dropdown, but this works as an alternative
            toolbarMenu = new ToolbarMenu();

            //go through each language to display them
            Array languages = DS_LanguageType.GetValues(typeof(DS_LanguageType));
            foreach (DS_LanguageType language in languages)
            {
                toolbarMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction> (x => Language(language, toolbarMenu)));
            }
            

            //add it into the toolbar
            toolbar.Add(toolbarMenu);

            /* NAME OF CURRENT DIALOGUE CONTAINER YOU HAVED OPPENED */
            nameOfConversationTree = new Label("");
            toolbar.Add(nameOfConversationTree);

            //giving the name a class (like HTML/CSS classes)
            nameOfConversationTree.AddToClassList(c_containerNameClass);

            //Add the toolbar to the Editor
            rootVisualElement.Add(toolbar);

            toolbarHeight = Screen.height;
        }

        private void InitializeStyles()
        {
            panelStyle = new GUIStyle();

            panelStyle.normal.background = CreateTexture(5, 5, Color.black);
        }
        private void GenerateDockedWindow()
        {
            //start a panel on the right side and below the toolbar, and extend it the screen's height
            panelRect = new Rect(200, 200, 1000, Screen.height - toolbarHeight);

            GUI.BeginGroup(panelRect);

            GUILayout.BeginArea(panelRect, panelStyle);

            GUILayout.EndArea();

            GUI.EndGroup();
            
        }
        private void OnGUI()
        {
           
            GenerateDockedWindow();
        }
        #endregion

        #region Graph Features
        private void Save()
        {
            //save current Graph and make sure we have a conversation tree selected.
            //currentDialogueContainer = current conversation tree we are editing in the project folder
            if (currentConversationTree == null)
                return;
          
            saveAndLoad.Save(currentConversationTree);
            Debug.Log("Graph Saved");
        }

        private void Load()
        {
            //load graph and set the name of the editor toolbar to the SO's name
            if (currentConversationTree == null)
                return;

            toolbarMenu.text = $"Language:  {DS_LanguageType.English.ToString()}" ;
            nameOfConversationTree.text = $"Name: {currentConversationTree.name}";

            saveAndLoad.Load(currentConversationTree);
            Debug.Log("Graph Loaded");
        }

        //needs to know which language you're changing too, as well as the toolbar
        private void Language(DS_LanguageType _language, ToolbarMenu _toolbarMenu)
        {
            _toolbarMenu.text = $"Language:  {_language.ToString()}";

            languageType = _language;

            //when we actually click the language, then we reload all the languages
            graphView.LanguageReload();

        }
        #endregion


        public static void ExitGUI()
        {
            
            throw new ExitGUIException();
        }

        public Texture2D CreateTexture(int width, int height, Color colour)
        {
            Texture2D texture = new Texture2D(width, height);

            //now set the colour of each pixel in the texture
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    texture.SetPixel(i, j, colour);
                }
            }

            return texture;
        }
    }


}
