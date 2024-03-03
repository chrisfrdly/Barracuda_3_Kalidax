using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Aliens Container", menuName = "Aliens/Aliens Container")]
public class SO_AliensContainer : ScriptableObject
{
    private int currentAlienID = 0; // when we make a new alien, give it a unique number ID

    private SO_AliensContainer thisContainer;
    private string filePath;

    //have a string field for the name of the alien
    [SerializeField] private string alienName;
    [SerializeField] private AlienTierType alienTier;

    //Create a List of strings so we can visually see the Dictionary contents
    
    [SerializeField] private string searchAlien;
    [SerializeField] private List<AlienDatabase> alienDatabase = new List<AlienDatabase>();

    //Properties
    public List<AlienDatabase> m_AlienDatabase { get => alienDatabase; set => alienDatabase = value; }
    public SO_AliensContainer m_ThisContainer { get => thisContainer; }
    public string m_FilePath { get => filePath; }
    public string m_AlienName { get => alienName; }
    public AlienTierType m_AlienTier { get => alienTier; }
    public int CurrentAlienID { get => currentAlienID; set => currentAlienID = value; }


    //public List<string> m_AliensInDatabase { get => aliensInDatabase; set => aliensInDatabase = value; }


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
    public void MakeNewAlien(string newAlienName, SO_Alien _newAlien)
    {
       
        SO_Alien alien = _newAlien;

        //Adding to Dictionary
        alien.name = newAlienName;

        alien.m_Name = _newAlien.m_Name;

        alien.Initialize(this);
        alienDatabase.Add(new AlienDatabase(newAlienName, alien));
        

        //Making the alien a parent of this object
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
    
    }
#endif
}
[System.Serializable]
public class AlienDatabase
{
    public AlienDatabase(string _alienName, SO_Alien _alien)
    {
        db_AlienName = _alienName;
        db_SO_Alien = _alien;
    }
    public string db_AlienName;
    public SO_Alien db_SO_Alien;
}