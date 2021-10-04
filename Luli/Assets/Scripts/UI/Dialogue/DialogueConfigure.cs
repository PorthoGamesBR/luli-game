using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueConfigure : MonoBehaviour
{
    
    public Image face;
    public Text dialogueText;
    string textToWrite;
    string textWritten;

    AudioClip actualVoice;
    public AudioSource voiceSource;
    Coroutine voicePlaying;

    bool finishedWritting;

    public UnityEvent onEndText;
    


    public void SetUpDialogue(Sprite faceSprite, string text, float speed, AudioClip voice = null){
        textToWrite = text;
        if(faceSprite != null){
            face.color = Color.white;
            face.sprite = faceSprite;
        }else{
            face.color = new Color32(0,0,0,0);
        }
        
        actualVoice = voice;

        StartCoroutine(WriteTextLetterByLetter(speed));

        
        
    }
    

    IEnumerator WriteTextLetterByLetter(float delay){
        int index = 0;
        textWritten = "";
        int indexVoice = 0;

        while(textToWrite != textWritten){
            textWritten += textToWrite[index];
            dialogueText.text = textWritten;
            index++;
            
            if(actualVoice != null){
            indexVoice++;
                if(indexVoice >3){
                voiceSource.clip = actualVoice;
                voiceSource.pitch = Random.Range(0.9f, 1.1f);
                voiceSource.Play();
                //voicePlaying = StartCoroutine(VoiceOverText());
                indexVoice  = 0;
                }
            }
           
            yield return new WaitForSeconds(delay);
        }
        onEndText.Invoke();
        finishedWritting = true;
        yield return null;
    }

    IEnumerator VoiceOverText(){

        SoundManager.getSoundManager().PlaySound(actualVoice, Random.Range(0.4f, 1f));
        yield return new WaitForSeconds(actualVoice.length);
        voicePlaying = null;
            
    }
}
