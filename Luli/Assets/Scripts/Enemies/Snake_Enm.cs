using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Enm : MonoBehaviour
{
    //Smell area for snake
    Collider2D smellArea;
    public float range, xOffset, yOffset;
    public LayerMask playerLayerMask;

    //Player Reference
    GameObject player;

    //Enemy components
    Combat_Enm combat_Enm;
    Ent_Jump jump_enm;

    Rigidbody2D enmRb;

    public Animator enmAnimSprite;

    //Enemy Variables
    public float jumpForce;
    public float moveSpeed;

    Vector2 direction;

    bool isOnDamageCooldown;
    Coroutine cooldownCorout;

    public Particle jumpParticle;

    void Start()
    {

        combat_Enm = GetComponent<Combat_Enm>();
        jump_enm = GetComponent<Ent_Jump>();
        enmRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void Update() {

            isOnDamageCooldown = combat_Enm.hasTakenDamage;

            enmAnimSprite.SetBool("onDamage",isOnDamageCooldown);


    }
    void FixedUpdate()
    {
        smellArea = Physics2D.OverlapCircle(
            new Vector2(transform.position.x + xOffset, transform.position.y + yOffset), range, playerLayerMask);
        if(smellArea){

            if(!isOnDamageCooldown){
            SetPlayer();

            if(jump_enm.isOnGround){
                enmAnimSprite.SetBool("onJump",false);
                setDirection();
                if(combat_Enm.actualEnemyAttack != null){
                   combat_Enm.ResetAttack();
                }

                if(cooldownCorout == null)
                cooldownCorout = StartCoroutine(PrepareAttack(1f));

            }
        }else if(cooldownCorout != null){      
            StopCoroutine(cooldownCorout);
            cooldownCorout = null;

        }
            

        }else{
            player = null;
            /*if(cooldownCorout != null){
            StopCoroutine(cooldownCorout);
            cooldownCorout = null;
            }*/
        }

        
    }

    void SetPlayer(){
        //if(player == null){
        Collider2D[] pList = new Collider2D[1];
        ContactFilter2D pContact = new ContactFilter2D();
        pContact.SetLayerMask(playerLayerMask);

        Physics2D.OverlapCircle
        (new Vector2(transform.position.x + xOffset, transform.position.y + yOffset), range,pContact,pList);
        player = pList[0].gameObject;
        //}
        
    }

    float playerYDistance(){
        return player.transform.position.y - transform.position.y;
    }

    IEnumerator PrepareAttack(float time){
        
        enmRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(time);

        if(player != null && playerYDistance() > 1){
            jump_enm.Jump(jumpForce * enmRb.mass);
            enmRb.AddForce(direction * ( (moveSpeed/2) * enmRb.mass) );
        }else{
            jump_enm.Jump((jumpForce/2) * enmRb.mass);
            GetComponent<Rigidbody2D>().AddForce(direction * moveSpeed * enmRb.mass);
        }
        GameObject dustparticle = Instantiate(jumpParticle.gameObject, transform.position, Quaternion.identity);
        dustparticle.transform.localScale = transform.localScale;

        enmAnimSprite.SetBool("onJump",true);

        combat_Enm.Attack(0);

        cooldownCorout = null;
    }

    void setDirection(){
        if(player.transform.position.x < transform.position.x){
            direction = Vector2.left;
            transform.rotation = new Quaternion(0,0,0,0);
        }else{
            direction = Vector2.right;
            transform.rotation = new Quaternion(0,180,0,0);
        }
    }

    private void OnDrawGizmos() {
    Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(xOffset, yOffset), range);
    }
}
