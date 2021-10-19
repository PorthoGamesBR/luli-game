using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FinalPuppyCutscene : MonoBehaviour
{
    public GameObject player, bus;

    public Contdown_UI countdown;

    public Cutscene_Actor luliActor,luliOwnerActor;

    public Dialogue finalPuppy1, finalPuppy2;
    public Dialogue_Manager dialogue_Manager;

    public Cutscene_Manager manager;

    public CinemachineVirtualCamera camera1;

    public Fade_UI fadeUI;

    bool cutsceneStarted;

    public string endStageIndex;

    public string toLoad;

    bool custceneStarted;

    int actualDialogue = -1;

    List<Cutscene_Frame> frames = new List<Cutscene_Frame>();
    Queue<IEnumerator> executionOrder = new Queue<IEnumerator>();


    public void StartCutscene(){
        luliActor.transform.position = player.transform.position;
        luliActor.gameObject.SetActive(true);
        player.SetActive(false);
        

        manager.StartCutscene();
        manager.PlayFrame(Frame1());

        dialogue_Manager.StartDialogue(finalPuppy1);
        dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue);
        
       
       executionOrder.Enqueue(TimeBetweenCutFrames(Frame2(),4f));
       executionOrder.Enqueue(TimeBetweenCutFrames(Frame3(),3f));
       executionOrder.Enqueue(WaitUntilDialogue(Frame4(),9));
       executionOrder.Enqueue(TimeBetweenCutFrames(Frame5(),5f));
       executionOrder.Enqueue(TimeBetweenCutFrames(FinalCutscene(),18f));


       StartCoroutine(executionOrder.Dequeue()); 
    }

    public void onDialogueContinue(int dialogueIndex){
        actualDialogue = dialogueIndex;
    }

    public Cutscene_Frame Frame1(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addAction(() => countdown.StopCounter());
        thisFrame.addSetActive(countdown.gameObject, false);
        thisFrame.addMusic(null);
        thisFrame.changeCameraFocus(camera1, luliActor.transform);
        thisFrame.addMoveWithTime(luliActor,new Vector2(luliActor.transform.position.x + 32, luliActor.transform.position.y),
        4f);
        thisFrame.addAnim(luliActor, 4);
        thisFrame.addSetActive(bus, true);
        return thisFrame;
    }

    public Cutscene_Frame Frame2(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addSetMusic(1);
        thisFrame.addSetActive(luliActor.gameObject, false);
        thisFrame.addSetActive(luliOwnerActor.gameObject, true);
        return thisFrame;
    }

    public Cutscene_Frame Frame3(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addSetActive(bus, false);
        
         thisFrame.addAction(()=> dialogue_Manager.textSpeed = 0.1f);
        thisFrame.addAction(()=> dialogue_Manager.StartDialogue(finalPuppy2));
        thisFrame.addAction(()=> dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue));
        return thisFrame;
    }

    public Cutscene_Frame Frame4(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addAnim(luliOwnerActor, 1);
        thisFrame.addAction(()=> fadeUI.speed = 30);
        return thisFrame;
    }

    public Cutscene_Frame Frame5(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addAction(()=> fadeUI.reverse = true);
        return thisFrame;
    }



    public Cutscene_Frame FinalCutscene(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        
        thisFrame.addAction(() => manager.EndCutscene());
        thisFrame.addAction(() => Player_ResManag.ConfigurePlayerData(
            player.GetComponent<Player_Manager>().getBonesCollected(),
             Player_Combat._extraLifes));
        thisFrame.addAction(() => Player_ResManag.TransitionToNewScene(
            toLoad,Translation_Loader.LoadText("Transition",endStageIndex)));
        return thisFrame;
    }

        IEnumerator WaitUntilPlayerOnGround(){
        yield return new WaitUntil(() => Player_Move.isPlayerOnGround);
        Debug.Log("Started");
        StartCutscene();
    }

     IEnumerator WaitUntilDialogue(Cutscene_Frame frame, int dialogueInd){
        yield return new WaitUntil(() => actualDialogue == dialogueInd);
        manager.PlayFrame(frame);
        actualDialogue = -1;

        if(executionOrder.Count > 0)
        yield return StartCoroutine(executionOrder.Dequeue());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !custceneStarted){
            player = other.gameObject;
            custceneStarted = true;
            player.GetComponent<Player_Move>().DeactivateAllMove();
            StartCoroutine(WaitUntilPlayerOnGround());
        }
        
        }

    IEnumerator TimeBetweenCutFrames(Cutscene_Frame frame, float time,
    System.Action toPlayAfter = null){
        yield return new WaitForSeconds(time);
        manager.PlayFrame(frame);
       
        if(executionOrder.Count > 0)
        yield return StartCoroutine(executionOrder.Dequeue());
    }
    }

   
