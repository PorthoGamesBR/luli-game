using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buldog_Boss1 : Ent_Combat
{
    public int moveStage = 0;
    //0 = Movimento basico, 1 = Movimento para atacar player, 2 = Rush, 3 = Parado depois de rush

    DetectPlayer playerDetect;
    Ent_Movement dogMovement;
    public Animator spriteAnim;

    float dir = -1;
    public float speed;
    public Vector2 wallDetectPoint, wallDetectSize;

    //Combat
    GameObject actualAttack;
    bool isAttacking;
    public Vector2 pointOfDetect, sizeOfDetect;

    //Rushing
    bool isRushing;
    int numberOfWall;
    public int hitsOnTheWall = 3;

    public override void Start()
    {
        base.Start();

        maxLifes = lifes;

        dogMovement = GetComponent<Ent_Movement>();
        playerDetect = GetComponent<DetectPlayer>();
    }

    public void Update()
    {
        moveStage = 0;
        if (numberOfWall >= hitsOnTheWall)
        {
            moveStage = 2;
        }
        if (playerDetect.wasPlayerDetected || playerDetect.playerDetected != null)
        {
            moveStage = 1;
        }
        


        switch (moveStage)
        {
            case 0:
                BasicMovement();
                break;
            case 1:
                FollowPlayer();
                break;
            case 2:
                Rush();
                break;
            case 3:

                break;
        }
    }

    public void BasicMovement()
    {
        dogMovement.Move(dir, speed);

        if (Physics2D.OverlapBox((Vector2)transform.position + new Vector2(wallDetectPoint.x * dir, wallDetectPoint.y),
               wallDetectSize, 0f, LayerMask.GetMask("Ground")))
        {
            numberOfWall++;
            FlipMoveDirection();
        }
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

    public void FollowPlayer()
    {
    if(playerDetect.playerDetected != null)
        {
            float playerDirection = Mathf.Sign(playerDetect.playerDetected.transform.position.x - transform.position.x);

            if (playerDirection != dir)
            {
                FlipMoveDirection();
            }

            if (AttackArea() && !isAttacking)
            {
                isAttacking = true;
                spriteAnim.SetTrigger("Attack");
            }

            if(Vector2.Distance(playerDetect.playerDetected.transform.position, transform.position) > 2.5f)
            {
                playerDetect.playerDetected = null;
            }
        }
        
    }

    public void Attack()
    {
        //Do attack summon here
        actualAttack = Instantiate(attacks[0], attackPoint.transform.position, transform.rotation);
    }

    public void FinishAttack()
    {
        isAttacking = false;
        playerDetect.playerDetected = null;
    }

    bool AttackArea()
    {
        return Physics2D.OverlapBox
        (new Vector2(pointOfDetect.x * -dir, pointOfDetect.y) + (Vector2)transform.position,
        sizeOfDetect, 0, playerDetect.playerLayer);
    }

    public void Rush()
    {

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(wallDetectPoint.x * dir, wallDetectPoint.y),
                wallDetectSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(pointOfDetect.x * -dir, pointOfDetect.y) + (Vector2)transform.position, sizeOfDetect);
    }
}
