using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private KeyCode jumpKey = KeyCode.Space;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private bool isDead;

    [Header("Knife")]
    [SerializeField] private GameObject knife;
    [SerializeField] private Transform knifeTransform;
    private bool canShooting;
    [SerializeField] private float throwCooldown;
    private float throwCooldownCounter;


    [Header("Knockback")]
    [SerializeField] private Vector2 knockbackDir;
    private bool isKnocked;
    private bool canBeKnocked = true;

    //################################################################//

    [Header("Collision")]
    public bool isGrounded;
    private bool wallDetected;

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float groundCheckDistance;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    //################################################################//
    [Header("Speed Settings")]
    [SerializeField] private float maxSpeed;
    private float defaultSpeed;
    [SerializeField] private float speedMultiplier;
    [Space]
    [SerializeField] private float milestoneIncreaser;

    private float defaultMilestoneIncreaser;
    private float speedMilestone;

    //################################################################//

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private float jumpForce;

    //################################################################//

    [Header("Slide Settings")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float ceillingCheckDistance;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideCooldown;
    private float slideCooldownCounter;
    private float slideTimerCounter;
    private bool isSliding;
    private bool ceillingDetected;
    [HideInInspector] public bool ledgeDetected;

    //################################################################//

    [Header("Climb Settings")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true;
    private bool canClimb;

    //################################################################//

    [Header("Booleans")]
    private bool playerUnlocked;
    private bool canDoubleJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;

    }

    private void Update()
    {
        CheckCollision();
        AnimatorController();

        slideTimerCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        throwCooldownCounter -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();

        if (Input.GetKeyDown(KeyCode.K))
            Knockback();

        if (Input.GetKeyDown(KeyCode.L) && !isDead)
            StartCoroutine(Die());

        if (isDead)
            return;

        if (isKnocked)
            return;

        if (playerUnlocked)
            Movement();

        if (isGrounded)
            canDoubleJump = true;

        SpeedController();

        FinishSlide();
        CheckInput();
        CheckForLedge();

    }

    public void Damage()
    {
        if (moveSpeed >= maxSpeed)
            Knockback();
        else
            StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        isDead = true;
        canBeKnocked = false;
        rb.velocity = knockbackDir;
        anim.SetBool("isDead", true);

        yield return new WaitForSeconds(.5f);
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1f);
        GameManager.instance.RestartLevel();

    }



    private IEnumerator Invincibility()
    {
        Color originalColor = sr.color;
        Color darkenColor = new Color(sr.color.r, sr.color.g, sr.color.b, .5f);
        canBeKnocked = false;

        sr.color = darkenColor;
        yield return new WaitForSeconds(.1f);

        sr.color = originalColor;
        yield return new WaitForSeconds(.1f);

        sr.color = darkenColor;
        yield return new WaitForSeconds(.15f);

        sr.color = originalColor;
        yield return new WaitForSeconds(.15f);

        sr.color = darkenColor;
        yield return new WaitForSeconds(.25f);

        sr.color = originalColor;
        yield return new WaitForSeconds(.25f);

        sr.color = darkenColor;
        yield return new WaitForSeconds(.3f);

        sr.color = originalColor;
        yield return new WaitForSeconds(.3f);


        sr.color = originalColor;
        canBeKnocked = true;
    }

    private void Knockback()
    {
        if (!canBeKnocked)
            return;

        StartCoroutine(Invincibility());
        SpeedReset();
        isKnocked = true;
        rb.velocity = knockbackDir;
    }

    public void FinishKnockback() => isKnocked = false;

    private void SpeedReset()
    {
        if (isSliding)
            return;

        moveSpeed = defaultSpeed;
        milestoneIncreaser = defaultMilestoneIncreaser;
    }

    private void SpeedController()
    {
        if (moveSpeed == maxSpeed)
            return;

        if (transform.position.x > speedMilestone)
        {
            speedMilestone += milestoneIncreaser;
            moveSpeed *= speedMultiplier;
            milestoneIncreaser *= speedMultiplier;

            if (moveSpeed > maxSpeed)
                moveSpeed = maxSpeed;

        }
    }

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            rb.gravityScale = 0;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetected>().transform.position;

            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
        }
        if (canClimb)
            transform.position = climbBegunPosition;
    }

    private void FinishClimb()
    {
        canClimb = false;
        rb.gravityScale = 5;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", .1f);
    }

    private void AllowLedgeGrab() => canGrabLedge = true;

    private void FinishSlide()
    {
        if (slideTimerCounter < 0 && !ceillingDetected)
            isSliding = false;
    }

    private void Movement()
    {
        if (wallDetected)
        {
            SpeedReset();
            return;
        }

        if (isSliding && isGrounded)
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        else

            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
            playerUnlocked = true;


        if (jumpInput())
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Slide();

    }

    private void Slide()
    {
        if (rb.velocity.x != 0 && slideCooldownCounter < 0 && !canShooting)
        {
            isSliding = true;
            slideTimerCounter = slideTimer;
            slideCooldownCounter = slideCooldown;
        }
    }

    private void Jump()
    {
        if (isSliding)
            return;

        RollAnimFinished();

        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }

    private void Shoot()
    {
        if (isGrounded && !isSliding && throwCooldownCounter < 0)
        {
            canShooting = true;
            Instantiate(knife, knifeTransform.position, Quaternion.identity);
            throwCooldownCounter = throwCooldown;
        }

    }

    private void AnimatorController()
    {
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);


        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("canClimb", canClimb);
        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("canShooting", canShooting);



        /* if (rb.velocity.y < -10)
             anim.SetBool("canRoll", true);*/
    }
    private void ShootingFinished() => canShooting = false;

    private void RollAnimFinished() => anim.SetBool("canRoll", false);

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }

    private bool jumpInput()
    {
        return Input.GetKeyDown(jumpKey);
    }
}
