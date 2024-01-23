using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.InputSystem;

public class PlayerInteractWithObjects : MonoBehaviour
{
    ///This will cast an overlap sphere around the player and check for any object that can be interacted with
    ///If there is an object in radius, it's interaction prompt will appear, if it's further away, it hides 


    //References
    [SerializeField] private SO_GrassTileParameters grassTileParams;
    [SerializeField] private SO_InteractableObject interactableObject;

    private PlayerMovement playerMovement;
    private PlayerInputHandler playerInputHandler;

    //Variables
    [Header("Interaction Variables")]
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask grassMask;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private float angleToLookAtTarget = 50f;

    private RaycastHit2D hit;
    private Collider2D closestCollider = null;



    private float closestDistance = Mathf.Infinity;
    private float currentClosestColliderDistance;
    private float closestAngle = 180;
    private float currentClosestColliderAngle;

    private Transform objTransform;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInputHandler = PlayerInputHandler.Instance;
        objTransform = transform;
    }

    private void Update()
    {
        if (playerInputHandler.m_PlayerInput.actions["Interact"].WasPressedThisFrame())
            interactableObject.ClickedInteractButtonEventSend();

        InteractObjects();
        RaycastGrassTile();
    }

    #region Interacting With Objects
    private void InteractObjects()
    {
        //overlap sphere around player, 1 for showing the UI, and the other to Hide it after it left the interaction radius
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableMask);
        Collider2D[] hideCollider = Physics2D.OverlapCircleAll(transform.position, interactRadius + 0.3f, interactableMask);

        //To save computational power, just don't do anything if there's nothing to detect
        if (collider.Length == 0 && hideCollider.Length == 0) return;

        //for each collider detected, check to see which one is closest and then show that UI
        foreach (Collider2D col in collider)
        {
            InteractableObject io = col.GetComponent<InteractableObject>();
            Vector3 colPos = col.transform.position;

            //if we have an object that is closest to the player
            Vector3 directionToTarget = colPos - objTransform.position;
            float distanceFromPlayer = directionToTarget.sqrMagnitude;

            // COLLIDER WITH CLOSEST ANGLE //
            if (io.IsRequiredToLookAtTarget())
            {

                float a = GetAngleBetweenObjects(colPos);

                if (a < angleToLookAtTarget)
                {
                    if (a < closestAngle || a < currentClosestColliderAngle)
                    {

                        if (col != closestCollider)
                        {
                            closestCollider = col;
                            closestAngle = a;
                            io.PlayerInRange(a);
                        }
                    }
                }
                else
                {
                    //If it's still the closest collider BUT the angle isn't in threshold anymore, HIDE UI
                    //Without this code the panel will still be shown since they are still in the interaction sphere and the UI isn't hidden until they leave it
                    if (col == closestCollider)
                    {
                        closestDistance = Mathf.Infinity;
                        closestCollider = null;
                        closestAngle = 180;
                        io.PlayerOutOfRange();

                    }


                }
            }

            // COLLIDER WITH CLOSEST DISTANCE //
            else
            {
                if (distanceFromPlayer < closestDistance || distanceFromPlayer < currentClosestColliderDistance)
                {
                    if (col != closestCollider)
                    {
                        closestCollider = col;
                        closestDistance = distanceFromPlayer;
                        io.PlayerInRange(0);

                    }
                }

            }



            //For any other collider, OR any collider that exits the interaction Radius, HIDE their UI
            if (col != closestCollider)
            {
                if (io.InPlayerRange)
                {
                    //Hide UI
                    io.PlayerOutOfRange();

                }

            }
            //Get the current closest collider's distance from the player, and if the distance is < than that, we switch it.
            //Without this variable, the closestDistance keeps getting smaller and smaller, and only resets when the current-
            //closest collider exits the interact radius
            else
            {
                currentClosestColliderDistance = distanceFromPlayer;
                currentClosestColliderAngle = GetAngleBetweenObjects(colPos);

            }

        }

        // HIDE UI WHEN LEAVE RADIUS//

        foreach (Collider2D col in hideCollider)
        {
            //If they are outside the collider array, Hide the UI
            if (!collider.Contains(col))
            {
                //Hide UI
                InteractableObject io = col.GetComponent<InteractableObject>();

                if (io.InPlayerRange)
                    io.PlayerOutOfRange();

                //if it's the current closest collider, we have to reset the variables or else when we enter the radius again-
                //the UI won't show again since it's the same object
                if (col == closestCollider)
                {
                    closestDistance = Mathf.Infinity;
                    closestCollider = null;
                    closestAngle = 180;
                }

            }
        }

    }


    private float GetAngleBetweenObjects(Vector3 otherPos)
    {
        //Help from this post https://forum.unity.com/threads/finding-the-angle-between-a-direction-and-a-point.30639/
        Vector3 targetDir = otherPos - objTransform.position;
        Vector3 forwards = objTransform.forward;

        targetDir.Normalize();
        forwards.Normalize();

        Debug.DrawLine(objTransform.position, objTransform.position + targetDir * 5); //player to target line
        Debug.DrawLine(objTransform.position, objTransform.position + forwards * 5, Color.red); //player look ahead line

        float dot = Vector3.Dot(targetDir, forwards);
        float angleXZ = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return angleXZ;
    }

    #endregion

    #region Grass Tile
    //Creates a raycast in the player's forward direction and if the player clicks AND it detects a grass tile, brak it
    private void RaycastGrassTile()
    {
        Vector2 directionFacing = playerMovement.m_DirectionFacing;

        //Create the raycast

        hit = Physics2D.CircleCast(transform.position, 0.5f, directionFacing, interactRadius - 0.5f, grassMask);

        if(hit.collider != null)
            Debug.DrawLine(transform.position, hit.transform.position, Color.blue);

        //if we did NOT detect a grass tile, return
        if (!hit)
            return;

        //check to see if the player clicked the mouse button. Update with new input system
        if (playerInputHandler.m_PlayerInput.actions["BreakGrass"].WasPressedThisFrame() || 
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Get the transform that we interacted with
            Transform grassTransform = hit.transform;

            //Break Grass Tile
            grassTileParams.BreakGrassEventSend(grassTransform);
        }


    }


    #endregion

    private void OnDrawGizmos()
    {
        //Displays the interaction radius around the player
        Gizmos.color = new Color(0, 255, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, interactRadius);

        Gizmos.color = Color.white;
        if (hit.collider != null)
            Gizmos.DrawWireCube(hit.collider.transform.position, Vector3.one);

        //make closest collider white
        if (closestCollider != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(closestCollider.transform.position, 0.6f);
        }

        //Displays object interaction showing and hiding UI
        Gizmos.color = new Color(0, 0, 25, 0.1f);
        Gizmos.DrawWireSphere(transform.position, interactRadius + 0.3f);
            
    }
}
