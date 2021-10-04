using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float delayTime = 0.1f;
    public int attackStrengh = 1;


    protected virtual void Start()
    {
        if(this.transform.parent.gameObject!= null){            
            Destroy(this.transform.parent.gameObject, delayTime);
        }else{
            Destroy(gameObject, delayTime);
        }
        
    }

}
