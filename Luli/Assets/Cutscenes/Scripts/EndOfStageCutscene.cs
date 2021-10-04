using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfStageCutscene : MonoBehaviour
{
    public GameObject player;

    public Cutscene_Actor luliActor;

    public Cutscene_Manager manager;

    public Fade_UI fadeUI;

    bool cutsceneStarted;

    public string endStageIndex;

    public string toLoad;

    List<Cutscene_Frame> frames = new List<Cutscene_Frame>();
    Queue<IEnumerator> executionOrder = new Queue<IEnumerator>();


    public void StartCutscene(bool startScenes){
        fadeUI.reverse = true;

        manager.StartCutscene();

        if(startScenes){
        luliActor.transform.position = player.transform.position;
        luliActor.gameObject.SetActive(true);
        player.SetActive(false);

        manager.PlayFrame(Frame1());
        }
       
       executionOrder.Enqueue(TimeBetweenCutFrames(FinalCutscene(),2f));
       StartCoroutine(executionOrder.Dequeue()); 
    }

    public Cutscene_Frame Frame1(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addMoveWithTime(luliActor,new Vector2(luliActor.transform.position.x + 10, luliActor.transform.position.y),
        3f);
        thisFrame.addAnim(luliActor, 1);
        return thisFrame;
    }

    public Cutscene_Frame FinalCutscene(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addSetActive(luliActor.gameObject,false);
        thisFrame.addAction(() => manager.EndCutscene());
        thisFrame.addAction(() => Player_ResManag.ConfigurePlayerData(
            player.GetComponent<Player_Manager>().getBonesCollected(),
             Player_Combat._extraLifes));
        thisFrame.addAction(() => Player_ResManag.TransitionToNewScene(
            toLoad,Translation_Loader.LoadText("Transition",endStageIndex)));
        return thisFrame;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && Player_Move.isPlayerOnGround){
            player = other.gameObject;
            player.GetComponent<Player_Move>().DeactivateAllMove();
            StartCutscene(true);
            }else if(other.CompareTag("Player") && !Player_Move.isPlayerOnGround){
            player = other.gameObject;
            player.GetComponent<Player_Move>().DeactivateAllMove();
            StartCutscene(false);
            }
        
         cutsceneStarted = true;
        }

    IEnumerator TimeBetweenCutFrames(Cutscene_Frame frame, float time,
    System.Action toPlayAfter = null){
        yield return new WaitForSeconds(time);
        manager.PlayFrame(frame);
       
        if(executionOrder.Count > 0)
        yield return StartCoroutine(executionOrder.Dequeue());
    }
    }

   
