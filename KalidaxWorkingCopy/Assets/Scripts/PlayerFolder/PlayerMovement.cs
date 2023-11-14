using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public bool canControl;

    //movement variables
    [SerializeField] float speed;
    float horizontalInput;
    float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
