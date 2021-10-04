using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_MusicManager : MonoBehaviour
{
    Dialogue_Manager diaManager;
    bool changedVolume;

    SoundManager sm;
    void Start()
    {
        diaManager = GetComponent<Dialogue_Manager>();
        sm = SoundManager.getSoundManager();
        StartCoroutine(waitToChangeDownVolume());
    }


    IEnumerator waitToChangeDownVolume(){
        yield return new WaitUntil(()=>diaManager.dialogueActive);
        StartCoroutine(changeSmooth(-0.05f, 0.3f));
        StartCoroutine(waitToChangeUpVolume());
    }

    IEnumerator waitToChangeUpVolume(){
        yield return new WaitUntil(()=>!diaManager.dialogueActive);
        StartCoroutine(changeSmooth(+0.05f, 1f));
        StartCoroutine(waitToChangeDownVolume());
    }

    IEnumerator changeSmooth(float ammount,float minmax){

    if(ammount < 0){
        while(sm.getVolume() > minmax){
            sm.setVolume(sm.getVolume() + ammount);
             yield return new WaitForSeconds(0.08f);
        }
    }
    if(ammount > 0){
        while(sm.getVolume() < minmax){
            sm.setVolume(sm.getVolume() + ammount);
             yield return new WaitForSeconds(0.08f);
        }
    }
        
    yield return null;
       

    }
}
