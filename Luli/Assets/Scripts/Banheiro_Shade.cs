using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banheiro_Shade : MonoBehaviour
{
    public float distanceToZero;
    public Vector2 playerPos;
    public Vector2 playerEntrancePoint;

    bool playerOnArea;
    bool playerInDeadZone;
    float maxDis;
    float deadX;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerOnArea){
            GetPlayerDeadZone();

            if(!playerInDeadZone){
            float actualDistance =  Mathf.Abs(playerPos.x - StartDeadZone());
            float actualAlpha = 2.55f *  PercentageConvertor(actualDistance, maxDis);

            GetComponent<SpriteRenderer>().color =
            new Color32(255, 255, 255, (byte)actualAlpha);
            }else{

                 GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
            }
            
            
        }
    }

    public float PercentageConvertor(float actualValue, float maxValue){
        float percentage = (actualValue * 100)/maxValue;
        return percentage;
    }


    public void CalculateMaxDistance(){
        maxDis = Mathf.Abs(playerEntrancePoint.x - StartDeadZone());
    }

    public void GetPlayerDeadZone(){
        if(playerPos.x > transform.position.x - distanceToZero &&
        playerPos.x < transform.position.x + distanceToZero){
            playerInDeadZone = true;
        }else{
            playerInDeadZone = false;
        }
    }

    public float StartDeadZone(){
        if(playerPos.x < transform.position.x){
            return transform.position.x - distanceToZero;
        }else{
            return transform.position.x + distanceToZero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            playerEntrancePoint = other.transform.position;
            playerPos = other.transform.position;
            playerOnArea = true;
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            CalculateMaxDistance();
        }
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player")){
            playerPos = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            playerPos = playerEntrancePoint;
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            playerOnArea = false;
        }
    }
}
