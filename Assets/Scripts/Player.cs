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
    [SerializeField] Vector2 deathKick = new Vector2(0f, 15f);

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D bodyCollider2D;
    Collider2D feetCollider2D;
    float gravityScaleAtStart;

    // Message then methods
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        bodyCollider2D = GetComponent<CapsuleCollider2D>();
        feetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
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

    private void ExecuteRun()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }

    private void Run()
    {
        ExecuteRun();

        SetupRunAnimation();
    }

    private void Jump()
    {
        bool isOnTheGround = feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isOnTheGround) return;

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void ExecuteClimb()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
    }

    private void ClimbLadder()
    {
        bool isTouchingLadder = bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        SetupClimbAnimation(isTouchingLadder);
        SetupGravityWhileClimbing(isTouchingLadder);

        if (!isTouchingLadder) return;
        ExecuteClimb();
    }

    private void DisablePlayerInput()
    {
        isAlive = false;
    }

    private void LaunchInTheAir()
    {
        myRigidBody.velocity += deathKick;
    }

    private void PlayDeathAnimation()
    {
        myAnimator.SetTrigger("Die");
        LaunchInTheAir();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        bool isTouchingEnemyOrHazard = myRigidBody.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"));
        if (!isTouchingEnemyOrHazard) return;
        DisablePlayerInput();
        PlayDeathAnimation();
        DisableAllColliders();
    }

    private void DisableAllColliders()
    {
        var colliders = GetComponents<Collider2D>();
        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void SetupGravityWhileClimbing(bool isTouchingLadder)
    {
        if (!isTouchingLadder)
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
        }
        else
        {
            myRigidBody.gravityScale = 0f;
        }
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
