using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Idle, Running, Jumping, Falling, Attacking, Dashing }

public class PlayerController : MonoBehaviour
{
    // --- SERIALIZED FIELDS --- //
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float distance;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float dashTime;
    [SerializeField] private float dashForce;
    [SerializeField] private TrailRenderer tr;

    // --- FIELDS --- //
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;

    private float xInput;
    private bool isDashing;
    private bool isAttacking;
    private PlayerState currentState;

    // --- UNITY METHODS --- //
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Flip();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        ChangeAnimState();
    }

    private void FixedUpdate()
    {    
        if (isDashing)
        {
            rb.velocity = new Vector2(dashForce * transform.localScale.x, 0);
            return;
        }

        HandleMovement();
    }

    // --- PRIVATE METHODS --- //
    private void HandleMovement()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        Vector2 move = new Vector2(xInput * speed, rb.velocity.y);
        rb.velocity = move;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        if (xInput > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (xInput < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ChangeAnimState()
    {
        if (isAttacking || isDashing) { return; }

        if (isGrounded())
        {
            if (xInput == 0 && currentState != PlayerState.Jumping)
            {
                ChangeState(PlayerState.Idle);
            } 
            else if (Mathf.Abs(xInput) > 0 && currentState != PlayerState.Jumping)
            {
                ChangeState(PlayerState.Running);
            }
        }
        else
        {
            if (rb.velocity.y < -0.5f)
            {
                ChangeState(PlayerState.Falling);
            }
            else if (rb.velocity.y > 0.5f)
            {
                ChangeState(PlayerState.Jumping);
            }
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        tr.emitting = true;
        rb.gravityScale = 0;

        ChangeState(PlayerState.Dashing);

        yield return new WaitForSeconds(dashTime);

        tr.emitting = false;
        rb.gravityScale = 1;
        isDashing = false;
    }

    // --- PUBLIC METHODS --- //
    public void ChangeState(PlayerState newState)
    {
        if (newState == currentState) return;

        currentState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                anim.SetTrigger("Idle");
                break;
            case PlayerState.Running:
                anim.SetTrigger("Run");
                break;
            case PlayerState.Jumping:
                anim.SetTrigger("Jump");
                break;
            case PlayerState.Falling:
                anim.SetTrigger("Fall");
                break;
            case PlayerState.Attacking:
                anim.SetTrigger("Attack");
                break;
            case PlayerState.Dashing:
                anim.SetTrigger("Dash");
                break;
        }
    }

    public bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, distance, whatIsGround);
        return hit.collider != null;
    }

    public void CompleteAttack()
    {
        isAttacking = !isAttacking;
    }
}
