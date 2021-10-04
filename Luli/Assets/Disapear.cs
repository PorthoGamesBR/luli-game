using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disapear : MonoBehaviour
{
    public Animator animToDisapear;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            animToDisapear.SetBool("onContact", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            animToDisapear.SetBool("onContact", false);
        }
    }
}
