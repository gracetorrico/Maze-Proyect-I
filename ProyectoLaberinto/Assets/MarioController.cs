using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    public float moveSpeed;
    public float forceMultiplier;
    private Animator animator;
    private Vector3 originalScale;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
        forceMultiplier = 5f;
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        animator.SetBool("IsLateralWalking", Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput));
        animator.SetBool("IsUpWalking", verticalInput > 0);
        animator.SetBool("IsDownWalking", verticalInput < 0);
        rb.velocity = movement * moveSpeed;

        if (movement != Vector2.zero)
        {
            if (horizontalInput > 0) // Derecha
            {
                transform.localScale = originalScale;
            }
            else if (horizontalInput < 0) // Izquierda
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
        }
    }
}
