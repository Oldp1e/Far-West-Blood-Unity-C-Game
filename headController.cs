using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headController : MonoBehaviour {

    //   Animation anim;
    Animator anim;
    PhysicsController pC;
    MainController mC;
    private bool isFalling;
    private bool isJumping;
    private bool isIdle;


    private string idle_animation = "headidle";
    private string air_animation = "headair";
    private string air_animation_falling = "headairfalling";
    private string land_animation = "headland";
    private string jump_animation = "headjump";
    private string run_animation = "headrun";
    private string walk_animation = "headwalk";

    

	// Use this for initialization
	void Awake () {
        //     anim = GetComponent<Animation>();
        anim = GetComponent<Animator>();
        pC = GameObject.Find("Player v1").GetComponentInChildren<PhysicsController>();
        mC = GameObject.Find("Player v1").GetComponentInChildren<MainController>();
    }
	
	// Update is called once per frame
	void Update () {




        if (mC.isSprinting == false && mC.isWalking == false && pC.isGrounded == true && isJumping == false)
        {
            play_idle_animation_f(true);
            isIdle = true;

        }
        else if(isIdle){
            isIdle = false;
        }
        if (Input.GetButtonDown("Jump"))
        {
            play_jump_animation_f();
            isJumping = true;
        }
        if (pC.isGrounded == false && !isJumping)
        {
            isFalling = true;
            play_air_animation_falling_f();                                                        
        }
        else {
            isFalling = false;
        }
        if (mC.isWalking && !isJumping && isFalling == false)
        {
            play_walk_animation_f();
        }
        if (mC.isSprinting && !isJumping && isFalling == false) {
            play_run_animation_f();
        }
        
	}


    void play_idle_animation_f(bool isIdle) {
        if (isIdle)
        {
            anim.Play(idle_animation);
        }
    }
    void play_air_animation_f()
    {
        anim.Play(air_animation);
    }
    void play_air_animation_falling_f()
    {
        anim.Play(air_animation_falling);
    }

    void play_jump_animation_f()
    {
        anim.Play(jump_animation);
    }

    void play_land_animation_f()
    {
        anim.Play(land_animation);
    }

    void play_run_animation_f()
    {
        anim.Play(run_animation);
    }

    void play_walk_animation_f()
    {
        anim.Play(walk_animation);
    }

    void hasLanded() {
        isJumping = false;
    }



}
