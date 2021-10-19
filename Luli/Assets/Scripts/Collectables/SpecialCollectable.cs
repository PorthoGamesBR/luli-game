using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCollectable : Collectable
{
    public string itemName;
    override public CollectableType thisType(){
        return CollectableType.SPECIAL;
    }

    public override void OnCollect()
    {
        Destroy(gameObject);
    }
}
