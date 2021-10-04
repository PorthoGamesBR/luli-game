using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class FirstCutscene3 : MonoBehaviour
{
public Cutscene_Actor luli,teco;

    public GameObject playerObject;
    public GameObject counterObj;
    public Contdown_UI countdown;
    
    public Cutscene_Manager manager;

    //Cameras
    public CinemachineVirtualCamera camera1,camera2;

    List<Cutscene_Frame> frames = new List<Cutscene_Frame>();
    Queue<IEnumerator> executionOrder = new Queue<IEnumerator>();

    Cutscene_Frame actualFrame;

    public Dialogue_Manager dialogue_Manager;
    public Dialogue startingDialogue;

    public string thisStageName;
    public TextMeshProUGUI stageStartText;

    int actualDialogue = -1;

    void Start()
    {

    	dialogue_Manager.StartDialogue(startingDialogue);
        dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue);

        manager.StartCutscene();
        manager.PlayFrame(Frame1());
        executionOrder.Enqueue(WaitUntilDialogue(StartingDialogue(), 1));
         executionOrder.Enqueue(WaitUntilDialogue(Frame2(), 2));
         executionOrder.Enqueue(WaitUntilDialogue(Frame3(), 5));
         executionOrder.Enqueue(WaitUntilDialogue(Frame4(), 6));
        executionOrder.Enqueue(WaitUntilDialogue(Frame5(), 7));
         executionOrder.Enqueue(WaitUntilDialogue(Frame6(), 8));
         StartCoroutine(executionOrder.Dequeue());

    }

    public Cutscene_Frame Frame1(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addAction(() => playerObject.SetActive(false));
        
        return actualFrame;
    }

    public Cutscene_Frame StartingDialogue(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addAnim(luli, 2);
        return actualFrame;
    }

    public Cutscene_Frame Frame2(){
        actualFrame = new Cutscene_Frame();
        actualFrame.ChangeActiveCamera(camera2, camera1);
        return actualFrame;
    }

    public Cutscene_Frame Frame3(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addAnim(teco, 1);
        return actualFrame;
    }

    public Cutscene_Frame Frame4(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addSetActive(counterObj, true);
        return actualFrame;
    }

    public Cutscene_Frame Frame5(){
        actualFrame = new Cutscene_Frame();
        actualFrame.ChangeActiveCamera(camera1, camera2);
        return actualFrame;
    }


    public Cutscene_Frame Frame6(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addAction(() => playerObject.SetActive(true));
        actualFrame.addSetActive(luli.gameObject, false);
        if(stageStartText!= null){
            actualFrame.addAction(() => stageStartText.text = thisStageName);
            actualFrame.addSetActive(stageStartText.gameObject, true);
        }else{
            Debug.LogError("This Stage dont have a Stage Name TMPro");
        }
        actualFrame.addAction(() => countdown.StartCounter());
        actualFrame.addAction(() => SoundManager.getSoundManager().PlayMusicList(0,1f));
        actualFrame.addAction(() => manager.EndCutscene());
        actualFrame.changeCameraFocus(camera1, playerObject.transform);
        
        return actualFrame;
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
     IEnumerator WaitUntilActorGetsToDestination(Cutscene_Frame frame,Cutscene_Actor actor, Vector3 destination
    , System.Action toPlayAfter = null){
        yield return new WaitUntil(() => (actor.transform.position == destination));
        manager.PlayFrame(frame);
        /*if(toPlayAfter != null){
            toPlayAfter.Invoke();
        }*/
        if(executionOrder.Count > 0)
        yield return StartCoroutine(executionOrder.Dequeue());
    }

    IEnumerator TimeBetweenCutFrames(Cutscene_Frame frame, float time,
    System.Action toPlayAfter = null){
        yield return new WaitForSeconds(time);
        manager.PlayFrame(frame);
        /*if(toPlayAfter != null){
            toPlayAfter.Invoke();
        }*/
        if(executionOrder.Count > 0)
        yield return StartCoroutine(executionOrder.Dequeue());
    }
}
