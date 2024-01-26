using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    [Separator(2, 20)]
    [SerializeField] private List<string> aliensInDatabase = new List<string>();

    //Properties
    private Dictionary<string, SO_Alien> alienDatabase = new Dictionary<string, SO_Alien>();
    public Dictionary<string, SO_Alien> m_AlienDatabase { get => alienDatabase; set => alienDatabase = value; }
    public SO_AliensContainer m_ThisContainer { get => thisContainer; }
    public string m_FilePath { get => filePath; }
    public string m_AlienName { get => alienName; }
    public AlienTierType m_AlienTier { get => alienTier; }
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
        #if UNITY_EDITOR
        filePath = AssetDatabase.GetAssetPath(this);
        #endif
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