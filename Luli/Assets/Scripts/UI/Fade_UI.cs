using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade_UI : MonoBehaviour
{

public bool isActive = true;
    public bool reverse;
    public float speed;

    Image image;
    float actualAlpha = 255;
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive){
            if(!reverse && image.color.a > 0){
            
            actualAlpha -= Time.deltaTime * speed;
            image.color = new Color32(0,0,0,(byte)actualAlpha);            
        }else if(reverse && image.color.a < 1){
            image.raycastTarget = true;
            actualAlpha += Time.deltaTime * speed;
            image.color = new Color32(0,0,0,(byte)actualAlpha);
        }else if(image.color.a == 0){
            image.raycastTarget = false;
        }
        }
       
    }

    public void SetShadeActive(bool set){
        isActive = set;
    }
}
