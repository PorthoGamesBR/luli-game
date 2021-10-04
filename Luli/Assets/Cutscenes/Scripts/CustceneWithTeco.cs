using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CustceneWithTeco : MonoBehaviour
{

    public GameObject player, fishToMission;

    public Cutscene_Actor luliActor, tecoActor, portaActor;

    public Collider2D door;

    int custceneIndex = 1;
    bool custceneStarted;
    //Igual a primeira cutscene
    public Cutscene_Manager manager;

    public Dialogue_Manager dialogue_Manager;
    public Dialogue tecoAndLuli, tecoAndLuli2;
    int actualDialogue = -1;

    public CinemachineVirtualCamera camera1, camera2;

    List<Cutscene_Frame> frames = new List<Cutscene_Frame>();
    Queue<IEnumerator> executionOrder = new Queue<IEnumerator>();

    Cutscene_Frame actualFrame;
    
    void Start()
    {
        /*frames.Add(Frame1());
        frames.Add(Frame2());
        frames.Add(Frame3());
        frames.Add(Frame4());
        frames.Add(Frame5());
        frames.Add(Frame6());
        frames.Add(Frame7());
        frames.Add(Frame8());
        frames.Add(Frame9());
        frames.Add(Frame10());*/

    	
    }

    public void StartCutscene(){
        luliActor.transform.position = player.transform.position;
        luliActor.gameObject.SetActive(true);
        player.SetActive(false);
        
        manager.StartCutscene();

        if(custceneIndex == 1){
        manager.PlayFrame(Frame1());

        dialogue_Manager.StartDialogue(tecoAndLuli);
        dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue);
        
        executionOrder.Enqueue(WaitUntilDialogue(Frame2(),0));
        executionOrder.Enqueue(WaitUntilDialogue(Frame3(),2));
        executionOrder.Enqueue(WaitUntilDialogue(Frame4(),5));
        executionOrder.Enqueue(WaitUntilDialogue(Frame5(),6));
        executionOrder.Enqueue(WaitUntilDialogue(Frame6(),8));
        executionOrder.Enqueue(WaitUntilDialogue(Frame7(),10));
        executionOrder.Enqueue(WaitUntilDialogue(Frame8(),11));
        executionOrder.Enqueue(WaitUntilDialogue(Frame9(),12));
        executionOrder.Enqueue(WaitUntilDialogue(Frame10(),13));
        executionOrder.Enqueue(WaitUntilDialogue(FinalFrame1(),14));
        
        }
        if(custceneIndex == 2){
        manager.PlayFrame(Frame11());

        dialogue_Manager.StartDialogue(tecoAndLuli2);
        dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue);

        executionOrder.Enqueue(WaitUntilDialogue(Frame12(),2));
        executionOrder.Enqueue(TimeBetweenCutFrames(Frame13(),2f));
        executionOrder.Enqueue(TimeBetweenCutFrames(Frame14(),3f));
        executionOrder.Enqueue(TimeBetweenCutFrames(FinalFrame2(),2f));
        }

        StartCoroutine(executionOrder.Dequeue());
        

    }

    //Custcene 1
#region
    Cutscene_Frame Frame1(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.changeCameraFocus(camera2, tecoActor.transform);
        return thisFrame;
    }

   Cutscene_Frame Frame2(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();       
        thisFrame.ChangeActiveCamera(camera2, camera1);
        return thisFrame;
    }

    Cutscene_Frame Frame3(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        
        return thisFrame;
    }

    Cutscene_Frame Frame4(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
    //Mudar animação do Teco aqui
        thisFrame.addAnim(tecoActor, 1);
        return thisFrame;
    }

    Cutscene_Frame Frame5(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.changeCameraFocus(camera1, luliActor.transform);
        thisFrame.ChangeActiveCamera(camera1, camera2);
        //Mudar animação luli
        thisFrame.addAnim(luliActor, 2);
        return thisFrame;
    }

    Cutscene_Frame Frame6(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera2, camera1);
        thisFrame.addAnim(tecoActor, 0);
        return thisFrame;
    }

    Cutscene_Frame Frame7(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera1, camera2);
        return thisFrame;
    }

    Cutscene_Frame Frame8(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.ChangeActiveCamera(camera2, camera1);
        return thisFrame;
    }

    Cutscene_Frame Frame9(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addAnim(tecoActor, 1);
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
        thisFrame.changeCameraFocus(camera2, tecoActor.transform);
        return thisFrame;
    }

    Cutscene_Frame Frame12(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();       
         thisFrame.ChangeActiveCamera(camera2, camera1);
        return thisFrame;
    }

    Cutscene_Frame Frame13(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();       
       thisFrame.addAnim(tecoActor, 2);
        return thisFrame;
    }

    Cutscene_Frame Frame14(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.changeCameraFocus(camera2, portaActor.transform);       
        //Animação porta abrindo
        thisFrame.addAnim(portaActor, 1);
        thisFrame.addAction(() => door.enabled = false);
        return thisFrame;
    }

 #endregion

    Cutscene_Frame FinalFrame1(){
        Cutscene_Frame thisFrame = new Cutscene_Frame();
        thisFrame.addAnim(luliActor, 0);
        thisFrame.addSetActive(player,true);
        thisFrame.addSetActive(fishToMission, true);
        thisFrame.changeCameraFocus(camera1, player.transform);
        thisFrame.addAction(() => player.transform.position = luliActor.transform.position);
        thisFrame.addSetActive(luliActor.gameObject,false);
        thisFrame.addAction(() => player.GetComponent<Player_Move>().ReactivateAllMove());
        thisFrame.addAction(() => manager.EndCutscene());
        thisFrame.addAction(() => EndThisCutscene());
        return thisFrame;
    }
    
    Cutscene_Frame FinalFrame2(){
          Cutscene_Frame thisFrame = new Cutscene_Frame();
          thisFrame.ChangeActiveCamera(camera1, camera2);
          thisFrame.addSetActive(player,true);
          thisFrame.changeCameraFocus(camera1, player.transform);
          thisFrame.addSetActive(luliActor.gameObject,false);
          thisFrame.addAction(() => player.GetComponent<Player_Move>().ReactivateAllMove());
          thisFrame.addAction(() => manager.EndCutscene());
          thisFrame.addAction(() => EndThisCutscene());
          return thisFrame;
    }

    public void EndThisCutscene(){
        if(custceneIndex == 1){
        }else{
            gameObject.SetActive(false);
        }
        
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
        }else if(other.CompareTag("Player") && Player_Manager.getIfPlayerCollected("Fish")){
            custceneIndex = 2;
            player = other.gameObject;
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
