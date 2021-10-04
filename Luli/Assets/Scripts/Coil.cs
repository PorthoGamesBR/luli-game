using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coil : MonoBehaviour
{
    public float coilForce = 10;

    public Animator scaleAnim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.CompareTag("Player") && other.TryGetComponent(out Ent_Jump jumpPlayer)){
            Debug.Log("Coil");
            jumpPlayer.Jump(coilForce * jumpPlayer.rb.mass, "Coil");

            scaleAnim?.SetTrigger("Active");
        }
    }
}
