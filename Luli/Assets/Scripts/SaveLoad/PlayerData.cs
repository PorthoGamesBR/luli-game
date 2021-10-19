using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public string lastStageUnlocked;
    public int bones;
    public int extraLifes;
    

    public PlayerData(string stage,int bns, int xtra)
    {
        lastStageUnlocked = stage;
        bones = bns;
        extraLifes = xtra;
    }

    
}
