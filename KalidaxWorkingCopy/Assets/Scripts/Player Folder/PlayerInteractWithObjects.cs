using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class PlayerInteractWithObjects : MonoBehaviour
{
    ///This will cast an overlap sphere around the player and check for any object that can be interacted with
    ///If there is an object in radius, it's interaction prompt will appear, if it's further away, it hides 


    //References
    [SerializeField] private SO_GrassTileParameters grassTileParams;
    [SerializeField] private SO_InteractableObject interactableObject;

    private PlayerMovement playerMovement;

    //Variables
    [Header("Interaction Variables")]
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask grassMask;
    [SerializeField] private LayerMask interactableMask;

    private RaycastHit2D hit;
    private Collider2D closestCollider = null;

    private float closestDistance = Mathf.Infinity;
    private float currentClosestColliderDistance;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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

        // SHOW UI WHEN ENTER THE RADIUS//
       
        //for each collider detected, check to see which one is closest and then show that UI
        foreach (Collider2D col in collider)
        {
            Debug.DrawLine(transform.position, col.transform.position);

            Vector3 directionToTarget = col.transform.position - transform.position;
            float distanceFromPlayer = directionToTarget.sqrMagnitude;

            //if we have an object that is closest to the player
            if(distanceFromPlayer < closestDistance || distanceFromPlayer < currentClosestColliderDistance)
            {
                if(col != closestCollider)
                {
                    closestCollider = col;
                    closestDistance = distanceFromPlayer;
                    closestCollider.GetComponent<InteractableObject>().ShowUI();
                }
            }

            //For any other collider, OR any collider that exits the interaction Radius, HIDE their UI
            if (col != closestCollider)
            {
                //Hide UI
                col.GetComponent<InteractableObject>().HideUI();
            }
            //Get the current closest collider's distance from the player, and if the distance is < than that, we switch it.
            //Without this variable, the closestDistance keeps getting smaller and smaller, and only resets when the current-
            //closest collider exits the interact radius
            else
                currentClosestColliderDistance = distanceFromPlayer;

        }

        // HIDE UI WHEN LEAVE RADIUS//

        foreach (Collider2D col in hideCollider)
        {
            //If they are outside the collider array, Hide the UI
            if (!collider.Contains(col))
            {
                //Hide UI
                col.GetComponent<InteractableObject>().HideUI();

                //if it's the current closest collider, we have to reset the variables or else when we enter the radius again-
                //the UI won't show again since it's the same object
                if (col == closestCollider)
                {
                    closestDistance = Mathf.Infinity;
                    closestCollider = null;
                }

            }
        }

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
        if (Input.GetMouseButtonDown(0))
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

        //Displays object interaction showing and hiding UI
        Gizmos.color = new Color(0, 0, 25, 0.1f);
        Gizmos.DrawWireSphere(transform.position, interactRadius + 0.3f);
            
    }
}
