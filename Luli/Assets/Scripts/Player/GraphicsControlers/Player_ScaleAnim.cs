using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ScaleAnim : MonoBehaviour
{
 
    public Animator playerAnim;

    void Start()
    {
        playerAnim = GetComponent<Animator>();

        StartCoroutine(lateStart());
    }

    public void AnimJump(){ playerAnim.SetTrigger("Jump"); }
    public void AnimRun(){
        playerAnim.SetTrigger("Run");
    }
    IEnumerator lateStart(){
        yield return new WaitForSeconds(0.1f);
        //Do late start things

    }
}
