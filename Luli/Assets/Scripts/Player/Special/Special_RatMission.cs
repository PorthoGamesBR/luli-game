using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special_RatMission : MonoBehaviour
{
    public int ammountToKill;
    int numberOfRats;
    Move_Rat[] listOfRats;
    void Start()
    {
        listOfRats = GameObject.FindObjectsOfType<Move_Rat>();
        numberOfRats = listOfRats.Length;
    }

    public void SetUpMission(int ammount){
        ammountToKill = ammount;
        Start();
    }

    public bool FinishedMission(){
        listOfRats = GameObject.FindObjectsOfType<Move_Rat>();
        if(ammountToKill != 0){
            if(numberOfRats - listOfRats.Length > ammountToKill){
                return true;
            }else{
                return false;
            }
        }else{
            if(listOfRats.Length <= 0){
                return true;
            }else{
                return false;
            }
        }
    }

    
}
