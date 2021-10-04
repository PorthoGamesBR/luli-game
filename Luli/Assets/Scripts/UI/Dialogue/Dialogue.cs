using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField]
    Dialogue_Frame[] frames;
    int dialogueIndex;

    public string getDialogueText(int index){
        Dialogue_Frame frame = frames[index];
        return Translation_Loader.LoadText("Dialogue:" + frame.dialogueName + ":" + frame.dialogueIndex);
    }

    public Sprite getDialogueFace(int index){
        return frames[index].face;
    }
    public int getDialogueMaxIndex(){
        return frames.Length;
    }

    public AudioClip getVoice(int index){
        return frames[index].voice;
    }

    [Serializable]
    public struct Dialogue_Frame{
    public string dialogueName;
    public string dialogueIndex;
    public Sprite face;

    public AudioClip voice;
    }

}

   
