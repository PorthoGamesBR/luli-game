using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Rat : Basic_Enm
{

    bool isMoveActivated;
    bool shouldMove = true;
    bool isAttacking;

    public float smellAreaX;
    public float smellAreaY;
    int playerLayerNum;
    bool playerInArea;
    ContactFilter2D playerFilter = new ContactFilter2D();

    private Coroutine attackCoroutine;

    Combat_Enm ratCombat;

    public Animator sprite_anim;
    GameObject player_object;

    override protected void Start()
    {
        base.Start();
        ratCombat = GetComponent<Combat_Enm>();
        playerFilter.SetLayerMask(LayerMask.GetMask("Player"));
    }

    private void Update() {
        if(MainManager.getMainPlayer() != null && 
        Mathf.Abs(MainManager.getMainPlayerPos().x - transform.position.x) < 11f){
            isMoveActivated = true;

        }else if(player_object != null){
            isMoveActivated = true;
        }
        
        else{
             isMoveActivated = false;
        }
    }

    override public void FixedUpdate()
    {
    FlipDir();
    
    if(isAttacking && !ratCombat.hasTakenDamage){
        enemyMove.Stop();
        animationSet("isWalking",false);
        animationSet("isAttacking",true);
    }

    if(isMoveActivated && !isAttacking && !ratCombat.hasTakenDamage){
        if(playerLayerNum == 0 && MainManager.getMainPlayer() != null){
            playerLayerNum = MainManager.getMainPlayer().layer;
        }
        /*Collider2D[] smellies = new Collider2D[1];
        playerInArea = Physics2D.OverlapCircle(transform.position, smellArea, playerFilter, smellies) > 0;*/
        Collider2D playerSmell = Physics2D.OverlapBox(
            new Vector2(transform.position.x, transform.position.y + smellAreaY/2),
            new Vector2(smellAreaX, smellAreaY),0,
            LayerMask.GetMask("Player"));
        

        if(playerSmell != null){
            player_object = playerSmell.gameObject;
           //Attack Logic
           if(Vector2.Distance(playerSmell.transform.position, transform.position) < 1.3f){

            xDir = new Vector2(Mathf.Sign(playerSmell.transform.position.x - transform.position.x), 0);
           attackCoroutine = StartCoroutine(TimeToAttack(0.45f)); 
           animationSet("isWalking",false);
           }else{
            xDir = new Vector2(Mathf.Sign(playerSmell.transform.position.x - transform.position.x), 0);
            enemyMove.Move(xDir.x, speed);  
            animationSet("isWalking",true);
           }
            }
        else{
            BasicMovement();
            animationSet("isWalking",true);
            
        }
        
    }else if(ratCombat.hasTakenDamage){
        if(ratCombat.actualEnemyAttack != null){
            Destroy(ratCombat.actualEnemyAttack);
        }
        if(shouldMove){

            if(attackCoroutine != null){
                StopCoroutine(attackCoroutine);
            }
            
            animationSet("isWalking",false);
            StartCoroutine(TimeOut(2f));
            }

    }
    animationSet("takeDamage",ratCombat.hasTakenDamage);
        
    }
    
    public void animationSet(string name, bool set){
        sprite_anim.SetBool(name, set);
    }
    IEnumerator TimeToAttack(float delay){
        animationSet("isAttacking",true); 
        isAttacking = true;
        Transform atkPoint = ratCombat.attackPoint;
        atkPoint.localPosition = new Vector2(xDir.x * Mathf.Abs(atkPoint.localPosition.x), atkPoint.localPosition.y);
        yield return new WaitForSeconds(delay);
        ratCombat.Attack(0);
        yield return new WaitForSeconds(0.8f);
        animationSet("isAttacking",false);
        isAttacking = false;
    }

    IEnumerator TimeOut(float time){
        GetComponent<Blinking_Enm>()?.StartBlinking();
        if(isAttacking == true){
            isAttacking = false;
            animationSet("isAttacking",false);
        }

        shouldMove = false;
        yield return new WaitUntil(() => !ratCombat.isInvencible);
        ratCombat.TakeDamage(false);
        shouldMove = true;
        GetComponent<Blinking_Enm>()?.StopBlinking();
    }
    
    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            new Vector2(transform.position.x, transform.position.y + smellAreaY/2), new Vector2(smellAreaX, smellAreaY));
        Gizmos.color = Color.white;
        Gizmos.DrawRay(new Vector2(transform.position.x + lookForColisionX, transform.position.y + lookForCollisionY)
        , xDir * 1);
    }
}
