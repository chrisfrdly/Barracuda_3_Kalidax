using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //References
    [SerializeField] private SO_GrassTileParameters grassTileParams;

    //Components
    private Rigidbody2D rb;

    

    //movement variables
    [Header("Movement Variables")]
    public bool canControl;

    [SerializeField] private float speed;
    private float horizontalInput;
    private float verticalInput;
    private Vector2 directionFacing;
    
    [Header("Interaction Variables")]
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask grassMask;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //Get the Last direction the player was facing
        if (horizontalInput != 0 || verticalInput != 0)
            directionFacing = new Vector2(horizontalInput, verticalInput).normalized;
        GetMovementInputs();

        if (canControl)
        {
            Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;

            rb.velocity = direction * speed;
        }
        else
            rb.velocity = Vector2.zero;

        


        RaycastGrassTile();
    }

    //this will get both the vertical and horizontal inputs
    void GetMovementInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    //Creates a raycast in the player's forward direction and if the player clicks AND it detects a grass tile, brak it
    private void RaycastGrassTile()
    {
        //Create the raycast
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, directionFacing, interactRadius, grassMask);
        Debug.DrawLine(transform.position, (Vector2)transform.position + directionFacing * interactRadius);

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

    private void OnDrawGizmos()
    {
        //Displays the interaction radius around the player
        Gizmos.color = new Color(0, 255, 0, 0.5f);
 
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
