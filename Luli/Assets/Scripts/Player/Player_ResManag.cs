using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_ResManag : MonoBehaviour
{
    public static int player_Bones;
    public static int player_ExtraLifes;


    static string sceneToLoad;
    static string messageToShow;

    public static PlayerData playerData;

    TransitionManager tm;

    void Start()
    {

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Resources");
        if(objs.Length > 1){
            Debug.Log("Deleting..." + this.name);
            Destroy(this.gameObject);
        }
        
        if(Save.LoadPlayer() != null){
            playerData = Save.LoadPlayer();
            ConfigurePlayerData(playerData.bones, playerData.extraLifes);
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += CheckScene;
        
    }

    public void CheckScene(Scene scene, LoadSceneMode mode){
        if(scene.name == "Transition"){
            tm = GameObject.Find("Transition").GetComponent<TransitionManager>();
            tm.ConfigureTransition(messageToShow, sceneToLoad);
        }
    }

    public static void ConfigurePlayerData(int bones, int extraLifes){
        player_Bones = bones;
        player_ExtraLifes = extraLifes;
    }
    
    public static void TransitionToNewScene(string sceneName, string text){
        if(sceneName != "MainMenu"){
        playerData = new PlayerData(sceneName, player_Bones, player_ExtraLifes);
        Save.SavePlayer(playerData);
        }

        sceneToLoad = sceneName;
        messageToShow = text;
        SceneManager.LoadScene("Transition");
        
    }
}
