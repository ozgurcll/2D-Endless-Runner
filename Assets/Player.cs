using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private KeyCode jumpKey = KeyCode.Space;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Collision Settings")]
    public bool isGrounded;

    [SerializeField]
    private LayerMask whatIsLayer;

    [SerializeField]
    private float groundCheckDistance;

    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpForce;

    [Header("Booleans")]
    private bool playerUnlocked;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorController();

        if (playerUnlocked)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        CheckCollision();
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
            playerUnlocked = true;
        if (jumpInput() && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void AnimatorController()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundCheckDistance,
            whatIsLayer
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            transform.position,
            new Vector2(transform.position.x, transform.position.y - groundCheckDistance)
        );
    }

    private bool jumpInput()
    {
        return Input.GetKeyDown(jumpKey);
    }
}
