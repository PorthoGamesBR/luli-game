using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle_Enm : MonoBehaviour
{
    Vector2 moveDirection;
    float speed = 1.5f;
    public float moveSpeed = 0.3f;
    public float timeToMove = 0.05f;
    float xDir = 1;
    public float descendDir = -8;

    bool isDescending;
    int ammountOfTurns;

    float maxHeight;

    public GameObject eagleAttack;
    public Transform attackPoint;

    SpriteRenderer enm_spr;
    public Sprite normal, descending;

    public void Start(){
        ActivateMovement(1);
        maxHeight = transform.localPosition.y;

        enm_spr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(xDir>0){
        transform.rotation = new Quaternion(0, 0,0,0); 
       }else if(xDir < 0){
        transform.rotation = new Quaternion(0, 180,0,0); 
       }
        
        if(isDescending){
            enm_spr.sprite = descending;
        }else{
            enm_spr.sprite = normal;
        }

        if(ammountOfTurns == 3){
            ammountOfTurns = 0;
            StartCoroutine(Descend());
        }
        if(!(transform.localPosition.y > maxHeight)){
            transform.position = Vector2.LerpUnclamped(transform.position, (Vector2)transform.position + moveDirection,
        Time.deltaTime * speed);
        }else{
            transform.localPosition = new Vector3(transform.localPosition.x, maxHeight);
        }
        
    }

    public void ActivateMovement(float dir){
        xDir = dir;
        moveDirection = new Vector2(0, descendDir);
        StartCoroutine(Descend());
        StartCoroutine(AcelerateHorizontal());
    
}

    /*public void ActivateDescend(){
    if(atZeroSpeed){
        moveDirection = new Vector2(0, descendDir);
        StartCoroutine(Descend());
    }
}*/
 
    IEnumerator Descend(){
        Attack();
        moveDirection = new Vector2(0, descendDir);
        while(moveDirection.y < descendDir*-1)
        {
 
            moveDirection = new Vector2(moveDirection.x,moveDirection.y += moveSpeed);
            yield return new WaitForSeconds(timeToMove);
            
            isDescending = true;
        }
        isDescending = false;
        moveDirection = new Vector2(moveDirection.x, 0);
        yield return null;
    }

    IEnumerator AcelerateHorizontal(){
    if(xDir > 0){
        while(moveDirection.x < descendDir * -1){

            if(Mathf.Abs(moveDirection.y) > 0.1 && isDescending){
                moveDirection = new Vector2(moveDirection.x += moveSpeed, moveDirection.y);
                yield return new WaitForSeconds(timeToMove);
            }else if(!isDescending){
                moveDirection = new Vector2(moveDirection.x += moveSpeed, moveDirection.y);
                yield return new WaitForSeconds(timeToMove);
            }else{
                yield return new WaitForSeconds(timeToMove);
            }
            
        }
    }else if(xDir < 0){
    while(moveDirection.x > descendDir ){
        if(Mathf.Abs(moveDirection.y) > 0.1 && isDescending){
                moveDirection = new Vector2(moveDirection.x -= moveSpeed, moveDirection.y);
                yield return new WaitForSeconds(timeToMove);
            }else if(!isDescending){
                moveDirection = new Vector2(moveDirection.x -= moveSpeed, moveDirection.y);
                yield return new WaitForSeconds(timeToMove);
            }else{
                yield return new WaitForSeconds(timeToMove);
            }
        }
    }
        
        StartCoroutine(DesacelerateHorizontal());
        yield return null;
    }

    IEnumerator DesacelerateHorizontal(){
    if(xDir > 0){
        while(moveDirection.x > 0){
            moveDirection = new Vector2(moveDirection.x -= moveSpeed, moveDirection.y);
            yield return new WaitForSeconds(timeToMove);
        }
    }else if(xDir < 0){
        while(moveDirection.x < 0){
            moveDirection = new Vector2(moveDirection.x += moveSpeed, moveDirection.y);
            yield return new WaitForSeconds(timeToMove);
        }
    }

    xDir = xDir * -1;
    ammountOfTurns += 1;
    StartCoroutine(AcelerateHorizontal());
    yield return null;
}

public void Attack(){
     GameObject actualEnemyAttack = Instantiate(eagleAttack,attackPoint);
            actualEnemyAttack.tag = "EnemyDamage";
            foreach(Transform child in actualEnemyAttack.transform){
            child.tag = "EnemyDamage";
            }       
}

private void OnTriggerEnter2D(Collider2D other) {
    if(other.CompareTag("PlayerDamage")){
        Destroy(transform.parent.gameObject);
    }
}
}
