using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Collector : MonoBehaviour
{
    

    [SerializeField]
    private int bonesColected = 0;

    public UnityEvent<Collectable.CollectableType> onCollectSomething;
    public UnityEvent<string> onCollectSpecial;

    private void Start() {
        
    }

    public void AddBones(int ammount){
        bonesColected += ammount;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Collectable") && other.TryGetComponent(out SpecialCollectable collectable)){
            onCollectSpecial.Invoke(collectable.itemName);
            collectable.OnCollect();
        }
        else if(other.CompareTag("Collectable") && other.TryGetComponent(out Collectable collect)){
            CollectAction(collect.thisType());
            collect.OnCollect();
            
        }

        
    }

    private void CollectAction(Collectable.CollectableType type){
        if(type == Collectable.CollectableType.BONE)
        AddBones(1);
        if(type == Collectable.CollectableType.MEGA_BONE)
        AddBones(100);

        onCollectSomething.Invoke(type);
    }
}
