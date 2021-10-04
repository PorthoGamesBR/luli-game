using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Enm : MonoBehaviour
{
    protected Ent_Movement enemyMove;

    public float lookForColisionX;
    public float lookForCollisionY;

    public Vector2 xDir = Vector2.left;
    public float speed = 1;

    public LayerMask colLayer;

    virtual protected void Start() {
    enemyMove = GetComponent<Ent_Movement>();
    }

    virtual public void FixedUpdate() {
        BasicMovement();
        FlipDir();
        
    }

    public void BasicMovement(){
        enemyMove.Move(xDir, speed);

        if(Physics2D.Raycast(new Vector2(transform.position.x + lookForColisionX, transform.position.y+lookForCollisionY)
        , xDir, 1, colLayer)){

            if(xDir == Vector2.left){
                xDir = Vector2.right;
            }else{
                xDir = Vector2.left;
            }
        }
    }

    public void BasicMovementWithouPhys(){
        enemyMove.MoveWithoutPhys(xDir.x, speed);

        if(Physics2D.Raycast(transform.position, xDir, 1, colLayer)){

            if(xDir == Vector2.left){
                xDir = Vector2.right;
            }else{
                xDir = Vector2.left;
            }
        }
    }

    public void FlipDir(){
        GetComponentInChildren<SpriteRenderer>().flipX = (xDir.x < 0);
    }


     private void OnDrawGizmos() {
        Gizmos.DrawRay(new Vector2(transform.position.x + lookForColisionX, transform.position.y + lookForCollisionY)
        , xDir * 1);
     }
}
