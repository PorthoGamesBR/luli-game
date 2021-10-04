using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent_Combat : MonoBehaviour
{
    public int lifes = 1;
    protected int maxLifes;

    public Transform attackPoint;

    public GameObject[] attacks;

    public bool isInvencible;
    public float invencibleTimer = 0.5f;

    //The entity recoil should not be a necessity in every entity with combat. Create more subclasses godamnit
    public Ent_Recoil entRecoil;
    public bool hasRecoil;

    virtual public void Start()
    {
        maxLifes = lifes;
        
        if(entRecoil != null){
            hasRecoil = true;
        }else if(TryGetComponent(out Ent_Recoil recoil)){
            entRecoil = recoil;
            hasRecoil = true;
        }

    }

    virtual public void Damage(int ammount, Attack damager){
        if(!isInvencible){
            lifes -= ammount;
            AfterDamageInvencibility();
        if(lifes <= 0){
            Die();
        }
        }
        
    }

    virtual public void Die(){
        Destroy(this.gameObject);
    }

    virtual protected void AfterDamageInvencibility(){
        StartCoroutine(InvencibilityTime(invencibleTimer));
        isInvencible = true;
    }

    protected IEnumerator InvencibilityTime(float time){
        yield return new WaitForSeconds(time);
        isInvencible = false;
        
    }
}
