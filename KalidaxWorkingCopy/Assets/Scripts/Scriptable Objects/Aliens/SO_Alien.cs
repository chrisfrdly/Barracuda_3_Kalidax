using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class SO_Alien : ScriptableObject
{
    [SerializeField] private SO_AliensContainer _container;

    public SO_AliensContainer m_Container { get => _container;}
   

    [SerializeField] private string _name;
    [SerializeField] private AlienTierType _alienTier;

    public string m_Name { get => _name; set => _name = value; }
    public AlienTierType m_AlienTier { get => _alienTier; set => _alienTier = value; }


#if UNITY_EDITOR
    public void Initialize(SO_AliensContainer _alienContainer)
    {
        _container = _alienContainer;
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Rename Asset")]
    private void Rename()
    {
        //Find this in the dictionary, delete it, and then create a new alien with the name
        if(_container.AlienDatabase.ContainsValue(this))
        {
            _container.AlienDatabase.Remove(this.name);
        }

        //Change the asset's name to the name we have here
        this.name = $"[{_alienTier}] {_name}";
        _container.MakeNewAlien(this.name);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Delete Asset")]
    private void DeleteAsset()
    {
        //Find this in the dictionary, delete it, and then create a new alien with the name
        if (_container.AlienDatabase.ContainsValue(this))
        {
            _container.AlienDatabase.Remove(this.name);
        }

        //Delete the asset
        Undo.DestroyObjectImmediate(this);
        AssetDatabase.SaveAssets();
    }
#endif
}

