using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCutsceneSystem : MonoBehaviour
{
    public Cutscene_Actor playerActor;
    public Cutscene_Manager manager;

    public Transform playerTransform;
    public Cinemachine.CinemachineVirtualCamera cv;

    void Start()
    {
        Cutscene_Frame frame1 = new Cutscene_Frame();
        frame1.addMoveWithTime
        (playerActor, new Vector2(playerActor.transform.position.x + 1, 
        playerActor.transform.position.y), 2f);
        frame1.addAnim(playerActor, 1);

        Cutscene_Frame frame2 = new Cutscene_Frame();
        frame2.addMove
        (playerActor, new Vector2(playerActor.transform.position.x - 5, 
        playerActor.transform.position.y));
        frame2.addAnim(playerActor, 0);
        frame2.changeCameraFocus(cv, playerTransform);

        Cutscene_Frame frame3 = new Cutscene_Frame();
        frame3.changeCameraFocus(cv, playerActor.transform);
        

        StartCoroutine(TimeBetweenCutFrames(frame1, 2f));
        StartCoroutine(WaitUntilActorGetsToDestination(frame2, playerActor, 
        new Vector2(playerActor.transform.position.x + 1, 
        playerActor.transform.position.y),
        () => StartCoroutine(TimeBetweenCutFrames(frame3, 2f))));

        ;

    }

    IEnumerator WaitUntilActorGetsToDestination(Cutscene_Frame frame,Cutscene_Actor actor, Vector3 destination
    , System.Action toPlayAfter = null){
        yield return new WaitUntil(() => (actor.transform.position == destination));
        manager.PlayFrame(frame);
        if(toPlayAfter != null){
            toPlayAfter.Invoke();
        }
    }

    IEnumerator TimeBetweenCutFrames(Cutscene_Frame frame, float time,
    System.Action toPlayAfter = null){
        yield return new WaitForSeconds(time);
        manager.PlayFrame(frame);
        if(toPlayAfter != null){
            toPlayAfter.Invoke();
        }
    }
}
