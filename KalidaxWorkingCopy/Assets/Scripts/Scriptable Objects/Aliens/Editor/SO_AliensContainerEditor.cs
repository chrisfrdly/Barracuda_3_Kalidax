

using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SO_AliensContainer))]
public class SO_AliensContainerEditor : Editor
{
    private SerializedProperty alienName;
    private SerializedProperty alienTier;
    private SerializedProperty alienDatabase;
    private SerializedProperty searchAlien;

    private readonly float databaseHeight = 40;
    private readonly float databaseOffset = 5;

    Vector2 scrollPos;
    SO_Alien a;
    float extraYLogWarning = 0; //if there's a warning, I want to draw the AlienDatabase line lower, so this
                                //value will be added to the Y if there's a warning

    //Taken From the AlienTierTypeDrawer
    Color32[] tierTypeColours = new Color32[]
    {
        new Color32(220,220,240,255),   //Tier 1
        new Color32(51,153,255,255),    //Tier 2
        new Color32(0, 255, 153, 255),  //Tier 3
        new Color32(255,204,0,255)      //Tier 4
    };

    private void OnEnable()
    {
        alienName = serializedObject.FindProperty("alienName");
        alienTier = serializedObject.FindProperty("alienTier");
        alienDatabase = serializedObject.FindProperty("alienDatabase");
        searchAlien = serializedObject.FindProperty("searchAlien");


    }
    public override void OnInspectorGUI()
    {

        SO_AliensContainer aliens = (SO_AliensContainer)target;

        //Updates the Inspector if we make a change
        serializedObject.UpdateIfRequiredOrScript();

        if (aliens == null) return;

        GUILayout.Space(30f);

        //base.OnInspectorGUI();


        //-- Adding A Custom Title --//
        GUIStyle headStyle = new GUIStyle();
        headStyle.fontSize = 30;
        headStyle.normal.textColor = Color.white;
        headStyle.fontStyle = FontStyle.Bold;
        headStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField("Alien Creation \n Machine", headStyle);

        GUILayout.Space(40f);



        //-- Alien Information --//
        EditorGUILayout.PropertyField(alienName);
        EditorGUILayout.PropertyField(alienTier);

        GUILayout.Space(10f);

        //loop through the database and see if there's an alien with the same name and tier
        //If so, then log a warning
        for (int i = 0; i < aliens.m_AlienDatabase.Count; i++)
        {
            a = aliens.m_AlienDatabase[i].db_SO_Alien;

            if (a.name == $"[{(AlienTierType)alienTier.enumValueIndex}] {alienName.stringValue}")
            {

                EditorGUILayout.HelpBox($"An alien named {a.name} already exists.", MessageType.Warning);
                extraYLogWarning = 40;
            }
            else
            {
                extraYLogWarning = 0;
            }

        }

        //-- Create Alien Button --//
        var style1 = new GUIStyle(GUI.skin.button);
        if(extraYLogWarning > 0)
        {
            style1.normal.textColor = Color.yellow;
            style1.active.textColor = Color.yellow;
            style1.hover.textColor = new Color(1, 0.756f, 0.027f);
        }
        else
        {
            style1.normal.textColor = Color.green;
            style1.active.textColor = Color.green;
            style1.hover.textColor = new Color(0, 0.8f, 0);
        }
        
        

        if (GUILayout.Button("Create New Alien", style1, GUILayout.Height(40)))
        {
            CreateNewAlien(aliens);
        }

        

        GUILayout.Space(10f);


        float width = Screen.width - 40;

        // ALIEN DATABASE //
        GUIStyle header = new GUIStyle();
        header.fontSize = 20;
        header.fontStyle = FontStyle.Bold;
        header.normal.textColor = Color.white;

        GUILayout.Label("Alien Database", header);

        //Separator Line
        Rect separatorLine = new Rect((Screen.width / 2) - (width / 2), 220 + extraYLogWarning, width, 1);
        EditorGUI.DrawRect(separatorLine, Color.white);


        GUILayout.Space(10);

        //Label width makes the field box closer to the searchAlien Text to the left
        EditorGUIUtility.labelWidth = 90;
        EditorGUILayout.PropertyField(searchAlien);

        //Over here, I want to first create a rect for where the database should be

        Rect alienDatabaseRect = new Rect((Screen.width / 2) - (width / 2), 250, width, databaseHeight);

        //create strings for the searching of the alien
        string alienDatabaseName;
        string inputtedText = searchAlien.stringValue;
        int order = 0;


        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Height(200));

