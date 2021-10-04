using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue_Manager : MonoBehaviour
{
    public DialogueConfigure dialogueConfigure;

    public GameObject dialogueBox;
    public GameObject indicator;

    int dialogueIndex;
    Dialogue actualDialogue;

    public float textSpeed;

    bool textWritten;
    public bool dialogueActive;

    public UnityEvent<int> onDialogueProgress;

    private void Update() {
        if(Input.GetButtonDown("Fire1") && dialogueActive){
            OnClickEvent();
        }
    }
    public void StartDialogue(Dialogue dialogue){
        
        actualDialogue = dialogue;
        dialogueBox.SetActive(true);
        if(actualDialogue.getVoice(dialogueIndex) != null){
            dialogueConfigure.SetUpDialogue
        (actualDialogue.getDialogueFace(dialogueIndex), actualDialogue.getDialogueText(dialogueIndex), textSpeed, 
        actualDialogue.getVoice(dialogueIndex));
        }else{
            dialogueConfigure.SetUpDialogue
        (actualDialogue.getDialogueFace(dialogueIndex), actualDialogue.getDialogueText(dialogueIndex), textSpeed);
        }
       
        dialogueActive = true;
        onDialogueProgress = new UnityEvent<int>();
    }

    public void SetUpNextDialogue(){
        if(dialogueIndex < actualDialogue.getDialogueMaxIndex() - 1){
            dialogueIndex++;
            if(actualDialogue.getVoice(dialogueIndex) != null){
            dialogueConfigure.SetUpDialogue
            (actualDialogue.getDialogueFace(dialogueIndex), actualDialogue.getDialogueText(dialogueIndex), textSpeed, 
            actualDialogue.getVoice(dialogueIndex));
            }else{
            dialogueConfigure.SetUpDialogue
            (actualDialogue.getDialogueFace(dialogueIndex), actualDialogue.getDialogueText(dialogueIndex), textSpeed);
            }
            setTextWritten(false);
            
        }else{
            dialogueIndex = 0;
            dialogueBox.SetActive(false);
            textWritten = false;
            dialogueActive = false;
            
        }
    }

    public void setTextWritten(bool set){
        textWritten = set;
        if(set){
            indicator.SetActive(true);
        }else{
            indicator.SetActive(false);
        }
    }

    public void OnClickEvent(){
        if(textWritten){
            onDialogueProgress.Invoke(dialogueIndex);
            SetUpNextDialogue();
        }
    }


}
