using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Actor : MonoBehaviour
{
    public Animator actorAnim;

    bool isMoving;
    int animationIndex;

    public void Move(Vector3 where){
        transform.position = where;
    }

    public void MoveWithTime(Vector3 where, float duration){
        StartCoroutine(MoveToPlace(where, duration));
    }

    public void SetAnimation(int index){
        actorAnim.Play(index.ToString(),0);
        animationIndex = index;
    }

    public void Flip(bool flip){
        GetComponent<SpriteRenderer>().flipX = flip;
    }

    public bool getMoving(){
        return isMoving;
    }

    public int getAnimationIndex(){
        return animationIndex;
    }


    public IEnumerator MoveToPlace(Vector3 to, float time){
        Vector3 startPos = this.transform.position;
        isMoving = true;
        float counter = 0;
        while(counter < time){
            counter += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, to, counter/time);
            yield return null;
        }
        isMoving = false;
    }
}
