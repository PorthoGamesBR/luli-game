using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent_Recoil : MonoBehaviour
{
    public float recoilStrenght = 0.1f;
    public Rigidbody2D entRb;

    protected bool initiated;

    virtual public void Start(){
        if(entRb != null){
            initiated = true;
        }else{
            entRb = GetComponent<Rigidbody2D>();
        }
    }

   virtual public void InitializeRecoil(Rigidbody2D rb){
        entRb = rb;
        initiated = true;
    }

    virtual public void ActivateRecoil(Vector2 dir){
        if(initiated){
            entRb.AddForce(dir * recoilStrenght * entRb.mass);
        }
    }

    virtual public void ActivateRecoil(Vector2 dir, float strenght){
        if(initiated){
            entRb.AddForce(dir * strenght);
        }
    }

    
}
