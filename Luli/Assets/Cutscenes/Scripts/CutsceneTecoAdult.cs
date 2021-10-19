using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutsceneTecoAdult : MonoBehaviour
{

    public GameObject player;

    public Cutscene_Actor luliActor, tecoActor, portaActor;

    public Collider2D door;

    int custceneIndex = 1;
    bool custceneStarted;
    //Igual a primeira cutscene
    public Cutscene_Manager manager;

    public Dialogue_Manager dialogue_Manager;
    public Dialogue tecoAndLuli3,tecoAndLuli4,tecoAndLuli5;
    int actualDialogue = -1;

    public CinemachineVirtualCamera camera1, camera2;

    List<Cutscene_Frame> frames = new List<Cutscene_Frame>();
    Queue<IEnumerator> executionOrder = new Queue<IEnumerator>();

    Cutscene_Frame actualFrame;

    Special_RatMission killRatMission;
    

    public void StartCutscene(){
        luliActor.transform.position = player.transform.position;
        luliActor.gameObject.SetActive(true);
        player.SetActive(false);
        killRatMission = player.AddComponent<Special_RatMission>();

        manager.StartCutscene();

        if(custceneIndex == 1){
        manager.PlayFrame(Frame1());

        dialogue_Manager.StartDialogue(tecoAndLuli3);
        dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue);
        
        executionOrder.Enqueue(WaitUntilDialogue(Frame2(),0));
        executionOrder.Enqueue(WaitUntilDialogue(Frame3(),1));
        executionOrder.Enqueue(WaitUntilDialogue(Frame4(),3));
        executionOrder.Enqueue(WaitUntilDialogue(Frame5(),4));
        executionOrder.Enqueue(WaitUntilDialogue(Frame6(),8));
        executionOrder.Enqueue(WaitUntilDialogue(Frame7(),9));
        executionOrder.Enqueue(WaitUntilDialogue(Frame8(),10));
        executionOrder.Enqueue(WaitUntilDialogue(FinalFrame1(),12));
        
        if(killRatMission.FinishedMission()){
            ContinueCutscene();
        }else{
            StartCoroutine(executionOrder.Dequeue());
        }
        }else{
        manager.PlayFrame(Frame9());

        dialogue_Manager.StartDialogue(tecoAndLuli4);
        dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue);

        executionOrder.Enqueue(WaitUntilDialogue(Frame10(),2));
        executionOrder.Enqueue(TimeBetweenCutFrames(Frame11(),2f));
        executionOrder.Enqueue(TimeBetweenCutFrames(FramePortaAbrindo(),3f));
        executionOrder.Enqueue(TimeBetweenCutFrames(FinishFrame(),2f));
        StartCoroutine(executionOrder.Dequeue());
        }

        
      
        
        

    }

    public void ContinueCutscene(){
        /*luliActor.transform.position = player.transform.position;
        luliActor.gameObject.SetActive(true);
        player.SetActive(false);
        
        manager.StartCutscene();

        manager.PlayFrame(ContinueFrame1());*/

        
        
        executionOrder.Enqueue(WaitUntilDialogue(ContinueFrame1(),0));
        executionOrder.Enqueue(WaitUntilDialogue(ContinueFrame2(),2));
        executionOrder.Enqueue(WaitUntilDialogue(ContinueFrame3(),3));
        executionOrder.Enqueue(WaitUntilDialogue(Frame10(),5));
        executionOrder.Enqueue(TimeBetweenCutFrames(Frame11(),2f));
        executionOrder.Enqueue(TimeBetweenCutFrames(FramePortaAbrindo(),3f));
        executionOrder.Enqueue(TimeBetweenCutFrames(FinishFrame(),2f));


        StartCoroutine(executionOrder.Dequeue());
    }

    //Custcene 1
