using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent_Movement : MonoBehaviour
{
    //VARIABLE DECLARATION

    //Logic

    //Rigidbody and Physics
    [Header("Components")]
    public Rigidbody2D rb;

    [Header("Characteristics")]

    [SerializeField]
    private bool usesRb;
    [SerializeField]
    private Vector2 moveDirection;

    private float actualVelocity;
    public float maxVelocity;


    virtual protected void Start()
    {
        //Check if Rigidbody exists
        if(rb != null || GetComponent<Rigidbody2D>() != null){
            usesRb = true;
            //If the variable hasnt been initialized, get the RB in the object
            if(rb == null){
                rb = GetComponent<Rigidbody2D>();
            }
        }
    }

    virtual protected void FixedUpdate() {
        if(usesRb){
        //Get the actual horizontal speed of the entity
        actualVelocity = rb.velocity.x;
        }
        
    }

    public void MoveWithoutPhys(float dir, float speed){
        transform.position = Vector3.Lerp (transform.position, transform.position + new Vector3(dir,0), 
        Time.deltaTime * speed);
    }

    public void Move(float dir, float speed){
        if(usesRb && speed <= maxVelocity){
        moveDirection = new Vector2(dir, rb.velocity.y);
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
        }  
    }

    public void MoveIgnoreMax(float dir, float speed){
        moveDirection = new Vector2(dir, rb.velocity.y);
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }

     public void Move(Vector3 dir, float speed = 1){
        if(usesRb){
        Move(dir.x, speed);
        }  
    }
    public void MoveRight(float speed = 1){
        Move(Vector2.right.x, speed);
    }
    public void MoveLeft(float speed = 1){
        Move(Vector2.left.x, speed);
    }

    public void Acelerate(float dir, float baseSpeed, float speedOfAcel){
        
        //Make the player start at some speed
        if(Mathf.Abs(rb.velocity.x) < baseSpeed){
            Move(dir, baseSpeed);
        }

        //Gets what direction the player is going now
        Vector2 nowDirection = new Vector2(dir, rb.velocity.y); ;

        //If is different than the last one, the player is turning so change direction
        if(nowDirection.x != moveDirection.x && rb.velocity.x != 0 && dir != 0){
            ChangeDir(dir);  
        }

         //If the player isnt going above the speed limit
            if(Mathf.Abs(rb.velocity.x) <= maxVelocity && dir!= 0){
            //And then add the aceleration
            rb.AddForce(new Vector2(dir*speedOfAcel, 0));
            moveDirection = new Vector2(dir, rb.velocity.y);
            }else{
            //If the player is at max speed, just keep the same speed
            Move(dir, maxVelocity);
            moveDirection = new Vector2(dir, rb.velocity.y);
            }
         
    }

    public void ChangeDir(float dir){
        rb.velocity = new Vector2(dir * Mathf.Abs(rb.velocity.x), rb.velocity.y);
    }

public void Stop(){
    if(usesRb){
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    
}

public void StopAcel(float deceleration){
    if(rb.velocity.x != 0){
        if(Mathf.Abs(rb.velocity.x) < 0.6f){
            rb.velocity = new Vector2(0, rb.velocity.y );
        }else{
             rb.velocity = new Vector2(rb.velocity.x + ((deceleration * Time.deltaTime) * -Mathf.Sign(rb.velocity.x)), rb.velocity.y );
        }
       
    }
    
}
    public Vector2 GetMovingDir(){
        return moveDirection;
    }

    public Vector3 getMovingDir(){
        return (Vector3)moveDirection;
    }
}
