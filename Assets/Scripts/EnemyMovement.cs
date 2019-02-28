using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D myRigidBody;

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    private void Move()
    {
        if (IsFacingRight())
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FlipSprite();
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector2(
                -Mathf.Sign(myRigidBody.velocity.x) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y
                );
    }
}
