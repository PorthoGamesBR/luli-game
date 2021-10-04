using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHearth : Collectable{
    public override CollectableType thisType()
    {
        return CollectableType.EXTRA_LIFE;
    }

    public override void OnCollect()
    {
        Destroy(gameObject);
    }
}
