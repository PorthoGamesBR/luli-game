using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cutscene1_2Cats : MonoBehaviour
{
    public GameObject player;
    public Transform centerOfDialogue;

    public Cutscene_Actor luliActor, cat1Actor, cat2Actor;


    bool custceneStarted;
    //Igual a primeira cutscene
    public Cutscene_Manager manager;

    public Dialogue_Manager dialogue_Manager;
    public Dialogue dialogue1,dialogue2,dialogue3;
    int actualDialogue = -1;

    public CinemachineVirtualCamera camera1, camera2;

    List<Cutscene_Frame> frames = new List<Cutscene_Frame>();
    Queue<IEnumerator> executionOrder = new Queue<IEnumerator>();

    Cutscene_Frame actualFrame;
    
    public void StartCutscene(){
        luliActor.transform.position = player.transform.position;
        luliActor.gameObject.SetActive(true);
        player.SetActive(false);
        
        manager.StartCutscene();


        manager.PlayFrame(Frame1());

        dialogue_Manager.StartDialogue(dialogue1);
        dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue);
        
        executionOrder.Enqueue(WaitUntilDialogue(Frame2(),1));
        executionOrder.Enqueue(WaitUntilDialogue(Frame3(),2));
        executionOrder.Enqueue(WaitUntilDialogue(Frame4(),8));
        executionOrder.Enqueue(WaitUntilDialogue(Frame5(),9));

        executionOrder.Enqueue(TimeBetweenCutFrames(Frame6(),3f));
        executionOrder.Enqueue(WaitUntilDialogue(Frame7(),2));
        executionOrder.Enqueue(WaitUntilDialogue(Frame8(),5));
        executionOrder.Enqueue(WaitUntilDialogue(Frame9(),8));
        executionOrder.Enqueue(WaitUntilDialogue(Frame10(),9));
        executionOrder.Enqueue(WaitUntilDialogue(Frame11(),10));
        executionOrder.Enqueue(WaitUntilDialogue(Frame12(),12));


        executionOrder.Enqueue(TimeBetweenCutFrames(Frame13(),1f));
        executionOrder.Enqueue(WaitUntilDialogue(FinalFrame1(),3));
        
        StartCoroutine(executionOrder.Dequeue());
        

    }

    //Custcene 1
#region
    Cutscene_Frame Frame1(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.changeCameraFocus(camera1, luliActor.transform);
        thisFrame.addMoveWithTime(luliActor, new Vector3(
            luliActor.transform.position.x + 1,luliActor.transform.position.y,0),2f);
        thisFrame.addAnim(luliActor,1);
        return thisFrame;
    }

   Cutscene_Frame Frame2(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();       
        thisFrame.addAnim(luliActor,0);
        thisFrame.changeCameraFocus(camera2, centerOfDialogue);
        return thisFrame;
    }

    Cutscene_Frame Frame3(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera2, camera1);
        return thisFrame;
    }

    Cutscene_Frame Frame4(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
         thisFrame.ChangeActiveCamera(camera1, camera2);
        return thisFrame;
    }

    Cutscene_Frame Frame5(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera2, camera1);
        thisFrame.addFlip(cat1Actor,false);
        return thisFrame;
    }

    Cutscene_Frame Frame6(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addAction(() => dialogue_Manager.StartDialogue(dialogue2));
        thisFrame.addAction(()=> dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue));
        return thisFrame;
    }

    Cutscene_Frame Frame7(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addFlip(cat1Actor,true);
        return thisFrame;
    }

    Cutscene_Frame Frame8(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addFlip(cat1Actor,false);
        return thisFrame;
    }

    Cutscene_Frame Frame9(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addFlip(cat1Actor,true);
        return thisFrame;
    }

    Cutscene_Frame Frame10(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera1, camera2);
        return thisFrame;
    }
#endregion

 //Cutscene 2
 #region 

    Cutscene_Frame Frame11(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();       
        thisFrame.ChangeActiveCamera(camera2, camera1);
        return thisFrame;
    }

    Cutscene_Frame Frame12(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addAnim(cat1Actor, 1);
        thisFrame.addAnim(cat2Actor, 1);
        thisFrame.addMoveWithTime(cat1Actor, new Vector3(cat2Actor.transform.position.x + 10, cat1Actor.transform.position.y, 0),1f);
        thisFrame.addMoveWithTime(cat2Actor, new Vector3(cat2Actor.transform.position.x + 10, cat1Actor.transform.position.y, 0),1f);
        return thisFrame;
    }

    Cutscene_Frame Frame13(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera1, camera2);
        thisFrame.addAction(() => dialogue_Manager.StartDialogue(dialogue3));
        thisFrame.addAction(()=> dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue));
        return thisFrame;
    }
 #endregion

    Cutscene_Frame FinalFrame1(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addSetActive(player,true);
        thisFrame.changeCameraFocus(camera1, player.transform);
        thisFrame.addAction(() => player.transform.position = luliActor.transform.position);
        thisFrame.addSetActive(luliActor.gameObject,false);
        thisFrame.addAction(() => player.GetComponent<Player_Move>().ReactivateAllMove());
        thisFrame.addSetActive(cat1Actor.gameObject, false);
        thisFrame.addSetActive(cat2Actor.gameObject, false);
        thisFrame.addAction(() => manager.EndCutscene());
        thisFrame.addAction(() => EndThisCutscene());
        return thisFrame;
    }
    


    public void EndThisCutscene(){

     gameObject.SetActive(false);
     
        
    }
    public void onDialogueContinue(int dialogueIndex){
        actualDialogue = dialogueIndex;
    }


    IEnumerator WaitUntilDialogue(Cutscene_Frame frame, int dialogueInd){
        yield return new WaitUntil(() => actualDialogue == dialogueInd);
        manager.PlayFrame(frame);
        actualDialogue = -1;

        if(executionOrder.Count > 0)
        yield return StartCoroutine(executionOrder.Dequeue());
    }

    IEnumerator WaitUntilPlayerOnGround(){
        yield return new WaitUntil(() => Player_Move.isPlayerOnGround);
        Debug.Log("Started");
        StartCutscene();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") &&!custceneStarted){
            player = other.gameObject;
            custceneStarted = true;
            player.GetComponent<Player_Move>().Stop();
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
