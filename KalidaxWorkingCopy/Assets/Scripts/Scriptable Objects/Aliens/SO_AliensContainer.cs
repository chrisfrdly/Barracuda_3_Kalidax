using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Aliens Container", menuName = "Data Containers/Aliens Container")]
public class SO_AliensContainer : ScriptableObject
{
    private Dictionary<string, SO_Alien> alienDatabase = new Dictionary<string, SO_Alien>();
    public Dictionary<string, SO_Alien> AlienDatabase { get => alienDatabase; set => alienDatabase = value; }

    //Create a List of strings so we can visually see the Dictionary contents
    [SerializeField] private  List<string> aliensInDatabase = new List<string>();

    //have a string field for the name of the alien
    [SerializeField] private string alienName;
    [SerializeField] private AlienTierType alienTier;


#if UNITY_EDITOR
    [ContextMenu("New Alien")]
    private void MakeNewAlien()
    {
        SO_Alien alien = ScriptableObject.CreateInstance<SO_Alien>();

        //Adding to Dictionary
        alien.name = $"[{alienTier}] {alienName}";

        alien.m_Name = alienName;
        alien.Initialize(this);
        alienDatabase.Add(alien.name, alien);

        //Adding to string list
        aliensInDatabase.Add(alien.name);

        //Making the alien a parent of this object
        AssetDatabase.AddObjectToAsset(alien, this);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(alien);
    }

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

#if UNITY_EDITOR
    //Delets all the Aliens we created under this asset
    [ContextMenu("Delete All")]
    private void DeleteAll()
    {
        var a = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/Scripts/Scriptable Objects/Aliens/Aliens Container.asset");
        Debug.Log(a.Length);
        for (int i = a.Length-1; i >= 0; i--)
        {
            //get the child and delete it
            Undo.DestroyObjectImmediate(a[i]);
        }

        alienDatabase.Clear();
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }
#endif
}

