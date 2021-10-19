using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buldog_Boss : Ent_Combat
{
    DetectPlayer playerDetect;
    Ent_Movement dogMovement;

    public float speed;
    float originSpeed;

    float dir = -1;

    bool playerInArea, isAttacking, receivedDamage, isRushing, isStartingRush;

    public Vector2 wallDetectPoint, wallDetectSize;

    [Header("Combat")]
    public Vector2 pointOfDetect, sizeOfDetect;
    public Vector2 pointOfGround, sizeOfGround;
    public Animator spriteAnim;

    //Logic for Boss
    int ammountOfTurns = 3;
        int actualTurns = 0;
    bool outOfCombat;

    GameObject enemyAttack;

    override public void Start()
    {
        base.Start();
        maxLifes = lifes;

        dogMovement = GetComponent<Ent_Movement>();
        playerDetect = GetComponent<DetectPlayer>();

        StartCoroutine(Phases());
    }

    // Update is called once per frame
    void Update()
    {

     
        if (!outOfCombat)
        {

            if (!isAttacking)
            {
                dogMovement.Move(dir, speed);
                spriteAnim.SetFloat("walkSpeed",speed/2);
            }

            if (Physics2D.OverlapBox((Vector2)transform.position + new Vector2(wallDetectPoint.x * dir, wallDetectPoint.y),
               wallDetectSize, 0f, LayerMask.GetMask("Ground")))
            {
                
                if (!isRushing)
                {
                    if (actualTurns >= ammountOfTurns)
                    {
                        StartCoroutine(Rush());
                    }
                }
                else
                {
                    dogMovement.Stop();
                    outOfCombat = true;
                    isRushing = false;
                    speed = originSpeed;
                    spriteAnim.SetBool("Damage", true);
                    StartCoroutine(TimeOut());

                }

                FlipMoveDirection();
                actualTurns++;

            }

            if(playerDetect.playerDetected != null && Vector2.Distance(playerDetect.playerDetected.transform.position, 
                transform.position) < 2)
            {
                if (dir != Mathf.Sign(playerDetect.playerDetected.transform.position.x - transform.position.x))
                {
                    FlipMoveDirection();
                }
            }else if(playerDetect.playerDetected != null && Vector2.Distance(playerDetect.playerDetected.transform.position,
                transform.position) > 2)
            {
                playerDetect.playerDetected = null;
            }

            if (playerDetect.wasPlayerDetected && !isAttacking && !isStartingRush && !isRushing)
            {


                if (AttackArea())
                {
                    isAttacking = true;
                    spriteAnim.SetTrigger("Attack");
                    
                }
            }

        }

        spriteAnim.SetBool("Rushing", isStartingRush||isRushing);

    }

   

    public void FlipMoveDirection()
    {
        dir *= -1;
        playerDetect.xDirection = -dir;
        if (dir == 1)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

      
    }

    bool AttackArea()
    {
        return Physics2D.OverlapBox
        (new Vector2(pointOfDetect.x * -dir, pointOfDetect.y) + (Vector2)transform.position,
        sizeOfDetect, 0, playerDetect.playerLayer);
    }

    bool onGround()
    {
        return Physics2D.OverlapBox
        (pointOfGround + (Vector2)transform.position, sizeOfGround, 0, LayerMask.GetMask("Ground"));
    }

    public void Attack()
    {
        //Do attack summon here
        enemyAttack = Instantiate(attacks[0], attackPoint.transform.position, transform.rotation);
    }

    public void FinishAttack()
    {
        isAttacking = false;
    }

    public override void Damage(int ammount, Attack damager)
    {
        lifes -= ammount;
        if(lifes <= 0)
        {
            Die();
        }
    }

    public IEnumerator Phases()
    {
        yield return new WaitUntil(() => lifes <= maxLifes / 2);
        ammountOfTurns--;
        speed++;
        originSpeed++;
        yield return new WaitUntil(() => lifes <= maxLifes / 4);
        ammountOfTurns--;
        speed++;
        originSpeed++;
    }
    public IEnumerator Rush()
    {
        originSpeed = speed;
        isStartingRush = true;
        enemyAttack = Instantiate(attacks[1], attackPoint);
        while (speed < originSpeed + 2)
        {
            speed += 0.2f;
            actualTurns = 0;
            yield return new WaitForSeconds(0.3f);
        }
        if (speed >= originSpeed + 2)
        {
            
            isRushing = true;
            isStartingRush = false;
        }
        yield return null;
    }

    public IEnumerator TimeOut()
    {
        Destroy(enemyAttack);

        yield return new WaitForSeconds(5f);
        spriteAnim.SetBool("Damage", false);
        yield return new WaitForSeconds(1f);
        outOfCombat = false;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage") && other.TryGetComponent(out Attack atk) && !isInvencible)
        {
            Damage(atk.attackStrengh, atk);
            SoundManager.getSoundManager().PlaySound(Resources.Load("AttackSound") as AudioClip, 2f);

        }
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(pointOfDetect.x * -dir, pointOfDetect.y) + (Vector2)transform.position, sizeOfDetect);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(wallDetectPoint.x * dir, wallDetectPoint.y),
                wallDetectSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pointOfGround + (Vector2)transform.position, sizeOfGround);
    }
}
