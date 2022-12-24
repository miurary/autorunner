using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// void helperSnoopy()
//     {
//             string text =
//         @"  ,-~~-.___.
//         / |  ' 	\    	 
//         (  )     	0  
//         \_/-, ,----'       	 
//             ====       	//
//         /  \-'~;	/~~~(O)
//         /  __/~|   /   	|	 
//         =(  _____| (_________|
//             )
//         ";
//    	    Debug.Log(text);
//     }

public class testPlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;

    public bool isGrounded;
    public bool canDoubleJump;

    public LayerMask whatIsGround;

    private Rigidbody2D myRigidBody;
    private Collider2D myCollider;
    private Animator myAnimator;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);

        //moves the player forward
        myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);

        //when player touches the ground value resets
        if(isGrounded && !Input.GetButtonDown("Jump"))
        {
            canDoubleJump = true;
        }

        //jump
        if(Input.GetButtonDown("Jump")){
            if(isGrounded || canDoubleJump)
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpForce);
                canDoubleJump = !canDoubleJump;
            }
            
        }

        //better jumping logic
        if(Input.GetButtonUp("Jump") && myRigidBody.velocity.y > 0f){
           myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myRigidBody.velocity.y * 0.5f);
        }

        //sets parameter values for animator purposes
        myAnimator.SetFloat("Speed", myRigidBody.velocity.x);
        myAnimator.SetFloat("Jumping", myRigidBody.velocity.y);
        myAnimator.SetBool("Grounded", isGrounded);
        myAnimator.SetBool("DoubleJump", canDoubleJump);

        //TODO: ANIMATION
            // RUN
            // RIP
            // JUMP
            // DOUBLE JUMP
            // FALLING
            // HURT

        //TODO: STAMINA/HEALTH
        //TODO: POWERUPS
        //TODO: SCORE
        //TODO: SCORE COLLECTIBLE
        //TODO: STAGE
        //TODO: SLIDE?
    }

}
