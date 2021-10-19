using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
    bool askingToContinue;

    public GameObject alertBox;
    public string firstScene;
    [TextArea(2,3)]
    public string firstMessage;

    public int startingLifes;
    public Button continueButton;

    static bool firstTimeOppening = true;
    public Fade_UI fade;
    public GameObject languages;
    

    void Start()
    {
        StartCoroutine(lateStart());
        if(firstTimeOppening){
            fade.SetShadeActive(false);
            languages.SetActive(true);
            Time.timeScale = 0;
        }else{
            StartMenuMusic();
        }
        firstTimeOppening = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    public void StartMenuMusic(){
        Time.timeScale = 1;
        SoundManager.getSoundManager().PlayMusicList(0, 1f);
    }

    public void NewGame(){
        if(Player_ResManag.playerData != null && !askingToContinue){
            askingToContinue = true;
            alertBox.SetActive(true);
        }else{
            Player_ResManag.ConfigurePlayerData(0, startingLifes);
            Player_ResManag.TransitionToNewScene(firstScene, firstMessage);
        }

    }

    public void Return(){
        askingToContinue = false;
        alertBox.SetActive(false);
    }

    public void Continue(){
        SceneManager.LoadScene(Player_ResManag.playerData.lastStageUnlocked);
    }

    IEnumerator lateStart(){
        yield return new WaitForSeconds(0.1f);
        //Late start stuff
        if(Player_ResManag.playerData == null){
            continueButton.interactable = false;
        }
    }
}
