using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TransitionManager : MonoBehaviour
{
    string sceneName;
    string textToShow;



    public void ConfigureTransition(string text, string sceneToGo){
        GetComponent<TextMeshProUGUI>().text = text;
        sceneName = sceneToGo;
        GetComponent<Animator>().SetTrigger("Start");
    }

    public void OpenScene(){
        if(sceneName != null){
            SceneManager.LoadScene(sceneName);
        }else{
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
}
