using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed;
    public Transform playerLocation;

    //these are the bounds for the camera
    [SerializeField]
    
    float leftBounds;
    [SerializeField]
    float rightBounds;
    [SerializeField]
    float topBounds;
    [SerializeField]
    float bottomBounds;


    public Vector3 offset;
    void Update()
    {
        Vector3 newPos = new Vector3(playerLocation.position.x, playerLocation.position.y + offset.y, -10);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBounds, rightBounds),
            Mathf.Clamp(transform.position.y, bottomBounds, topBounds),
            transform.position.z);
    }

    //this is gonna display our boundaries
    private void OnDrawGizmos()
    {
        //draws a box around our camera boundary
        Gizmos.color = Color.green;

        //top boundary line
        Gizmos.DrawLine(new Vector2(leftBounds, topBounds), new Vector2(rightBounds, topBounds));
        //right boundary line
        Gizmos.DrawLine(new Vector2(rightBounds, topBounds), new Vector2(rightBounds, bottomBounds));
        //bottom boundary line
        Gizmos.DrawLine(new Vector2(rightBounds, bottomBounds), new Vector2(leftBounds, bottomBounds));
        //left boundary line
        Gizmos.DrawLine(new Vector2(leftBounds, bottomBounds), new Vector2(leftBounds, topBounds));

    }
}
