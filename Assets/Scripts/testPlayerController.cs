using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

//TODO: STAGE | have to find out how to make stage transitions
//TODO: COUNTDOWN AT START && LOADING SCREEN?
//TODO: COYOTE JUMP
//TODO: MAKE OWN ASSETS
//TODO: ADD SMOKE EFFECT WHEN LANDING
//TODO: DUST PARTICLE EFFECT WHEN WALKING
//TODO: ENDLESS MODE
//TODO: *CHANGE SCORE SYSTEM SO ITS BASED ON DISTANCE
//TODO: DIFFERENT CHARACTER?
//TODO: CHARACTER SELECT SCREEN
//TODO: SLIDE? 
    
//POLISH: CAMERA VALUES (WITH THE FASTER POWERUP CAMERA JITTERS)
//POLISH: JUMP DASH
//POLISH: TRAIL COLORS
//POLISH: MENU
//POLISH: CLEANUP CODE(make into functions)

//REDO: RESPAWN/CHECKPOINT

//FOREVER: ALWAYS THINK ABOUT MOVEMENT

//GOAL: SOMETHING LIKE THIS LMAO (https://www.youtube.com/watch?v=C3JQld-qWls&t=44s)



public class testPlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public float fastFallForce;
    public float dashTime;

    public bool isHurt;
    public bool isInvincible;
    public bool isFall;
    
    private bool isGrounded;
    private bool isDoubleJump;
    private bool canDash; 

    public LayerMask whatIsGround;

    public GameOverScript myGameOver;

    public SpriteRenderer mySprite;
    public Collider2D myCollider;

    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private HealthSystem myHealth;

    private ScoreManager myScoreManager;

    [SerializeField] private TrailRenderer myTR;
     
    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();

        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myHealth = GetComponent<HealthSystem>();

        myScoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //is player touching the ground checker
        isGrounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);

        //if player is alive keep moving forward
        //else player is dead stop moving
        if(myHealth.isPlayerDead == false)
        {
            myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);

            if(!isFall && !isInvincible)
            {
                myHealth.TakeDamage(0.5f * Time.deltaTime);
            }
        }
        else
        {
            Debug.Log("PLAYER is now RIP");
            myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y);
            myScoreManager.scoreIncreasing = false;
            myGameOver.Setup(myScoreManager.scoreCount);

        }
        
        //if player has fallen into deathZone then freeze them in the air
        //else dont freeze
        if(isFall)
        {
            myRigidBody.isKinematic = true;
            myRigidBody.velocity = new Vector2(0, 0);
        }
        else
        {
            myRigidBody.isKinematic = false;
        }

        //if player is grounded and not pressing jump button
        if(isGrounded && !Input.GetButtonDown("Jump"))
        {
            myTR.emitting = false;

            isDoubleJump = true;
            canDash = true;
        }

        //jump
        if(Input.GetButtonDown("Jump") && !myHealth.isPlayerDead){
            myTR.startColor = new Color (1f, 0f, 0f);
            myTR.emitting = true;

            if(isGrounded || isDoubleJump)
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpForce);
                isDoubleJump = !isDoubleJump;
            }
        }
        //for better jumping
        if(Input.GetButtonUp("Jump") && myRigidBody.velocity.y > 0f){
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myRigidBody.velocity.y * 0.5f);
        }

        if(Input.GetKeyDown(KeyCode.D) && canDash && !isGrounded){
            StartCoroutine(Dash());
        }

        //fast falling
        if(Input.GetKeyDown(KeyCode.S) && !isGrounded){
            myTR.startColor = new Color (0.35f, 1f, 0.75f);
            myTR.emitting = true;

            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myRigidBody.velocity.y-fastFallForce);
        }

        //sets parameter values for animator 
        myAnimator.SetFloat("Speed", myRigidBody.velocity.x);
        myAnimator.SetFloat("Jumping", myRigidBody.velocity.y);

        myAnimator.SetBool("Grounded", isGrounded);
        myAnimator.SetBool("isHurt", isHurt);
        myAnimator.SetBool("DoubleJump", isDoubleJump);

    }

    private IEnumerator Dash()
    {
        myTR.startColor = Color.white;

        canDash = false;
        float cMoon = myRigidBody.gravityScale;
        myRigidBody.gravityScale = 0f;

        if(isInvincible)
        {
            moveSpeed = 15f;
        }
        else
        {
            moveSpeed = 12f;
        }

        myRigidBody.velocity = new Vector2(transform.localScale.x, 0f);
        myTR.emitting = true;
        yield return new WaitForSeconds(dashTime);
        
        if(isInvincible)
        {
            moveSpeed = 15f;
        }
        else
        {
            moveSpeed = 10f;
        }
        
        myTR.emitting = false;
        myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
        myRigidBody.gravityScale = cMoon;
    }

}
