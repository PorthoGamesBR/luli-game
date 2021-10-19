using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Events;

public class Contdown_UI : MonoBehaviour
{
    TextMeshProUGUI counter;
    public float maxTime;
    float currentTime;

    public UnityEvent onCountEnd;
    bool hasEnded = true;
    bool isCounting;

    void Start()
    {
        counter = GetComponent<TextMeshProUGUI>();
        var ts = TimeSpan.FromSeconds(maxTime);
        counter.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    public void StartCounter(){
        currentTime = maxTime;
        isCounting = true;
        hasEnded = false;
    }

    public void StopCounter(){
        isCounting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > 0 && isCounting){

            currentTime -= 1 * Time.deltaTime;
            var ts = TimeSpan.FromSeconds(currentTime);
            counter.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            if(currentTime < 11){
                counter.color = Color.red;
            }else{
                counter.color = Color.white;
            }
        }else if(!hasEnded){
            isCounting = false;
            hasEnded = true;
            currentTime = 0;
            onCountEnd.Invoke();
        }
    }
}
