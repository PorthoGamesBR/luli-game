using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Recoil : Ent_Recoil
{
    public UnityEvent onHitGround;
    override public void Start()
    {
        base.Start();
    }

    public override void ActivateRecoil(Vector2 dir)
    {
        base.ActivateRecoil(dir);
        StartCoroutine(WaitUntilHitGround());
    }

    bool isNotFalling(){
        if(initiated){
            if(Mathf.Abs(entRb.velocity.y) != 0){
                return false;
            }else{
                return true;
            }       
        }else{
            return true;
        }
    }
    public IEnumerator WaitUntilHitGround(){
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(isNotFalling);
        onHitGround.Invoke();
    }
}