        for (int i = 0; i < alienDatabase.arraySize; i++)
        {
            //I need to add an empty GUILayout for the scrollbar to work
            GUILayout.Label("", GUILayout.Height(databaseHeight + (databaseOffset * 1.9f)));



            order++;
            alienDatabaseName = aliens.m_AlienDatabase[i].db_AlienName;

            #region SEARCH ALIEN LOGIC
            //See if "searchAlien" != null and if not, check the name of the alien if it contains letters
            if (searchAlien.stringValue != null)
            {

                //store the strings as lowercase strings
                string temp = alienDatabaseName.ToLower();
                string temp2 = inputtedText.ToLower();

                //see if the alien's name contains the inputted text
                if (!temp.Contains(temp2))
                {
                    order--;
                    continue;
                }


            }
            #endregion

            //Background Rectangle

            Rect databaseElement = new Rect(0, ((databaseHeight + databaseOffset) * order), alienDatabaseRect.width, alienDatabaseRect.height);
            Rect alienTextureRect = new Rect(databaseElement.x, databaseElement.y, databaseHeight, databaseHeight);
            Rect IDLabel = new Rect(alienTextureRect.x + alienTextureRect.width + 20, alienTextureRect.y + databaseElement.height / 4, databaseElement.width, databaseElement.height);
            Rect alienNameLabel = new Rect(IDLabel.x + 60, IDLabel.y, IDLabel.width, IDLabel.height);

            //Background image
            EditorGUI.DrawRect(databaseElement, new Color(0.165f, 0.165f, 0.165f));


            a = aliens.m_AlienDatabase[i].db_SO_Alien;

            //ID
            GUIStyle tierColour = new GUIStyle();
            tierColour.fontSize = 14;
            tierColour.normal.textColor = tierTypeColours[Mathf.Clamp((int)a.m_AlienTier, 0, tierTypeColours.Length - 1)];

            EditorGUILayout.BeginHorizontal();
            EditorGUI.LabelField(IDLabel, new GUIContent($"ID: {a.m_AlienID}"), tierColour);

            EditorGUI.LabelField(alienNameLabel, new GUIContent($"Alien: {alienDatabaseName}"), tierColour);
            EditorGUILayout.EndHorizontal();


            //Draw a Texture2D Rect

            EditorGUI.DrawRect(alienTextureRect, Color.black);

            if (a.m_AlienSprite != null)
            {
               GUI.DrawTexture(alienTextureRect, a.m_AlienTexture);
              
            }
          
          

        }

        EditorGUILayout.EndScrollView();


        //EditorGUILayout.PropertyField(aliensInDatabase);



        GUILayout.Space(10);

        // ORGANIZE BUTTONS //
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Organize By Tier"))
        {
            OrganizeDatabaseByTier(aliens);
        }
        if (GUILayout.Button("Organize By ID"))
        {
            OrganizeDatabaseByID(aliens);
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);

        //-- Delete All ALiens Button --//
        var style = new GUIStyle(EditorStyles.toolbarButton);
        style.normal.textColor = Color.red;
        style.active.textColor = Color.red;
        style.hover.textColor = new Color(0.8f, 0, 0);

        if (GUILayout.Button("Delete All Aliens", style))
        {
            DeleteAllAliens(aliens);
        }

