using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Combat : Ent_Combat{

    //Characteristics
    [Header("Characteristics")]
    public int extraLifes = 1;
    public static int _extraLifes;
    public float hitBackForce = 100;
    public LayerMask enemyLayerMask;



    //Variables and Logic
   public static bool _isInvencible;
   public bool attackInputEnabled = true;
   public bool isAttacking;
    public GameObject obj_playerAttack;
    public Attack playerAttack;

    Vector3 lastCheckpoint;
    //Events
    public UnityEvent onAttackStart;
    public UnityEvent onAttackEnd;
    public UnityEvent playerTookDamage;
    public UnityEvent onInvencibilityEnd;
    public UnityEvent onNoLifes;

    public AudioClip attackSound;


    public override void Start()
    {
        base.Start();
        lastCheckpoint = transform.position;
        StartCoroutine(LateStart());

    }

    IEnumerator LateStart(){
        yield return new WaitForSeconds(0.1f);
        //Do late start things
        //Please, review this UI things. They realy dont need to be directly associated with player combat
    }
    private void Update() {
        _extraLifes = extraLifes;
        //Player attack after pressing a button
        if(Input.GetButtonDown("Fire2") && !isAttacking && attackInputEnabled){
            Attack(0);
        }

        _isInvencible = isInvencible;
    }

    public void Attack(int index){
        obj_playerAttack = Instantiate(attacks[index],attackPoint);
        obj_playerAttack.tag = "PlayerDamage";
        foreach(Transform child in obj_playerAttack.transform){
            child.tag = "PlayerDamage";
        }
        playerAttack = obj_playerAttack.GetComponentInChildren<Attack>();
        SoundManager.getSoundManager().PlaySound(attackSound,1f);
        StartCoroutine(AttackDelay(playerAttack.delayTime));
    }
    public void addExtraLifes(int extra){
        extraLifes += extra;
    }

    public void setExtraLifes(int total){
        extraLifes = total;
    }
    public void loseExtraLife(int lose){
        extraLifes -= lose;
        transform.position = lastCheckpoint;
    }

//For enemy damage
    public override void Damage(int ammount, Attack damager)
    {
        //Do the normal damage
        base.Damage(ammount, damager);
        //Call the event      
        playerTookDamage.Invoke();
        //Call the hitback and start invencibility
        if(hasRecoil && lifes > 0 || hasRecoil && extraLifes == 0)
        HitBack(damager.transform.position);
    }

//For ambient Damage (That only applies to the player)
    public void AmbientDamage(int ammount, Transform damager){
        //Do the same thing the base does with the life
    if(!isInvencible){
        lifes -= ammount;
        if(lifes <= 0){
            Die();
        }else{
        //Call the event   
         playerTookDamage.Invoke();
         //Call the hitback and start invencibility
         AfterDamageInvencibility();
         if(hasRecoil)
         HitBack(damager.position);
        }
    }
    }

    override public void Die(){
        if(extraLifes > 0){
            loseExtraLife(1);
            Cure(maxLifes);
            
        }else{
            //GameOver logic
            onNoLifes.Invoke();
        }
    }

//Cure (Since the player is actually the only one capable of healing)
    public void Cure(int ammount){
        if(lifes + ammount <= maxLifes){
            lifes+= ammount;
        }else{
            lifes = maxLifes;
        }     
    }

    protected override void AfterDamageInvencibility()
    {
        base.AfterDamageInvencibility();
    }
    private void OnTriggerEnter2D(Collider2D other) {
    if(!isInvencible){

        //If its being attacked by an enemy
        if(other.CompareTag("EnemyDamage") && other.TryGetComponent(out Attack atk)){

            Debug.Log(atk.attackStrengh + "/ Player Lifes: " + lifes);
            Damage(atk.attackStrengh, atk);
            SoundManager.getSoundManager().PlaySound(Resources.Load("AttackSound") as AudioClip, 2f);
        //If its being attacked by the ambient
        }else if(other.CompareTag("EnemyDamage")){

            Debug.Log(1 + "/ Player Lifes: " + lifes);
            AmbientDamage(1, other.transform);
            SoundManager.getSoundManager().PlaySound(Resources.Load("AttackSound") as AudioClip, 2f);

        }
            
    }
    }

    //Not the best way to do it but i will keep it for simplicity
    //Yeah, needs to be remade because it calculates direction wrong when its too close to the damage font
    public void HitBack(Vector2 damagePosition){
        entRecoil.ActivateRecoil( 
              new Vector2(Mathf.Sign(transform.position.x - damagePosition.x),
                 Mathf.Sign(transform.position.y - damagePosition.y)));

    }

    public IEnumerator AttackDelay(float attackTime){
        isAttacking = true;

        onAttackStart.Invoke();
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
        onAttackEnd.Invoke();
        obj_playerAttack = null;
        playerAttack = null;
        
    }

}
