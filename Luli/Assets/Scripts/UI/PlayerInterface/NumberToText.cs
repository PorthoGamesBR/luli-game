using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberToText : MonoBehaviour
{
    public TextMeshProUGUI numText;
    void Awake()
    {
        numText = GetComponent<TextMeshProUGUI>();
    }

   public void SetTextValue(int value){
       numText.text = value.ToString();
   }

   public void SetTextValue(double value){
       numText.text = value.ToString();
   }

   public void SetTextValue(float value){
       numText.text = value.ToString();
   }
}
