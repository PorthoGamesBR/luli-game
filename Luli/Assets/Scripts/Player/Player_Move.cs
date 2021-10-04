using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class Player_Move : Ent_Movement
{
    public Ent_Jump playerJump;

    //Horizontal Move
    [Space]
    [Header("Horizontal Movement")]

    public float speed = 1;
    public float baseRunSpeed = 2;
    public float runAcel = 1;
    public float decelerationScale = 10;
    private float actualSpeed;
    private static float rbActualHorSpeed;
    public float xMove;
    float rawXMove;
    public bool runPressed;

    public AudioClip jumpSound;

    

    //Vertical Move
    [Header("Vertical Movement")]
    public bool jumpPress;
    bool startedJumpPress;
    
    private static float rbActualVertSpeed;
    public static bool isPlayerOnGround = true;
    

    //Events
    public UnityEvent<float> playerWalking_event;
    public UnityEvent playerJumping_event;

    //Logic
    [Header("Logic")]
    public float timeToReactivate;
    bool canMove = true;
    bool haveCombat;
    bool usingXInput = true;
    bool usingYInput = true;

    [Header("Particles")]

    public Particle jumpParticle;

    override protected void Start()
    {

        base.Start();

        //Check if Jump script exists
        if(playerJump == null){
            if(GetComponent<Ent_Jump>() != null){
                playerJump = GetComponent<Ent_Jump>();
            }else{
                Debug.LogError("You forgot the Entity Jump on the player!!!");
            }
        }
    }

    private void Update() {
        if(canMove && usingXInput){
            //Input manager
            xMove = Input.GetAxis("Horizontal");
            rawXMove = Input.GetAxisRaw("Horizontal");
            runPressed = Input.GetButton("Fire1");
            jumpPress = Input.GetButton("Jump");
        }
        
       
    if(usingYInput){
        //Jump (if pressed and is on ground)
        if(jumpPress && playerJump.isOnGround && !startedJumpPress && Mathf.Abs(rb.velocity.y) < 0.5f){
           StartCoroutine(DelayJump(0.09f));
        }else if(jumpPress && playerJump.jumpCaller == "Coil" && Mathf.Abs(rb.velocity.y) < 0.5f){
            playerJump.Jump("Player");
            GameObject rParticle = Instantiate(jumpParticle.gameObject, transform.position, Quaternion.identity);
            //rParticle.transform.localScale = transform.GetChild(0).GetChild(0).localScale;
            SoundManager.getSoundManager().PlaySound(jumpSound, 0.5f);
        }


        //If stops pressing the button on air and its not falling already
        if(Input.GetButtonUp("Jump") && !playerJump.isOnGround && !playerJump.GetFalling()){
            playerJump.SetFalling();
        }
    }
        

        rbActualHorSpeed = rb.velocity.x;
        rbActualVertSpeed = rb.velocity.y;
        isPlayerOnGround = playerJump.isOnGround;
    }

    override protected void FixedUpdate()
    {

     if(canMove){
      if(!runPressed){
            if(xMove!= 0){
                actualSpeed = speed;
                Move(xMove, actualSpeed);
                playerWalking_event.Invoke(xMove);

        }else{            
            Stop();
        }

      }else{ //If it is running
        if(rawXMove!= 0){

            //Run controll (Can start running on ground, can keep acceleration on air, cant run on air)
            if(playerJump.isOnGround){
                Acelerate(rawXMove,baseRunSpeed, runAcel);

            } else{
                //If the player press run and its not on the ground, it will check if the player is at the min speed
                //If its above, keep that momentum
                Move(xMove, maxVelocity);
                
                /*if(Mathf.Abs(rb.velocity.x) > speed){
                actualSpeed = Mathf.Clamp(Mathf.Abs(rb.velocity.x), speed, maxVelocity);
                }
                else{
                    //If its not, just set the min speed
                    actualSpeed = speed;
                }

                Move(xMove, actualSpeed);*/
                
            }

            playerWalking_event.Invoke(rawXMove);
        }else{
            StopAcel(decelerationScale);
        }
      }
     }

    base.FixedUpdate();
    }

    //To calculate the jump force based on the time pressing the jump button
    IEnumerator DelayJump(float time){
        startedJumpPress = true;
        //After some time, check if the player is stil pressing the jump button
        yield return new WaitForSeconds(time);
        if(!jumpPress){
            playerJumping_event.Invoke();         
            playerJump.Jump(playerJump.jumpForce/2,"Player");
            playerJump.isOnGround = false;
        }else{
            playerJumping_event.Invoke();         
            playerJump.Jump("Player");
            playerJump.isOnGround = false;
        }
         GameObject rParticle = Instantiate(jumpParticle.gameObject, transform.position, Quaternion.identity);
         //rParticle.transform.localScale = transform.GetChild(0).GetChild(0).localScale;
        SoundManager.getSoundManager().PlaySound(jumpSound, 0.5f);

        startedJumpPress = false;
    }

    public void LockInput(){
        usingXInput = !usingXInput;
        usingYInput = !usingYInput;
    }
    public void LockInput(bool locked){
        usingXInput = !locked;
        usingYInput = !locked;
    }

    public void LockInput(bool locked, float time){
        usingXInput = !locked;
        usingYInput = !locked;
        Invoke("LockInput", time);
    }
    

    public static float getRbHorSpeed(){
        return rbActualHorSpeed;
    }

    public static float getRbVertSpeed(){
        return rbActualVertSpeed;
    }

    //Deactivate and Reactivate Movement of Player
    public void DeactivateAllMove(){
        Stop();
        xMove = 0;
        rawXMove = 0;
        jumpPress = false;
        runPressed = false;
        canMove = false;

        //For comodity, i will put smooth fall here too
        setSmoothFall(false);
    }

      public void DeactivateAllMove(float time){
        Stop();
        xMove = 0;
        rawXMove = 0;
        jumpPress = false;
        runPressed = false;
        canMove = false;
        
        ReactivateAllMove(time);
        
    }

    public void setSmoothFall(bool set){
        playerJump.isUsingSmoothFall = set;
    }



    public void ReactivateAllMove(float time = 0){
        if(time > 0){
            StartCoroutine(ReactivateAfterTime(time));
        }else{
            StartCoroutine(ReactivateAfterTime(0));
        }
    }

    IEnumerator ReactivateAfterTime(float timeToReactivate){
        yield return new WaitForSeconds(timeToReactivate);
        canMove = true;

        //For comodity, i will put smooth fall here too
        setSmoothFall(true);

    }


    

    
}
