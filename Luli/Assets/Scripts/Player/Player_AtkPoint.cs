using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AtkPoint : MonoBehaviour
{
    void Start()
    {
         StartCoroutine(lateStart());
    }

    public void SetAttackDirection(float dir){
        if(dir > 0){
        transform.localRotation = new Quaternion(0,0,0,0);
        transform.localPosition = new Vector2(Mathf.Abs(transform.localPosition.x), transform.localPosition.y);
        }else if (dir < 0){
        transform.localRotation = new Quaternion(0,-180,0,0);
        transform.localPosition = new Vector2(-Mathf.Abs(transform.localPosition.x), transform.localPosition.y);
        }
    }
    IEnumerator lateStart(){
        yield return new WaitForSeconds(0.1f);
        //Do late start things

        //Player_Move.playerWalking_event?.AddListener(SetAttackDirection);
    }
}
