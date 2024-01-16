using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Aliens Container", menuName = "Aliens/Aliens Container")]
public class SO_AliensContainer : ScriptableObject
{

    private SO_AliensContainer thisContainer;
    private string filePath;

    //have a string field for the name of the alien
    [SerializeField] private string alienName;
    [SerializeField] private AlienTierType alienTier;

    //Create a List of strings so we can visually see the Dictionary contents
    [Separator(2,20)]
    [SerializeField] private List<string> aliensInDatabase = new List<string>();

    //Properties
    private Dictionary<string, SO_Alien> alienDatabase = new Dictionary<string, SO_Alien>();
    public Dictionary<string, SO_Alien> m_AlienDatabase { get => alienDatabase; set => alienDatabase = value; }
    public SO_AliensContainer m_ThisContainer { get => thisContainer; }
    public string m_FilePath { get => filePath; }
    public string m_AlienName { get => alienName;}
    public AlienTierType m_AlienTier { get => alienTier;}
    public List<string> m_AliensInDatabase { get => aliensInDatabase; set => aliensInDatabase = value; }

    private void OnEnable()
    {
        InitializeContainer();
    }

    private void OnValidate()
    {
        InitializeContainer();
    }

    private void InitializeContainer()
    {
        //Initialize
        thisContainer = this;
        filePath = AssetDatabase.GetAssetPath(this);
    }


#if UNITY_EDITOR
    //make a new alien from the SO_Alien class (when we want to change the name inside the dictionary without deleting the asset)
    public void MakeNewAlien(string newAlienName)
    {
        SO_Alien alien = ScriptableObject.CreateInstance<SO_Alien>();

        //Adding to Dictionary
        alien.name = newAlienName;

        alien.m_Name = newAlienName;
        alien.Initialize(this);
        alienDatabase.Add(alien.name, alien);

        //Adding to string list
        aliensInDatabase.Add(alien.name);

        //Making the alien a parent of this object
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
    }
#endif

}
[CustomEditor(typeof(SO_AliensContainer))]
public class SO_AliensContainerEditor : Editor
{
    private SerializedProperty alienName;
    private SerializedProperty alienTier;
    private SerializedProperty aliensInDatabase;

    private void OnEnable()
    {
        alienName = serializedObject.FindProperty("alienName");
        alienTier = serializedObject.FindProperty("alienTier");
        aliensInDatabase = serializedObject.FindProperty("aliensInDatabase");

        
    }
    public override void OnInspectorGUI()
    {

        SO_AliensContainer aliens = (SO_AliensContainer)target;

        //Updates the Inspector if we make a change
        serializedObject.UpdateIfRequiredOrScript();

        if (aliens.IsDestroyed()) return;
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


        //-- Create Alien Button --//
        var style1 = new GUIStyle(GUI.skin.button);
        style1.normal.textColor = Color.green;
        style1.active.textColor = Color.green;
        style1.hover.textColor = new Color(0,0.8f,0);  
        
        if (GUILayout.Button("Create New Alien",style1,GUILayout.Height(40)))
        {
            CreateNewAlien(aliens);
        }

        GUILayout.Space(10f);

        EditorGUILayout.PropertyField(aliensInDatabase);


        //-- Delete All ALiens Button --//
        var style = new GUIStyle(EditorStyles.toolbarButton);
        style.normal.textColor = Color.red;
        style.active.textColor = Color.red;
        style.hover.textColor = new Color(0.8f,0,0);

        if (GUILayout.Button("Delete All Aliens",style))
        {
            DeleteAllAliens(aliens);
        }



        //If we made a change, apply it
        serializedObject.ApplyModifiedProperties();

    }


    private void CreateNewAlien(SO_AliensContainer _aliens)
    {
        SO_Alien alien = ScriptableObject.CreateInstance<SO_Alien>();

        //Adding to Dictionary
        alien.name = $"[{_aliens.m_AlienTier}] {_aliens.m_AlienName}";

        alien.m_Name = _aliens.m_AlienName;
        alien.m_AlienTier = _aliens.m_AlienTier;
        alien.Initialize(_aliens.m_ThisContainer);
        _aliens.m_AlienDatabase.Add(alien.name, alien);

        //Adding to string list
        _aliens.m_AliensInDatabase.Add(alien.name);

        //Making the alien a parent of this object
        AssetDatabase.AddObjectToAsset(alien, _aliens.m_ThisContainer);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(_aliens.m_ThisContainer);
        EditorUtility.SetDirty(alien);
    }



    //Delets all the Aliens we created under this asset
    private void DeleteAllAliens(SO_AliensContainer _aliens)
    {
        var a = AssetDatabase.LoadAllAssetRepresentationsAtPath(_aliens.m_FilePath);
        
        for (int i = a.Length - 1; i >= 0; i--)
        {
            //get the child and delete it
            Undo.DestroyObjectImmediate(a[i]);
        }

        _aliens.m_AlienDatabase.Clear();
        _aliens.m_AliensInDatabase.Clear();

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(_aliens.m_ThisContainer);

        Debug.Log("Deleted All Aliens");

    }
}