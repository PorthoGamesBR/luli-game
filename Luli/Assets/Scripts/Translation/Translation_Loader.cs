using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Translation_Loader : MonoBehaviour
{
    
    public enum Languages{BRAZILLIAN_PORTUGUESE, ENGLISH, FRENCH}
    public static Languages actualLanguage = Languages.BRAZILLIAN_PORTUGUESE;

    public string path = "textsfortranslation";

    public static Dictionary<string, Translation_Text> trans_dictionary = new Dictionary<string, Translation_Text>();

    Translation_List texts;

    public static UnityEvent onChangeLanguage;

    private void Awake() {
        texts = Translation_List.Load(path);
        CreateDictionary();
        onChangeLanguage = new UnityEvent();
        
    }

    void Start()
    {
        
    }


    public void CreateDictionary(){
        foreach(Translation_Text text in texts.text_trans){
            trans_dictionary.Add(text.index, text);
        }
    }

    public static string LoadText(string scenename ="", string index = "0"){
        string key = scenename + ":" + index;
        Translation_Text text;
        if(trans_dictionary.ContainsKey(key)){
            text = trans_dictionary[key];
        }else{
            return key;
        }
        //If is in default/brazillian language
        string translatedText = text.Brazil;

        switch(actualLanguage){
            case Languages.ENGLISH:
            translatedText = text.English;
            break;
            case Languages.FRENCH:
            translatedText = text.French;
            break;
        }
        
        return translatedText;
    }

    public static string LoadText(string index){
        Translation_Text text;
        if(trans_dictionary.ContainsKey(index)){
            text = trans_dictionary[index];
        }else{
            return index;
        }
        
        //If is in default/brazillian language
        string translatedText = text.Brazil;

        switch(actualLanguage){
            case Languages.ENGLISH:
            translatedText = text.English;
            break;
            case Languages.FRENCH:
            translatedText = text.French;
            break;
        }

        return translatedText;
    }

    public void ChangeLanguage(string language){
        actualLanguage = (Languages)System.Enum.Parse(typeof(Languages), language);
        onChangeLanguage.Invoke();
        
    }
    public static void ChangeLanguage(Languages language){
        actualLanguage = language;
        onChangeLanguage.Invoke();
        
    }
}
