using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifes : MonoBehaviour
{
   public GameObject heart;
   public GameObject[] allHearts;
   public Transform startPos;

   int numberOfLifes;
   static int actualLifes;

   float boxHeight, boxWidth;

   public void Start(){
        boxWidth = this.GetComponent<RectTransform>().sizeDelta.x;
   }

    int CheckAllActives(){
        int ammount = 0;
        foreach(GameObject heart in allHearts){
            if(heart.activeSelf){
                ammount++;
            }
        }
        return ammount;
    }

   public void ConfigureHearts(int number){
       boxWidth = this.GetComponent<RectTransform>().sizeDelta.x;
       numberOfLifes = number;
       actualLifes = number;
    allHearts = new GameObject[numberOfLifes];
       for(int i = 0; i < numberOfLifes; i++){
           allHearts[i] = Instantiate(heart, startPos);
           allHearts[i].transform.SetParent(startPos.parent);
           startPos.localPosition = new Vector2
           (startPos.localPosition.x + boxWidth/numberOfLifes, startPos.localPosition.y);
           
       }
   }

   public void SetLifes(int lifes){
       if(lifes <= numberOfLifes && lifes >= 0){
        if(CheckAllActives() != lifes){
            //Organize active and inactive lifes
            if(CheckAllActives() < lifes){
                AddLifes(lifes - CheckAllActives());
            }else{
                LoseLifes(CheckAllActives() - lifes);
            }
        }
       }
   }

   public void AddLifes(int ammount){
       if(ammount + actualLifes <= numberOfLifes){
           for(int i = 0; i < ammount; i++){
               allHearts[actualLifes].SetActive(true);
               actualLifes += 1;
               
           }
           }else{
               foreach(GameObject heart in allHearts){
                   heart.SetActive(true);
            }
            actualLifes = ammount;
       }
   }

   public void LoseLifes(int ammount){
       if(ammount <= actualLifes){
           
           if(ammount < allHearts.Length){
                for(int i = ammount; i > 0; i--){
               actualLifes -= 1;
               allHearts[actualLifes].SetActive(false);
            }
        }
       }
   }
}
