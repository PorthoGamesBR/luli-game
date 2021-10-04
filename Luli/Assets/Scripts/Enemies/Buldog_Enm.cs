using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buldog_Enm : Ent_Combat
{
    DetectPlayer playerDetect;
    Ent_Movement dogMovement;
    
    Vector2 startPos;
    public Vector2 minPos, maxPos;

    public float speed, runSpeed;
    float dir =-1;

    bool playerInArea, isAttacking, receivedDamage;

    Coroutine findPlayer, afterDamageCooldown;


    [Header("Combat")]
    public Vector2 pointOfDetect, sizeOfDetect;
    public Vector2 pointOfGround, sizeOfGround;
    public Animator spriteAnim;

    int damageTakenBeforeCooldown = 0;

    override public void Start()
    {
        base.Start();
        dogMovement = GetComponent<Ent_Movement>();
        playerDetect = GetComponent<DetectPlayer>();

        startPos = transform.position;
        if(maxPos.x < minPos.x){
            Vector2 min = minPos;
            minPos = maxPos;
            maxPos = min;
        }

    }


    public override void Damage(int ammount, Attack damager)
    {
        receivedDamage = true;
        if(afterDamageCooldown != null){
            StopCoroutine(afterDamageCooldown);
        }

        if(findPlayer!= null){
              StopCoroutine(findPlayer);
              findPlayer = null;
        }   

        if(Mathf.Sign(transform.position.x - damager.transform.position.x) == dir){
            FlipMoveDirection();
        }
        afterDamageCooldown = StartCoroutine(DamageCooldown());
        dogMovement.Stop();
        damageTakenBeforeCooldown+=ammount;
        if(damageTakenBeforeCooldown>2){
            entRecoil.ActivateRecoil(new Vector2(Mathf.Sign(transform.position.x - damager.transform.position.x),1));
            isInvencible = true;
            damageTakenBeforeCooldown = 0;
        }else{
            entRecoil.ActivateRecoil(new Vector2(Mathf.Sign(transform.position.x - damager.transform.position.x),0));
        }

        lifes-=ammount;
        if(lifes<=0){
            Die();
        }
        
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    private void Update() {
        if(receivedDamage){
            if(isAttacking){
                isAttacking = false;
            }

            
           spriteAnim.SetBool("Damage", true);
           
        }else{
            spriteAnim.SetBool("Damage", false);
        }
    }

    private void FixedUpdate() {
        
    if(!isAttacking && !receivedDamage){
         if(!playerDetect.wasPlayerDetected && !playerInArea){
        dogMovement.Move(dir,speed);
        spriteAnim.SetFloat("walkSpeed",1);
        if(transform.position.x > maxPos.x && dir == 1|| transform.position.x < minPos.x && dir == -1){
            FlipMoveDirection();
        }

        if(Physics2D.OverlapBox(transform.position, new Vector2(1f* dir, 1f),0f, LayerMask.GetMask("Ground"))){
            FlipMoveDirection();
        }
        }else if(playerDetect.wasPlayerDetected && !playerInArea){
        playerInArea = true;

        }else if(!playerDetect.wasPlayerDetected && playerInArea){
        if(findPlayer == null){
            findPlayer = StartCoroutine(FlipToFindPlayer());
        }
        }else if(playerDetect.wasPlayerDetected && playerInArea){
            if(findPlayer!= null){
              StopCoroutine(findPlayer);
              findPlayer = null;
            }

             if(AttackArea()){
            //Attack logic
            isAttacking = true;
            spriteAnim.SetTrigger("Attack");

           }else{
               spriteAnim.SetFloat("walkSpeed",3);
               dogMovement.Move(dir,runSpeed);
           }
        }
    }
   

    }

    public void FlipMoveDirection(){
        dir *= -1;
        playerDetect.xDirection = -dir;
        if(dir == 1){
            transform.rotation = new Quaternion(0,180,0,0);
        }else{
            transform.rotation = new Quaternion(0,0,0,0);
        }
        
    }

    bool AttackArea(){
        return Physics2D.OverlapBox
        (new Vector2(pointOfDetect.x * -dir, pointOfDetect.y) + (Vector2)transform.position, 
        sizeOfDetect, 0, playerDetect.playerLayer);
    }

    bool onGround(){
        return Physics2D.OverlapBox
        (pointOfGround + (Vector2)transform.position, sizeOfGround, 0, LayerMask.GetMask("Ground"));
    }
    public void Attack(){
        //Do attack summon here
        GameObject enemyAttack = Instantiate(attacks[0], attackPoint.transform.position, transform.rotation);
    }

    public void FinishAttack(){
        isAttacking = false;
    }

    IEnumerator FlipToFindPlayer(){
        int ammountOfTurns = 3;
        dogMovement.Stop();
        for(int i = 0; i <= ammountOfTurns; i++){
            FlipMoveDirection();
            yield return new WaitForSeconds(1f);
        }
        playerInArea = false;
        findPlayer = null;
    }

    IEnumerator DamageCooldown(){

        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => onGround());
        isInvencible = false;
        yield return new WaitForSeconds(1f);
        receivedDamage = false;
        damageTakenBeforeCooldown = 0;
        

    }


    protected void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayerDamage") && other.TryGetComponent(out Attack atk) && !isInvencible){
            Damage(atk.attackStrengh, atk);
            SoundManager.getSoundManager().PlaySound(Resources.Load("AttackSound") as AudioClip, 2f);
            
            /*TakeDamage(true);

            if(hasRecoil)
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
             
            entRecoil.ActivateRecoil(
                new Vector2(Mathf.Sign(transform.position.x - atk.transform.position.x), 1));

            Debug.Log(atk.attackStrengh + ":" + lifes);
            Damage(atk.attackStrengh, atk);
            
            StartCoroutine(ResetDamage());
            SoundManager.getSoundManager().PlaySound(Resources.Load("AttackSound") as AudioClip, 2f);*/
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(maxPos, 0.3f);
        Gizmos.DrawSphere(minPos, 0.3f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(pointOfDetect.x * -dir, pointOfDetect.y) + (Vector2)transform.position, sizeOfDetect);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pointOfGround + (Vector2)transform.position, sizeOfGround);
    }
}
