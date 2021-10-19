using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class FirstCutscene2 : MonoBehaviour
{
public Cutscene_Actor luli;
    public GameObject playerObject;
    
    public Cutscene_Manager manager;

    //Cameras
    public CinemachineVirtualCamera camera1;

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
         Frame1();
         Frame3();
         Frame2();


        Dictionary<string, Cutscene_Actor> actorsInCutscene = new Dictionary<string, Cutscene_Actor>();
        actorsInCutscene.Add("Player", luli);
        manager.StartCutscene(actorsInCutscene);
        manager.PlayFrame(frames[0]);

    	dialogue_Manager.StartDialogue(startingDialogue);
        dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue);

         executionOrder.Enqueue(TimeBetweenCutFrames(frames[1], 0.5f));
         executionOrder.Enqueue(WaitUntilDialogue(frames[2], 2));
         StartCoroutine(executionOrder.Dequeue());

    }

    void Frame1(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addAction(() => playerObject.SetActive(false));

        Cutscene_Frame frame1 = actualFrame;
        frames.Add(frame1);
    }

    void Frame3(){
        actualFrame = new Cutscene_Frame();
        frames.Add(actualFrame);
    }
    void Frame2(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addAction(() => playerObject.SetActive(true));
        if(stageStartText!= null){
            actualFrame.addAction(() => stageStartText.text = thisStageName);
            actualFrame.addSetActive(stageStartText.gameObject, true);
        }else{
            Debug.LogError("This Stage dont have a Stage Name TMPro");
        }
        
        actualFrame.addAction(() => SoundManager.getSoundManager().PlayMusicList(0,1f));
        actualFrame.addAction(() => manager.EndCutscene());
        actualFrame.changeCameraFocus(camera1, playerObject.transform);
        frames.Add(actualFrame);
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
