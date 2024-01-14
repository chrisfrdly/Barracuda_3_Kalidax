using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif


public class SO_Alien : ScriptableObject
{
 
    private SO_AliensContainer _container;
    private SO_Alien _thisAlien;


    [SerializeField] private string _name;
    [SerializeField] private AlienTierType _alienTier;

    [Separator(1, 10)]
    [Header("Parent Aliens")]
    [SerializeField] private SO_Alien alien1;
    [SerializeField] private SO_Alien alien2;

    [Separator(1, 10)]
    [SerializeField] private List<int> sellingPrice = new List<int>();

    [Separator(1, 10)]
    [SerializeField] private AlienArt alienArt;

    //Properties
    public SO_AliensContainer m_Container { get => _container; }
    public SO_Alien m_ThisAlien { get => _thisAlien;}
    public string m_Name { get => _name; set => _name = value; }
    public AlienTierType m_AlienTier { get => _alienTier; set => _alienTier = value; }
    

#if UNITY_EDITOR
    public void Initialize(SO_AliensContainer _alienContainer)
    {
        _container = _alienContainer;
        _thisAlien = this;
        
        //Adding the days to the list
        for(int i = 0; i < 15; i++)
        {
            sellingPrice.Add(0);
        }
    }
#endif
}

[System.Serializable]
public class AlienArt
{
    public Animator idleAnim;
    public Animator wallkingAnim;
    public Sprite alienSprite;
}

[CustomEditor(typeof(SO_Alien))]
public class SO_AlienEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        GUILayout.Space(20f);

        SO_Alien alien = (SO_Alien)target;

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Rename Alien"))
        {
            RenameAlien(alien);
        }
        if (GUILayout.Button("Delete Alien"))
        {
            DeleteAlien(alien);
        }
        GUILayout.EndHorizontal();
    }
    private void RenameAlien(SO_Alien _alien)
    {
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
        //Find this in the dictionary, delete it, and then create a new alien with the name
        if (_alien.m_Container.m_AlienDatabase.ContainsValue(_alien.m_ThisAlien))
        {
            _alien.m_Container.m_AlienDatabase.Remove(_alien.m_ThisAlien.name);
        }

        //Delete the asset
        Undo.DestroyObjectImmediate(_alien.m_ThisAlien);
        AssetDatabase.SaveAssets();
    }

   
}