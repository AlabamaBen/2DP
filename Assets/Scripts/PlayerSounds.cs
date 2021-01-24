using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SFX_List
{
    Jump,
    WallJump,
    Land, 
    Slide, 
    Dash, 
    DashRecover, 
    Step
}

public class PlayerSounds : MonoBehaviour
{

    public AudioSource EffectSource;
    public AudioSource EffectLoopSource;

    public AudioClip JumpClip;
    public AudioClip WallJumpClip;
    public AudioClip LandClip;
    public AudioClip SlideClip;
    public AudioClip DashClip;
    public AudioClip DashRecoverClip;
    public List<AudioClip> StepClips;

    SFX_List Last_SFX_Played; 

    public bool IsWallSliding;

    // Start is called before the first frame update
    void Start()
    {
        Last_SFX_Played = SFX_List.Step; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(IsWallSliding)
        {
            EffectLoopSource.clip = SlideClip;
            EffectLoopSource.volume = 1f;
        }
        else
        {
            EffectLoopSource.volume = 0;
        }
    }

    public void SFX_Jump()
    {
        EffectSource.clip = JumpClip;
        EffectSource.Play();
        Last_SFX_Played = SFX_List.Jump;
    }

    public void SFX_WallJump()
    {
        EffectSource.clip = WallJumpClip;
        EffectSource.Play();
        Last_SFX_Played = SFX_List.WallJump;
    }

    public void SFX_Dash()
    {
        EffectSource.clip = DashClip;
        EffectSource.Play();
        Last_SFX_Played = SFX_List.Dash;
    }

    public void SFX_RecoverDash()
    {
        EffectSource.clip = DashRecoverClip;
        EffectSource.Play();
        Last_SFX_Played = SFX_List.DashRecover;
    }



    public void SFX_Land()
    {
        if (!EffectSource.isPlaying)
        {
            EffectSource.clip = LandClip;
            EffectSource.Play();
            Last_SFX_Played = SFX_List.Land;
        }
    }

    public void SFX_Steps()
    {
        if (!EffectSource.isPlaying || Last_SFX_Played == SFX_List.Step)
        {
            Last_SFX_Played = SFX_List.Step;
            EffectSource.clip = StepClips[Random.Range(0, StepClips.Count)];
            EffectSource.Play();
        }
    }


}
