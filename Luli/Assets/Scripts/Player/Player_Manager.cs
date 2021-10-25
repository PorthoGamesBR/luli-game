using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public NumberToText bones_text;

    public Player_Move playerMove;
   
    Player_Recoil playerRecoil;

    Player_ScaleAnim scaleAnim;
    Player_SpriteControler playerSprt;

    private int bonesCollected = 0;

    bool runSingle = true;
    bool runSound;

    public float frontXOffset, frontYOffset;
    float xDirection = 1;
    public Vector2 cornerDetectPosition;


    static Dictionary<string, int> specialsCollected = new Dictionary<string, int>();

    public AudioClip runningSound;

    
    public Particle runDust, fallParticle;

    //Player Combat
     public Player_Combat playerCombat;
     public Lifes ui_lifes;
     public NumberToText ui_extraLifes;
     int _playerLifes;

    public void Start(){
      StartCoroutine(LateStart());  
      scaleAnim = GetComponentInChildren<Player_ScaleAnim>();
      playerSprt = GetComponentInChildren<Player_SpriteControler>();

      _playerLifes = playerCombat.lifes;
      ui_lifes.ConfigureHearts(_playerLifes);
    }

    public IEnumerator LateStart(){
        yield return new WaitForSeconds(0.1f);
        bonesCollected = Player_ResManag.player_Bones;
        playerCombat.setExtraLifes(Player_ResManag.player_ExtraLifes);
    }

    private void Update() {

        //Player UI
        bones_text?.SetTextValue(bonesCollected);
        _playerLifes = playerCombat.lifes;
        ui_lifes.SetLifes(_playerLifes);
        ui_extraLifes.SetTextValue(playerCombat.extraLifes);

        if(playerMove.runPressed && Mathf.Abs(playerMove.xMove) > 0 && playerMove.playerJump.isOnGround){

        StartCoroutine(RunningSound());
            if(!runSingle){
            Debug.Log("Run");
            runSingle = true;
            scaleAnim.AnimRun();

            GameObject rParticle = Instantiate(runDust.gameObject, transform.position, Quaternion.identity);     
            //rParticle.transform.localScale = transform.GetChild(0).GetChild(0).localScale;
            if(xDirection < 0){
                rParticle.GetComponent<SpriteRenderer>().flipX = true;
            }
            }
                  
        }else if(!playerMove.runPressed && runSingle){
            runSingle = false;
        }

        playerSprt.isOnCorner = PlayerOnCorner();
        
    }

    public void FallParticle(string animationName){
        Debug.Log(animationName);
        if(animationName == "Fall"){
            GameObject rParticle = Instantiate(fallParticle.gameObject, transform.position, Quaternion.identity);
            //rParticle.transform.localScale = transform.GetChild(0).GetChild(0).localScale;
        }
        
    }

    bool PlayerOnCorner(){

        if(Mathf.Abs(playerMove.xMove) > 0){
            xDirection = Mathf.Sign(playerMove.xMove);
        }
        cornerDetectPosition = new Vector2(transform.position.x + (frontXOffset * xDirection)
            , transform.position.y+frontYOffset);
        
        if(Physics2D.Raycast(
           cornerDetectPosition, Vector2.down,0.2f,
            LayerMask.GetMask("Ground"))){

            return false;
        }else{
            return true;
        }
    }

    public IEnumerator RunningSound(){
        if(!runSound){
            runSound= true;
             SoundManager.getSoundManager().PlaySound(runningSound, 0.5f);
             yield return new WaitForSeconds(runningSound.length);
             runSound = false;
        }else{
            yield return null;
        }
    }

    public void onAttack(){
        if(playerMove.playerJump.isOnGround){
            playerMove.LockInput(true, playerCombat.playerAttack.delayTime);
            playerMove.DeactivateAllMove(playerCombat.playerAttack.delayTime);

        }
    }

    public void AddSpecial(string name){
        if(specialsCollected.ContainsKey(name)){
            specialsCollected[name]++;
        }else{
            specialsCollected.Add(name, 1);
        }
        
    }

    public static bool getIfPlayerCollected(string name, int ammount = 1){
        if(specialsCollected.ContainsKey(name) && specialsCollected[name] == ammount){
            return true;
        }else{
            return false;
        }
    }

    public int getBonesCollected(){
        return bonesCollected;
    }
    public void CollectManager(Collectable.CollectableType collect){
        if(collect == Collectable.CollectableType.EXTRA_LIFE){
        playerCombat.Cure(1);
        }else if(collect == Collectable.CollectableType.BONE){
            bonesCollected += 1;
            if(bonesCollected == 100){
                playerCombat.addExtraLifes(1);
            }
        }else if(collect == Collectable.CollectableType.MEGA_BONE){
                playerCombat.addExtraLifes(1);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(cornerDetectPosition,Vector2.down*0.2f);
            
    }
}
