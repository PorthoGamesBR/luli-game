using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public enum CollectableType {BONE, EXTRA_LIFE, MEGA_BONE, SPECIAL}

    public abstract CollectableType thisType();

    public abstract void OnCollect();
}
