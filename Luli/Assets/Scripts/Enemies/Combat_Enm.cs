using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Enm : Ent_Combat
{
    public bool hasTakenDamage;
    public float timeToReset = 1f;

    protected bool onAttack;

    public GameObject actualEnemyAttack;

    Coroutine attackTimeCorout;

    virtual public void Attack(int numberOfAttack){
        if(numberOfAttack < attacks.Length && !onAttack){
            actualEnemyAttack = Instantiate(attacks[numberOfAttack],attackPoint);
            actualEnemyAttack.tag = "EnemyDamage";
            foreach(Transform child in actualEnemyAttack.transform){
            child.tag = "EnemyDamage";
            }

            attackTimeCorout = StartCoroutine(OnAttackTime(attacks[numberOfAttack].GetComponentInChildren<Attack>().delayTime));
        }else if(numberOfAttack > attacks.Length){
            Debug.Log("Enemy " + this.name + " does not have attack n " + numberOfAttack);
        }
    }

    virtual public void PhysicalAttack(int numberOfAttack, float direction){
            if(numberOfAttack < attacks.Length && !onAttack){
            actualEnemyAttack = Instantiate(attacks[numberOfAttack],transform);
            float yDir = direction < 0? -180 : 0;
            actualEnemyAttack.transform.rotation =  new Quaternion(0,yDir, 0,0);
            actualEnemyAttack.tag = "EnemyDamage";

            foreach(Transform child in actualEnemyAttack.transform){
            child.tag = "EnemyDamage";
            }
            StartCoroutine(OnAttackTime(attacks[numberOfAttack].GetComponentInChildren<Attack>().delayTime));
        }else{
            Debug.Log("Enemy " + this.name + " does not have attack n " + numberOfAttack);
        }
    }
    

    virtual protected void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayerDamage") && other.TryGetComponent(out Attack atk) && !isInvencible){
            TakeDamage(true);

            if(hasRecoil)
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
             
            entRecoil.ActivateRecoil(
                new Vector2(Mathf.Sign(transform.position.x - atk.transform.position.x), 1));

            Debug.Log(atk.attackStrengh + ":" + lifes);
            Damage(atk.attackStrengh, atk);
            
            StartCoroutine(ResetDamage());
            SoundManager.getSoundManager().PlaySound(Resources.Load("AttackSound") as AudioClip, 2f);
        }
    }

    virtual public void TakeDamage(bool set){
        hasTakenDamage = set;
    }

    virtual public IEnumerator ResetDamage(){
        yield return new WaitForSeconds(timeToReset);
        hasTakenDamage = false;
    }

    virtual public IEnumerator OnAttackTime(float time){
        onAttack = true;
        yield return new WaitForSeconds(time);
        onAttack = false;
    }

    public void ResetAttack(){
        StopCoroutine(attackTimeCorout);
        Destroy(actualEnemyAttack);
        onAttack = false;
    }
}
