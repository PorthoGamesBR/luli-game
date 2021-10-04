using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cutscene_Manager : MonoBehaviour
{
    public Dictionary<string,Cutscene_Actor> allActorsOnCutscene
    = new Dictionary<string,Cutscene_Actor>();

    public UnityEvent onCutsceneStart;
    public UnityEvent onCutsceneEnd;

    public void StartCutscene(Dictionary<string, Cutscene_Actor> actors){
        allActorsOnCutscene = actors;
        foreach(KeyValuePair<string, Cutscene_Actor> actor in actors){
            actor.Value.gameObject.SetActive(true);
        }
        onCutsceneStart.Invoke();
    }

    public void StartCutscene() {
        onCutsceneStart.Invoke();
    }

    public Cutscene_Actor getActorByName(string name){
        return allActorsOnCutscene[name];
    }

    /*public Cutscene_Frame GenerateFrame(int[] actions, string[] names){
        Cutscene_Frame newFrame = new Cutscene_Frame();
        int maxNum =  actions.Length > names.Length ? names.Length : actions.Length;
        
        for(int i = 0; i < maxNum; i++){
            AddActionBasedOnNumer(newFrame, allActorsOnCutscene[names[i]], actions[i]);
        }

        return newFrame;
    }

    void AddActionBasedOnNumer(Cutscene_Frame frame,Cutscene_Actor actor, int action){
        switch(action){
            case 0:
            break;
        }
    }*/

    public void PlayFrame(Cutscene_Frame frame){
        foreach(var action in frame.actionsToExecute){
            action.Invoke();
        }
    }

    public void EndCutscene(){
        foreach(KeyValuePair<string, Cutscene_Actor> actor in allActorsOnCutscene){
            actor.Value.gameObject.SetActive(false);
        }
        allActorsOnCutscene = new Dictionary<string,Cutscene_Actor>();
        onCutsceneEnd.Invoke();
    }
}
