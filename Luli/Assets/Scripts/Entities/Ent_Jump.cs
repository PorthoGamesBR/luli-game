using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent_Jump : MonoBehaviour
{
    //Logic
    public bool isOnGround;

    public string jumpCaller;

    //Serves to deactivate the ground check for a little time after jumping
    bool afterJumpDelay;

    private bool isFalling;

    //Characteristics
     public float jumpForce;

       //This one is decided by the Rigidbody Gravity Scale
        float commonGravity;
     public float fallGravity;
     public float gravityCurveScale;
    
     public float groundDistance;
     public float groundDistanceXOffset;
     public float groundDistanceYOffset;

     public LayerMask groundLayerMask;
     

    //Physics
    public Rigidbody2D rb;
    public BoxCollider2D col2d;

    public bool isUsingSmoothFall = true;


    //TODO: Coyote time
    virtual public void Start()
    {
        //Check if Rigidbody exists
        if(rb != null || GetComponent<Rigidbody2D>() != null){
            //If the variable hasnt been initialized, get the RB in the object
            if(rb == null){
                rb = GetComponent<Rigidbody2D>();
            }
        }

        //Check if colider 2d exists
        if(col2d != null || GetComponent<BoxCollider2D>() != null){
            //If the variable hasnt been initialized, get the col2D in the object
            if(col2d == null){
                col2d = GetComponent<BoxCollider2D>();
            }
        }

        commonGravity = rb.gravityScale;
        if(fallGravity <= 0){
            fallGravity = commonGravity;
        }
    }

    private void Update() {

    //Check if its on ground
    if(!afterJumpDelay){
    bool groundBox = Physics2D.OverlapBox(
        new Vector2(transform.position.x + groundDistanceXOffset, transform.position.y + groundDistanceYOffset), 
        new Vector2(col2d.size.x - 0.05f, groundDistance), 0f, groundLayerMask);

        isOnGround = groundBox;
    }
    
    
    } 
    
    virtual protected void FixedUpdate() {
        
        //Makes so the entity fall speed scales with its actual fall velocity, so it stays on the max jump height more time
        //But still falls fast after that
        if(isUsingSmoothFall){
            SmoothGravity();         
        }

        //If its on the ground, its not falling
        if(isOnGround){
            rb.gravityScale = commonGravity;
            isFalling = false;
        }
    }

    public void SmoothGravity(){
        if(!isOnGround){
            if(rb.velocity.y < 0){
                 rb.gravityScale = Mathf.Clamp((fallGravity * Mathf.Abs(rb.velocity.y)/gravityCurveScale), commonGravity, fallGravity);
            }else if(isFalling){
                rb.gravityScale = Mathf.Clamp((fallGravity * 16 * Mathf.Abs(rb.velocity.y)/gravityCurveScale), commonGravity, fallGravity);
            }
            else{
                 rb.gravityScale = Mathf.Clamp((commonGravity * Mathf.Abs(rb.velocity.y)/gravityCurveScale), commonGravity, fallGravity);
            }
        }
    }
    public void ChangeGravity(float gravityScale){
        commonGravity = gravityScale;
    }
    public void Jump(string caller = ""){
        isFalling = false;
        jumpCaller = caller;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce);
        isOnGround = false;
        StartCoroutine(AfterJump());
        
    }
    public void Jump(float jump, string caller = ""){
         isFalling = false;
         jumpCaller = caller;
         rb.velocity = new Vector2(rb.velocity.x, 0);
         rb.AddForce(Vector2.up * jump);
         isOnGround = false;
         StartCoroutine(AfterJump());
    }

    public void SetFalling(){
        isFalling = true;
    }

    public bool GetFalling(){
        return isFalling;
    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + groundDistanceXOffset, transform.position.y + groundDistanceYOffset), 
        new Vector2(col2d.size.x - 0.05f, groundDistance));
    }

    IEnumerator AfterJump(){
        afterJumpDelay = true;
        yield return new WaitForSeconds(0.05f);
        afterJumpDelay = false;
        yield return new WaitForSeconds(0.1f);
        jumpCaller = "";
    }
    
}
