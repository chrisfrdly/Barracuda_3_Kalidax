
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
        style1.hover.textColor = new Color(0, 0.8f, 0);

        if (GUILayout.Button("Create New Alien", style1, GUILayout.Height(40)))
        {
            CreateNewAlien(aliens);
        }

        GUILayout.Space(10f);

        EditorGUILayout.PropertyField(aliensInDatabase);


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