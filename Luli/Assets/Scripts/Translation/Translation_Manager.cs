using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Translation_Manager : MonoBehaviour
{
    [SerializeField]
    public List<Translatable> allToTranslate;

    private void Start() {
      StartCoroutine(LateStart());
    }

    public void SetAllText(){
        foreach(Translatable trans in allToTranslate){
            if(trans.toTranslate.TryGetComponent<Text>(out Text uiText)){
                uiText.text = Translation_Loader.LoadText(SceneManager.GetActiveScene().name, trans.index.ToString());
            }else if(trans.toTranslate.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmpText)){
                tmpText.text = Translation_Loader.LoadText(SceneManager.GetActiveScene().name, trans.index.ToString());
            }
        }
    }

    public IEnumerator LateStart(){
        yield return new WaitForSeconds(0.1f);
          Translation_Loader.onChangeLanguage.AddListener(SetAllText);
          SetAllText();
    }

    [Serializable]
    public struct Translatable{
    public GameObject toTranslate;
    public int index;
    }

}
