using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    // Config
    [SerializeField] float runSpeed = 3f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 3f;

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider2D;

    // Message then methods
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        ClimbLadder();
        FlipSprite();
        Jump();
    }

    private void SetupRunAnimation()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        SetupRunAnimation();
    }

    private void Jump()
    {
        bool isOnTheGround = myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isOnTheGround) return;

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder()
    {
        bool isTouchingLadder = myCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        SetupClimbAnimation(isTouchingLadder);
        if (!isTouchingLadder)
        {
            myRigidBody.gravityScale = 1;
            return;
        }
        else
        {
            myRigidBody.gravityScale = 0;
        }


        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
    }

    private void SetupClimbAnimation(bool isTounchingLadder)
    {
        if (!isTounchingLadder)
        {
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(
                Mathf.Sign(myRigidBody.velocity.x) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y
                );
        }
    }
}
