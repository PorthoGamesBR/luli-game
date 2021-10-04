using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public Vector2 size, point;
    public float xDirection = 1;

    Collider2D[] playerDetector = new Collider2D[1];
    public LayerMask playerLayer;
    ContactFilter2D playerFilter = new ContactFilter2D();

    public GameObject playerDetected;
    public bool wasPlayerDetected;

    void Start()
    {
        playerFilter.SetLayerMask(playerLayer);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 finalPoint = (Vector2)transform.position +  new Vector2(point.x * xDirection, point.y);
        Vector2 finalSize = new Vector2(size.x , size.y);

        if(Physics2D.OverlapBox(finalPoint, finalSize,0f, playerLayer)){

            wasPlayerDetected = true;

            Physics2D.OverlapBox(finalPoint, finalSize, 0f,playerFilter, playerDetector);

            playerDetected = playerDetector[0].gameObject;
 
            
        }else{
            wasPlayerDetected = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube((Vector2)transform.position + 
        new Vector2(point.x * xDirection, point.y), new Vector2(size.x * xDirection, size.y));
    }

}
