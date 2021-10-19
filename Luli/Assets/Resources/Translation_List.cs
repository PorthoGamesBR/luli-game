using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("ItemCollection")]
public class Translation_List
{
    [XmlArray("Texts")]
    [XmlArrayItem("Text")]
    public List<Translation_Text> text_trans = new List<Translation_Text>();



    public static Translation_List Load(string path){
        TextAsset _xml = Resources.Load<TextAsset>(path);
    if(_xml != null){
        XmlSerializer serializer = new XmlSerializer(typeof(Translation_List));

        StringReader reader = new StringReader(_xml.text);

        Translation_List textList = serializer.Deserialize(reader) as Translation_List;

        reader.Close();

        return textList;
    }else{
        Debug.LogError("Path leads to null archive");
        return null;
    }
       
    }


}
