using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock_Enm : MonoBehaviour
{
    Ent_Jump clockJump;

    public GameObject attack;
    GameObject attackActive;
    public float timeBetweenChanges;
    public float rangeToActivate;
    public float jumpForce;
    public LayerMask playerMask;

    bool hasStartedCountdown;
    bool hasAttacked;

    void Start()
    {
        clockJump = GetComponent<Ent_Jump>();
        
    }

    private void Update() {
        if(Physics2D.OverlapCircle(transform.position, rangeToActivate, playerMask) && !hasStartedCountdown){
            Debug.Log("Player in Area");

            Physics2D.IgnoreCollision(
                Physics2D.OverlapCircle(transform.position, rangeToActivate, playerMask).GetComponent<Collider2D>(),
                GetComponent<Collider2D>()
            );
            
            hasStartedCountdown = true;
            GetComponent<Animator>().SetTrigger("Active");
            
        }
        if(clockJump.isOnGround && hasAttacked){
           StartCoroutine(Cooldown());
           
        }

    }

    public void AttackNow(){
        clockJump.Jump(jumpForce);

        attackActive = Instantiate(attack, transform);
        attackActive.tag = "EnemyDamage";

        foreach(Transform child in attackActive.transform){
            child.tag = "EnemyDamage";
        }

        hasAttacked = true;
    }

    IEnumerator Cooldown(){
         hasAttacked = false;
        GetComponent<Animator>().SetTrigger("Inactive");
        Destroy(attackActive);        
        yield return new WaitForSeconds(timeBetweenChanges);
        hasStartedCountdown =false;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, rangeToActivate);
    }
}
