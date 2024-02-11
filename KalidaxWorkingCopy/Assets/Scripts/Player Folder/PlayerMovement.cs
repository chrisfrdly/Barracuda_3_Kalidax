using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //References
    private PlayerInputHandler inputHandler;

    //Components
    private Rigidbody2D rb;

    //movement variables
    [Header("Movement Variables")]
    public bool canControl;

    [SerializeField] private float speed;
    private float horizontalInput;
    private float verticalInput;
    private Vector2 directionFacing;

    public Vector2 m_DirectionFacing { get => directionFacing; set => directionFacing = value; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = FindObjectOfType<PlayerInputHandler>();

    }


    void Update()
    {
        horizontalInput = inputHandler.m_MoveInput.x;
        verticalInput = inputHandler.m_MoveInput.y;

        // Get the Last direction the player was facing
        if (horizontalInput != 0 || verticalInput != 0)
            directionFacing = new Vector2(horizontalInput, verticalInput).normalized;

        if (canControl)
        {
            Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;
            rb.velocity = direction * speed;

            // Flip the sprite by adjusting the local scale
            if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Facing right
            }
            else if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Facing left
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }



}
