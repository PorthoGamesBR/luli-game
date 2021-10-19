using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring_Enm : MonoBehaviour
{
    public GameObject attack;

    public Vector3 placeToActivate;
    public float rangeToActivate;
     public LayerMask playerMask;

     bool hasStartedCountdown;

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCircle(transform.position + placeToActivate, rangeToActivate, playerMask) 
        && !hasStartedCountdown){
            hasStartedCountdown = true;
            GetComponent<Animator>().SetTrigger("Active");
        }
    }

    public void ActivateAttack(){
        Instantiate(attack, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position + placeToActivate, rangeToActivate);
    }
}