#region
    Cutscene_Frame Frame1(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.changeCameraFocus(camera1, luliActor.transform);
        thisFrame.changeCameraFocus(camera2, tecoActor.transform);
        thisFrame.addAnim(luliActor, 2);
        return thisFrame;
    }

   Cutscene_Frame Frame2(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();       
        thisFrame.ChangeActiveCamera(camera2, camera1);
        return thisFrame;
    }

    Cutscene_Frame Frame3(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera1, camera2);
        return thisFrame;
    }

    Cutscene_Frame Frame4(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera2, camera1);
        thisFrame.addAnim(tecoActor, 1);
        return thisFrame;
    }

    Cutscene_Frame Frame5(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera1, camera2);
        
        return thisFrame;
    }

        Cutscene_Frame Frame6(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();       
       thisFrame.ChangeActiveCamera(camera2, camera1);
        return thisFrame;
    }

    Cutscene_Frame Frame7(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addAnim(tecoActor, 0);
        return thisFrame;
    }



      Cutscene_Frame Frame8(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera1, camera2);      
        return thisFrame;
    }


 #endregion
    //Cutscene 2
#region 
     Cutscene_Frame Frame9(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.changeCameraFocus(camera1, luliActor.transform);
        thisFrame.changeCameraFocus(camera2, tecoActor.transform);

        return thisFrame;
    }

    Cutscene_Frame Frame10(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();       
         thisFrame.ChangeActiveCamera(camera2, camera1);
         thisFrame.addAction(()=> Debug.Log("Called Frame 10"));
        return thisFrame;
    }

    Cutscene_Frame Frame11(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addAction(()=> Debug.Log("Called Frame 11"));       
       thisFrame.addAnim(tecoActor, 2);
        return thisFrame;
    }
#endregion
//Continuation
 #region 

        Cutscene_Frame ContinueFrame1(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.changeCameraFocus(camera1, luliActor.transform);
        thisFrame.changeCameraFocus(camera2, tecoActor.transform);
        return thisFrame;
    }

    Cutscene_Frame ContinueFrame2(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera2, camera1);
        return thisFrame;
    }

        Cutscene_Frame ContinueFrame3(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera1, camera2);
        return thisFrame;
    }

        Cutscene_Frame ContinueFrame0(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();

        return thisFrame;
    }

 #endregion

    Cutscene_Frame FramePortaAbrindo(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.changeCameraFocus(camera2, portaActor.transform);       
        //Animação porta abrindo
        thisFrame.addAnim(portaActor, 1);
        thisFrame.addAction(() => door.enabled = false);
        return thisFrame;
        }
 
    Cutscene_Frame FinalFrame1(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();

        if(killRatMission.FinishedMission()){
            thisFrame.addAction(()=> dialogue_Manager.StartDialogue(tecoAndLuli5));
            thisFrame.addAction(()=> dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue));
            thisFrame.addAction(()=>custceneIndex = 2);
            /*thisFrame.addAction(() => manager.EndCutscene());
            thisFrame.addAction(() => ContinueCutscene());*/
        }else{
            thisFrame = FinishFrame();
        }
        
        return thisFrame;
    }

     Cutscene_Frame FinishFrame(){
         Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera1, camera2);
        thisFrame.addAnim(luliActor, 0);
        thisFrame.addSetActive(player,true);
        thisFrame.changeCameraFocus(camera1, player.transform);
        thisFrame.addAction(() => player.transform.position = luliActor.transform.position);
        thisFrame.addSetActive(luliActor.gameObject,false);
        thisFrame.addAction(() => player.GetComponent<Player_Move>().ReactivateAllMove());
        thisFrame.addAction(() => manager.EndCutscene());
        thisFrame.addAction(() => EndThisCutscene());

        return thisFrame;
     }


    public void EndThisCutscene(){
        if(custceneIndex != 1)
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
            player.GetComponent<Player_Move>().DeactivateAllMove();
            StartCoroutine(WaitUntilPlayerOnGround());
        }else if(other.CompareTag("Player") && other.TryGetComponent(out Special_RatMission mission)){
            if(mission.FinishedMission()){
            killRatMission = mission;
            custceneIndex = 2;
            player = other.gameObject;
            player.GetComponent<Player_Move>().DeactivateAllMove();
            StartCoroutine(WaitUntilPlayerOnGround());
            }
            
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
