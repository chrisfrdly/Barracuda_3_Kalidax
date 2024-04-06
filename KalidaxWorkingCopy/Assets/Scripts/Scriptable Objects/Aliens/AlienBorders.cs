using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBorders : MonoBehaviour
{
    public float leftBorders;
    public float rightBorders;
    public float topBorders;
    public float bottomBorders;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        //draws a box around our camera boundary
        Gizmos.color = Color.green;

        //top boundary line
        Gizmos.DrawLine(new Vector2(leftBorders, topBorders), new Vector2(rightBorders, topBorders));
        //right boundary line
        Gizmos.DrawLine(new Vector2(rightBorders, topBorders), new Vector2(rightBorders, bottomBorders));
        //bottom boundary line
        Gizmos.DrawLine(new Vector2(rightBorders, bottomBorders), new Vector2(leftBorders, bottomBorders));
        //left boundary line
        Gizmos.DrawLine(new Vector2(leftBorders, bottomBorders), new Vector2(leftBorders, topBorders));

    }
}
