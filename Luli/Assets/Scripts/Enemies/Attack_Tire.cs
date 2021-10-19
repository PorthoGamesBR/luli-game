using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Tire : Attack_Coil
{
    Collider2D groundCheck;

    public Vector2 point;
    public Vector2 size;
    public float velocity;

    public Particle groundParticle;
    override protected void Start()
    {
        
        
    }

    private void Update() {
        if(!Physics2D.OverlapBox(transform.position, new Vector2(50,50),0,LayerMask.GetMask("Player") )){
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(velocity, rb.velocity.y);

        groundCheck = Physics2D.OverlapBox((Vector2)transform.position + point, size,0,LayerMask.GetMask("Ground"));
        if(groundCheck){
            rb.AddForce(forceToJump * Vector2.up);
            GameObject dustparticle = Instantiate(groundParticle.gameObject, transform.position, Quaternion.identity);
            dustparticle.transform.localScale = transform.localScale;
        }
    }


    private void OnDrawGizmos() {
        Gizmos.DrawWireCube((Vector2)transform.position + point, size);
    }
}