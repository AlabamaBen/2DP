using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{

    public bool drawDebugRaycasts = true;   //Should the environment checks be visualized
    public PlayerAnimation playeranimation;
    public PlayerManager playerManager; 

    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed
    public float speedair = 6.5f;                //Player air speed
    public float coyoteDuration = .05f;     //How long the player can jump after falling
    public float maxFallSpeed = -25f;       //Max speed player can fall
    public float maxFallWall = -5f;       //Max speed player can fall
    public float wallJumpCounterForce = 15f;       //Max speed player can fall


    [Header("Jump Properties")]
    public float jumpForce = 6.3f;          //Initial force of jump
    public float jumpHoldForce = 2.1f;      //Incremental force when jump is held
    public float jumpHoldDuration = .1f;    //How long the jump key can be held
    public float wallJumpMoveBlockDuration = 0.7f;
    public float walljumpForceLeft = 11f;          //Initial force of jump
    public float walljumpForceTop = 5f;          //Initial force of jump

    [Header("Dash Properties")]
    public float dashSpeed = 35f;
    public float dashLenght = 0.15f;
    public float dashCooldown = 0.3f;


    [Header("Environment Check Properties")]
    public float footOffset = .4f;          //X Offset of feet raycast
    public float groundDistance = .2f;      //Distance player is considered to be on the ground
    public LayerMask groundLayer;           //Layer of the ground
    public LayerMask DeadzoneLayer;

    [Header("Status Flags")]
    public bool isOnGround;                 //Is the player on the ground?
    public bool isJumping;                  //Is player jumping?
    public bool isWalled;
    public bool isDashing;
    private bool canDash; 

    PlayerInput input;                      //The current inputs for the player
    BoxCollider2D bodyCollider;             //The collider component
    Rigidbody2D rigidBody;                  //The rigidbody component
    bool move_block;                  //The rigidbody component

    //Timers
    float jumpTime;                         //Variable to hold jump duration
    float coyoteTime;                       //Variable to hold coyote duration
    float moveBlockTime;                       
    float playerHeight;                     //Height of the player
    float dashTime;

    Vector2 StartPosition; 

    float originalXScale;                   //Original scale on X axis
    float originalGravityScale;                   
    int direction = 1;                      //Direction player is facing
    int dashDirection = 1; 

    Vector2 colliderStandSize;              //Size of the standing collider
    Vector2 colliderStandOffset;            //Offset of the standing collider

    Vector2 LastUpdateVelocity;

    const float smallAmount = .05f;			//A small amount used for hanging position

    public float XSpeed
    {
        get
        {
            return rigidBody.velocity.x;
        }
    }

    public float YSpeed
    {
        get
        {
            return rigidBody.velocity.y;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;

        playerManager = PlayerManager.Instance;

        //Get a reference to the required components
        input = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();

        originalGravityScale = rigidBody.gravityScale; 

        //Record the original x scale of the player
        originalXScale = transform.localScale.x;

        //Record the player's height from the collider
        playerHeight = bodyCollider.size.y;

        //Record initial collider size and offset
        colliderStandSize = bodyCollider.size;
        colliderStandOffset = bodyCollider.offset;

        isOnGround = false;

        canDash = true;

        LastUpdateVelocity = Vector2.zero;

        StartPosition = transform.position;

        playeranimation.Animation_Spawn();
    }

    void FixedUpdate()
    {
        //Check the environment to determine status
        PhysicsCheck();

        //Process ground and air movements
        MidAirMovement();
        GroundMovement();
        Checkgame();
    }

    void Checkgame()
    {
        if(bodyCollider.IsTouchingLayers(DeadzoneLayer))
        {
            Die();
        }
    }

    void Die()
    {
        transform.position = playerManager.Current_Checkpoint;
        rigidBody.velocity = Vector2.zero;
        isOnGround = true;
        isWalled = false;
        isJumping = false;
        move_block = false;
        playeranimation.Animation_Spawn();
    }

    void PhysicsCheck()
    {
        //Start by assuming the player isn't on the ground and the head isn't blocked
        bool lastIsOnGround = isOnGround;
        isOnGround = false;
        isWalled = false;

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(-bodyCollider.size.x/2, -bodyCollider.size.y/2 - 0.2f), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(bodyCollider.size.x/2, -bodyCollider.size.y /2 - 0.2f), Vector2.down, groundDistance);

        RaycastHit2D DownWallCheck = Raycast(new Vector2(bodyCollider.size.x / 2 * direction, -bodyCollider.size.y / 2 - 0.2f), Vector2.right * direction, groundDistance);

        //If either ray hit the ground, the player is on the ground
        if (leftCheck || rightCheck)
        {
            isOnGround = true;
            move_block = false;
        }

        if(!lastIsOnGround && isOnGround && LastUpdateVelocity.y < -7)
        {
            playeranimation.Animation_Land();
        }
        LastUpdateVelocity = rigidBody.velocity;

        if (DownWallCheck)
            isWalled = true;

        if(playerManager.HaveDash && isOnGround && !canDash &&  dashTime + dashLenght + dashCooldown < Time.time)
        {
            canDash = true;
            playeranimation.Animation_Dash_Recover();
        }
    }

    void GroundMovement()
    {
        if(playerManager.HaveDash && input.dashPressed && !isDashing && canDash)
        {
            isDashing = true;
            dashTime = Time.time + dashLenght;
            rigidBody.gravityScale = 0f;
            dashDirection = direction;
            playeranimation.Animation_Dash();
            canDash = false;
        }

        if (dashTime < Time.time && isDashing)
        {
            isDashing = false;
            rigidBody.gravityScale = 3f;
        }

        if (isDashing)
        {
            float xVelocity = dashSpeed * dashDirection;
            rigidBody.velocity = new Vector2(xVelocity, 0f);
        }
        else
        {
            float xVelocity = 0f;

            if (isOnGround)
            {
                xVelocity = speed * input.horizontal;
            }
            else
            {
                xVelocity = speedair * input.horizontal;
            }

            //If the sign of the velocity and direction don't match, flip the character
            if (xVelocity * direction < 0f)
                FlipCharacterDirection(); 

            if(move_block && moveBlockTime < Time.time)
                move_block = false;

            //Apply the desired velocity 
            if (!move_block)
            {
                rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
            }
            else
            {
                rigidBody.AddForce(new Vector2(wallJumpCounterForce * input.horizontal, 0), ForceMode2D.Force);

                if (Mathf.Abs(rigidBody.velocity.x) > speedair)
                {
                    rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * speedair, rigidBody.velocity.y);
                }
            }

            if (transform.parent)
            {
                rigidBody.velocity += transform.parent.GetComponent<Rigidbody2D>().velocity;
            }


            //If the player is on the ground, extend the coyote time window
            if (isOnGround)
                coyoteTime = Time.time + coyoteDuration;

        }
    }

    void MidAirMovement()
    {
        //If the jump key is pressed AND the player isn't already jumping AND EITHER
        //the player is on the ground or within the coyote time window...
        if (input.jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time ))
        {
            //...The player is no longer on the groud and is jumping...
            isOnGround = false;
            isJumping = true;

            //...record the time the player will stop being able to boost their jump...
            jumpTime = Time.time + jumpHoldDuration;

            //...add the jump force to the rigidbody...
            playeranimation.Animation_Jump();
            rigidBody.AddForce(new Vector2(jumpForce * direction, jumpForce), ForceMode2D.Impulse);
            
        }
        //Otherwise, if currently within the jump time window...
        else if (isJumping)
        {

            //...and the jump button is held, apply an incremental force to the rigidbody...
            if (input.jumpHeld)
            {
                rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }

            //...and if jump time is past, set isJumping to false
            if (jumpTime <= Time.time)
                isJumping = false;
        }

        if (!isOnGround && isWalled && !isJumping)
        {
            if (input.jumpPressed)
            {
                isJumping = true;
                //...record the time the player will stop being able to boost their jump...
                jumpTime = Time.time + jumpHoldDuration;
                rigidBody.velocity = Vector2.zero;
                rigidBody.AddForce(new Vector2(walljumpForceLeft * -direction, walljumpForceTop), ForceMode2D.Impulse);
                move_block = true;
                moveBlockTime = Time.time + wallJumpMoveBlockDuration;
                playeranimation.Animation_WallJump();
            }
        }

        //If player is falling to fast, reduce the Y velocity to the max
        if (rigidBody.velocity.y < maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);

        if (rigidBody.velocity.y < maxFallWall && isWalled)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallWall);
    }

    void FlipCharacterDirection()
    {
        //Turn the character by flipping the direction
        direction *= -1;

        //Record the current scale
        Vector3 scale = transform.localScale;

        //Set the X scale to be the original times the direction
        scale.x = originalXScale * direction;

        //Apply the new scale
        transform.localScale = scale;
    }

    //These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
    //functionality
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        //Call the overloaded Raycast() method using the ground layermask and return 
        //the results
        return Raycast(offset, rayDirection, length, groundLayer);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        //Record the player's position
        Vector2 pos = transform.position;

        //Send out the desired raycasr and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        //If we want to show debug raycasts in the scene...
        if (drawDebugRaycasts)
        {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        //Return the results of the raycast
        return hit;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }

    // Check if the Player leaves a Platform and set it back out of Parent
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }


}
