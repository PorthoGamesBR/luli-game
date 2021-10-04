using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking_Enm : MonoBehaviour
{
   public bool isBlinking;
   public SpriteRenderer enmSprite;

   public float timeSinceLastBlink;
   public float delay;

    Color32 originColor;
    public Color32 blinkColor;
    void Start()
    {
        originColor = enmSprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBlinking){
            if(timeSinceLastBlink + delay >= Time.time){
                timeSinceLastBlink = Time.time;
                SwitchColors();
            }
        }
    }


    void SwitchColors(){
        if(enmSprite.color == originColor){
            enmSprite.color = blinkColor;
        }else{
            enmSprite.color = originColor;
        }
    }

    public void StartBlinking(){
        timeSinceLastBlink = Time.time;
        isBlinking = true;
    }

    public void StopBlinking(){
        isBlinking = false;
        enmSprite.color = originColor;
    }
}
