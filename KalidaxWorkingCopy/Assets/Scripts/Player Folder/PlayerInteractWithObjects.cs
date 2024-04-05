using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

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


    private RaycastHit2D hit;
    [SerializeField] private Collider2D closestCollider = null;
    private GameObject prevGrassTile = null;


    private Collider2D[] colliders = new Collider2D[5];
    private int numOfColliders;

    private InteractableObject closestIO;
    private Transform objTransform;
    private float closestDistance = Mathf.Infinity;

    private Color[] debugLineColours =
    {
        Color.red,
        Color.green,
        Color.cyan,
        Color.magenta
    };
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInputHandler = PlayerInputHandler.Instance;
        objTransform = transform;
    }

    private void Update()
    {
        //if (playerInputHandler.m_PlayerInput.actions["Interact"].WasPressedThisFrame())
        //    interactableObject.ClickedInteractButtonEventSend();

        //InteractObjects();

        bool pressedInteract = playerInputHandler.m_PlayerInput.actions["Interact"].WasPressedThisFrame();

        if (pressedInteract && closestIO != null)
        {
            interactableObject.ClickedInteractButtonEventSend();
            
            closestIO.OnInteract(gameObject);
            
        }


        InteractObjects();
        RaycastGrassTile();
    }

    #region Interacting With Objects
    private void InteractObjects()
    {
        //overlap sphere around player, 1 for showing the UI, and the other to Hide it after it left the interaction radius
        numOfColliders = Physics2D.OverlapCircleNonAlloc(objTransform.position, interactRadius, colliders, interactableMask);

        if (numOfColliders == 0)
        {
            if (closestIO != null)
            {
                closestIO.OnPlayerExitRange();

                closestIO = null;
                closestCollider = null;
                closestDistance = Mathf.Infinity;
            }
            return;
        }

        

        // -- GET THE CLOSEST COLLIDER -- //

        //for each collider detected, check to see which one is closest and then show that UI
        for (int i = 0; i < numOfColliders; i++)
        {
            Collider2D col = colliders[i];
            InteractableObject io = col.GetComponent<InteractableObject>();

            //Getting distance to player
            Vector3 colPos = col.transform.position;
            Vector3 directionToTarget = colPos - objTransform.position;
            float distanceFromPlayer = directionToTarget.sqrMagnitude;

            
            //if this collider is the closest one to the player AND it's not already the closest...
            if (distanceFromPlayer < closestDistance - 0.1f && closestCollider != col)
            {
                
                //if there was a collider before, then Hide it's UI and say it's out of range
                if (closestCollider != null && closestIO != null)
                {
                    closestIO.OnPlayerExitRange();
                }

                //then set this collider as the closest
                closestCollider = col;
            }


             Debug.DrawLine(transform.position, col.transform.position, debugLineColours[i]);
 
            
        }
        


        if (closestCollider == null) return;
        


        // -- NOW ACTIVATE THE CLOSEST COLLIDER -- //

        //Get distance to closest collider
        Vector3 directionToClosest = closestCollider.transform.position - objTransform.position;
        float distanceFromClosest = directionToClosest.sqrMagnitude;

        //Now update the closest Distance for the closest collider's distance

        closestDistance = distanceFromClosest;

        //Get the Script from the closest collider so we can have reference to it
        closestIO = closestCollider.GetComponent<InteractableObject>();

       
        //Check to see if we're already showing the UI and if not, then show it
        if (closestIO != null && !closestIO.m_InPlayerRange && closestIO.CheckIsInteractable())
        {
            closestIO.OnPlayerEnterRange(0);
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
        {
            Debug.DrawLine(transform.position, hit.transform.position, Color.blue);
            if (prevGrassTile != null) prevGrassTile.SetActive(false);
            prevGrassTile = hit.transform.GetChild(0).transform.gameObject;
            prevGrassTile.SetActive(true);
        }
            

        //if we did NOT detect a grass tile, return
        if (!hit)
        {
            if(prevGrassTile != null) prevGrassTile.SetActive(false);
            return;
        }
            

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

            
    }
}
