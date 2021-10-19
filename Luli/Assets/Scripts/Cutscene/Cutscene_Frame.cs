using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Frame
{
    List<Cutscene_Actor> actors;
    public List<System.Action> actionsToExecute = new List<System.Action>();

    
    public void addAction(System.Action action){
        actionsToExecute.Add(action);
    }

    public void addMove(Cutscene_Actor actor, Vector3 where){
        actionsToExecute.Add(() => actor.Move(where));
    }

    public void addMoveWithTime(Cutscene_Actor actor, Vector3 where, float duration){
        actionsToExecute.Add(() => actor.MoveWithTime(where, duration));
    }

    public void addAnim(Cutscene_Actor actor, int index){
         actionsToExecute.Add(() => actor.SetAnimation(index));
    }

    public void addFlip(Cutscene_Actor actor, bool flip){
         actionsToExecute.Add(() => actor.Flip(flip));
    }

    public void changeCameraFocus(Cinemachine.CinemachineVirtualCamera virtualCamera, Transform focus){
        actionsToExecute.Add(() => virtualCamera.Follow = focus);
    }

    public void ChangeActiveCamera(Cinemachine.CinemachineVirtualCamera toActivate, 
    Cinemachine.CinemachineVirtualCamera toDeactivate){
        int maxPriority = toDeactivate.Priority;
        int lessPriority = toActivate.Priority;

        actionsToExecute.Add(() => toActivate.Priority = 10);
       actionsToExecute.Add(() => toDeactivate.Priority = 9);
    }
    
    public void addSetActive(GameObject gameObject, bool active){
         actionsToExecute.Add(() => gameObject.SetActive(active));
    }

    public void addMusic(AudioClip music){
        actionsToExecute.Add(()=>SoundManager.getSoundManager().PlayMusic(music,1f));
    }

    public void addSetMusic(int index){
        actionsToExecute.Add(()=>SoundManager.getSoundManager().PlayMusicList(index, 1f));
    }


}
