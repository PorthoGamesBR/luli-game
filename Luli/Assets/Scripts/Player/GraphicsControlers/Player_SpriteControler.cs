using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_SpriteControler : MonoBehaviour
{
    public SpriteRenderer playerSprt;
    public Animator spriteAnim;

    Coroutine blinkingCoroutine;
    
    public float timeBlinking;
    bool isBlinking;

    public bool isOnCorner;

    public UnityEvent<string> onAnimationEnd;

    void Start()
    {
        playerSprt = GetComponent<SpriteRenderer>();
        spriteAnim = GetComponent<Animator>();

    }

    private void Update() {
        spriteAnim.SetFloat("Speed", Mathf.Abs(Player_Move.getRbHorSpeed()));
        spriteAnim.SetFloat("YSpeed", Player_Move.getRbVertSpeed());
            spriteAnim.SetBool("isInGround", Player_Move.isPlayerOnGround);
        spriteAnim.SetBool("isOnCorner", isOnCorner);
        

    }

    public void FlipSprite(float direction){
        if(direction > 0){
            playerSprt.flipX = false;
        }else if(direction < 0){
            playerSprt.flipX = true;
        }
    }


    /*IEnumerator lateStart(){
        yield return new WaitForSeconds(0.1f);
        //Do late start things

        Player_Move.playerWalking_event?.AddListener(FlipSprite);
    }*/

    public void StartBlinking(){
        if(!isBlinking){
            blinkingCoroutine = StartCoroutine(BlinkSprite(new Color32(255,255,255,100)));
        }
          
    }

    public void StopBlinking(){
        if(isBlinking){
            StopCoroutine(blinkingCoroutine);
        }
    }

    IEnumerator BlinkSprite(Color32 colorToBlink){
        
    yield return new WaitForSeconds(0.09f);
        isBlinking = true;
        if(Player_Combat._isInvencible){
         if(playerSprt.color.Equals(colorToBlink)){
            playerSprt.color = new Color32(255,255,255,255);
         }else{
             playerSprt.color = colorToBlink;
         }
        blinkingCoroutine = StartCoroutine(BlinkSprite(colorToBlink));

        }else{
            playerSprt.color = new Color32(255,255,255,255);
            isBlinking = false;
            yield return null;
        }
    }
        
    public void CallOnAnimEnd(string name){
        onAnimationEnd.Invoke(name);
    }
}
