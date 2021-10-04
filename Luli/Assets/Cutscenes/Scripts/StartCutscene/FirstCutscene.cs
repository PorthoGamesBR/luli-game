using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class FirstCutscene : MonoBehaviour
{
    public Cutscene_Actor luli, dono;
    public GameObject playerObject;
    
    public Cutscene_Manager manager;

    //Cameras
    public CinemachineVirtualCamera camera1, camera2;

    List<Cutscene_Frame> frames = new List<Cutscene_Frame>();
    Queue<IEnumerator> executionOrder = new Queue<IEnumerator>();

    Cutscene_Frame actualFrame;

    public Dialogue_Manager dialogue_Manager;
    public Dialogue startingDialogue, secondDialogue;

    public string thisStageName;
    public TextMeshProUGUI stageStartText;

    int actualDialogue = -1;

    void Start()
    {
         Frame1();
         Frame2();
         Frame3();
         Frame4();
         Frame5();

        Dictionary<string, Cutscene_Actor> actorsInCutscene = new Dictionary<string, Cutscene_Actor>();
        actorsInCutscene.Add("Player", luli);
        actorsInCutscene.Add("Dono", dono);
        manager.StartCutscene(actorsInCutscene);
        manager.PlayFrame(frames[0]);

    	dialogue_Manager.StartDialogue(startingDialogue);
        dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue);

         executionOrder.Enqueue(WaitUntilDialogue(frames[1], 0));
         executionOrder.Enqueue(WaitUntilActorGetsToDestination(frames[2], dono,
         new Vector2(dono.transform.position.x + 5, dono.transform.position.y)));
         executionOrder.Enqueue(TimeBetweenCutFrames(frames[3], 2f));
         executionOrder.Enqueue(WaitUntilDialogue(frames[4], 1));
         StartCoroutine(executionOrder.Dequeue());

    }

    void Frame1(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addAction(() => camera2.Priority = 10);
        actualFrame.addAction(() => camera1.Priority = 9);
        actualFrame.addAction(() => playerObject.SetActive(false));
        actualFrame.changeCameraFocus(camera2, dono.transform);
        actualFrame.addFlip(luli, true);
        actualFrame.addAnim(dono, 1);
        Cutscene_Frame frame1 = actualFrame;
        frames.Add(frame1);
    }

    void Frame2(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addAnim(dono, 2);
        actualFrame.changeCameraFocus(camera2, null);
        actualFrame.addMoveWithTime(dono,
        new Vector2(dono.transform.position.x + 5, dono.transform.position.y), 5f);

        
        Cutscene_Frame frame2 = actualFrame;
        frames.Add(frame2);
    }

    void Frame3(){
        actualFrame = new Cutscene_Frame();
        actualFrame.changeCameraFocus(camera1, luli.transform);
        actualFrame.addAction(() => camera2.Priority = 9);
        actualFrame.addAction(() => camera1.Priority = 10);
        actualFrame.addAnim(luli, 3);
        actualFrame.addAction(() => dialogue_Manager.StartDialogue(secondDialogue));
        actualFrame.addAction(() => dialogue_Manager.onDialogueProgress.AddListener(onDialogueContinue));
        Cutscene_Frame frame3 = actualFrame;
        frames.Add(frame3);
    }

    void Frame4(){
        actualFrame = new Cutscene_Frame();
        actualFrame.addFlip(luli, false);
        actualFrame.addAnim(luli, 0);
        Cutscene_Frame frame4 = actualFrame;
        frames.Add(frame4);
    }

    void Frame5(){
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