        //If we made a change, apply it
        serializedObject.ApplyModifiedProperties();

    }

    private void OrganizeDatabaseByTier(SO_AliensContainer _aliens)
    {
        //Start end
        
        for (int i = 0; i < _aliens.m_AlienDatabase.Count; i++)
        {
            for (int j = 0; j < _aliens.m_AlienDatabase.Count; j++)
            {
                //Compares this alien's tier to the other aliens, and if it's less than the other, move swap it
                if ((_aliens.m_AlienDatabase[i].db_SO_Alien.m_AlienTier < _aliens.m_AlienDatabase[j].db_SO_Alien.m_AlienTier))
                {

                    //performing the swap for the database
                    AlienDatabase temp = _aliens.m_AlienDatabase[i];

                    _aliens.m_AlienDatabase[i] = _aliens.m_AlienDatabase[j];
                   
                    _aliens.m_AlienDatabase[j] = temp;

                }
            }
        }

    }
    private void OrganizeDatabaseByID(SO_AliensContainer _aliens)
    {
        //Start end

        for (int i = 0; i < _aliens.m_AlienDatabase.Count; i++)
        {
            for (int j = 0; j < _aliens.m_AlienDatabase.Count; j++)
            {
                //Compares this alien's tier to the other aliens, and if it's less than the other, move swap it
                if ((_aliens.m_AlienDatabase[i].db_SO_Alien.m_AlienID < _aliens.m_AlienDatabase[j].db_SO_Alien.m_AlienID))
                {

                    //performing the swap for the database
                    AlienDatabase temp = _aliens.m_AlienDatabase[i];

                    _aliens.m_AlienDatabase[i] = _aliens.m_AlienDatabase[j];

                    _aliens.m_AlienDatabase[j] = temp;

                }
            }
        }

    }

    private void CreateNewAlien(SO_AliensContainer _aliens)
    {
        string alienName = $"[{_aliens.m_AlienTier}] {_aliens.m_AlienName}";

        //First consult the database to see if this alien already exists. If so, then return
        for(int i = 0; i < _aliens.m_AlienDatabase.Count; i++) 
        {
            string strAlienName = _aliens.m_AlienDatabase[i].db_AlienName;

            if (strAlienName.Equals(alienName))
            {
                Debug.Log($"An Alien named '{alienName}' already exists!");
                return;
            }
        }

        SO_Alien alien = ScriptableObject.CreateInstance<SO_Alien>();

        //Adding to Dictionary
        alien.name = alienName;
        
        alien.m_Name = _aliens.m_AlienName;
        alien.m_AlienTier = _aliens.m_AlienTier;
        alien.m_AlienID = _aliens.CurrentAlienID;


        alien.Initialize(_aliens.m_ThisContainer);

        //Adding the Alien to the Alien Database
        _aliens.m_AlienDatabase.Add(new AlienDatabase(alienName, alien));

        //Making the alien a parent of this object
        AssetDatabase.AddObjectToAsset(alien, _aliens.m_ThisContainer);

        _aliens.CurrentAlienID++;


        EditorUtility.SetDirty(_aliens.m_ThisContainer);
        
        EditorUtility.SetDirty(alien);
        AssetDatabase.SaveAssets();
        serializedObject.ApplyModifiedProperties();
    }



    //Delets all the Aliens we created under this asset
    private void DeleteAllAliens(SO_AliensContainer _aliens)
    {
        _aliens.CurrentAlienID = 0;

        Undo.RecordObject(_aliens, "Deleting Aleins");
        var a = AssetDatabase.LoadAllAssetRepresentationsAtPath(_aliens.m_FilePath);

        _aliens.m_AlienDatabase.Clear();

        Undo.RecordObjects(a, "Aliens In Database");

        for (int i = a.Length - 1; i >= 0; i--)
        {
            //get the child and delete it
            Undo.DestroyObjectImmediate(a[i]);
        }

        
        EditorUtility.SetDirty(_aliens.m_ThisContainer);
        AssetDatabase.SaveAssets();

    }
}