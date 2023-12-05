using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

    }

    //this will get both the vertical and horizontal inputs
    void GetMovementInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }


}
