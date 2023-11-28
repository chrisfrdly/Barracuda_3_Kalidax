using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollect : MonoBehaviour
{
    //References

    //Components
    private Rigidbody2D rb;

    //Variables
    [SerializeField] private float initialVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.AddForce(initialVelocity * Random.onUnitSphere, ForceMode2D.Impulse);
    }

}
