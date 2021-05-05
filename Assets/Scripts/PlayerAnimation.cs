using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    public PlayerMovement movement;
    public Rigidbody2D rigidBody;      //Reference to the Rigidbody2D component
    public PlayerInput input;          //Reference to the PlayerInput script component
    public Animator anim;              //Reference to the Animator component
    public Animation land_animation;

    int speedParamID;           //ID of the speed parameter
    int fallParamID;            //ID of the verticalVelocity parameter
    int groundedParamID;
    int walledParamID;
    int dashParamID;


    public ParticleSystem walljumpparticles;
    public ParticleSystem walkparticles;
    public ParticleSystem jumpparticles;
    public ParticleSystem dashparticles;
    public ParticleSystem dashtrailparticles;
    public ParticleSystem dashtrecoverparticles;
    public ParticleSystem spawnparticles;



    public PlayerSounds playerSounds;

    public SpriteRenderer spriteRenderer;

    public Color Dash_Color;

    public float Land_Shake_Intensity = 1f; 
    public float Land_Shake_Time = 0.1f;

    public float Dash_Shake_Intensity = 1f;
    public float Dash_Shake_Time = 0.1f;

    public float Spawn_Shake_Intensity = 1f;
    public float Spawn_Shake_Time = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        //Get the integer hashes of the parameters. This is much more efficient
        //than passing strings into the animator
        speedParamID = Animator.StringToHash("speed");
        fallParamID = Animator.StringToHash("verticalVelocity");
        groundedParamID = Animator.StringToHash("grounded");
        walledParamID = Animator.StringToHash("walled");
        dashParamID = Animator.StringToHash("dashing");

        //Grab a reference to this object's parent transform
        Transform parent = transform.parent;

        //If any of the needed components don't exist...
        if (movement == null || rigidBody == null || input == null || anim == null)
        {
            //...log an error and then remove this component
            Debug.LogError("A needed component is missing from the player");
            Destroy(this);
        }


    }

    // Update is called once per frame
    void Update()
    {

        //Update the Animator with the appropriate values 
        anim.SetFloat(fallParamID, rigidBody.velocity.y);

        //Use the absolute value of speed so that we only pass in positive numbers
        anim.SetFloat(speedParamID, Mathf.Abs(input.horizontal));

        anim.SetBool(groundedParamID, movement.isOnGround);

        anim.SetBool(walledParamID, movement.isWalled);

        anim.SetBool(dashParamID, movement.isDashing);

        if (movement.isWalled && !walljumpparticles.isPlaying && !movement.isOnGround)
        {
            walljumpparticles.Play();
        }
        if (walljumpparticles.isPlaying && movement.isOnGround || !movement.isWalled)
        {
            walljumpparticles.Stop();
        } 

        if (movement.isOnGround && walkparticles.isStopped && Mathf.Abs(movement.XSpeed) > 7f  )
        {
            walkparticles.Play();
        }
        if(walkparticles.isPlaying && (Mathf.Abs(movement.XSpeed) < 7f || !movement.isOnGround ) )
        {
            walkparticles.Stop();
        }

        if (movement.isWalled && movement.YSpeed < -0.1f)
        {
            playerSounds.IsWallSliding = true;
        }
        if (!movement.isWalled || movement.YSpeed > -0.1f || movement.isOnGround)
        {
            playerSounds.IsWallSliding = false;
        }

        if(dashtrailparticles.isPlaying && !movement.isDashing)
        {
            dashtrailparticles.Stop();
        }

    }

    public void Animation_Jump()
    {
        jumpparticles.Play();
        playerSounds.SFX_Jump();
    }

    public void Animation_Land()
    {
        land_animation.Play();
        jumpparticles.Play();
        playerSounds.SFX_Land();
        CinemachineShake.Instance.ShakeCamera(Land_Shake_Intensity, Land_Shake_Time);
    }

    public void Animation_Step()
    {
        playerSounds.SFX_Steps();
    }

    public void Animation_Dash()
    {
        dashparticles.Play();
        dashtrailparticles.Play();
        playerSounds.SFX_Dash();
        spriteRenderer.color = Dash_Color;
        CinemachineShake.Instance.ShakeCamera(Dash_Shake_Intensity, Dash_Shake_Time);
    }

    public void Animation_Spawn()
    {
        spawnparticles.Play();
        playerSounds.SFX_RecoverDash();
    }

    public void Animation_Die()
    {
        spawnparticles.Play();
        playerSounds.SFX_RecoverDash();
        CinemachineShake.Instance.ShakeCamera(Spawn_Shake_Intensity, Spawn_Shake_Time);
    }



    public void Animation_Dash_Recover()
    {
        spriteRenderer.color = new Color(255, 255, 255);
        dashtrecoverparticles.Play();
        playerSounds.SFX_RecoverDash();
    }

    public void Animation_WallJump()
    {
        playerSounds.SFX_WallJump();
    }
}
