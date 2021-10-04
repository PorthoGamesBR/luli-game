using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Bone : Collectable
{
    public override CollectableType thisType()
    {
        return CollectableType.BONE;
    }
    public override void OnCollect()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
        GetComponent<Animator>().SetTrigger("Collected");
    }

    public void AutoDestroy(){
        Destroy(gameObject);
    }
}
