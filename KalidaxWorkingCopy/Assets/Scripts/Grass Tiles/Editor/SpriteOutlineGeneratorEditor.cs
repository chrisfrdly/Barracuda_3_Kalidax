
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteOutlineGenerator))]
public class SpriteOutlineGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpriteOutlineGenerator generator = (SpriteOutlineGenerator)target;

        if (GUILayout.Button("Generate Outline"))
        {
            generator.GenerateOutline();
        }
    }
}