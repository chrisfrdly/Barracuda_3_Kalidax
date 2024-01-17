using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine.UIElements;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif


public class SO_Alien : ScriptableObject
{
 
    private SO_AliensContainer _container;
    private SO_Alien _thisAlien;

    //[Title("Alien Information",TextAlignment.Center)]
    [SerializeField] private string _name;
    [SerializeField] private AlienTierType _alienTier;

    [Separator(1, 10)]
    [Header("Parent Aliens")]
    [SerializeField] private SO_Alien alien1;
    [SerializeField] private SO_Alien alien2;

    [Separator(1, 10)]
    [Header("Cost and Sell Values")]
    [SerializeField] private int costValue;
    [SerializeField] private int sellValue;

    [Header("Days to Grow Alien")]
    [SerializeField] private int minDaysToGrow;
    [SerializeField] private int maxDaysToGrow;

    [Separator(1, 10)]
    [SerializeField] private AlienArt alienArt;
    [SerializeField] private Sprite alienSprite;
    [SerializeField] private Texture2D alienTexture;

    //Properties
    public SO_AliensContainer m_Container { get => _container; set => _container = value; }
    public SO_Alien m_ThisAlien { get => _thisAlien;}
    public string m_Name { get => _name; set => _name = value; }
    public AlienTierType m_AlienTier { get => _alienTier; set => _alienTier = value; }
    public Sprite m_AlienSprite { get => alienSprite;}
    public Texture2D m_AlienTexture { get => alienTexture; set => alienTexture = value; }


#if UNITY_EDITOR
    public void Initialize(SO_AliensContainer _alienContainer)
    {
        _container = _alienContainer;

        _thisAlien = this;
      
    }
#endif

    private void OnEnable()
    {
        //need to set this every time or else we get a nullreference error
        _thisAlien = this;
    }
}

[System.Serializable]
public class AlienArt
{
    public Animator idleAnim;
    public Animator wallkingAnim;
    
}

[CustomEditor(typeof(SO_Alien))]
public class SO_AlienEditor : Editor
{
    //instead of accessing variables through target, we want to access their serialize properties

    private SerializedProperty _name;
    private SerializedProperty _alienTier;

    private SerializedProperty minDaysToGrow;
    private SerializedProperty maxDaysToGrow;

    private SerializedProperty alien1;
    private SerializedProperty alien2;

    private SerializedProperty costValue;
    private SerializedProperty sellValue;
    private SerializedProperty alienArt;
    private SerializedProperty alienSprite;
    private SerializedProperty alienTexture;

    private void OnEnable()
    {
        _name = serializedObject.FindProperty("_name");
        _alienTier = serializedObject.FindProperty("_alienTier");

        minDaysToGrow = serializedObject.FindProperty("minDaysToGrow");
        maxDaysToGrow = serializedObject.FindProperty("maxDaysToGrow");

        alien1 = serializedObject.FindProperty("alien1");
        alien2 = serializedObject.FindProperty("alien2");

        costValue = serializedObject.FindProperty("costValue");
        sellValue = serializedObject.FindProperty("sellValue");

        alienArt = serializedObject.FindProperty("alienArt");
        alienSprite = serializedObject.FindProperty("alienSprite");
        alienTexture = serializedObject.FindProperty("alienTexture");

        //Without this code, we would get null reference errors since the SO_Aliens's reference to the parent's container was null
        //when opening the project again, so we need to make sure to get the parent asset and set this script's container to it.

        //Get this asset
        SO_Alien alien = (SO_Alien)target;

        //Get the Parent
        var a = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(alien));

        //Convert the Parent into a variable so we can get the variable from the script
        SO_AliensContainer aliens = (SO_AliensContainer)a;

