using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    public static MainManager main;
    public GameObject mainPlayer;
    static GameObject _mainPlayer;
    static Vector2 _mainPlayerPos;

    //Game Over
    public GameObject gameOverUI;

    //Pause
    public float originTimeSpeed;
    public GameObject pauseMenu;
    public UnityEvent onPauseStart;
    public UnityEvent onPauseEnd;
    public KeyCode pauseKey;

    //Player UI
    public GameObject playerUI;

    //Camera
    public CinemachineVirtualCamera cineCamera;

    //Logic
    public static bool gameOver = false;
    public static bool onPause = false;
    public static bool onCutscene = false;


    void Start()
    {
        if(main != null){
            Destroy(this);
        }else{
            main = this;
        }

        gameOver = false;

        if(mainPlayer!= null){
            _mainPlayer = mainPlayer;
        }else{
        _mainPlayer = GameObject.FindGameObjectWithTag("Player");
        }
        
        SceneManager.sceneLoaded += OnLoadScene;
    }

    public static GameObject getMainPlayer(){
        return _mainPlayer;
    }

    public void OnLoadScene(Scene scene, LoadSceneMode mode){
        setMainPlayer();
    }

    public void setMainPlayer(){
        _mainPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    public static Vector2 getMainPlayerPos(){
        return _mainPlayerPos;
    }

    void Update()
    {
        if(!gameOver && !onCutscene && _mainPlayer){
             _mainPlayerPos = _mainPlayer.transform.position;

        }
        if(gameOver || onCutscene || onPause || !_mainPlayer){
            playerUI.SetActive(false);
        }else{
            playerUI.SetActive(true);
        }

        if(Input.GetKeyDown(pauseKey)){
            Pause();
        }
       
    }

    public void Cutscene(){
        onCutscene = !onCutscene;
    }



    public void Pause(){
        if(onPause == false){
            onPause = true;
            pauseMenu.SetActive(true);
            originTimeSpeed = Time.timeScale;
            Time.timeScale = 0;
            onPauseStart.Invoke();
        }else{
             onPause = false;
             pauseMenu.SetActive(false);
             Time.timeScale = originTimeSpeed;
             onPauseEnd.Invoke();
        }
        
    }


    public void GameOver(){      
        _mainPlayer = null;
        gameOver = true;
        cineCamera.Follow = null;
        gameOverUI.SetActive(true);
    }

    public void ResetStage(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void LoadMenu(){
        Time.timeScale = 1;
        onPause = false;
        onCutscene = false;
        gameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}
