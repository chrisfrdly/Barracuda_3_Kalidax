using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class SpriteOutlineGenerator : MonoBehaviour
{
    //Find a pre-existing line renderer if it already exists
    //delete it

    //if null, add component to this game object

    //set the positions to the length of the collider

    //in a for loop, go through all the corners and input their positions
    [SerializeField] private Collider2D objectToOutline;
    [SerializeField] private Material outlineMaterial;
    private LineRenderer lr;

    public void GenerateOutline()
    {
        //Check to see if we already have a line renderer on the object
        lr = GetComponent<LineRenderer>();

        //if not, add one
        if(lr == null)
        {
            lr = gameObject.AddComponent<LineRenderer>();
        }

        CreateOutline();

    }

    private void CreateOutline()
    {
        //A physicsShapeGroup is essentially all the shpes that make up a collider2D
        PhysicsShapeGroup2D physicsShapeGroup = new PhysicsShapeGroup2D();

        objectToOutline.GetShapes(physicsShapeGroup);

        //Set input data
        lr.loop = true;
        lr.startWidth = 0.08333284f;

        lr.material = outlineMaterial;
        lr.sortingOrder = 1;
        lr.useWorldSpace = false;

        lr.generateLightingData = false;
        lr.shadowBias = 0;
        lr.receiveShadows = false;

        lr.positionCount = physicsShapeGroup.vertexCount;
        
        //now loop through the positions inputting the vertex data from the collider
        for (int i = 0; i < physicsShapeGroup.vertexCount; i++)
        {
            Vector2 vertexData = physicsShapeGroup.GetShapeVertex(0, i);
            lr.SetPosition(i, vertexData);
        }

        //now tell Unity to save the prefab once complete
        EditorUtility.SetDirty(objectToOutline.gameObject);
    }

}

[CustomEditor(typeof(SpriteOutlineGenerator))]  
public class SpriteOutlineGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpriteOutlineGenerator generator = (SpriteOutlineGenerator)target;  

        if(GUILayout.Button("Generate Outline"))
        {
            generator.GenerateOutline();
        }
    }
}