        //Set this container to the parent's container
        alien.m_Container = aliens.m_ThisContainer;
        
    }

    public override void OnInspectorGUI()
    {

        SO_Alien alien = (SO_Alien)target;

       
        //Updates the Inspector if we make a change
        serializedObject.UpdateIfRequiredOrScript();

        if (alien.IsDestroyed()) return;
        if (alien == null) return;
        //base.OnInspectorGUI();


        //-- ALIEN Information Title --//
        GUILayout.Space(20f);
        GUIStyle headStyle = new GUIStyle();
        headStyle.fontSize = 30;
        headStyle.normal.textColor = Color.white;
        headStyle.fontStyle = FontStyle.Bold;
        headStyle.alignment = TextAnchor.MiddleCenter;
        string alienName = $"[{_alienTier.enumDisplayNames[_alienTier.enumValueIndex]}] {_name.stringValue}";
        EditorGUILayout.LabelField(alienName, headStyle);

        GUILayout.Space(30f);




        //-- ALIEN INFORMATION - NAME, TIER, SPRITE --//
        GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();

                //Create Texture
                Rect text = new Rect(20, 80, 75, 75);
                EditorGUI.DrawRect(text, Color.black);

                if(alien.m_AlienSprite != null)
                {
                    alien.m_AlienTexture = alien.m_AlienSprite.texture;

                    GUI.DrawTexture(text, alien.m_AlienTexture);
                }
                    
            GUILayout.EndVertical();

            GUILayout.BeginVertical();  
                
                    EditorStyles.label.alignment = TextAnchor.MiddleRight;
                    EditorGUILayout.PropertyField(_name, new GUIContent("Name  "));
                    EditorGUILayout.PropertyField(_alienTier, new GUIContent("Tier  "));
                    EditorGUILayout.PropertyField(alienSprite, new GUIContent("Sprite  "));
                    EditorStyles.label.alignment = TextAnchor.MiddleLeft;

     
            GUILayout.EndVertical();
      
        GUILayout.EndHorizontal();

        GUILayout.Space(40f);

      


        //-- ALIEN RENAME BUTTONS --//
        if (alien.IsDestroyed()) return;
        if (alien == null) return;

        GUIStyle deleteStyle = new GUIStyle(EditorStyles.toolbarButton);
        deleteStyle.normal.textColor = Color.red;
        deleteStyle.active.textColor = Color.red;
        deleteStyle.hover.textColor = new Color(0.7f, 0 , 0);

        bool shouldRename = !alienName.Equals(alien.m_ThisAlien.name);
        GUIStyle renameStyle = new GUIStyle(GUI.skin.button);

        if(!shouldRename)
        {
            renameStyle.normal.textColor = Color.white;
            renameStyle.active.textColor = Color.white;
            renameStyle.hover.textColor = Color.white;
        }
        else
        {
            renameStyle.normal.textColor = Color.yellow;
            renameStyle.active.textColor = Color.yellow;
            renameStyle.hover.textColor = new Color(255f/255f, 215f / 255f, 0);
        }
        

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Rename Alien", renameStyle))
        {
            RenameAlien(alien);
        }
        if (GUILayout.Button("Delete Alien", deleteStyle))
        {
            DeleteAlien(alien);
        }
        GUILayout.EndHorizontal();




        //-- HERLPER BOX TO SUGGEST THAT THE NAMES OF THE ALIEN AND ASSET SHOULD BE THE SAME--//
        if (alien.IsDestroyed()) return;
        if (alien == null) return;
        if (shouldRename)
        {
            renameStyle.normal.textColor = Color.yellow;
            renameStyle.active.textColor = Color.yellow;
            renameStyle.hover.textColor = new Color(255f / 255f, 215 / 255f, 0);
            EditorGUILayout.HelpBox("The name of the Alien and the Asset do not match. " +
                "Please Press the 'Rename Alien' Button, or Update the 'name' and 'tier' fields " +
                "above to match them!", MessageType.Warning);
        }



        //-- REVEAL / HIDE PARENT ALIENS IF A TIER 1 --//
        if (_alienTier.enumValueIndex != 0)
        {
            EditorGUILayout.PropertyField(alien1);
            EditorGUILayout.PropertyField(alien2);

            //Check to see if the alien's tiers are the same
            if(alien1.objectReferenceValue != null && alien2.objectReferenceValue != null)
            {
                SO_Alien a1 = (SO_Alien)alien1.objectReferenceValue;
                SO_Alien a2 = (SO_Alien)alien2.objectReferenceValue;

                int tier1 = (int)a1.m_AlienTier;
                int tier2 = (int)a2.m_AlienTier;

                if (tier1 != tier2)
                {
                    EditorGUILayout.HelpBox("The Parent Aliens are not of the same Tier. " +
                                            "Please make sure they are the same Tier. ", MessageType.Warning);
                }
            }
            

        }



        //-- OTHER FIELDS --//




        EditorGUILayout.PropertyField(costValue, new GUIContent("Cost Value ($)"));
        EditorGUILayout.PropertyField(sellValue, new GUIContent("Sell Value ($)"));


        EditorGUILayout.PropertyField(minDaysToGrow, new GUIContent("Min Days"));
        EditorGUILayout.PropertyField(maxDaysToGrow, new GUIContent("Max Days"));

        EditorGUILayout.Space(10);

        EditorGUILayout.PropertyField(alienArt);



        //If we made a change, apply it
        serializedObject.ApplyModifiedProperties();
    }



    private void RenameAlien(SO_Alien _alien)
    {
        RemoveAlienInVisualDatabase(_alien);

        //Find this in the dictionary, delete it, and then create a new alien with the name
        if (_alien.m_Container.m_AlienDatabase.ContainsValue(_alien.m_ThisAlien))
        {
            _alien.m_Container.m_AlienDatabase.Remove(_alien.m_ThisAlien.name);
        }

       
        //Change the asset's name to the name we have here
        _alien.m_ThisAlien.name = $"[{_alien.m_AlienTier}] {_alien.m_Name}";
        _alien.m_Container.MakeNewAlien(_alien.m_ThisAlien.name);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(_alien.m_ThisAlien);
    }

    private void DeleteAlien(SO_Alien _alien)
    {
        RemoveAlienInVisualDatabase(_alien);

        //Find this in the dictionary, delete it, and then create a new alien with the name
        if (_alien.m_Container.m_AlienDatabase.ContainsValue(_alien.m_ThisAlien))
        {
            _alien.m_Container.m_AlienDatabase.Remove(_alien.m_ThisAlien.name);
        }

        //Delete the asset
        Undo.DestroyObjectImmediate(_alien.m_ThisAlien);
        AssetDatabase.SaveAssets();
    }

    private void RemoveAlienInVisualDatabase(SO_Alien _alien)
    {
        for (int i = 0; i < _alien.m_Container.m_AliensInDatabase.Count; i++)
        {
            //Check to see if there's a match, if so, remove it from the list
            if (_alien.m_Container.m_AliensInDatabase[i].Equals(_alien.m_ThisAlien.name))
            {
                _alien.m_Container.m_AliensInDatabase.Remove(_alien.m_Container.m_AliensInDatabase[i]);
            }
        }
    }
   
}